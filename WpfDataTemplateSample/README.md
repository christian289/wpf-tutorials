# WPF DataTemplate 자동 매핑 Sample

## 개요 (Overview)

이 프로젝트는 WPF의 `DataTemplate`을 활용하여 **ViewModel을 Content에 설정하면 자동으로 해당 View가 렌더링**되는 구조를 보여주는 Sample Application입니다.

This project demonstrates a WPF structure where **Views are automatically rendered when ViewModels are set as Content** using `DataTemplate`.

## 핵심 개념 (Core Concept)

### 1. Mappings.xaml - DataTemplate 정의

`Mappings.xaml` 파일에 ViewModel 타입과 View를 매핑하는 `DataTemplate`을 정의합니다:

```xml
<ResourceDictionary xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                    xmlns:views="clr-namespace:WpfDataTemplateSample.Views"
                    xmlns:viewmodels="clr-namespace:WpfDataTemplateSample.ViewModels">
    
    <DataTemplate DataType="{x:Type viewmodels:HomeViewModel}">
        <views:HomeView />
    </DataTemplate>
    
    <DataTemplate DataType="{x:Type viewmodels:SettingsViewModel}">
        <views:SettingsView />
    </DataTemplate>
    
</ResourceDictionary>
```

### 2. App.xaml - Resource 통합

`App.xaml`에서 `Mappings.xaml`을 Application Resource로 병합합니다:

```xml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="Mappings.xaml" />
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

### 3. MainWindow.xaml - ContentControl 활용

`ContentControl`의 `Content`에 ViewModel을 바인딩하면 자동으로 매핑된 View가 표시됩니다:

```xml
<ContentControl Content="{Binding CurrentViewModel}" />
```

### 4. ViewModel에서 Navigation

ViewModel에서 다른 ViewModel의 인스턴스를 생성하여 할당하면 자동으로 View가 전환됩니다:

```csharp
[RelayCommand]
private void NavigateToHome()
{
    // HomeViewModel을 설정하면 자동으로 HomeView가 렌더링됨
    // HomeView is automatically rendered when HomeViewModel is set
    CurrentViewModel = new HomeViewModel();
}

[RelayCommand]
private void NavigateToSettings()
{
    // SettingsViewModel을 설정하면 자동으로 SettingsView가 렌더링됨
    // SettingsView is automatically rendered when SettingsViewModel is set
    CurrentViewModel = new SettingsViewModel();
}
```

## 동작 원리 (How It Works)

1. **DataTemplate 정의**: `Mappings.xaml`에서 각 ViewModel 타입에 대응하는 View를 정의
2. **Resource 병합**: `App.xaml`에서 전역 리소스로 등록
3. **자동 탐색**: WPF는 `ContentControl`의 `Content`에 설정된 객체의 타입을 확인
4. **템플릿 적용**: 해당 타입과 일치하는 `DataTemplate`을 찾아 자동으로 View를 렌더링

## 프로젝트 구조 (Project Structure)

```
WpfDataTemplateSample/
├── App.xaml                          # Application Entry Point
├── App.xaml.cs
├── Mappings.xaml                     # ViewModel-View 매핑 정의
├── MainWindow.xaml                   # Main Window (ContentControl 활용)
├── MainWindow.xaml.cs
├── ViewModels/
│   ├── MainWindowViewModel.cs        # Navigation 로직 포함
│   ├── HomeViewModel.cs
│   ├── SettingsViewModel.cs
│   └── UserProfileViewModel.cs
└── Views/
    ├── HomeView.xaml
    ├── HomeView.xaml.cs
    ├── SettingsView.xaml
    ├── SettingsView.xaml.cs
    ├── UserProfileView.xaml
    └── UserProfileView.xaml.cs
```

## 사용 기술 (Technologies Used)

- **.NET Framework 4.8**
- **WPF (Windows Presentation Foundation)**
- **CommunityToolkit.Mvvm** - MVVM 패턴 구현을 위한 프레임워크
  - `ObservableObject` - Property Change Notification
  - `ObservableProperty` - Source Generator를 활용한 자동 Property 생성
  - `RelayCommand` - Command 패턴 구현

## 실행 방법 (How to Run)

1. 솔루션 파일 열기
2. `WpfDataTemplateSample` 프로젝트 빌드
3. 실행

버튼 클릭으로 각 페이지 간 Navigation이 자동으로 이루어지는 것을 확인할 수 있습니다.

## 장점 (Advantages)

1. **선언적 View 매핑**: XAML에서 명확하게 ViewModel-View 관계 정의
2. **자동 View 전환**: Code-behind에서 View를 직접 생성할 필요 없음
3. **MVVM 준수**: ViewModel이 View에 대한 직접적인 참조를 갖지 않음
4. **유지보수 용이**: 중앙 집중식 매핑 관리
5. **확장성**: 새로운 ViewModel-View 쌍 추가가 간단함

## 추가 확장 가능 사항 (Possible Extensions)

- Navigation Service 추가하여 History 관리
- ViewModel 간 데이터 전달 메커니즘
- Transition Animation 추가
- ViewModel 인스턴스 재사용 전략 (Singleton vs Transient)
