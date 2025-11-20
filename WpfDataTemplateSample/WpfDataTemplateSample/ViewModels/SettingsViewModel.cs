namespace WpfDataTemplateSample.ViewModels;

public sealed partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty] private string _title = "Settings Page";
    [ObservableProperty] private bool _isDarkModeEnabled = false;
    [ObservableProperty] private int _fontSize = 14;
    [ObservableProperty] private string _description = "Configure your application settings here. This view is automatically rendered via DataTemplate.";
}
