---
name: axaml-coding
description: "AvaloniaUI CustomControl과 ControlTheme를 통한 Stand-Alone Control Style 작성"
---

# 6.5 AXAML 코드 작성
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

