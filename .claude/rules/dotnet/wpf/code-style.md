# WPF Project 코드 생성 지침

- UI를 커스터마이징해서 사용할 때는 사용자 지정 컨트롤 프로젝트(WPF Custom Control Library Project)를 솔루션에 추가해서 정의할 것.
- Converter나 WPF UI Service Layer의 개발은 WPF Class Library 프로젝트를 사용할 것.
- CommunityToolkit.Mvmm Nuget Package를 이용할 것.

## 1. Generic Host를 이용하여 Dependency Injection 구현

- 기본적으로는 AddSingleton()만 사용.

### 1.1 App.xaml.cs

```csharp
// App.xaml.cs
namespace MyApp;

using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public partial class App : Application
{
    private readonly IHost _host;

    public App()
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
                services.AddTransient<SettingsViewModel>();

                // Views 등록
                // Register Views
                services.AddSingleton<MainWindow>();
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();

        // MainWindow를 ServiceProvider로부터 가져오기
        // Get MainWindow from ServiceProvider
        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        using (_host)
        {
            await _host.StopAsync();
        }

        base.OnExit(e);
    }
}
```

## 2. WPF 솔루션 및 프로젝트 구조

### 2.1. 솔루션 구조 원칙

**솔루션 이름은 애플리케이션 이름**

- 예시: `GameDataTool` 솔루션 = 실행 가능한 .NET Assembly 이름

### 2.2. 프로젝트 명명 규칙

```
SolutionName/
├── SolutionName.Abstractions      // .NET Class Library (Interface, abstract class 등 추상 타입)
├── SolutionName.Core              // .NET Class Library (비즈니스 로직, 순수 C#)
├── SolutionName.Core.Tests        // xUnit Test Project
├── SolutionName.ViewModels        // .NET Class Library (MVVM ViewModel)
├── SolutionName.WpfServices       // WPF Class Library (WPF 관련 서비스)
├── SolutionName.WpfLib            // WPF Class Library (재사용 가능한 WPF 컴포넌트)
├── SolutionName.WpfApp            // WPF Application Project (실행 진입점)
├── SolutionName.UI                // WPF Custom Control Library (커스텀 컨트롤)
└── [Solution Folders]
    ├── SolutionName/              // 주요 프로젝트 그룹
    └── Common/                    // 범용 프로젝트 그룹
```

**프로젝트 타입별 명명:**

- `.Abstractions`: .NET Class Library - Interface, abstract class 등 추상 타입 정의 (Inversion of Control)
- `.Core`: .NET Class Library - 비즈니스 로직, 데이터 모델, 서비스 (UI 프레임워크 독립)
- `.Core.Tests`: xUnit/NUnit/MSTest Test Project
- `.ViewModels`: .NET Class Library - MVVM ViewModel (UI 프레임워크 독립)
- `.WpfServices`: WPF Class Library - WPF 관련 서비스 (DialogService, NavigationService 등)
- `.WpfLib`: WPF Class Library - 재사용 가능한 WPF UserControl, Window, Converter, Behavior, AttachedProperty
- `.WpfApp`: WPF Application Project - 실행 진입점, App.xaml
- `.UI`: WPF Custom Control Library - ResourceDictionary 기반 커스텀 컨트롤

**프로젝트 의존성 계층:**

```
SolutionName.WpfApp
    ↓ 참조
SolutionName.Core
    ↓ 참조
SolutionName.Abstractions (최상단 - 다른 프로젝트에 의존하지 않음)
```

**Abstractions 레이어의 역할:**

- 모든 Interface와 abstract class 보관
- 구체 타입을 직접 참조하지 않고 추상 타입으로 의존성 역전 (Dependency Inversion Principle)
- 런타임에 DI 컨테이너를 통해 실제 구현체 주입
- 테스트 시 Mock 객체로 교체 가능

### 2.3. Solution Folder 활용

**범용 기능 분리:**

```
Solution 'GameDataTool'
├── Solution Folder: GameDataTool
│   ├── GameDataTool.Abstractions
│   ├── GameDataTool.Core
│   ├── GameDataTool.Core.Tests
│   ├── GameDataTool.ViewModels
│   ├── GameDataTool.WpfServices
│   ├── GameDataTool.WpfLib
│   ├── GameDataTool.WpfApp
│   └── GameDataTool.UI
└── Solution Folder: Common
    ├── Common.Utilities
    ├── Common.Logging
    └── Common.Configuration
```

**규칙:**

- 애플리케이션 고유 기능: `SolutionName` Solution Folder에 배치
- 범용/재사용 기능: `Common` Solution Folder에 배치
- Solution Folder는 가상 폴더 (실제 파일 시스템과 무관)

### 2.4. 실제 예시

```
Solution 'GameDataTool'
├── GameDataTool (Solution Folder)
│   ├── GameDataTool.Abstractions
│   │   ├── Services/
│   │   │   ├── IUserService.cs
│   │   │   └── IDataService.cs
│   │   └── Repositories/
│   │       ├── IUserRepository.cs
│   │       └── IDataRepository.cs
│   ├── GameDataTool.Core
│   │   ├── Models/
│   │   ├── Services/
│   │   │   ├── UserService.cs
│   │   │   └── DataService.cs
│   │   └── Repositories/
│   │       ├── UserRepository.cs
│   │       └── DataRepository.cs
│   ├── GameDataTool.Core.Tests
│   │   └── Services/
│   ├── GameDataTool.ViewModels
│   │   ├── MainViewModel.cs
│   │   ├── HomeViewModel.cs
│   │   └── SettingsViewModel.cs
│   ├── GameDataTool.WpfServices
│   │   ├── DialogService.cs
│   │   ├── NavigationService.cs
│   │   └── WindowService.cs
│   ├── GameDataTool.WpfLib
│   │   ├── Controls/
│   │   ├── Converters/
│   │   ├── Behaviors/
│   │   └── AttachedProperties/
│   ├── GameDataTool.WpfApp
│   │   ├── App.xaml
│   │   └── Views/
│   └── GameDataTool.UI
│       ├── Themes/
│       └── CustomControls/
└── Common (Solution Folder)
    ├── Common.Utilities
    ├── Common.Logging
    └── Common.Configuration
```

---

## 3. MVVM 패턴

- ViewModel 클래스에 UI 프레임워크 의존성 금지
  - ViewModel class에 `System.Windows`로 시작하는 클래스들이 참조되지 않는 것을 의미
  - 단, Custom Control 프로젝트 내부에서 CustomControl을 위한 ViewModel을 정의할 때는 ViewModel에서도 System.Windows Type 사용 가능.
- MVVM 제약은 ViewModel에만 적용
  - ViewModel이라는 접미사가 있는 Class들에만 위 제한 규칙 적용
  - Converter, Service, Manager, Locator 같은 클래스들은 UI 프레임워크 참조 가능

### 3.1. MVVM 프레임워크

- MVVM을 구조로 잡을 때는 CommunityToolkit.Mvvm을 기본으로 이용.

#### 3.1.1. CommunityToolkit.Mvvm Attribute 스타일 가이드

##### 3.1.1.1. ObservableProperty Attribute 작성 규칙

```csharp
// ✅ 좋은 예: Attribute가 1개일 경우 inline으로 작성
// Good: Single attribute written inline
[ObservableProperty] private string _userName = string.Empty;
[ObservableProperty] private int _age;
[ObservableProperty] private bool _isActive;

// ✅ 좋은 예: Attribute가 여러 개일 경우, ObservableProperty는 항상 inline
// Good: Multiple attributes, ObservableProperty always inline
[NotifyPropertyChangedRecipients]
[NotifyCanExecuteChangedFor(nameof(SaveCommand))]
[ObservableProperty] private string _email = string.Empty;

[NotifyDataErrorInfo]
[Required(ErrorMessage = "이름은 필수입니다.")]
[MinLength(2, ErrorMessage = "이름은 최소 2글자 이상이어야 합니다.")]
[ObservableProperty] private string _name = string.Empty;

[NotifyPropertyChangedRecipients]
[NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
[NotifyCanExecuteChangedFor(nameof(UpdateCommand))]
[ObservableProperty] private User? _selectedUser;

// ❌ 나쁜 예: ObservableProperty를 별도 줄에 작성
// Bad: ObservableProperty on separate line
[NotifyPropertyChangedRecipients]
[NotifyCanExecuteChangedFor(nameof(SaveCommand))]
[ObservableProperty]
private string _email = string.Empty;
```

**실제 ViewModel 예시:**

```csharp
namespace MyApp.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public sealed partial class UserViewModel : ObservableObject
{
    // 단일 Attribute
    // Single attribute
    [ObservableProperty] private string _firstName = string.Empty;
    [ObservableProperty] private string _lastName = string.Empty;
    [ObservableProperty] private int _age;

    // 여러 Attribute - ObservableProperty는 inline
    // Multiple attributes - ObservableProperty inline
    [NotifyPropertyChangedRecipients]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    [ObservableProperty] private string _email = string.Empty;

    [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
    [NotifyCanExecuteChangedFor(nameof(UpdateCommand))]
    [ObservableProperty] private User? _selectedUser;

    [RelayCommand(CanExecute = nameof(CanSave))]
    private async Task SaveAsync()
    {
        // 저장 로직
        // Save logic
    }

    private bool CanSave() => !string.IsNullOrWhiteSpace(Email);
}
```

**핵심 규칙:**

- CommunityToolkit.Mvvm 관련 Attribute가 1개: `[ObservableProperty]` inline으로 필드 선언 바로 앞에 작성
- CommunityToolkit.Mvvm 관련 Attribute가 n개: 다른 Attribute들은 각 줄에 작성하되, `[ObservableProperty]`는 항상 마지막에 inline으로 작성
- 목적: 코드 가독성 향상 및 일관된 코딩 스타일 유지

---

## 4. XAML 코드 작성

- **XAML 코드를 생성할 때는 CustomControl을 사용하여 ResourceDictionary를 통한 Stand-Alone Control Style Resource를 사용**
- 목적: StaticResource 불러올 때 시점 고정 및 스타일 의존성 최소화

### 4.1. WPF Custom Control Library 프로젝트 구조

**프로젝트 생성 시 기본 구조:**

```
YourProject/
├── Dependencies/
├── Themes/
│   └── Generic.xaml
├── AssemblyInfo.cs
└── CustomControl1.cs
```

**권장 프로젝트 구조로 재구성:**

```
YourProject/
├── Dependencies/
├── Properties/
│   └── AssemblyInfo.cs          ← 이동
├── Themes/
│   ├── Generic.xaml             ← MergedDictionaries 허브로 사용
│   ├── CustomButton.xaml        ← 개별 컨트롤 스타일
│   └── CustomTextBox.xaml       ← 개별 컨트롤 스타일
├── CustomButton.cs
└── CustomTextBox.cs
```

**단계별 설정:**

1. **Properties 폴더 생성 및 AssemblyInfo.cs 이동**

   - 프로젝트에 Properties 폴더 생성
   - AssemblyInfo.cs를 Properties 폴더로 이동

2. **Generic.xaml 구성 - MergedDictionaries 허브로 사용**

Generic.xaml은 직접 스타일을 정의하지 않고, 개별 ResourceDictionary들을 병합하는 역할만 수행:

```xml
<!-- Themes/Generic.xaml -->
<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation">
    <ResourceDictionary.MergedDictionaries>
        <ResourceDictionary Source="/YourProjectName;component/Themes/CustomButton.xaml" />
        <ResourceDictionary Source="/YourProjectName;component/Themes/CustomTextBox.xaml" />
    </ResourceDictionary.MergedDictionaries>
</ResourceDictionary>
```

3. **개별 컨트롤 스타일 정의**

각 컨트롤마다 독립적인 XAML 파일에 스타일 정의:

```xml
<!-- Themes/CustomButton.xaml -->
<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                    xmlns:local="clr-namespace:YourNamespace">

    <!-- 컨트롤 전용 리소스 정의 -->
    <SolidColorBrush x:Key="ButtonBackground" Color="#FF2D5460" />
    <SolidColorBrush x:Key="ButtonBackground_MouseOver" Color="#FF1D5460" />
    <SolidColorBrush x:Key="ButtonForeground" Color="#FFFFFFFF" />

    <!-- 컨트롤 스타일 정의 -->
    <Style TargetType="{x:Type local:CustomButton}">
        <Setter Property="Background" Value="{StaticResource ButtonBackground}" />
        <Setter Property="Foreground" Value="{StaticResource ButtonForeground}" />
        <Setter Property="Template">
            <Setter.Value>
                <ControlTemplate TargetType="{x:Type local:CustomButton}">
                    <Border Background="{TemplateBinding Background}"
                            BorderBrush="{TemplateBinding BorderBrush}"
                            BorderThickness="{TemplateBinding BorderThickness}">
                        <ContentPresenter HorizontalAlignment="Center"
                                        VerticalAlignment="Center"/>
                    </Border>
                    <ControlTemplate.Triggers>
                        <Trigger Property="IsMouseOver" Value="True">
                            <Setter Property="Background"
                                    Value="{StaticResource ButtonBackground_MouseOver}" />
                        </Trigger>
                    </ControlTemplate.Triggers>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>
</ResourceDictionary>
```

**실제 프로젝트 예시:**

```xml
<!-- Themes/Generic.xaml -->
<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation">
    <ResourceDictionary.MergedDictionaries>
        <ResourceDictionary Source="/GameDataTool.Controls.Popup;component/Themes/GdtBranchSelectionPopup.xaml" />
    </ResourceDictionary.MergedDictionaries>
</ResourceDictionary>
```

```xml
<!-- Themes/GdtBranchSelectionPopup.xaml -->
<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                    xmlns:local="clr-namespace:GameDataTool.Controls.Popup"
                    xmlns:ui="clr-namespace:GameDataTool.Controls.GdtCore.UI;assembly=GameDataTool.Controls.GdtCore"
                    xmlns:unit="clr-namespace:GameDataTool.Controls.GdtUnits;assembly=GameDataTool.Controls.GdtUnits">

    <SolidColorBrush x:Key="ApplyButtonBackground" Color="{DynamicResource Theme_PopupConfirmButtonColor}" />
    <SolidColorBrush x:Key="ApplyButtonBackground_MouseOver" Color="#FF1D5460" />
    <SolidColorBrush x:Key="ApplyButtonForeground" Color="{DynamicResource Theme_PopupConfirmButtonTextColor}" />
    <SolidColorBrush x:Key="CancelButtonBackground" Color="#FFE8EBED" />
    <SolidColorBrush x:Key="CancelButtonBackground_MouseOver" Color="#FFC9CDD2" />
    <SolidColorBrush x:Key="CancelButtonForeground" Color="#FF323334" />

    <Style TargetType="{x:Type local:GdtBranchSelectionPopup}">
        <Setter Property="Template">
            <Setter.Value>
                <ControlTemplate TargetType="{x:Type local:GdtBranchSelectionPopup}">
                    <Border Width="{DynamicResource BranchSelectionPopupWidthSize}"
                            Height="{DynamicResource BranchSelectionPopupHeightSize}"
                            Background="{TemplateBinding Background}"
                            BorderBrush="{TemplateBinding BorderBrush}"
                            BorderThickness="{TemplateBinding BorderThickness}">
                        <Grid>
                            <!-- 컨트롤 내용 -->
                        </Grid>
                    </Border>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>
</ResourceDictionary>
```

**장점:**

- 각 컨트롤의 스타일이 독립적인 파일로 분리되어 관리 용이
- Generic.xaml은 단순히 병합 역할만 수행하여 구조가 명확함
- StaticResource 참조 시점이 명확하고 의존성 최소화
- 팀 작업 시 파일 단위로 작업 분리 가능

### 4.2. CollectionView를 사용한 MVVM 패턴

#### 4.2.1. 문제 상황

하나의 원본 컬렉션을 여러 View에서 각각 다른 조건으로 필터링하여 사용하면서도 MVVM 원칙을 준수해야 하는 경우

#### 4.2.2. 핵심 원칙

- ViewModel은 WPF 관련 어셈블리를 참조하면 안 됨 (MVVM 위반)
- Service Layer를 통해 `CollectionViewSource` 접근을 캡슐화
- ViewModel은 `IEnumerable` 또는 순수 BCL 타입만 사용

#### 4.2.3. 아키텍처 계층 구조

```
View (XAML)
    ↓ DataBinding
ViewModel Layer (IEnumerable 사용, WPF 어셈블리 참조 X)
    ↓ IEnumerable 인터페이스
Service Layer (CollectionViewSource 직접 사용)
    ↓
Data Layer (ObservableCollection<T>)
```

#### 4.2.4. 구현 패턴

**1. Service Layer (CollectionViewFactory/Store)**

```csharp
// Services/MemberCollectionService.cs
// 이 클래스는 PresentationFramework 참조 가능
// This class can reference PresentationFramework
namespace MyApp.Services;

public sealed class MemberCollectionService
{
    private ObservableCollection<Member> Source { get; } = [];

    // Factory Method: 필터링된 뷰 생성
    // IEnumerable로 반환하여 ViewModel이 WPF 타입을 모르게 함
    // Factory Method: Create filtered view
    // Returns IEnumerable so ViewModel doesn't know WPF types
    public IEnumerable CreateView(Predicate<object>? filter = null)
    {
        var viewSource = new CollectionViewSource { Source = Source };
        var view = viewSource.View;

        if (filter is not null)
        {
            view.Filter = filter;
        }

        return view; // ICollectionView는 IEnumerable을 상속
                     // ICollectionView inherits IEnumerable
    }

    public void Add(Member item) => Source.Add(item);

    public void Remove(Member? item)
    {
        if (item is not null)
            Source.Remove(item);
    }

    public void Clear() => Source.Clear();
}
```

**2. ViewModel Layer**

```csharp
// ViewModel은 IEnumerable만 사용 (순수 BCL 타입)
// ViewModel uses only IEnumerable (pure BCL type)
namespace MyApp.ViewModels;

public abstract class BaseFilteredViewModel
{
    public IEnumerable? Members { get; }

    protected BaseFilteredViewModel(Predicate<object> filter)
    {
        // Service에서 IEnumerable로 받음
        // Receives IEnumerable from Service
        Members = memberService.CreateView(filter);
    }
}

// 각 필터링된 ViewModel
// Each filtered ViewModel
public sealed class WalkerViewModel : BaseFilteredViewModel
{
    public WalkerViewModel()
        : base(item => (item as Member)?.Type == DeviceTypes.Walker)
    {
    }
}

// 또는 직접 타입 캐스팅하여 사용
// Or use with direct type casting
public sealed class AppViewModel : ObservableObject
{
    public IEnumerable? Members { get; }

    public AppViewModel()
    {
        Members = memberService.CreateView();
    }

    // 필요시 LINQ로 컬렉션 조작
    // Manipulate collection with LINQ when needed
    private void ProcessMembers()
    {
        var memberList = Members?.Cast<Member>().ToList();
        // 처리 로직...
        // Processing logic...
    }
}
```

**3. View에서 CollectionView 초기화 (대안 방법)**

이 방법은 ViewModel이 완전히 WPF로부터 독립적이지만, View의 Code-Behind에서 초기화 로직이 필요합니다.

```csharp
// ViewModel - 순수 BCL만 사용
// ViewModel - Uses pure BCL only
namespace MyApp.ViewModels;

public sealed partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<Person> people = [];

    private ICollectionView? _peopleView;

    // View에서 주입받음
    // Injected from View
    public void InitializeCollectionView(ICollectionView collectionView)
    {
        _peopleView = collectionView;
        _peopleView.Filter = FilterPerson;
    }

    private bool FilterPerson(object item)
    {
        // 필터링 로직
        // Filtering logic
        return true;
    }
}

// MainWindow.xaml.cs - View의 Code-Behind
// MainWindow.xaml.cs - View's Code-Behind
namespace MyApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var viewModel = new MainViewModel();
        DataContext = viewModel;

        // View 레이어에서 CollectionViewSource 생성
        // Create CollectionViewSource in View layer
        ICollectionView collectionView =
            CollectionViewSource.GetDefaultView(viewModel.People);

        // ViewModel에 주입
        // Inject into ViewModel
        viewModel.InitializeCollectionView(collectionView);
    }
}
```

**주의**: 이 방법은 ViewModel이 `ICollectionView` 타입을 알게 되므로, WindowsBase.dll 참조가 필요합니다. 완전한 독립을 원한다면 Service Layer 방식을 사용하세요.

#### 4.2.5. 프로젝트 구조 (엄격한 MVVM)

```
MyApp.Models/              // 순수 C# 모델, BCL만 사용
                          // Pure C# models, BCL only

MyApp.ViewModels/         // 순수 C# ViewModel
                          // Pure C# ViewModel
                          // WPF 어셈블리 참조 X
                          // No WPF assembly references
                          // IEnumerable만 사용
                          // Uses IEnumerable only

MyApp.Services/           // PresentationFramework 참조 O
                          // PresentationFramework reference: YES
                          // WindowsBase 참조 O
                          // WindowsBase reference: YES
                          // CollectionViewSource 사용
                          // Uses CollectionViewSource

MyApp.Views/              // 모든 WPF 어셈블리 참조
                          // References all WPF assemblies
```

#### 4.2.6. 참조 어셈블리 규칙

**ViewModel 프로젝트가 참조하면 안 되는 어셈블리:**

- ❌ `WindowsBase.dll` (ICollectionView 포함)
- ❌ `PresentationFramework.dll` (CollectionViewSource 포함)
- ❌ `PresentationCore.dll`

**ViewModel 프로젝트가 참조 가능한 어셈블리:**

- ✅ BCL (Base Class Library) 타입만 사용
- ✅ `System.Collections.IEnumerable`
- ✅ `System.Collections.ObjectModel.ObservableCollection<T>`
- ✅ `System.ComponentModel.INotifyPropertyChanged`

**Service 프로젝트가 참조 가능한 어셈블리:**

- ✅ `WindowsBase.dll`
- ✅ `PresentationFramework.dll`
- ✅ 모든 WPF 관련 어셈블리

#### 4.2.7. 핵심 장점

1. **단일 원본 유지**: 모든 View가 하나의 `ObservableCollection` 공유
2. **자동 동기화**: 원본 변경 시 모든 필터링된 View에 자동 반영
3. **MVVM 준수**: ViewModel이 UI 프레임워크에 완전 독립적
4. **재사용성**: 다양한 필터 조건으로 여러 View 생성 가능
5. **테스트 용이성**: ViewModel을 WPF 없이 단위 테스트 가능

#### 4.2.8. Service Layer에서 CollectionView 기능 활용

Service Layer에서 CollectionView의 다양한 기능을 캡슐화하여 제공할 수 있습니다.

```csharp
// Services/MemberCollectionService.cs
namespace MyApp.Services;

public sealed class MemberCollectionService
{
    private ObservableCollection<Member> Source { get; } = [];

    public IEnumerable CreateView(Predicate<object>? filter = null)
    {
        var viewSource = new CollectionViewSource { Source = Source };
        var view = viewSource.View;

        if (filter is not null)
        {
            view.Filter = filter;
        }

        return view;
    }

    // 정렬된 뷰 생성
    // Create sorted view
    public IEnumerable CreateSortedView(
        string propertyName,
        ListSortDirection direction = ListSortDirection.Ascending)
    {
        var viewSource = new CollectionViewSource { Source = Source };
        var view = viewSource.View;

        view.SortDescriptions.Add(
            new SortDescription(propertyName, direction)
        );

        return view;
    }

    // 그룹화된 뷰 생성
    // Create grouped view
    public IEnumerable CreateGroupedView(string groupPropertyName)
    {
        var viewSource = new CollectionViewSource { Source = Source };
        var view = viewSource.View;

        view.GroupDescriptions.Add(
            new PropertyGroupDescription(groupPropertyName)
        );

        return view;
    }

    public void Add(Member item) => Source.Add(item);
    public void Remove(Member? item) { if (item is not null) Source.Remove(item); }
    public void Clear() => Source.Clear();
}
```

#### 4.2.9. DI/IoC 적용 시

```csharp
// Interface 정의 (순수 BCL 타입만 사용)
// Interface definition (uses pure BCL types only)
namespace MyApp.Services;

public interface IMemberCollectionService
{
    IEnumerable CreateView(Predicate<object>? filter = null);
    void Add(Member member);
    void Remove(Member? member);
    void Clear();
}

// DI 컨테이너 등록
// DI container registration
services.AddSingleton<IMemberCollectionService, MemberCollectionService>();

// ViewModel 생성자 주입
// ViewModel constructor injection
namespace MyApp.ViewModels;

public sealed partial class AppViewModel(IMemberCollectionService memberService)
    : ObservableObject
{
    public IEnumerable? Members { get; } = memberService.CreateView();
}
```

#### 4.2.10. 실무 적용 시 권장사항

1. **프로젝트 분리**: ViewModel과 Service를 별도 프로젝트로 분리
2. **Interface 활용**: Service는 인터페이스로 정의하여 테스트 용이성 확보
3. **Singleton 또는 DI**: Service는 Singleton 또는 DI 컨테이너로 관리
4. **명명 규칙**:
   - `MemberCollectionService` (Service 접미사)
   - `MemberViewFactory` (Factory 접미사)
   - `MemberStore` (Store 접미사)

#### 4.2.11. Microsoft 공식 문서

- [CollectionViewSource Class](https://learn.microsoft.com/en-us/dotnet/api/system.windows.data.collectionviewsource?view=windowsdesktop-10.0)
- [Data Binding Overview - Collection Views](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/data/#binding-to-collections)
- [How to: Filter Data in a View](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/data/how-to-filter-data-in-a-view)
- [Service Layer Pattern](https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions-1/models-data/validating-with-a-service-layer-cs#creating-a-service-layer)

## 5. Popup 컨트롤 사용 시 주의사항

- WPF에서 Popup 컨트롤은 WPF Application에 포커스가 있을 때만 정상적으로 동작합니다. 다른 애플리케이션으로 포커스가 이동한 상태에서는 Popup이 제대로 표시되지 않거나 동작하지 않을 수 있습니다.

### 5.1. 포커스 관리 패턴

Window나 UserControl에서 Popup 컨트롤을 사용할 경우, **PreviewMouseDown 이벤트를 통해 포커스를 강제로 가져와야 합니다.**

**필수 구현 패턴:**

```csharp
// MainWindow.xaml.cs
namespace MyApp;

using System.Windows;
using System.Windows.Input;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // Popup이 정상 동작하도록 포커스 관리
        // Manage focus to ensure Popup works correctly
        PreviewMouseDown += MainWindow_PreviewMouseDown;
    }

    private void MainWindow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        // 키보드 포커스가 없으면 윈도우 활성화
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
```

**UserControl에서도 동일하게 적용:**

```csharp
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
```

### 5.2. 핵심 원칙

- **Popup 동작 조건**: WPF Application에 포커스가 있을 때만 동작
- **PreviewMouseDown 이벤트**: 마우스 클릭 시 포커스 상태 체크
- **IsKeyboardFocused 체크**: 키보드 포커스 여부 확인
- **Activate() 호출**: 포커스가 없으면 윈도우 활성화하여 포커스 복원
- **UserControl 경우**: `Window.GetWindow(this)?.Activate()`로 부모 윈도우 활성화

### 5.3. 왜 필요한가?

1. **다른 앱으로 포커스 이동**: 사용자가 다른 애플리케이션을 클릭했다가 다시 돌아올 때
2. **백그라운드 실행**: WPF 앱이 백그라운드에 있을 때 Popup 동작 보장
3. **사용자 경험**: Popup이 항상 예상대로 동작하도록 보장

**⚠️ 주의사항:**

- Popup을 사용하는 모든 Window 및 UserControl에 이 패턴을 적용해야 함
- PreviewMouseDown은 터널링 이벤트이므로 자식 컨트롤보다 먼저 처리됨
- MVVM 패턴에서는 Code-Behind에서 처리 (View 레이어의 포커스 관리 로직)

### 5.4. 참고 자료

- [WPF Popup 포커스 이슈 - .NET Dev 포럼](https://forum.dotnetdev.kr/t/wpf-popup/8296)

## 6. Mappings.xaml 작성

### 6.1. DataTemplate을 사용한 View-ViewModel 자동 매핑

- WPF에서 DataTemplate을 사용하면 ViewModel 타입과 View를 자동으로 매핑할 수 있습니다. 이 패턴은 네비게이션 시나리오나 동적 콘텐츠 표시에 매우 유용합니다.

### 6.2. 핵심 개념

ContentControl의 Content에 ViewModel 인스턴스를 바인딩하면, WPF가 자동으로 해당 ViewModel 타입에 맞는 DataTemplate을 찾아서 View를 렌더링합니다.

이 패턴의 핵심:

1. `Mappings.xaml`에 ViewModel 타입별 DataTemplate 정의
2. `ContentControl.Content`에 ViewModel 인스턴스 바인딩
3. WPF가 자동으로 타입을 매칭하여 해당 View 렌더링

### 6.3. Mappings.xaml 패턴

**Mappings.xaml - ViewModel과 View 매핑 정의:**

```xml
<!-- Mappings.xaml -->
<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                    xmlns:viewmodels="clr-namespace:WpfDataTemplateSample.ViewModels"
                    xmlns:views="clr-namespace:WpfDataTemplateSample.Views">

    <!--  ViewModel과 View를 자동으로 매핑하는 DataTemplate 정의  -->
    <!--  ContentControl의 Content에 ViewModel을 설정하면 자동으로 해당 View가 렌더링됨  -->

    <DataTemplate DataType="{x:Type viewmodels:HomeViewModel}">
        <views:HomeView />
    </DataTemplate>

    <DataTemplate DataType="{x:Type viewmodels:SettingsViewModel}">
        <views:SettingsView />
    </DataTemplate>

    <DataTemplate DataType="{x:Type viewmodels:UserProfileViewModel}">
        <views:UserProfileView />
    </DataTemplate>

</ResourceDictionary>
```

**App.xaml - Mappings.xaml을 Application Resources에 병합:**

```xml
<!-- App.xaml -->
<Application x:Class="WpfDataTemplateSample.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             StartupUri="MainWindow.xaml">
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="Mappings.xaml" />
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</Application>
```

### 6.4. 네비게이션 패턴 구현

**MainWindowViewModel - CurrentViewModel 속성으로 화면 전환:**

```csharp
// ViewModels/MainWindowViewModel.cs
namespace WpfDataTemplateSample.ViewModels;

public sealed partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty] private object? _currentViewModel;

    public MainWindowViewModel()
    {
        CurrentViewModel = new HomeViewModel();
    }

    [RelayCommand]
    private void NavigateToHome()
    {
        CurrentViewModel = new HomeViewModel();
    }

    [RelayCommand]
    private void NavigateToSettings()
    {
        CurrentViewModel = new SettingsViewModel();
    }

    [RelayCommand]
    private void NavigateToUserProfile()
    {
        CurrentViewModel = new UserProfileViewModel();
    }
}
```

**MainWindow.xaml - ContentControl로 동적 콘텐츠 표시:**

```xml
<!-- MainWindow.xaml -->
<Window x:Class="WpfDataTemplateSample.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:viewmodels="clr-namespace:WpfDataTemplateSample.ViewModels"
        Title="DataTemplate 자동 매핑 Sample"
        Width="800"
        Height="500"
        WindowStartupLocation="CenterScreen">

    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto" />
            <RowDefinition Height="*" />
        </Grid.RowDefinitions>

        <!--  Navigation 버튼 영역  -->
        <StackPanel Grid.Row="0"
                    Margin="10"
                    HorizontalAlignment="Center"
                    Orientation="Horizontal">
            <Button Width="100"
                    Height="35"
                    Margin="5"
                    Command="{Binding NavigateToHomeCommand}"
                    Content="Home" />
            <Button Width="100"
                    Height="35"
                    Margin="5"
                    Command="{Binding NavigateToSettingsCommand}"
                    Content="Settings" />
            <Button Width="100"
                    Height="35"
                    Margin="5"
                    Command="{Binding NavigateToUserProfileCommand}"
                    Content="User Profile" />
        </StackPanel>

        <!--  ContentControl에 ViewModel을 바인딩하면 Mappings.xaml의 DataTemplate에 의해 자동으로 View가 렌더링됨  -->
        <!--  핵심: Content에 ViewModel 타입을 설정하면 자동으로 해당 View가 표시됨  -->
        <Border Grid.Row="1"
                Margin="10"
                BorderBrush="Gray"
                BorderThickness="1">
            <ContentControl Content="{Binding CurrentViewModel}" />
        </Border>

    </Grid>
</Window>
```

```csharp
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
```

#### 5.8.4 ViewModel 및 View 구현 예시

**HomeViewModel:**

```csharp
// ViewModels/HomeViewModel.cs
namespace WpfDataTemplateSample.ViewModels;

public sealed partial class HomeViewModel : ObservableObject
{
    [ObservableProperty] private string _welcomeMessage = "Welcome to Home Page!";
    [ObservableProperty] private string _description = "This is the home page content. DataTemplate automatically maps this ViewModel to HomeView.";
}
```

**HomeView:**

```xml
<!-- Views/HomeView.xaml -->
<UserControl x:Class="WpfDataTemplateSample.Views.HomeView"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:viewmodels="clr-namespace:WpfDataTemplateSample.ViewModels"
             d:DataContext="{d:DesignInstance Type=viewmodels:HomeViewModel}">

    <Grid Background="#F0F8FF">
        <StackPanel HorizontalAlignment="Center"
                    VerticalAlignment="Center">
            <TextBlock Margin="0,0,0,20"
                       HorizontalAlignment="Center"
                       FontSize="32"
                       FontWeight="Bold"
                       Foreground="#2C3E50"
                       Text="{Binding WelcomeMessage}" />

            <Border MaxWidth="600"
                    Padding="30"
                    Background="White"
                    BorderBrush="#3498DB"
                    BorderThickness="2"
                    CornerRadius="10">
                <TextBlock FontSize="16"
                           Foreground="#34495E"
                           LineHeight="24"
                           Text="{Binding Description}"
                           TextAlignment="Center"
                           TextWrapping="Wrap" />
            </Border>
        </StackPanel>
    </Grid>
</UserControl>
```

```csharp
// Views/HomeView.xaml.cs
namespace WpfDataTemplateSample.Views;

public partial class HomeView : UserControl
{
    public HomeView()
    {
        InitializeComponent();
    }
}
```

#### 5.8.5 핵심 원칙

1. **DataTemplate의 DataType 속성**: ViewModel 타입을 지정하여 자동 매핑
2. **x:Key 없이 정의**: DataType만 지정하면 WPF가 타입으로 자동 검색
3. **ContentControl 사용**: Content 속성에 ViewModel 인스턴스 바인딩
4. **Application Resources에 등록**: Mappings.xaml을 App.xaml에서 MergedDictionaries로 병합
5. **View는 UserControl**: 재사용 가능한 UserControl로 View 정의

#### 5.8.6 장점

1. **View-ViewModel 결합도 감소**: Code-Behind에서 View를 직접 생성하지 않음
2. **선언적 매핑**: XAML에서 명시적으로 매핑 관계 정의
3. **네비게이션 간소화**: ViewModel 인스턴스만 교체하면 자동으로 화면 전환
4. **테스트 용이성**: ViewModel만으로 로직 테스트 가능
5. **디자인 타임 지원**: `d:DataContext`를 통한 디자이너 미리보기 지원

#### 5.8.7 주의사항

**⚠️ 중요:**

- DataTemplate은 `x:Key` 없이 정의해야 자동 매핑 동작
- Mappings.xaml은 반드시 Application.Resources에 병합 필요
- ViewModel 타입은 정확히 일치해야 함 (상속 관계 고려 안 됨)
- ContentControl.Content에는 ViewModel 인스턴스를 바인딩 (타입이 아닌 인스턴스)
- View는 DataContext를 자동으로 받음 (별도 설정 불필요)

#### 5.8.8 프로젝트 구조 예시

```
WpfDataTemplateSample/
├── ViewModels/
│   ├── MainWindowViewModel.cs
│   ├── HomeViewModel.cs
│   ├── SettingsViewModel.cs
│   └── UserProfileViewModel.cs
├── Views/
│   ├── HomeView.xaml
│   ├── HomeView.xaml.cs
│   ├── SettingsView.xaml
│   ├── SettingsView.xaml.cs
│   ├── UserProfileView.xaml
│   └── UserProfileView.xaml.cs
├── App.xaml
├── App.xaml.cs
├── MainWindow.xaml
├── MainWindow.xaml.cs
├── Mappings.xaml              ← DataTemplate 매핑 정의
└── GlobalUsings.cs
```
