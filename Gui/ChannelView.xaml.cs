using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Controller;

namespace RemotePowerSupplyGui;

public partial class ChannelView : UserControl
{
    private IChannel Channel { get; }

    public ChannelView(IChannel channel)
    {
        Channel = channel;
        InitializeComponent();
        ChannelLabel.Content = $"Channel {Channel.Id}";
    }

    private void OutputButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (Channel.OutputEnable)
        {
            Channel.OutputEnable = true;
            OutputButton.Content = "Disable";
        }
        else
        {
            Channel.OutputEnable = false;
            OutputButton.Content = "Enable";
        }
    }

    public void RefreshView()
    {
        VoltageTextBox.Text = Channel.Voltage.ToString("F3",CultureInfo.InvariantCulture);
        CurrentTextBox.Text = Channel.Current.ToString("F3",CultureInfo.InvariantCulture);
        if (Channel.OutputEnable)
        {
            OutputStatusTextBox.Text = "On";
            OutputStatusTextBox.Background = new SolidColorBrush(Colors.Chartreuse);
        }
        else
        {
            OutputStatusTextBox.Text = "Off";
            OutputStatusTextBox.Background = new SolidColorBrush(Colors.Crimson);
        }
    }
}