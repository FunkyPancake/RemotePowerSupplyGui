using System;
using System.IO;
using Ardalis.GuardClauses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RemotePowerSupplyGui.Config;

public class AppConfig
{
    public static T Load<T>(string configPath) where T : IConfig
    {
        Guard.Against.InvalidPath(configPath, nameof(configPath));
        try
        {
            var json = JObject.Parse(File.ReadAllText(configPath));
            return json.ToObject<T>() ?? throw new InvalidOperationException();
        }
        catch (Exception e) when (e is InvalidOperationException)
        {
        }

        throw new InvalidOperationException();
    }

    public static void Save<T>(string configPath, T config) where T : IConfig
    {
        var json = JsonConvert.SerializeObject(config, Formatting.Indented);
        File.WriteAllText(configPath,json);
    }
}

public interface IConfig
{
}