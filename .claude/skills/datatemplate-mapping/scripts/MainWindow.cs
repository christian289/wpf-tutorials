// MainWindow.xaml.cs
using WpfDataTemplateSample.ViewModels;

namespace WpfDataTemplateSample;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }
}
