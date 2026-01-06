using ShapeVsDrawingContext.ViewModels;

namespace ShapeVsDrawingContext;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // ViewModel에 델리게이트 전달 (View 타입 참조 없이)
        // Pass delegates to ViewModel (without View type references)
        Loaded += (_, _) =>
        {
            if (DataContext is MainViewModel vm)
            {
                vm.SetDrawActions(
                    ShapePanel.DrawTrianglesAsync,
                    DrawingContextPanel.DrawTrianglesAsync,
                    ShapePanel.Clear,
                    DrawingContextPanel.Clear);
            }
        };
    }
}
