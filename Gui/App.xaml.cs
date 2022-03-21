using System.IO;
using System.Windows;
using Controller;
using RemotePowerSupplyGui.Config;
using Serilog;

namespace RemotePowerSupplyGui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("log.txt")
                .CreateLogger();
            var config = ConfigReader.LoadConfig<PowerSupplyConfig>(
                Path.GetFullPath(@"./config.json"));
            var powerSupply = new PowerSupplySerial(config.Name, config.ChannelCount);
            var mainWindow = new MainWindow(powerSupply);
            mainWindow.Show();
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            Log.CloseAndFlush();
        }
    }
}