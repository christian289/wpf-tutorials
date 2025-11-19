# Claude AI Assistant - WPF & C# 코드 작성 지침

이 문서는 WPF와 C# 프로젝트 개발 시 Claude AI Assistant가 따라야 할 코딩 지침을 정의합니다.

---

## 1. 기본 지침

### 1.1 응답 언어
- **모든 질문에 대해 한글로 답변**
- 영문 번역 요청 시: 최대한 정확하고 오해가 없는 명료한 한글로 번역
- 영문 작문 요청 시: 최대한 정중하고 상냥한 문체로 작성

### 1.2 도구 추천
- 프로그래밍을 할 때 질문자가 필요해 보이는 도구가 있다고 판단하면 도구를 적극 추천하고 기본적인 사용법을 남겨줄 것

### 1.3 지식 검증
- 지식을 열거할 때 정확한 지식인지 답변을 마무리할 때 검증과정을 반드시 거칠 것

---

## 2. Coding 관련 공통 지침

### 2.1 MCP 사용
- **Coding 질문에 대해서는 Context7, Serena mcp를 반드시 사용할 것**

### 2.2 다른 언어 코드 변환
- .NET C#이 아닌 프로그래밍 코드를 질문할 때 다른 언어의 코드 생성 후 C#으로 코드를 변환했을 때는 어떤 뉘앙스의 코드인지 함께 남겨줄 것

### 2.3 ProtoTyping 원칙
- ProtoTyping 관련 코드는 최소한으로 생성
- 코드는 짧고 명료하게 생성
- **추상화를 하지 않을 것**

### 2.4 답변 범위
- **기본 지침 및 질문으로 요청한 것에 대해서만 답변하고 알아서 판단하여 미리 답변하지 않을 것**
- **요청하지 않은 것에 대해서는 코딩하지 말고 추가 제안들로서 제안만 할 것**

### 2.5 한글 문장과 영문 병기
- Log, Comment, Exception Message 등의 한글 문장을 작성할 때는 영작한 문장도 함께 작성할 것
- 코드 페이지 전체를 적는 것이 아니라 한글 Log, 한글 Comment, 한글 Exception Message 코드의 바로 밑에 영문 Log, 영문 Comment, 영문 Exception Message를 한글 코드와 동일한 영문 버전의 코드로 작성할 것

**예시:**
```csharp
// 사용자 인증 실패
// User authentication failed
throw new AuthenticationException("인증에 실패했습니다.");
throw new AuthenticationException("Authentication failed.");
```

---

## 3. .NET CSharp 코드 생성 지침

### 3.1 .NET 버전
- **Newest .NET을 기본으로 사용**

### 3.2 최신 C# 문법 사용
- **Context7 mcp를 사용하여 기본적으로 최신 C# 문법을 사용할 것**
- 따로 .NET 또는 .NET Framework에 대한 요청이 있는 경우에만 그 버전에 맞춘 C# 문법을 사용할 것
- 예시: collection expressions, spread element, Range Operator, string interpolation 등

### 3.3 Class 설계
- **상속이 필요하지 않은 Class는 `sealed` 키워드 적용**
- **Primary constructors를 적용할 수 있는 상황에서는 기본적으로 적용**

### 3.4 Namespace 관리
- BCL, FCL, Nuget Package에서 적용되는 Namespace는 .NET Project마다 **GlobalUsings.cs 파일을 생성하여 global using으로 처리**
- 실제 코드 페이지에서 using으로 적용되는 부분은 **직접 정의한 namespace로만 한정**

### 3.5 Console Application
- .NET으로 Console Application Project를 작성할 때는 특별한 요청이 있는 게 아니라면 **Top-Level Statement를 기본으로 설정**

### 3.6 프로그래밍 패러다임
- **기본적으로는 C# 코드를 Procedural Programming 형태로 개발**
- 요청을 할 경우에만 Object Orientation Programming Style로 작성

### 3.7 고성능 C# 코드
고성능 C# 코드를 요청하면 다음 기술들을 사용:
- Reactive Extensions
- ThreadLocal<T>
- Task Async
- PLINQ
- Parallel.ForEach
- .NET CLR GC Heap Memory Optimization
- ObjectPool<T>
- Span<T>

**코드 생성 후 효율화 한 내용에 대해서도 함께 설명**

추가로, 이 고성능 코드 생성 답변의 끝에 **Functional C# 스타일도 원하는지 물어보고 필요하다고 하면 함수형 코드도 생성할 것**

### 3.8 파일 구조
- **cs 파일 1개에는 Type을 1개만 정의할 것** (class와 interface 타입의 파일 분리)
- **class와 interface가 1:1 관계일 때는 디버깅이 오히려 불편하기 때문에 interface를 사용하지 않고 class만 사용할 것**

### 3.9 Namespace 스타일
- **File Scope namespace를 기본으로 사용할 것**
- Block scope 최소화

### 3.10 불변 타입 최적화
- **불변 타입을 통한 최적화를 위해서 `record`, `readonly struct`, `init;`과 같은 키워드를 적극적으로 활용**

### 3.11 컬렉션 사용
- **필요하다고 판단하는 경우에만** IReadOnlyCollection<T>, HashSet<T> 같은 클래스를 사용할 것
- 필요하지 않은 경우까지 사용하지 말 것
- **필요한 경우의 기준: 사용했을 때 명확하게 성능이 크게 증가할 수 있어야 할 때**

### 3.12 Span<T> 사용 주의사항
- **Span<T>, ReadOnlySpan<T>를 사용할 때는 async-await를 사용하지 못하므로 이 부분을 고려하여 코딩할 것**
- ⚠️ 당신이 자주 실수하는 부분

### 3.13 코드 스타일
- **if 문을 사용하거나 for/while 반복문을 사용할 때 Early Return 코드 스타일을 사용할 것**
- **가능한 한 Pattern Matching 코드 스타일을 사용할 것**

### 3.14 Literal String 처리
- **Literal string에 대해서는 가급적 `const string`으로 사전에 정의하여 사용할 것**

**예시:**
```csharp
// 좋은 예
const string ErrorMessage = "오류가 발생했습니다.";
// An error has occurred.

if (condition)
    throw new Exception(ErrorMessage);

// 나쁜 예
if (condition)
    throw new Exception("오류가 발생했습니다.");
```

---

## 4. .NET Excel 정보 다루는 지침

### 4.1 Excel 데이터 처리
- **Excel 데이터를 다루는 영역에 대해서는 MiniExcel을 사용**

### 4.2 Excel 스타일 적용
- **Excel 파일의 Style을 적용할 때는 ClosedXml을 사용**

### 4.3 AOT 및 Trimming 제한
- **Excel 파일을 다루는 프로젝트에 대해서는 NativeAot나 Trimming을 적용하지 않아야 함을 반드시 지킨다**

---

## 5. .NET WPF 코드 생성 지침

### 5.1 기본 원칙
- **.NET CSharp 코드 생성 지침을 기본으로 할 것**

### 5.2 MVVM 패턴
- **기본적으로 MVVM 형태로 코드를 생성할 것**
- **ViewModel 클래스에 UI 프레임워크 의존성 금지:**
  - **WPF**: ViewModel class에 `System.Windows`로 시작하는 클래스들이 참조되지 않는 것을 의미
  - **AvaloniaUI**: ViewModel class에 `Avalonia`로 시작하는 Type들을 참조해서는 안 됨
  - **⚠️ 절대로 UI 프레임워크 FCL 객체가 참조되지 않도록 한다**
- **MVVM 제약은 ViewModel에만 적용:**
  - ViewModel이라는 접미사가 있는 Class들에만 위 제한 규칙 적용
  - Converter, Service, Manager, Locator 같은 클래스들은 UI 프레임워크 참조 가능

### 5.3 MVVM 프레임워크
- **MVVM을 구조로 잡을 때는 CommunityToolkit.Mvvm을 기본으로 사용**

### 5.4 XAML 코드 작성
- **XAML 코드를 생성할 때는 CustomControl을 사용하여 ResourceDictionary를 통한 Stand-Alone Control Style Resource를 사용**
- 목적: StaticResource 불러올 때 시점 고정 및 스타일 의존성 최소화

#### 5.4.1 WPF Custom Control Library 프로젝트 구조

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

### 5.5 CollectionView를 사용한 MVVM 패턴

#### 5.5.1 문제 상황
하나의 원본 컬렉션을 여러 View에서 각각 다른 조건으로 필터링하여 사용하면서도 MVVM 원칙을 준수해야 하는 경우

#### 5.5.2 핵심 원칙
- **ViewModel은 `System.Windows.Data` 네임스페이스를 참조하면 안 됨** (MVVM 위반)
- **`System.ComponentModel.ICollectionView` 인터페이스를 사용**하여 UI 프레임워크에 독립적으로 유지
- **Service Layer를 통해 `CollectionViewSource` 접근을 캡슐화**

#### 5.5.3 아키텍처 계층 구조
```
View (XAML)
    ↓ DataBinding
ViewModel Layer (System.ComponentModel.ICollectionView 사용)
    ↓ IEnumerable/ICollectionView 인터페이스
Service Layer (CollectionViewSource 직접 사용)
    ↓
Data Layer (ObservableCollection<T>)
```

#### 5.5.4 구현 패턴

**1. Service Layer (CollectionViewFactory/Store)**

```csharp
// Services/MemberCollectionService.cs
namespace MyApp.Services;

public sealed class MemberCollectionService
{
    private ObservableCollection<Member> Source { get; } = [];

    // Factory Method: 필터링된 뷰 생성
    // Factory Method: Create filtered view
    public ICollectionView CreateView(Predicate<object>? filter = null)
    {
        var viewSource = new CollectionViewSource { Source = Source };
        var view = viewSource.View;

        if (filter is not null)
        {
            view.Filter = filter;
        }

        return view;
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
// ViewModel은 IEnumerable로 받아서 PresentationFramework 의존성 제거
// ViewModel receives IEnumerable to eliminate PresentationFramework dependency
namespace MyApp.ViewModels;

public abstract class BaseFilteredViewModel
{
    public IEnumerable? Members { get; }

    protected BaseFilteredViewModel(Predicate<object> filter)
    {
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

public sealed class RunnerViewModel : BaseFilteredViewModel
{
    public RunnerViewModel()
        : base(item => (item as Member)?.Type == DeviceTypes.Runner)
    {
    }
}
```

**3. View에서 CollectionView 초기화 (대안 방법)**

```csharp
// MainWindow.xaml.cs - Code-Behind에서 초기화
// MainWindow.xaml.cs - Initialize in Code-Behind
namespace MyApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var viewModel = new MainViewModel();
        DataContext = viewModel;

        // CollectionViewSource는 View 레이어에서 생성
        // CollectionViewSource is created in View layer
        ICollectionView collectionView = CollectionViewSource.GetDefaultView(viewModel.People);

        // ViewModel에 주입
        // Inject into ViewModel
        viewModel.InitializeCollectionView(collectionView);
    }
}

// ViewModel
public sealed partial class MainViewModel : ObservableObject
{
    public ObservableCollection<Person> People { get; } = [];

    public ICollectionView? PeopleView { get; private set; }

    public void InitializeCollectionView(ICollectionView collectionView)
    {
        PeopleView = collectionView;
        PeopleView.Filter = FilterPredicate;
    }

    private bool FilterPredicate(object obj)
    {
        return obj is Person person && person.IsActive;
    }
}
```

#### 5.5.5 프로젝트 구조 (엄격한 MVVM)

```
MyApp.Models/              // 순수 C# 모델
                          // Pure C# models

MyApp.ViewModels/         // WindowsBase 참조 O (ICollectionView)
                          // WindowsBase reference: YES (ICollectionView)
                          // PresentationFramework 참조 X
                          // PresentationFramework reference: NO

MyApp.Services/           // PresentationFramework 참조 O
                          // PresentationFramework reference: YES
                          // CollectionViewSource 사용
                          // Uses CollectionViewSource

MyApp.Views/              // 모든 WPF 어셈블리 참조
                          // References all WPF assemblies
```

#### 5.5.6 참조 어셈블리 규칙

**ViewModel 프로젝트가 참조하면 안 되는 어셈블리:**
- ❌ `PresentationFramework.dll` (CollectionViewSource 포함)
- ❌ `PresentationCore.dll`

**ViewModel 프로젝트가 참조 가능한 어셈블리:**
- ✅ `WindowsBase.dll` (ICollectionView는 System.ComponentModel 네임스페이스)

#### 5.5.7 핵심 장점

1. **단일 원본 유지**: 모든 View가 하나의 `ObservableCollection` 공유
2. **자동 동기화**: 원본 변경 시 모든 필터링된 View에 자동 반영
3. **MVVM 준수**: ViewModel이 UI 프레임워크에 독립적
4. **재사용성**: 다양한 필터 조건으로 여러 View 생성 가능

#### 5.5.8 ICollectionView 주요 기능

```csharp
// 필터링
// Filtering
collectionView.Filter = item => (item as Member)?.IsActive == true;

// 정렬
// Sorting
collectionView.SortDescriptions.Add(
    new SortDescription(nameof(Member.Name), ListSortDirection.Ascending)
);

// 그룹화
// Grouping
collectionView.GroupDescriptions.Add(
    new PropertyGroupDescription(nameof(Member.Department))
);

// 뷰 새로고침
// Refresh view
collectionView.Refresh();
```

#### 5.5.9 DI/IoC 적용 시

```csharp
// Interface 정의
// Interface definition
namespace MyApp.Services;

public interface IMemberCollectionService
{
    ICollectionView CreateView(Predicate<object>? filter = null);
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

#### 5.5.10 Microsoft 공식 문서

- [ICollectionView Interface](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.icollectionview?view=windowsdesktop-10.0)
- [CollectionViewSource Class](https://learn.microsoft.com/en-us/dotnet/api/system.windows.data.collectionviewsource?view=windowsdesktop-10.0)
- [Data Binding Overview - Collection Views](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/data/#binding-to-collections)
- [How to: Filter Data in a View](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/data/how-to-filter-data-in-a-view)

---

## 6. .NET에 관한 질문 답변 지침

### 6.1 이론 및 개념 질문
- **.NET의 이론과 개념에 대한 질문에 대해서는 MicrosoftDocs mcp를 반드시 사용**
- **Low-Level 지식까지 설명**
- **사용한 문서의 유효한 Link를 함께 남길 것**

---

## 7. 코드 예시

### 7.1 최신 C# 문법 활용 예시

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

### 7.2 WPF MVVM 예시

```csharp
// ViewModels/MainViewModel.cs
namespace MyApp.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

// ViewModel - System.Windows 참조 없음
public sealed partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string _userName = string.Empty;

    [ObservableProperty]
    private bool _isLoading;

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

## 8. 체크리스트

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

**WPF 프로젝트:**
- [ ] WPF ViewModel에 System.Windows 참조 없음 확인
- [ ] AvaloniaUI ViewModel에 Avalonia 참조 없음 확인
- [ ] Custom Control Library에서 AssemblyInfo.cs를 Properties 폴더로 이동
- [ ] Generic.xaml은 MergedDictionaries 허브로만 사용
- [ ] 각 컨트롤 스타일을 개별 XAML 파일로 분리
- [ ] CollectionView 사용 시 Service Layer 패턴 적용
- [ ] ViewModel 프로젝트에서 PresentationFramework.dll 참조 제거

---

## 9. 주의사항

### ⚠️ 자주 발생하는 실수

**C# 코딩 일반:**
1. **Span<T>와 async-await 함께 사용** - 불가능
2. **불필요한 IReadOnlyCollection<T>, HashSet<T> 사용** - 성능 향상이 명확하지 않은 경우
3. **요청하지 않은 기능까지 미리 구현** - 요청한 것만 구현, 나머지는 제안

**WPF/MVVM 관련:**
4. **WPF ViewModel에 System.Windows 네임스페이스 참조** - MVVM 위반
5. **AvaloniaUI ViewModel에 Avalonia 네임스페이스 참조** - MVVM 위반
6. **ViewModel 프로젝트에서 PresentationFramework.dll 직접 참조** - CollectionViewSource 사용 불가
7. **CollectionView 없이 여러 View에서 개별 컬렉션 관리** - 데이터 동기화 문제 발생
8. **Generic.xaml에 직접 스타일 작성** - 개별 XAML 파일로 분리하고 MergedDictionaries로 병합

**MVVM 계층 분리 원칙:**
- ViewModel: `WindowsBase.dll` 참조 가능 (ICollectionView)
- ViewModel: `PresentationFramework.dll` 참조 불가 (CollectionViewSource)
- Service Layer: `PresentationFramework.dll` 참조 가능
- View Layer: 모든 WPF 어셈블리 참조 가능

---

## 10. 문서 업데이트

이 문서는 .NET과 C# 생태계의 발전에 따라 지속적으로 업데이트됩니다.

**최종 업데이트:** 2025-11-19
