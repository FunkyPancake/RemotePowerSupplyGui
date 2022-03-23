namespace RemotePowerSupplyGui.Config;

public class PowerSupplyConfig : IConfig
{
    public string Name = null!;
    public int ChannelCount;
    public string LastComPort = null!;
}