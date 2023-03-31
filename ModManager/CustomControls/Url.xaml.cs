using Core.Model;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ModManager.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для Url.xaml
    /// </summary>
    public partial class Url : UserControl
    {
        public static readonly DependencyProperty LinkProperty =
            DependencyProperty.Register("Link", typeof(string), typeof(Url));
        public string Link
        {
            get { return (string)GetValue(LinkProperty); }
            set { SetValue(LinkProperty, value); }
        }

        public Url()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            string url = e.Uri.AbsoluteUri;
            try
            {
                Process.Start(new ProcessStartInfo("chrome.exe", url));
            }
            catch (Win32Exception)
            {
                Process.Start(new ProcessStartInfo(@"C:\Program Files\Google\Chrome\Application\chrome.exe", url));
            }
            e.Handled = true;
        }
    }
}
