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
        this.InitializeComponent();
    }

    public string Password
    {
        get { return (string)this.GetValue(PasswordProperty); }
        set { this.SetValue(PasswordProperty, value); }
    }

    public string Header
    {
        get { return (string)this.GetValue(HeaderProperty); }
        set { this.SetValue(HeaderProperty, value); }
    }

    public string PlaceholderText
    {
        get { return (string)this.GetValue(PlaceholderTextProperty); }
        set { this.SetValue(PlaceholderTextProperty, value); }
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        this._isPasswordChanging = true;
        this.Password            = this.PasswordBox.Password;
        this._isPasswordChanging    = false;
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
        if (!this._isPasswordChanging)
        {
            this.PasswordBox.Password = this.Password;
        }
    }
}
