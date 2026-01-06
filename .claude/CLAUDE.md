## 6. .NET AvaloniaUI 코드 생성 지침

### 6.1 기본 원칙

- **.NET CSharp 코드 생성 지침을 기본으로 할 것**
- **WPF 지침의 MVVM 원칙을 동일하게 적용**

### 6.2 AvaloniaUI 솔루션 및 프로젝트 구조

#### 6.2.1 프로젝트 명명 규칙

```
SolutionName/
├── SolutionName.Abstractions      // .NET Class Library (Interface, abstract class 등 추상 타입)
├── SolutionName.Core              // .NET Class Library (비즈니스 로직, 순수 C#)
├── SolutionName.Core.Tests        // xUnit Test Project
├── SolutionName.ViewModels        // .NET Class Library (MVVM ViewModel)
├── SolutionName.AvaloniaServices  // Avalonia Class Library (Avalonia 관련 서비스)
├── SolutionName.AvaloniaLib       // Avalonia Class Library (재사용 가능한 컴포넌트)
├── SolutionName.AvaloniaApp       // Avalonia Application Project (실행 진입점)
├── SolutionName.UI                // Avalonia Custom Control Library (커스텀 컨트롤)
└── [Solution Folders]
    ├── SolutionName/              // 주요 프로젝트 그룹
    └── Common/                    // 범용 프로젝트 그룹
```

**프로젝트 타입별 명명:**

- `.Abstractions`: .NET Class Library - Interface, abstract class 등 추상 타입 정의 (Inversion of Control)
- `.Core`: .NET Class Library - 비즈니스 로직, 데이터 모델, 서비스 (UI 프레임워크 독립)
- `.Core.Tests`: xUnit/NUnit/MSTest Test Project
- `.ViewModels`: .NET Class Library - MVVM ViewModel (UI 프레임워크 독립)
- `.AvaloniaServices`: Avalonia Class Library - Avalonia 관련 서비스 (DialogService, NavigationService 등)
- `.AvaloniaLib`: Avalonia Class Library - 재사용 가능한 UserControl, Window, Converter, Behavior, AttachedProperty
- `.AvaloniaApp`: Avalonia Application Project - 실행 진입점, App.axaml
- `.UI`: Avalonia Custom Control Library - ControlTheme 기반 커스텀 컨트롤

**프로젝트 의존성 계층:**

```
SolutionName.AvaloniaApp
    ↓ 참조
SolutionName.Abstractions (최상단 - 다른 프로젝트에 의존하지 않음)
    ↓ 참조
SolutionName.Core
```

**Abstractions 레이어의 역할:**

- 모든 Interface와 abstract class 보관
- 구체 타입을 직접 참조하지 않고 추상 타입으로 의존성 역전 (Dependency Inversion Principle)
- 런타임에 DI 컨테이너를 통해 실제 구현체 주입
- 테스트 시 Mock 객체로 교체 가능

### 6.3 MVVM 패턴

- **기본적으로 MVVM 형태로 코드를 생성할 것**
- **ViewModel 클래스에 UI 프레임워크 의존성 금지:**
  - **AvaloniaUI**: ViewModel class에 `Avalonia`로 시작하는 네임스페이스 참조 금지
  - **⚠️ 절대로 UI 프레임워크 FCL 객체가 참조되지 않도록 한다**
- **MVVM 제약은 ViewModel에만 적용:**
  - ViewModel이라는 접미사가 있는 Class들에만 위 제한 규칙 적용
  - Converter, Service, Manager, Locator 같은 클래스들은 UI 프레임워크 참조 가능

### 6.4 MVVM 프레임워크

- **MVVM을 구조로 잡을 때는 CommunityToolkit.Mvvm을 기본으로 사용**
- **CommunityToolkit.Mvvm Attribute 스타일은 WPF와 동일하게 적용** (Section 5.4.1 참조)

### 6.5 AXAML 코드 작성

- **AXAML 코드를 생성할 때는 CustomControl을 사용하여 ControlTheme을 통한 Stand-Alone Control Style 사용**
- 목적: 테마 분리 및 스타일 의존성 최소화

#### 6.5.1 AvaloniaUI Custom Control Library 프로젝트 구조

**권장 프로젝트 구조:**

```
YourProject/
├── Dependencies/
├── Themes/
│   ├── Generic.axaml            ← ControlTheme 정의
│   ├── CustomButton.axaml       ← 개별 컨트롤 테마
│   └── CustomTextBox.axaml      ← 개별 컨트롤 테마
├── CustomButton.cs
└── CustomTextBox.cs
```

**단계별 설정:**

1. **CustomControl 정의 - StyledProperty 사용**

```csharp
// CustomButton.cs
namespace YourNamespace;

using Avalonia;
using Avalonia.Controls;

public class CustomButton : Button
{
    // StyledProperty 정의
    // Define StyledProperty
    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<CustomButton, string>(nameof(Text), defaultValue: string.Empty);

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly StyledProperty<bool> IsHighlightedProperty =
        AvaloniaProperty.Register<CustomButton, bool>(nameof(IsHighlighted), defaultValue: false);

    public bool IsHighlighted
    {
        get => GetValue(IsHighlightedProperty);
        set => SetValue(IsHighlightedProperty, value);
    }
}
```

2. **Generic.axaml 구성 - ControlTheme 허브로 사용**

Generic.axaml은 MergedDictionaries를 통해 개별 테마를 병합:

```xml
<!-- Themes/Generic.axaml -->
<ResourceDictionary xmlns="https://github.com/avaloniaui"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <ResourceDictionary.MergedDictionaries>
        <ResourceInclude Source="/Themes/CustomButton.axaml" />
        <ResourceInclude Source="/Themes/CustomTextBox.axaml" />
    </ResourceDictionary.MergedDictionaries>
</ResourceDictionary>
```

**⚠️ ResourceInclude vs MergeResourceInclude 구분:**

- **ResourceInclude**: 일반 ResourceDictionary 파일 (Generic.axaml, Styles 등)에서 사용
- **MergeResourceInclude**: Application.Resources (App.axaml)에서만 사용

```xml
<!-- App.axaml - MergeResourceInclude 사용 -->
<Application xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             x:Class="YourNamespace.App">
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <MergeResourceInclude Source="/Assets/AppResources.axaml" />
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</Application>
```

3. **개별 컨트롤 ControlTheme 정의**

각 컨트롤마다 독립적인 AXAML 파일에 ControlTheme 정의:

```xml
<!-- Themes/CustomButton.axaml -->
<ResourceDictionary xmlns="https://github.com/avaloniaui"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                    xmlns:local="using:YourNamespace">

    <!-- 컨트롤 전용 리소스 정의 -->
    <!-- Control-specific resources -->
    <SolidColorBrush x:Key="ButtonBackground">#FF2D5460</SolidColorBrush>
    <SolidColorBrush x:Key="ButtonBackgroundPointerOver">#FF1D5460</SolidColorBrush>
    <SolidColorBrush x:Key="ButtonForeground">#FFFFFFFF</SolidColorBrush>

    <!-- ControlTheme 정의 -->
    <!-- ControlTheme definition -->
    <ControlTheme x:Key="{x:Type local:CustomButton}" TargetType="local:CustomButton">
        <Setter Property="Background" Value="{StaticResource ButtonBackground}" />
        <Setter Property="Foreground" Value="{StaticResource ButtonForeground}" />
        <Setter Property="Padding" Value="12,6" />
        <Setter Property="HorizontalContentAlignment" Value="Center" />
        <Setter Property="VerticalContentAlignment" Value="Center" />
        <Setter Property="Template">
            <ControlTemplate>
                <Border Background="{TemplateBinding Background}"
                        BorderBrush="{TemplateBinding BorderBrush}"
                        BorderThickness="{TemplateBinding BorderThickness}"
                        CornerRadius="4"
                        Padding="{TemplateBinding Padding}">
                    <ContentPresenter Content="{TemplateBinding Text}"
                                    HorizontalAlignment="{TemplateBinding HorizontalContentAlignment}"
                                    VerticalAlignment="{TemplateBinding VerticalContentAlignment}" />
                </Border>
            </ControlTemplate>
        </Setter>

        <!-- Pseudo Classes를 사용한 상태 스타일 -->
        <!-- State styles using Pseudo Classes -->
        <Style Selector="^:pointerover">
            <Setter Property="Background" Value="{StaticResource ButtonBackgroundPointerOver}" />
        </Style>

        <Style Selector="^.highlighted">
            <Setter Property="BorderBrush" Value="#FFFF9900" />
            <Setter Property="BorderThickness" Value="2" />
        </Style>
    </ControlTheme>
</ResourceDictionary>
```

4. **CSS Class 기반 스타일 적용**

```xml
<!-- 사용 예시 -->
<!-- Usage example -->
<Window xmlns="https://github.com/avaloniaui"
        xmlns:local="using:YourNamespace">

    <!-- 기본 스타일 -->
    <!-- Default style -->
    <local:CustomButton Text="Normal Button" />

    <!-- CSS Class를 통한 스타일 변형 -->
    <!-- Style variation through CSS Class -->
    <local:CustomButton Text="Highlighted Button"
                       Classes="highlighted" />
</Window>
```

**실제 프로젝트 예시:**

```xml
<!-- Themes/Generic.axaml -->
<ResourceDictionary xmlns="https://github.com/avaloniaui"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <ResourceDictionary.MergedDictionaries>
        <ResourceInclude Source="/Themes/GdtBranchSelectionDialog.axaml" />
        <ResourceInclude Source="/Themes/GdtDataGrid.axaml" />
    </ResourceDictionary.MergedDictionaries>
</ResourceDictionary>
```

```xml
<!-- Themes/GdtBranchSelectionDialog.axaml -->
<ResourceDictionary xmlns="https://github.com/avaloniaui"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                    xmlns:local="using:GameDataTool.Controls.Dialogs">

    <SolidColorBrush x:Key="DialogBackground">#FFFFFFFF</SolidColorBrush>
    <SolidColorBrush x:Key="ApplyButtonBackground">#FF2D5460</SolidColorBrush>
    <SolidColorBrush x:Key="ApplyButtonBackgroundPointerOver">#FF1D5460</SolidColorBrush>

    <ControlTheme x:Key="{x:Type local:GdtBranchSelectionDialog}"
                  TargetType="local:GdtBranchSelectionDialog">
        <Setter Property="Width" Value="500" />
        <Setter Property="Height" Value="400" />
        <Setter Property="Background" Value="{StaticResource DialogBackground}" />
        <Setter Property="Template">
            <ControlTemplate>
                <Border Background="{TemplateBinding Background}"
                        BorderBrush="{TemplateBinding BorderBrush}"
                        BorderThickness="1"
                        CornerRadius="8"
                        BoxShadow="0 4 16 0 #40000000">
                    <Grid RowDefinitions="Auto,*,Auto">
                        <!-- 헤더 -->
                        <!-- Header -->
                        <TextBlock Grid.Row="0"
                                 Text="{TemplateBinding Title}"
                                 FontSize="16"
                                 FontWeight="SemiBold"
                                 Margin="16" />

                        <!-- 콘텐츠 -->
                        <!-- Content -->
                        <ContentPresenter Grid.Row="1"
                                        Content="{TemplateBinding Content}"
                                        Margin="16,0" />

                        <!-- 버튼 영역 -->
                        <!-- Button area -->
                        <StackPanel Grid.Row="2"
                                  Orientation="Horizontal"
                                  HorizontalAlignment="Right"
                                  Spacing="8"
                                  Margin="16">
                            <Button Content="적용" Classes="apply" />
                            <Button Content="취소" Classes="cancel" />
                        </StackPanel>
                    </Grid>
                </Border>
            </ControlTemplate>
        </Setter>

        <Style Selector="^ Button.apply">
            <Setter Property="Background" Value="{StaticResource ApplyButtonBackground}" />
        </Style>

        <Style Selector="^ Button.apply:pointerover">
            <Setter Property="Background" Value="{StaticResource ApplyButtonBackgroundPointerOver}" />
        </Style>
    </ControlTheme>
</ResourceDictionary>
```

**장점:**

- ControlTheme 기반으로 테마와 로직 완전 분리
- CSS Class를 통한 유연한 스타일 변형
- Pseudo Classes (:pointerover, :pressed 등)를 통한 상태 관리
- ResourceInclude를 통한 테마 모듈화
- 팀 작업 시 파일 단위로 작업 분리 가능

#### 6.5.2 WPF vs AvaloniaUI 주요 차이점

| 항목        | WPF                                     | AvaloniaUI                           |
| ----------- | --------------------------------------- | ------------------------------------ |
| 파일 확장자 | .xaml                                   | .axaml                               |
| 스타일 정의 | Style + ControlTemplate                 | ControlTheme                         |
| 상태 관리   | Trigger, DataTrigger                    | Pseudo Classes, Style Selector       |
| CSS 지원    | ❌                                      | ✅ (Classes 속성)                    |
| 리소스 병합 | MergedDictionaries + ResourceDictionary | MergedDictionaries + ResourceInclude |
| 의존성 속성 | DependencyProperty                      | StyledProperty, DirectProperty       |

### 6.6 Dependency Injection 및 GenericHost 사용

AvaloniaUI에서도 WPF와 동일하게 GenericHost 패턴 적용:

```csharp
// App.axaml.cs
namespace MyApp;

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public partial class App : Application
{
    private IHost? _host;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // GenericHost 생성 및 서비스 등록
        // Create GenericHost and register services
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                // Services 등록
                // Register services
                services.AddSingleton<IUserRepository, UserRepository>();
                services.AddSingleton<IUserService, UserService>();
                services.AddTransient<IDialogService, DialogService>();

                // ViewModels 등록
                // Register ViewModels
                services.AddTransient<MainViewModel>();

                // Views 등록
                // Register Views
                services.AddSingleton<MainWindow>();
            })
            .Build();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = _host.Services.GetRequiredService<MainWindow>();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
```

### 6.7 CollectionView를 사용한 MVVM 패턴

**⚠️ 중요: AvaloniaUI는 WPF의 CollectionViewSource를 지원하지 않습니다.**

AvaloniaUI에서는 다음 방법들을 사용:

#### 6.7.1 DataGridCollectionView 사용 (권장)

```csharp
// NuGet: Avalonia.Controls.DataGrid
// Service Layer
namespace MyApp.Services;

using Avalonia.Controls;

public sealed class MemberCollectionService
{
    private ObservableCollection<Member> Source { get; } = [];

    // DataGridCollectionView 반환
    // Returns DataGridCollectionView
    public IEnumerable CreateView(Predicate<Member>? filter = null)
    {
        var view = new DataGridCollectionView(Source);

        if (filter is not null)
        {
            view.Filter = item => filter((Member)item);
        }

        return view;
    }

    public void Add(Member item) => Source.Add(item);
    public void Remove(Member? item) { if (item is not null) Source.Remove(item); }
    public void Clear() => Source.Clear();
}
```

#### 6.7.2 ReactiveUI 사용 (대안)

```csharp
// NuGet: ReactiveUI.Avalonia
namespace MyApp.ViewModels;

using ReactiveUI;
using DynamicData;

public sealed class MainViewModel : ReactiveObject
{
    private readonly SourceList<Member> _sourceList = new();
    private readonly ReadOnlyObservableCollection<Member> _members;

    public ReadOnlyObservableCollection<Member> Members => _members;

    public MainViewModel()
    {
        _sourceList
            .Connect()
            .Filter(m => m.IsActive) // 필터링
                                     // Filtering
            .Sort(SortExpressionComparer<Member>.Ascending(m => m.Name)) // 정렬
                                                                          // Sorting
            .Bind(out _members)
            .Subscribe();
    }

    public void AddMember(Member member) => _sourceList.Add(member);
    public void RemoveMember(Member member) => _sourceList.Remove(member);
}
```

### 6.8 참조 어셈블리 규칙

**ViewModel 프로젝트가 참조하면 안 되는 어셈블리:**

- ❌ `Avalonia.Base.dll`
- ❌ `Avalonia.Controls.dll`
- ❌ `Avalonia.Markup.Xaml.dll`

**ViewModel 프로젝트가 참조 가능한 어셈블리:**

- ✅ BCL (Base Class Library) 타입만 사용
- ✅ `System.Collections.IEnumerable`
- ✅ `System.Collections.ObjectModel.ObservableCollection<T>`
- ✅ `System.ComponentModel.INotifyPropertyChanged`
- ✅ `CommunityToolkit.Mvvm` (UI 프레임워크 독립적)

**Service 프로젝트가 참조 가능한 어셈블리:**

- ✅ 모든 AvaloniaUI 관련 어셈블리

### 6.9 필수 NuGet 패키지

```xml
<!-- AvaloniaUI Application -->
<ItemGroup>
  <PackageReference Include="Avalonia" Version="11.0.*" />
  <PackageReference Include="Avalonia.Desktop" Version="11.0.*" />
  <PackageReference Include="Avalonia.Themes.Fluent" Version="11.0.*" />
  <PackageReference Include="CommunityToolkit.Mvvm" Version="8.3.*" />
  <PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="9.0.0" />
  <PackageReference Include="Microsoft.Extensions.Hosting" Version="9.0.0" />
</ItemGroup>

<!-- Optional: DataGrid support -->
<ItemGroup>
  <PackageReference Include="Avalonia.Controls.DataGrid" Version="11.0.*" />
</ItemGroup>

<!-- Optional: ReactiveUI support -->
<ItemGroup>
  <PackageReference Include="ReactiveUI.Avalonia" Version="20.1.*" />
  <PackageReference Include="DynamicData" Version="9.0.*" />
</ItemGroup>
```

### 6.10 AvaloniaUI 공식 문서

- [AvaloniaUI Documentation](https://docs.avaloniaui.net/)
- [Styled Properties](https://docs.avaloniaui.net/docs/guides/custom-controls/defining-properties)
- [Control Themes](https://docs.avaloniaui.net/docs/guides/styles-and-resources/control-themes)
- [MVVM Pattern](https://docs.avaloniaui.net/docs/concepts/the-mvvm-pattern/)
- [Dependency Injection](https://docs.avaloniaui.net/docs/guides/implementation-guides/how-to-use-dependency-injection)

---

## 7. .NET에 관한 질문 답변 지침

### 7.1 이론 및 개념 질문

- **.NET의 이론과 개념에 대한 질문에 대해서는 MicrosoftDocs mcp를 반드시 사용**
- **Low-Level 지식까지 설명**
- **사용한 문서의 유효한 Link를 함께 남길 것**

---

## 8. 코드 예시

### 8.1 최신 C# 문법 활용 예시

```csharp
namespace MyApp.Services;

// Primary constructor와 sealed class
public sealed class UserService(ILogger<UserService> logger, IUserRepository repository)
{
    private readonly ILogger<UserService> _logger = logger;
    private readonly IUserRepository _repository = repository;

    // Collection expressions
    public async Task<List<User>> GetActiveUsersAsync()
    {
        var users = await _repository.GetAllAsync();

        // Pattern matching과 Early return
        if (users is null or { Count: 0 })
            return [];

        return users.Where(u => u.IsActive).ToList();
    }
}
```

### 8.2 WPF MVVM 예시

```csharp
// ViewModels/MainViewModel.cs
namespace MyApp.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

// ViewModel - System.Windows 참조 없음
public sealed partial class MainViewModel : ObservableObject
{
    [ObservableProperty] private string _userName = string.Empty;

    [ObservableProperty] private bool _isLoading;

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        IsLoading = true;

        try
        {
            // 데이터 로드 로직
            // Load data logic
        }
        catch (Exception ex)
        {
            // 오류 처리
            // Error handling
        }
        finally
        {
            IsLoading = false;
        }
    }
}
```

---

## 9. 체크리스트

코드 작성 전 확인사항:

**일반 C# 코딩:**

- [ ] 최신 .NET 버전 사용
- [ ] Context7 MCP로 최신 C# 문법 확인
- [ ] sealed 키워드 적용 가능 여부 확인
- [ ] Primary constructor 적용 가능 여부 확인
- [ ] GlobalUsings.cs 파일 생성
- [ ] File scope namespace 사용
- [ ] Early Return 패턴 적용
- [ ] Pattern Matching 활용
- [ ] Literal string은 const로 정의
- [ ] Span<T> 사용 시 async-await 충돌 확인
- [ ] 한글 메시지와 영문 메시지 병기
- [ ] GenericHost 및 DI(Dependency Injection) 설정
- [ ] Constructor Injection 패턴 사용
- [ ] ServiceProvider 직접 사용 지양 (Service Locator 패턴 금지)

**WPF 프로젝트:**

- [ ] WPF ViewModel에 System.Windows 참조 없음 확인
- [ ] ViewModel 프로젝트에서 WindowsBase.dll 참조 제거
- [ ] ViewModel 프로젝트에서 PresentationFramework.dll 참조 제거
- [ ] ViewModel은 순수 BCL 타입만 사용 (IEnumerable, ObservableCollection 등)
- [ ] Custom Control Library에서 AssemblyInfo.cs를 Properties 폴더로 이동
- [ ] Generic.xaml은 MergedDictionaries 허브로만 사용
- [ ] 각 컨트롤 스타일을 개별 XAML 파일로 분리
- [ ] CollectionView 사용 시 Service Layer 패턴 적용
- [ ] App.xaml.cs에서 GenericHost 설정 및 DI 컨테이너 구성
- [ ] MainWindow 및 ViewModel을 DI 컨테이너에 등록
- [ ] View와 ViewModel을 Constructor Injection으로 연결

**AvaloniaUI 프로젝트:**

- [ ] AvaloniaUI ViewModel에 Avalonia 참조 없음 확인
- [ ] ViewModel 프로젝트에서 Avalonia.Base.dll, Avalonia.Controls.dll 참조 제거
- [ ] ViewModel은 순수 BCL 타입만 사용 (IEnumerable, ObservableCollection 등)
- [ ] CustomControl은 기존 Avalonia 컨트롤 상속 (예: Button, TextBox 등)
- [ ] CustomControl에서 StyledProperty 사용
- [ ] Generic.axaml은 MergedDictionaries 허브로만 사용
- [ ] 각 컨트롤 ControlTheme을 개별 AXAML 파일로 분리
- [ ] CSS Class 기반 스타일 적용 (Classes 속성)
- [ ] Pseudo Classes를 사용한 상태 관리 (:pointerover, :pressed 등)
- [ ] CollectionView 대신 DataGridCollectionView 또는 ReactiveUI 사용
- [ ] App.axaml.cs에서 GenericHost 설정 및 DI 컨테이너 구성
- [ ] MainWindow 및 ViewModel을 DI 컨테이너에 등록
- [ ] View와 ViewModel을 Constructor Injection으로 연결

---

## 10. 주의사항

### ⚠️ 자주 발생하는 실수

**C# 코딩 일반:**

1. **Span<T>와 async-await 함께 사용** - 불가능
2. **불필요한 IReadOnlyCollection<T>, HashSet<T> 사용** - 성능 향상이 명확하지 않은 경우
3. **요청하지 않은 기능까지 미리 구현** - 요청한 것만 구현, 나머지는 제안
4. **ServiceProvider를 직접 주입받아 사용** - Service Locator 패턴(안티패턴), Constructor Injection 사용
5. **서비스 Lifetime 잘못 설정** - Singleton/Scoped/Transient 적절히 선택

**WPF/MVVM 관련:** 6. **WPF ViewModel에 System.Windows 네임스페이스 참조** - MVVM 위반 7. **ViewModel 프로젝트에서 WindowsBase.dll 참조** - ICollectionView 사용하려다 WPF 의존성 발생 8. **ViewModel 프로젝트에서 PresentationFramework.dll 직접 참조** - CollectionViewSource 사용 불가 9. **ViewModel에서 ICollectionView 타입 사용** - IEnumerable로 받아야 함 10. **CollectionView 없이 여러 View에서 개별 컬렉션 관리** - 데이터 동기화 문제 발생 11. **Generic.xaml에 직접 스타일 작성** - 개별 XAML 파일로 분리하고 MergedDictionaries로 병합 12. **App.xaml.cs에서 GenericHost 설정 누락** - DI 컨테이너 구성 필요

**AvaloniaUI/MVVM 관련:** 13. **AvaloniaUI ViewModel에 Avalonia 네임스페이스 참조** - MVVM 위반 14. **ViewModel 프로젝트에서 Avalonia.Base.dll, Avalonia.Controls.dll 참조** - AvaloniaUI 의존성 발생 15. **CustomControl을 TemplatedControl에서 직접 상속** - Button, TextBox 등 기존 컨트롤 상속 필요 16. **WPF의 DependencyProperty를 AvaloniaUI에서 그대로 사용** - StyledProperty 또는 DirectProperty 사용 필요 17. **WPF의 Trigger를 AvaloniaUI에서 그대로 사용** - Pseudo Classes와 Style Selector 사용 필요 18. **WPF의 CollectionViewSource를 AvaloniaUI에서 사용** - DataGridCollectionView 또는 ReactiveUI 사용 필요 19. **Generic.axaml에 직접 ControlTheme 작성** - 개별 AXAML 파일로 분리하고 ResourceInclude로 병합 20. **App.axaml.cs에서 GenericHost 설정 누락** - DI 컨테이너 구성 필요

**MVVM 계층 분리 원칙:**

**WPF:**

- ViewModel: 모든 WPF 어셈블리 참조 불가 (순수 BCL만 사용)
- ViewModel: `WindowsBase.dll` 참조 불가 (ICollectionView 포함)
- ViewModel: `PresentationFramework.dll` 참조 불가 (CollectionViewSource 포함)
- Service Layer: `WindowsBase.dll`, `PresentationFramework.dll` 참조 가능
- View Layer: 모든 WPF 어셈블리 참조 가능

**AvaloniaUI:**

- ViewModel: 모든 AvaloniaUI 어셈블리 참조 불가 (순수 BCL만 사용)
- ViewModel: `Avalonia.Base.dll` 참조 불가
- ViewModel: `Avalonia.Controls.dll` 참조 불가
- Service Layer: 모든 AvaloniaUI 어셈블리 참조 가능
- View Layer: 모든 AvaloniaUI 어셈블리 참조 가능

---

## 11. 문서 업데이트

이 문서는 .NET과 C# 생태계의 발전에 따라 지속적으로 업데이트됩니다.

**최종 업데이트:** 2025-11-20
