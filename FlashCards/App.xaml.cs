using FlashCards.Activation;
using FlashCards.Contracts.Services;
using FlashCards.Core.Contracts.Services;
using FlashCards.Core.Services;
using FlashCards.Data;
using FlashCards.Models;
using FlashCards.Services;
using FlashCards.ViewModels;
using FlashCards.Views;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Windows.Storage;
using WinUI3Localizer;

namespace FlashCards;

// To learn more about WinUI 3, see https://docs.microsoft.com/windows/apps/winui/winui3/.
public partial class App : Application
{
    // The .NET Generic Host provides dependency injection, configuration, logging, and other services.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
    public IHost Host
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public static WindowEx MainWindow { get; } = new MainWindow();

    public static UIElement? AppTitlebar { get; set; }

    public App()
    {
        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers

            // Services
            services.AddSingleton<ILocalizationService, LocalizationService>();
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddSingleton<IDemotionSettingsService, DemotionSettingsService>();
            services.AddSingleton<IDatabaseService, DatabaseService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();
            services.AddSingleton<ICreateSettingsService, CreateSettingsService>();
            services.AddSingleton<IJSONService, JSONService>();
            services.AddSingleton<IStorageService, StorageService>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Core Services
            services.AddSingleton<IFileService, FileService>();

            // Views and ViewModels
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();
            services.AddTransient<ProgressViewModel>();
            services.AddTransient<ProgressPage>();
            services.AddTransient<CreateViewModel>();
            services.AddTransient<CreatePage>();
            services.AddTransient<StudyViewModel>();
            services.AddTransient<StudyPage>();
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();

            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
        }).
        Build();

        UnhandledException += App_UnhandledException;

        using (FlashCardsContext context = new())
        {
            context.Database.EnsureCreated();
            context.SaveChanges();
        };
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);
        await InitializeLocalizer();
        await App.GetService<IActivationService>().ActivateAsync(args);
    }

    private static async Task InitializeLocalizer()
    {

        // Initialize a "Strings" folder in the "LocalFolder" for the packaged app.
        StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        StorageFolder stringsFolder = await localFolder.CreateFolderAsync(
            "Strings",
            CreationCollisionOption.OpenIfExists
        );

        // Create string resources file from app resources if doesn't exists.
        var resourceFileName = "Resources.resw";
        await MakeSureStringResourceFileExists(stringsFolder, "en-us", resourceFileName);
        await MakeSureStringResourceFileExists(stringsFolder, "de-de", resourceFileName);

        ILocalizer localizer = await new LocalizerBuilder()
            .AddStringResourcesFolderForLanguageDictionaries(stringsFolder.Path)
            .SetOptions(options =>
            {
                ILocalizationService localizationService = App.GetService<ILocalizationService>();
                options.DefaultLanguage = localizationService.SelectedLanguageTag;
            })
            .Build();
    }

    private static async Task MakeSureStringResourceFileExists(StorageFolder stringsFolder, string language, string resourceFileName)
    {
        StorageFolder languageFolder = await stringsFolder.CreateFolderAsync(
            desiredName: language,
            CreationCollisionOption.OpenIfExists);

        var appResourceFilePath = Path.Combine(stringsFolder.Name, language, resourceFileName);
        StorageFile appResourceFile = await LoadStringResourcesFileFromAppResource(appResourceFilePath);

        IStorageItem? localResourceFile = await languageFolder.TryGetItemAsync(resourceFileName);

        if (localResourceFile is null || await ResourceFileWasModifed(appResourceFile, localResourceFile))
        {
            _ = await appResourceFile.CopyAsync(
                destinationFolder: languageFolder,
                desiredNewName: appResourceFile.Name,
                option: NameCollisionOption.ReplaceExisting);
        }
    }

    private static async Task<bool> ResourceFileWasModifed(StorageFile appResourceFile, IStorageItem localResourceFile)
    {
        DateTimeOffset appResourceFile_DateModified = (await appResourceFile.GetBasicPropertiesAsync()).DateModified;
        DateTimeOffset localResourceFile_DateModified = (await localResourceFile.GetBasicPropertiesAsync()).DateModified;
        return (appResourceFile_DateModified > localResourceFile_DateModified);
    }

    private static async Task<StorageFile> LoadStringResourcesFileFromAppResource(string filePath)
    {
        Uri resourcesFileUri = new($"ms-appx:///{filePath}");
        return await StorageFile.GetFileFromApplicationUriAsync(resourcesFileUri);
    }
}
