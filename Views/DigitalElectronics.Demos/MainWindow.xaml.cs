using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DigitalElectronics.Demos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly string s_CurrentNamespace = typeof(MainWindow).Namespace!;

        public MainWindow()
        {
            InitializeComponent();

            frameworkVersionTextBox.Text = RuntimeInformation.FrameworkDescription;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string? tag = (sender as Button)?.Tag as string;

            if (!string.IsNullOrWhiteSpace(tag))
            {
                var windowTypeName = s_CurrentNamespace + "." + tag;
                var window =
                    Application.Current.Windows.OfType<Window>()
                        .FirstOrDefault(_ => _.Tag is string s && s == windowTypeName) ??
                    Assembly.GetExecutingAssembly().CreateInstance(windowTypeName) as Window;

                if (window != null)
                {
                    window.Tag = windowTypeName;
                    window.Show();
                    window.Activate();
                }

            }
        }
    }
}
