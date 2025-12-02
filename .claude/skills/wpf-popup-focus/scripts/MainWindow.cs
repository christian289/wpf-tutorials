namespace MyApp;

using System.Windows;
using System.Windows.Input;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // Manage focus to ensure Popup works correctly
        PreviewMouseDown += MainWindow_PreviewMouseDown;
    }

    private void MainWindow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        // Activate window if keyboard focus is lost
        if (!IsKeyboardFocused)
        {
            Activate();
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        popup.IsOpen = true;
    }
}
