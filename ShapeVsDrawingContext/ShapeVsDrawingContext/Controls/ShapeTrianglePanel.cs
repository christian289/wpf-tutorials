namespace ShapeVsDrawingContext.Controls;

/// <summary>
/// Shape(Polygon)을 사용하여 삼각형을 그리는 패널
/// Shape를 사용하면 각 도형마다 Visual 객체가 생성되어 오버헤드가 큼
/// </summary>
public sealed class ShapeTrianglePanel : Canvas
{
    private readonly Random _random = new();

    public async Task<TimeSpan> DrawTrianglesAsync(int count)
    {
        // 기존 삼각형 제거
        // Remove existing triangles
        Children.Clear();

        // Polygon 객체 생성 (측정 전)
        // Create Polygon objects before measurement
        var triangles = new List<Polygon>(count);
        for (int i = 0; i < count; i++)
        {
            triangles.Add(CreateRandomTriangle());

            // UI Hang 방지를 위해 주기적으로 양보
            // Yield periodically to prevent UI hang
            if (i % 100 == 0)
            {
                await Dispatcher.InvokeAsync(() => { }, DispatcherPriority.Background);
            }
        }

        // Visual Tree에 추가하는 시간 측정 (렌더링 포함)
        // Measure time to add to Visual Tree (includes rendering)
        var stopwatch = Stopwatch.StartNew();
        foreach (var triangle in triangles)
        {
            Children.Add(triangle);
        }
        await Dispatcher.InvokeAsync(() => { }, DispatcherPriority.Render);
        stopwatch.Stop();

        return stopwatch.Elapsed;
    }

    private Polygon CreateRandomTriangle()
    {
        double width = ActualWidth > 0 ? ActualWidth : 400;
        double height = ActualHeight > 0 ? ActualHeight : 400;

        double x = _random.NextDouble() * (width - 40);
        double y = _random.NextDouble() * (height - 40);
        double size = 20 + _random.NextDouble() * 20;

        var polygon = new Polygon
        {
            Points =
            [
                new Point(x + size / 2, y),
                new Point(x, y + size),
                new Point(x + size, y + size)
            ],
            Fill = new SolidColorBrush(Color.FromRgb(
                (byte)_random.Next(256),
                (byte)_random.Next(256),
                (byte)_random.Next(256))),
            Stroke = Brushes.Black,
            StrokeThickness = 1
        };

        return polygon;
    }

    public void Clear()
    {
        Children.Clear();
    }
}
