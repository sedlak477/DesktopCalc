using NCalc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DesktopCalc
{
    public partial class MainWindow : System.Windows.Window
    {
        private ObservableCollection<Coin> _coins = new ObservableCollection<Coin>();

        public MainWindow()
        {
            InitializeComponent();
            datagrid_cryptoData.ItemsSource = _coins;
        }

        private void _loadCoins(string currency)
        {
            try
            {
                string url = "https://api.coinmarketcap.com/v1/ticker/";
                if (!currency.Equals("usd", StringComparison.InvariantCultureIgnoreCase))
                    url += "?convert=" + currency.ToUpper();
                using (WebClient cl = new WebClient())
                {
                    JArray data = JArray.Parse(cl.DownloadString(url));
                    _coins.Clear();
                    foreach (JObject o in data)
                        _coins.Add(new Coin()
                        {
                            Name = (string)o["name"],
                            Ticker = (string)o["symbol"],
                            BtcValue = (double?)o["price_btc"],
                            FiatValue = (double?)o["price_" + currency.ToLower()]
                        });
                }
            }
            catch { }
        }

        // --- Event Handler ---

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

        private void Window_Deactivated(object sender, EventArgs e)
        {
            BorderBrush = new SolidColorBrush(Colors.Gray);
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            BorderBrush = new SolidColorBrush(Colors.Black);
        }

        private void Reload_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _loadCoins(((ComboBoxItem)combobox_currency.SelectedItem).Content as string);
        }

        private void Value_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            textbox_expression.Text += ((sender as DataGridCell).Content as TextBlock).Text;
        }

        private void Clear_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            textbox_expression.Text = "";
        }
    }
}
