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

### 3.15 Dependency Injection 및 GenericHost 사용

- **Microsoft.Extensions.DependencyInjection을 사용하여 의존성 주입 구현**
- **GenericHost (Microsoft.Extensions.Hosting)를 기본으로 사용**
- **Constructor Injection을 통한 서비스 주입 방식 적용**

#### 3.15.1 Console 및 일반 프로젝트 - Program.cs

```csharp
// Program.cs
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// GenericHost를 사용한 DI 설정
// Configure DI using GenericHost
var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // 서비스 등록
        // Register services
        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddTransient<IEmailService, EmailService>();

        // 메인 애플리케이션 서비스 등록
        // Register main application service
        services.AddSingleton<App>();
    })
    .Build();

// ServiceProvider를 통해 서비스 가져오기
// Get service through ServiceProvider
var app = host.Services.GetRequiredService<App>();
await app.RunAsync();

// 애플리케이션 클래스 - Constructor Injection
// Application class - Constructor Injection
public sealed class App(IUserService userService, IEmailService emailService)
{
    private readonly IUserService _userService = userService;
    private readonly IEmailService _emailService = emailService;

    public async Task RunAsync()
    {
        // 주입된 서비스 사용
        // Use injected services
        var users = await _userService.GetAllUsersAsync();

        foreach (var user in users)
        {
            await _emailService.SendWelcomeEmailAsync(user.Email);
        }
    }
}
```

#### 3.15.2 WPF 프로젝트 - App.xaml.cs

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

#### 3.15.3 MainWindow.xaml.cs - Constructor Injection

```csharp
// MainWindow.xaml.cs
namespace MyApp;

using System.Windows;

public partial class MainWindow : Window
{
    // Constructor Injection을 통한 ViewModel 주입
    // ViewModel injection through Constructor Injection
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
```

#### 3.15.4 ViewModel - Constructor Injection

```csharp
// ViewModels/MainViewModel.cs
namespace MyApp.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public sealed partial class MainViewModel : ObservableObject
{
    private readonly IUserService _userService;
    private readonly IDialogService _dialogService;

    // Constructor Injection
    public MainViewModel(IUserService userService, IDialogService dialogService)
    {
        _userService = userService;
        _dialogService = dialogService;

        LoadDataAsync();
    }

    [ObservableProperty]
    private ObservableCollection<User> users = [];

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        try
        {
            var userList = await _userService.GetAllUsersAsync();
            Users = new ObservableCollection<User>(userList);
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync("오류 발생", ex.Message);
            // Error occurred
        }
    }
}
```

#### 3.15.5 서비스 Lifetime 규칙

- **Singleton**: 애플리케이션 전체에서 하나의 인스턴스만 생성
  - Repository, 전역 상태 관리 서비스
  - `services.AddSingleton<IUserRepository, UserRepository>()`

- **Scoped**: 요청(Scope)당 하나의 인스턴스 생성
  - DbContext, 트랜잭션 단위 서비스
  - `services.AddScoped<IUserService, UserService>()`
  - ⚠️ WPF에서는 일반적으로 사용하지 않음 (Web 애플리케이션에서 주로 사용)

- **Transient**: 요청할 때마다 새 인스턴스 생성
  - ViewModel, 일회성 서비스
  - `services.AddTransient<MainViewModel>()`

#### 3.15.6 ServiceProvider 직접 사용 (권장하지 않음)

```csharp
// Service Locator 패턴 (안티패턴)
// Service Locator pattern (anti-pattern)
public sealed class SomeClass
{
    private readonly IServiceProvider _serviceProvider;

    public SomeClass(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void DoSomething()
    {
        // ⚠️ 권장하지 않음: ServiceProvider를 직접 사용
        // Not recommended: Using ServiceProvider directly
        var service = _serviceProvider.GetRequiredService<IUserService>();
    }
}

// 올바른 방법: Constructor Injection 사용
// Correct way: Use Constructor Injection
public sealed class SomeClass
{
    private readonly IUserService _userService;

    public SomeClass(IUserService userService)
    {
        _userService = userService;
    }

    public void DoSomething()
    {
        // ✅ 권장: Constructor Injection으로 주입받은 서비스 사용
        // Recommended: Use service injected through Constructor Injection
        _userService.GetAllUsersAsync();
    }
}
```

#### 3.15.7 필수 NuGet 패키지

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="9.0.0" />
  <PackageReference Include="Microsoft.Extensions.Hosting" Version="9.0.0" />
</ItemGroup>
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

### 5.2 WPF 솔루션 및 프로젝트 구조

#### 5.2.1 솔루션 구조 원칙

**솔루션 이름은 애플리케이션 이름**
- 예시: `GameDataTool` 솔루션 = 실행 가능한 .NET Assembly 이름

#### 5.2.2 프로젝트 명명 규칙

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

#### 5.2.3 Solution Folder 활용

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

#### 5.2.4 실제 예시

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

### 5.3 MVVM 패턴
- **기본적으로 MVVM 형태로 코드를 생성할 것**
- **ViewModel 클래스에 UI 프레임워크 의존성 금지:**
  - **WPF**: ViewModel class에 `System.Windows`로 시작하는 클래스들이 참조되지 않는 것을 의미
  - **⚠️ 절대로 UI 프레임워크 FCL 객체가 참조되지 않도록 한다**
- **MVVM 제약은 ViewModel에만 적용:**
  - ViewModel이라는 접미사가 있는 Class들에만 위 제한 규칙 적용
  - Converter, Service, Manager, Locator 같은 클래스들은 UI 프레임워크 참조 가능

### 5.4 MVVM 프레임워크
- **MVVM을 구조로 잡을 때는 CommunityToolkit.Mvvm을 기본으로 사용**

#### 5.4.1 CommunityToolkit.Mvvm Attribute 스타일 가이드

**ObservableProperty Attribute 작성 규칙:**

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
- **Attribute가 1개**: `[ObservableProperty]` inline으로 필드 선언 바로 앞에 작성
- **Attribute가 여러 개**: 다른 Attribute들은 각 줄에 작성하되, `[ObservableProperty]`는 **항상 마지막에 inline**으로 작성
- 목적: 코드 가독성 향상 및 일관된 코딩 스타일 유지

---

### 5.5 XAML 코드 작성
- **XAML 코드를 생성할 때는 CustomControl을 사용하여 ResourceDictionary를 통한 Stand-Alone Control Style Resource를 사용**
- 목적: StaticResource 불러올 때 시점 고정 및 스타일 의존성 최소화

#### 5.5.1 WPF Custom Control Library 프로젝트 구조

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

### 5.6 CollectionView를 사용한 MVVM 패턴

#### 5.6.1 문제 상황
하나의 원본 컬렉션을 여러 View에서 각각 다른 조건으로 필터링하여 사용하면서도 MVVM 원칙을 준수해야 하는 경우

#### 5.6.2 핵심 원칙
- **ViewModel은 WPF 관련 어셈블리를 참조하면 안 됨** (MVVM 위반)
- **Service Layer를 통해 `CollectionViewSource` 접근을 캡슐화**
- **ViewModel은 `IEnumerable` 또는 순수 BCL 타입만 사용**

#### 5.6.3 아키텍처 계층 구조
```
View (XAML)
    ↓ DataBinding
ViewModel Layer (IEnumerable 사용, WPF 어셈블리 참조 X)
    ↓ IEnumerable 인터페이스
Service Layer (CollectionViewSource 직접 사용)
    ↓
Data Layer (ObservableCollection<T>)
```

#### 5.6.4 구현 패턴

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

#### 5.6.5 프로젝트 구조 (엄격한 MVVM)

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

#### 5.6.6 참조 어셈블리 규칙

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

#### 5.6.7 핵심 장점

1. **단일 원본 유지**: 모든 View가 하나의 `ObservableCollection` 공유
2. **자동 동기화**: 원본 변경 시 모든 필터링된 View에 자동 반영
3. **MVVM 준수**: ViewModel이 UI 프레임워크에 완전 독립적
4. **재사용성**: 다양한 필터 조건으로 여러 View 생성 가능
5. **테스트 용이성**: ViewModel을 WPF 없이 단위 테스트 가능

#### 5.6.8 Service Layer에서 CollectionView 기능 활용

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

#### 5.6.9 DI/IoC 적용 시

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

#### 5.6.10 실무 적용 시 권장사항

1. **프로젝트 분리**: ViewModel과 Service를 별도 프로젝트로 분리
2. **Interface 활용**: Service는 인터페이스로 정의하여 테스트 용이성 확보
3. **Singleton 또는 DI**: Service는 Singleton 또는 DI 컨테이너로 관리
4. **명명 규칙**:
   - `MemberCollectionService` (Service 접미사)
   - `MemberViewFactory` (Factory 접미사)
   - `MemberStore` (Store 접미사)

#### 5.6.11 Microsoft 공식 문서

- [CollectionViewSource Class](https://learn.microsoft.com/en-us/dotnet/api/system.windows.data.collectionviewsource?view=windowsdesktop-10.0)
- [Data Binding Overview - Collection Views](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/data/#binding-to-collections)
- [How to: Filter Data in a View](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/data/how-to-filter-data-in-a-view)
- [Service Layer Pattern](https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions-1/models-data/validating-with-a-service-layer-cs#creating-a-service-layer)

### 5.7 Popup 컨트롤 사용 시 주의사항

WPF에서 Popup 컨트롤은 WPF Application에 포커스가 있을 때만 정상적으로 동작합니다. 다른 애플리케이션으로 포커스가 이동한 상태에서는 Popup이 제대로 표시되지 않거나 동작하지 않을 수 있습니다.

#### 5.7.1 포커스 관리 패턴

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

#### 5.7.2 핵심 원칙

- **Popup 동작 조건**: WPF Application에 포커스가 있을 때만 동작
- **PreviewMouseDown 이벤트**: 마우스 클릭 시 포커스 상태 체크
- **IsKeyboardFocused 체크**: 키보드 포커스 여부 확인
- **Activate() 호출**: 포커스가 없으면 윈도우 활성화하여 포커스 복원
- **UserControl 경우**: `Window.GetWindow(this)?.Activate()`로 부모 윈도우 활성화

#### 5.7.3 왜 필요한가?

1. **다른 앱으로 포커스 이동**: 사용자가 다른 애플리케이션을 클릭했다가 다시 돌아올 때
2. **백그라운드 실행**: WPF 앱이 백그라운드에 있을 때 Popup 동작 보장
3. **사용자 경험**: Popup이 항상 예상대로 동작하도록 보장

**⚠️ 주의사항:**
- Popup을 사용하는 모든 Window 및 UserControl에 이 패턴을 적용해야 함
- PreviewMouseDown은 터널링 이벤트이므로 자식 컨트롤보다 먼저 처리됨
- MVVM 패턴에서는 Code-Behind에서 처리 (View 레이어의 포커스 관리 로직)

#### 5.7.4 참고 자료

- [WPF Popup 포커스 이슈 - .NET Dev 포럼](https://forum.dotnetdev.kr/t/wpf-popup/8296)

### 5.8 DataTemplate을 사용한 View-ViewModel 자동 매핑

WPF에서 DataTemplate을 사용하면 ViewModel 타입과 View를 자동으로 매핑할 수 있습니다. 이 패턴은 네비게이션 시나리오나 동적 콘텐츠 표시에 매우 유용합니다.

#### 5.8.1 핵심 개념

**ContentControl의 Content에 ViewModel 인스턴스를 바인딩하면, WPF가 자동으로 해당 ViewModel 타입에 맞는 DataTemplate을 찾아서 View를 렌더링합니다.**

이 패턴의 핵심:
1. `Mappings.xaml`에 ViewModel 타입별 DataTemplate 정의
2. `ContentControl.Content`에 ViewModel 인스턴스 바인딩
3. WPF가 자동으로 타입을 매칭하여 해당 View 렌더링

#### 5.8.2 Mappings.xaml 패턴

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

#### 5.8.3 네비게이션 패턴 구현

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

---

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

| 항목 | WPF | AvaloniaUI |
|------|-----|------------|
| 파일 확장자 | .xaml | .axaml |
| 스타일 정의 | Style + ControlTemplate | ControlTheme |
| 상태 관리 | Trigger, DataTrigger | Pseudo Classes, Style Selector |
| CSS 지원 | ❌ | ✅ (Classes 속성) |
| 리소스 병합 | MergedDictionaries + ResourceDictionary | MergedDictionaries + ResourceInclude |
| 의존성 속성 | DependencyProperty | StyledProperty, DirectProperty |

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

**WPF/MVVM 관련:**
6. **WPF ViewModel에 System.Windows 네임스페이스 참조** - MVVM 위반
7. **ViewModel 프로젝트에서 WindowsBase.dll 참조** - ICollectionView 사용하려다 WPF 의존성 발생
8. **ViewModel 프로젝트에서 PresentationFramework.dll 직접 참조** - CollectionViewSource 사용 불가
9. **ViewModel에서 ICollectionView 타입 사용** - IEnumerable로 받아야 함
10. **CollectionView 없이 여러 View에서 개별 컬렉션 관리** - 데이터 동기화 문제 발생
11. **Generic.xaml에 직접 스타일 작성** - 개별 XAML 파일로 분리하고 MergedDictionaries로 병합
12. **App.xaml.cs에서 GenericHost 설정 누락** - DI 컨테이너 구성 필요

**AvaloniaUI/MVVM 관련:**
13. **AvaloniaUI ViewModel에 Avalonia 네임스페이스 참조** - MVVM 위반
14. **ViewModel 프로젝트에서 Avalonia.Base.dll, Avalonia.Controls.dll 참조** - AvaloniaUI 의존성 발생
15. **CustomControl을 TemplatedControl에서 직접 상속** - Button, TextBox 등 기존 컨트롤 상속 필요
16. **WPF의 DependencyProperty를 AvaloniaUI에서 그대로 사용** - StyledProperty 또는 DirectProperty 사용 필요
17. **WPF의 Trigger를 AvaloniaUI에서 그대로 사용** - Pseudo Classes와 Style Selector 사용 필요
18. **WPF의 CollectionViewSource를 AvaloniaUI에서 사용** - DataGridCollectionView 또는 ReactiveUI 사용 필요
19. **Generic.axaml에 직접 ControlTheme 작성** - 개별 AXAML 파일로 분리하고 ResourceInclude로 병합
20. **App.axaml.cs에서 GenericHost 설정 누락** - DI 컨테이너 구성 필요

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
