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

            _timer = new DispatcherTimer(new TimeSpan((int) 2e6), DispatcherPriority.Normal, OnTimerElapsed,
                Dispatcher.CurrentDispatcher)
            {
                IsEnabled = false
            };
        }

        private IPowerSupply PowerSupply { get; }


        private void OnTimerElapsed(object? state, EventArgs eventArgs)
        {
            PowerSupply.RefreshData();
            foreach (ChannelView channel in ChannelGrid.Children)
            {
                channel.RefreshView();
            }
        }

        private void ConnectButton_OnClick(object sender, RoutedEventArgs e)
        {
            var isConnected = PowerSupply.IsConnected;
            if (isConnected)
            {
                _timer.IsEnabled = false;
                PowerSupply.Disconnect();
            }
            else
            {
                var connected = PowerSupply.Connect();
                if (!connected)
                {
                    return;
                }
                _timer.IsEnabled = true;
                foreach (ChannelView channel in ChannelGrid.Children)
                {
                    channel.InitSetpoints();
                }
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