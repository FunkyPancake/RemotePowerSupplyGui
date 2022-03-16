using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Controller;

namespace RemotePowerSupplyGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IPowerSupply PowerSupply { get; }

        public MainWindow(IPowerSupply powerSupply)
        {
            PowerSupply = powerSupply;
            InitializeComponent();
            PowerSupplyNameLabel.Content = powerSupply.Name;
            foreach (var (channel, index) in powerSupply.Channels.Select((channel, index) => (channel, index)))
            {
                ChannelGrid.ColumnDefinitions.Add(new ColumnDefinition());
                var panel = new ChannelView(channel);
                Grid.SetColumn(panel, index);
                ChannelGrid.Children.Add(panel);
                ConnectionUpdateContent();
            }
        }

        private void ConnectButton_OnClick(object sender, RoutedEventArgs e)
        {
            var isConnected = PowerSupply.IsConnected;
            if (isConnected)
            {
                PowerSupply.Disconnect();
            }
            else
            {
                PowerSupply.Connect();
            }

            ConnectionUpdateContent();
        }

        private void ConnectionUpdateContent()
        {
            if (PowerSupply.IsConnected)
            {
                ConnectButton.Content = "Disconnect";
                ConnectStatusLabel.Content = "Connected";
                ConnectStatusLabel.Background = new SolidColorBrush(Colors.Chartreuse);
            }
            else
            {
                ConnectButton.Content = "Connect";
                ConnectStatusLabel.Content = "Disconnected";
                ConnectStatusLabel.Background = new SolidColorBrush(Colors.Crimson);
            }
        }
    }
}