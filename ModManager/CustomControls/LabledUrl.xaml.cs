using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ModManager.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для LabledUrl.xaml
    /// </summary>
    public partial class LabledUrl : UserControl
    {
        public static readonly DependencyProperty HyperLinkNameProperty =
            DependencyProperty.Register("HyperlinkName", typeof(string), typeof(LabledUrl), new PropertyMetadata(""));
        public event RequestNavigateEventHandler RequestNavigate;


        public string HyperlinkName
        {
            get { return (string)GetValue(HyperLinkNameProperty); }
            set { SetValue(HyperLinkNameProperty, value);}
        }
        public LabledUrl()
        {
            InitializeComponent();
            var hyperlink = (Hyperlink)this.FindName(HyperlinkName);
            hyperlink.RequestNavigate += Hyperlink_RequestNavigate;
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            RequestNavigate?.Invoke(this, e);
        }
    }
}
