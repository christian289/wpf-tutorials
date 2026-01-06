namespace ShapeVsDrawingContext.Controls;

/// <summary>
/// DrawingContext를 사용하여 삼각형을 그리는 패널
/// OnRender에서 직접 그리므로 Visual 객체 오버헤드가 없음
/// </summary>
public sealed class DrawingContextTrianglePanel : FrameworkElement
{
    private readonly Random _random = new();
    private readonly List<TriangleData> _triangles = [];
    private readonly Pen _pen = new(Brushes.Black, 1);

    public DrawingContextTrianglePanel()
    {
        // Pen을 Freeze하여 성능 최적화
        // Freeze pen for performance optimization
        _pen.Freeze();
    }

    public async Task<TimeSpan> DrawTrianglesAsync(int count)
    {
        // 기존 삼각형 데이터 제거
        // Clear existing triangle data
        _triangles.Clear();

        double width = ActualWidth > 0 ? ActualWidth : 400;
        double height = ActualHeight > 0 ? ActualHeight : 400;

        // 데이터 생성 (측정 전)
        // Generate data before measurement
        for (int i = 0; i < count; i++)
        {
            _triangles.Add(CreateRandomTriangleData(width, height));

            // UI Hang 방지를 위해 주기적으로 양보 (렌더링은 하지 않음)
            // Yield periodically to prevent UI hang (no rendering)
            if (i % 100 == 0)
            {
                await Dispatcher.InvokeAsync(() => { }, DispatcherPriority.Background);
            }
        }

        // 렌더링 시간만 측정
        // Measure only rendering time
        var stopwatch = Stopwatch.StartNew();
        InvalidateVisual();
        await Dispatcher.InvokeAsync(() => { }, DispatcherPriority.Render);
        stopwatch.Stop();

        return stopwatch.Elapsed;
    }

    protected override void OnRender(DrawingContext dc)
    {
        base.OnRender(dc);

        foreach (var triangle in _triangles)
        {
            var geometry = new StreamGeometry();
            using (var ctx = geometry.Open())
            {
                ctx.BeginFigure(triangle.Point1, true, true);
                ctx.LineTo(triangle.Point2, true, false);
                ctx.LineTo(triangle.Point3, true, false);
            }
            geometry.Freeze();

            dc.DrawGeometry(triangle.Fill, _pen, geometry);
        }
    }

    private TriangleData CreateRandomTriangleData(double width, double height)
    {
        double x = _random.NextDouble() * (width - 40);
        double y = _random.NextDouble() * (height - 40);
        double size = 20 + _random.NextDouble() * 20;

        var brush = new SolidColorBrush(Color.FromRgb(
            (byte)_random.Next(256),
            (byte)_random.Next(256),
            (byte)_random.Next(256)));
        brush.Freeze();

        return new TriangleData(
            new Point(x + size / 2, y),
            new Point(x, y + size),
            new Point(x + size, y + size),
            brush);
    }

    public void Clear()
    {
        _triangles.Clear();
        InvalidateVisual();
    }

    private readonly record struct TriangleData(Point Point1, Point Point2, Point Point3, Brush Fill);
}
