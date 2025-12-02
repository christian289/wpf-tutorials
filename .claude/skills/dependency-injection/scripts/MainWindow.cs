// MainWindow.xaml.cs
namespace MyApp;

using System.Windows;

public partial class MainWindow : Window
{
    // Constructor Injection을 통한 ViewModel 주입
    // ViewModel injection through Constructor Injection
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
