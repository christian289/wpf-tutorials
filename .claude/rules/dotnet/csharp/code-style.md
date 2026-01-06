# CSharp 코드 생성 지침

## 1. .NET 버전

- Newest .NET을 기본으로 사용.

## 2. 최신 C# 문법 사용

- Context7 mcp를 사용하여 기본적으로 최신 C# 문법을 사용할 것.
- 따로 .NET 또는 .NET Framework에 대한 요청이 있는 경우에만 그 버전에 맞춘 C# 문법을 이용할 것.
- 예시: collection expressions, spread element, Range Operator, string interpolation 등.

## 3. Class 설계

- 상속이 필요하지 않은 Class는 `sealed` 키워드 적용.
- Primary constructors를 적용할 수 있는 상황에서는 기본적으로 적용.

## 4. Namespace 관리

- BCL, FCL, Nuget Package에서 적용되는 Namespace는 .NET Project마다 GlobalUsings.cs 파일을 생성하여 global using을 적용할 것.
- 실제 코드 페이지에서 using으로 적용되는 부분은 사용자가 직접 정의한 namespace만 정의할 것.

## 5. Console Application

- .NET으로 Console Application Project를 작성할 때는 특별한 요청이 있는 게 아니라면 Top-Level Statement를 기본으로 설정.

## 6. 프로그래밍 패러다임

- 기본적으로는 C# 코드를 절차지향 프로그래밍(Procedural Programming)으로 작성할 것.
- 요청을 할 경우에만 객체지향 프로그래밍(Object Orientation Programming)으로 작성할 것.

## 7. 고성능 C# 코드

고성능 C# 코드를 요청하면 다음 기술들을 사용:

- Reactive Extensions
- ThreadLocal<T>
- Task Async
- PLINQ
- Parallel.ForEach
- .NET CLR GC Heap Memory Optimization
- ObjectPool<T>
- Memorycache<T>
- Span<T>
- HashSet<T>
- ConcurrentDictionary<T>

## 8. 파일 구조

- .cs 파일 1개에는 Type을 1개만 정의할 것. (class와 interface 타입의 파일 분리)
- class와 interface가 1:1 관계일 때는 디버깅이 오히려 불편하기 때문에 interface를 사용하지 않고 class만 이용할 것.

## 9. Namespace 스타일

- File Scope namespace를 기본으로 사용할 것.
- Block scope 최소화

## 10. 불변 타입 최적화

- 불변 타입을 통한 최적화를 위해서 `record`, `readonly struct`, `init;`과 같은 키워드를 적극적으로 활용.
- 함수에서 return할 때 불변의 의미를 가진 값을 리턴할 때는 Readonly Type을 적용할 것.

**예시:**

```csharp
public IReadOnlyList ReadOnlyReturnFunc()
{
    List<int> result = [];

    // 중략...

    return result.AsReadOnly();
}
```

## 12. Span<T> 사용 주의사항

- Span<T>, ReadOnlySpan<T>를 사용할 때는 async-await를 사용하지 못하므로 이 부분을 고려하여 코딩할 것.
- ⚠️ 당신이 자주 실수하는 부분

## 13. 코드 스타일

- Early Return 코드 스타일을 이용할 것.
- switch 문 사용 시 Pattern Matching 코드 스타일을 이용할 것.

## 14. Literal String 처리

- Literal string에 대해서는 `const string`으로 사전에 정의하여 사용할 것.
- 장문의 Literal string에 대해서는 string.Concat 또는 '+' Operator를 이용하지 말고 Raw Literal string 문법을 이용할 것.

**예시:**

```csharp
// 예시 1

// 좋은 예
const string ErrorMessage = "오류가 발생했습니다.";
// An error has occurred.

if (condition)
    throw new Exception(ErrorMessage);

// 나쁜 예
if (condition)
    throw new Exception("오류가 발생했습니다.");

// 예시 2

// 좋은 예
string exampleLongString = "어느새 길어진 그림자를 따라서" +
    "땅거미 진 어둠속을 그대와 걷고 있네요" +
    "손을 마주 잡고 그 언제까지라도" +
    "함께 있는것만으로 눈물이 나는 걸요";

// 나쁜 예
string exampleLongString = """
    어느새 길어진 그림자를 따라서
    땅거미 진 어둠속을 그대와 걷고 있네요
    손을 마주 잡고 그 언제까지라도
    함께 있는것만으로 눈물이 나는 걸요
    """;
```

### 15. Console Application 개발 시 Generic Host를 이용하여 Dependency Injection 구현

- Microsoft.Extensions.DependencyInjection을 사용하여 의존성 주입 구현
- GenericHost (Microsoft.Extensions.Hosting)를 기본으로 사용
- Constructor Injection을 통한 서비스 주입 방식 적용

**예시:**

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
