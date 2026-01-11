namespace ShapeVsDrawingContext.ViewModels;

public sealed partial class MainViewModel : ObservableObject
{
    private const int TriangleCount = 10000;

    // /communitytoolkit-mvvm 스킬 컨벤션 적용
    // Applied /communitytoolkit-mvvm skill convention
    [NotifyCanExecuteChangedFor(nameof(StartCommand))]
    [NotifyCanExecuteChangedFor(nameof(ClearCommand))]
    [ObservableProperty] private bool _isRunning;

    [ObservableProperty] private string _shapeElapsedTime = "대기 중... / Waiting...";

    [ObservableProperty] private string _drawingContextElapsedTime = "대기 중... / Waiting...";

    [ObservableProperty] private string _statusMessage = "시작 버튼을 눌러 테스트를 시작하세요.\nClick Start button to begin the test.";

    private Func<int, Task<TimeSpan>>? _drawShapeTriangles;
    private Func<int, Task<TimeSpan>>? _drawContextTriangles;
    private Action? _clearShapePanel;
    private Action? _clearContextPanel;

    public void SetDrawActions(
        Func<int, Task<TimeSpan>> drawShapeTriangles,
        Func<int, Task<TimeSpan>> drawContextTriangles,
        Action clearShapePanel,
        Action clearContextPanel)
    {
        _drawShapeTriangles = drawShapeTriangles;
        _drawContextTriangles = drawContextTriangles;
        _clearShapePanel = clearShapePanel;
        _clearContextPanel = clearContextPanel;
    }

    [RelayCommand(CanExecute = nameof(CanStart))]
    private async Task StartAsync()
    {
        if (_drawShapeTriangles is null || _drawContextTriangles is null)
        {
            return;
        }

        IsRunning = true;
        ShapeElapsedTime = "측정 중... / Measuring...";
        DrawingContextElapsedTime = "대기 중... / Waiting...";
        StatusMessage = $"Shape 방식으로 {TriangleCount}개 삼각형 그리는 중...\nDrawing {TriangleCount} triangles using Shape approach...";

        // Shape 방식 테스트
        // Test Shape method
        var shapeTime = await _drawShapeTriangles(TriangleCount);
        ShapeElapsedTime = $"{shapeTime.TotalMilliseconds:F2} ms";

        StatusMessage = $"DrawingContext 방식으로 {TriangleCount}개 삼각형 그리는 중...\nDrawing {TriangleCount} triangles using DrawingContext approach...";
        DrawingContextElapsedTime = "측정 중... / Measuring...";

        // DrawingContext 방식 테스트
        // Test DrawingContext method
        var drawingContextTime = await _drawContextTriangles(TriangleCount);
        DrawingContextElapsedTime = $"{drawingContextTime.TotalMilliseconds:F2} ms";

        // 결과 비교
        // Compare results
        double ratio = shapeTime.TotalMilliseconds / drawingContextTime.TotalMilliseconds;
        StatusMessage = $"완료! Shape 방식이 DrawingContext 방식보다 {ratio:F1}배 느림\nComplete! Shape approach is {ratio:F1}x slower than DrawingContext";

        IsRunning = false;
    }

    private bool CanStart() => !IsRunning;

    [RelayCommand(CanExecute = nameof(CanClear))]
    private void Clear()
    {
        _clearShapePanel?.Invoke();
        _clearContextPanel?.Invoke();
        ShapeElapsedTime = "대기 중... / Waiting...";
        DrawingContextElapsedTime = "대기 중... / Waiting...";
        StatusMessage = "초기화됨. 시작 버튼을 눌러 테스트를 시작하세요.\nCleared. Click Start button to begin the test.";
    }

    private bool CanClear() => !IsRunning;
}
