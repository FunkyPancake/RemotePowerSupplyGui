using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Controller;

namespace RemotePowerSupplyGui;

public partial class ChannelView : UserControl
{
    public ChannelView(IChannel channel)
    {
        Channel = channel;
        InitializeComponent();
        ChannelLabel.Content = $"Channel {Channel.Id}";
    }

    private IChannel Channel { get; }

    public void InitSetpoints()
    {
        var (voltage, current) = Channel.GetSetpoints();
        VoltageSetpointTextBox.Text = voltage.ToString(CultureInfo.InvariantCulture);
        CurrentTextBox.Text = current.ToString(CultureInfo.InvariantCulture);
        SetOutputButtonContent();
    }

    private void SetOutputButtonContent()
    {
        if (Channel.OutputEnable)
        {
            OutputButton.Content = "Disable";
        }
        else
        {
            OutputButton.Content = "Enable";
        }
    }

    private void OutputButton_OnClick(object sender, RoutedEventArgs e)
    {
        Channel.OutputEnable = !Channel.OutputEnable;
        SetOutputButtonContent();
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
                Channel.Current = value;
        }
    }

    private void VoltageSetpointTextBox_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            if (double.TryParse(VoltageSetpointTextBox.Text, out var value))
                Channel.Voltage = value;
        }
    }
}