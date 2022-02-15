using System;
using Dalamud.Configuration;
using Dalamud.Plugin;

namespace GrimcoPlugin;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;

    public string ApiKey { get; set; } = "";
    public string EndPoint { get; set; } = "";

    // the below exist just to make saving less cumbersome

    [NonSerialized] private DalamudPluginInterface? pluginInterface;

    public void Initialize(DalamudPluginInterface pluginInterface)
    {
        this.pluginInterface = pluginInterface;
    }

    public void Save()
    {
        this.pluginInterface!.SavePluginConfig(this);
    }
}