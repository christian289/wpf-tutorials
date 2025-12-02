// App.xaml.cs
namespace MyApp;

using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        // GenericHost 생성 및 서비스 등록
        // Create GenericHost and register services
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                // Services 등록
                // Register services
                services.AddSingleton<IUserRepository, UserRepository>();
                services.AddSingleton<IUserService, UserService>();
                services.AddTransient<IDialogService, DialogService>();

                // ViewModels 등록
                // Register ViewModels
                services.AddTransient<MainViewModel>();
                services.AddTransient<SettingsViewModel>();

                // Views 등록
                // Register Views
                services.AddSingleton<MainWindow>();
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();

        // MainWindow를 ServiceProvider로부터 가져오기
        // Get MainWindow from ServiceProvider
        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        using (_host)
        {
            await _host.StopAsync();
        }

        base.OnExit(e);
    }
}
