using System.Windows;
using System.Windows.Data;

namespace ChessTourManager.WPF.Helpers.BindablePassword;

/// <inheritdoc cref="System.Windows.Controls.UserControl" />
public partial class BindablePasswordBox
{
    public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register(
         nameof(Password), typeof(string), typeof(BindablePasswordBox),
         new FrameworkPropertyMetadata(string.Empty,
                                       FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                       OnPasswordPropertyChanged,
                                       null, false,
                                       UpdateSourceTrigger.PropertyChanged)
        );

    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
         nameof(Header), typeof(string), typeof(BindablePasswordBox),
         new FrameworkPropertyMetadata(string.Empty,
                                       FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                       OnPasswordPropertyChanged,
                                       null, false,
                                       UpdateSourceTrigger.PropertyChanged)
        );

    public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register(
         nameof(PlaceholderText), typeof(string), typeof(BindablePasswordBox),
         new FrameworkPropertyMetadata(string.Empty,
                                       FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                                       OnPasswordPropertyChanged,
                                       null, false,
                                       UpdateSourceTrigger.PropertyChanged)
        );

    private bool _isPasswordChanging;

    public BindablePasswordBox()
    {
        InitializeComponent();
    }

    public string Password
    {
        get { return (string)GetValue(PasswordProperty); }
        set { SetValue(PasswordProperty, value); }
    }

    public string Header
    {
        get { return (string)GetValue(HeaderProperty); }
        set { SetValue(HeaderProperty, value); }
    }

    public string PlaceholderText
    {
        get { return (string)GetValue(PlaceholderTextProperty); }
        set { SetValue(PlaceholderTextProperty, value); }
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        _isPasswordChanging = true;
        Password            = PasswordBox.Password;
        _isPasswordChanging = false;
    }

    private static void OnPasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is BindablePasswordBox passwordBox)
        {
            passwordBox.UpdatePassword();
        }
    }

    private void UpdatePassword()
    {
        if (!_isPasswordChanging)
        {
            PasswordBox.Password = Password;
        }
    }
}
