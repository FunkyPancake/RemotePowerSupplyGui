using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        VoltageTextBox.Text = Channel.Voltage.ToString("F3", CultureInfo.InvariantCulture);
        CurrentTextBox.Text = Channel.Current.ToString("F3", CultureInfo.InvariantCulture);
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

    private void CurrentSetpointTextBox_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            if (double.TryParse(CurrentSetpointTextBox.Text, out var value))
                Channel.Current = (decimal) value;
        }
    }

    private void VoltageSetpointTextBox_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            if (double.TryParse(VoltageSetpointTextBox.Text, out var value))
                Channel.Voltage = (decimal) value;
        }
    }
}