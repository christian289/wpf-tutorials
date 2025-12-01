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
