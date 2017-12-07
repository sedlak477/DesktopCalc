using NCalc;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DesktopCalc
{
    public partial class MainWindow : System.Windows.Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((sender as TextBox).Text == "")
                textblock.Text = "";
            else
            {
                try
                {
                    textblock.Text = new Expression((sender as TextBox).Text.Replace(',', '.')).Evaluate().ToString();
                }
                catch { }
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
            if (e.ChangedButton == MouseButton.Middle) Close();
        }

        private void Window_Deactivated(object sender, System.EventArgs e)
        {
            BorderBrush = new SolidColorBrush(Colors.Gray);
        }

        private void Window_Activated(object sender, System.EventArgs e)
        {
            BorderBrush = new SolidColorBrush(Colors.Black);
        }
    }
}
