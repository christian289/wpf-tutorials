// MyUserControl.xaml.cs
namespace MyApp.Controls;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

public partial class MyUserControl : UserControl
{
    public MyUserControl()
    {
        InitializeComponent();

        // Popup 포커스 관리
        // Popup focus management
        PreviewMouseDown += MyUserControl_PreviewMouseDown;
    }

    private void MyUserControl_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        // 키보드 포커스가 없으면 부모 윈도우 활성화
        // Activate parent window if keyboard focus is lost
        if (!IsKeyboardFocused)
        {
            Window.GetWindow(this)?.Activate();
        }
    }
}
