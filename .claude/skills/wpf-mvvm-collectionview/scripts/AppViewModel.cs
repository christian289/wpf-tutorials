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
