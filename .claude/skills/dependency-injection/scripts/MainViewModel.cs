// ViewModels/MainViewModel.cs
namespace MyApp.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public sealed partial class MainViewModel : ObservableObject
{
    private readonly IUserService _userService;
    private readonly IDialogService _dialogService;

    // Constructor Injection
    public MainViewModel(IUserService userService, IDialogService dialogService)
    {
        _userService = userService;
        _dialogService = dialogService;

        LoadDataAsync();
    }

    [ObservableProperty]
    private ObservableCollection<User> users = [];

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        try
        {
            var userList = await _userService.GetAllUsersAsync();
            Users = new ObservableCollection<User>(userList);
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("오류 발생", ex.Message);
            // Error occurred
        }
    }
}
