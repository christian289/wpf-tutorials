namespace WpfDataTemplateSample.ViewModels;

public sealed partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty] private object? _currentViewModel;

    public MainWindowViewModel()
    {
        CurrentViewModel = new HomeViewModel();
    }

    [RelayCommand]
    private void NavigateToHome()
    {
        CurrentViewModel = new HomeViewModel();
    }

    [RelayCommand]
    private void NavigateToSettings()
    {
        CurrentViewModel = new SettingsViewModel();
    }

    [RelayCommand]
    private void NavigateToUserProfile()
    {
        CurrentViewModel = new UserProfileViewModel();
    }
}
