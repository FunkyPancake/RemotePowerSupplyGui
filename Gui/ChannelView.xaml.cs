using System.Windows.Controls;
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
}