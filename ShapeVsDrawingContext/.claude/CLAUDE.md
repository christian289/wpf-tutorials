# ShapeVsDrawingContext 프로젝트

## 프로젝트 개요

WPF에서 Shape(Polygon) vs DrawingContext 렌더링 성능 비교 프로그램

## 기술 스택

- .NET 9.0 / WPF
- CommunityToolkit.Mvvm (MVVM 패턴)

## 프로젝트 구조

```
├── Controls/
│   ├── ShapeTrianglePanel.cs         # Polygon Shape 기반
│   └── DrawingContextTrianglePanel.cs # DrawingContext 기반
├── ViewModels/
│   └── MainViewModel.cs
├── GlobalUsings.cs
├── MainWindow.xaml                   # 5:5 분할 UI
└── MainWindow.xaml.cs
```

## WPF 렌더링 성능 주의사항

### InvalidateVisual() 루프 내 호출 금지

DrawingContext 사용 시 루프 내에서 `InvalidateVisual()`을 반복 호출하면 **O(n²) 복잡도**가 발생한다.

**잘못된 패턴**
```csharp
for (int i = 0; i < count; i++)
{
    _items.Add(data);
    if (i % 10 == 0)
    {
        InvalidateVisual();  // 매번 전체 컬렉션을 다시 그림!
    }
}
```

**올바른 패턴**
```csharp
// 1. 데이터 수집
for (int i = 0; i < count; i++)
{
    _items.Add(data);
}

// 2. 마지막에 한 번만 렌더링
InvalidateVisual();
```

### 이유

- `InvalidateVisual()` 호출 시 `OnRender(DrawingContext dc)`가 트리거됨
- `OnRender`에서 전체 컬렉션을 순회하며 그림
- 루프 내 호출 시: 10개 그리기 + 20개 그리기 + ... + n개 그리기 = O(n²)
- 마지막 한 번 호출 시: n개 그리기 × 1회 = O(n)

### Shape vs DrawingContext 성능 차이

| 방식 | 특징 | 적합한 경우 |
|------|------|------------|
| Shape | Visual Tree에 개별 객체 추가, 레이아웃 계산 오버헤드 | 소수의 인터랙티브 도형 |
| DrawingContext | 단일 Visual에서 직접 그리기, 오버헤드 최소 | 대량의 정적 도형 |

## 빌드 및 실행

```bash
dotnet build
dotnet run
```
