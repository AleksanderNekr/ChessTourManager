using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ChessTourManager.WPF.Helpers.BindablePassword
{
    /// <summary>
    /// Логика взаимодействия для BindablePasswordBox.xaml
    /// </summary>
    public partial class BindablePasswordBox
    {
        public BindablePasswordBox()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _isPasswordChanging = true;
            Password            = PasswordBox.Password;
            _isPasswordChanging = false;
        }

        private bool _isPasswordChanging;

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

        private static void OnPasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BindablePasswordBox passwordBox)
            {
                passwordBox.UpdatePassword();
            }
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

        private void UpdatePassword()
        {
            if (!_isPasswordChanging)
            {
                PasswordBox.Password = Password;
            }
        }
    }
}
