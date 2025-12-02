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
