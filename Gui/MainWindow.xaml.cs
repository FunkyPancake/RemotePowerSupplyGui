using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Controller;

namespace RemotePowerSupplyGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IPowerSupply PowerSupply { get; }
        private readonly DispatcherTimer _timer;

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

            _timer = new DispatcherTimer(new TimeSpan((int) 1e7), DispatcherPriority.Normal, OnTimerElapsed,
                Dispatcher.CurrentDispatcher)
            {
                IsEnabled = false
            };
        }


        private void OnTimerElapsed(object? state, EventArgs eventArgs)
        {
            PowerSupply.RefreshData();
        }

        private void ConnectButton_OnClick(object sender, RoutedEventArgs e)
        {
            var isConnected = PowerSupply.IsConnected;
            if (isConnected)
            {
                PowerSupply.Disconnect();
                _timer.IsEnabled = false;
            }
            else
            {
                PowerSupply.Connect();
                _timer.IsEnabled = true;
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