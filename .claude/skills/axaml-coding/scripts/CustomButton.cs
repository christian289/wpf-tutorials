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
