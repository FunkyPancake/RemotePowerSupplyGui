using System;
using System.Globalization;
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
        private PowerSupplySerial _powerSupply = null!;
        private const string ConfigJson = @"./config.json";


        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var today = DateTime.Today.ToString("dd-MM-yyyy",CultureInfo.InvariantCulture);
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File($"log-{today}.txt")
                .CreateLogger();
            
            var config = AppConfig.Load<PowerSupplyConfig>(
                Path.GetFullPath(ConfigJson));
            
            _powerSupply = new PowerSupplySerial(Log.Logger, config.Name, config.ChannelCount, config.LastComPort);
            var mainWindow = new MainWindow(_powerSupply);
            mainWindow.Show();
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            Log.CloseAndFlush();
            var config = AppConfig.Load<PowerSupplyConfig>(
                Path.GetFullPath(@"./config.json"));
            config.LastComPort = _powerSupply.ComPort;
            AppConfig.Save(ConfigJson, config);
        }
    }
}