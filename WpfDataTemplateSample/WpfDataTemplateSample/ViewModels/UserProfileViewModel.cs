namespace WpfDataTemplateSample.ViewModels;

public sealed partial class UserProfileViewModel : ObservableObject
{
    [ObservableProperty] private string _userName = "John Doe";
    [ObservableProperty] private string _email = "john.doe@example.com";
    [ObservableProperty] private string _department = "Development";
    [ObservableProperty] private string _description = "User profile information is displayed here. The View is automatically resolved through Mappings.xaml.";
}
