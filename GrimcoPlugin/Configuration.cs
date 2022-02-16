using System;
using Dalamud.Configuration;
using Dalamud.Plugin;

namespace GrimcoPlugin;

[Serializable]
public class Configuration : IPluginConfiguration
{
    // the below exist just to make saving less cumbersome

    [NonSerialized] private DalamudPluginInterface? pluginInterface;

    public string ApiKey { get; set; } = "";
    public string EndPoint { get; set; } = "";
    public int Version { get; set; } = 0;

    public void Initialize(DalamudPluginInterface pluginInterface)
    {
        this.pluginInterface = pluginInterface;
    }

    public void Save()
    {
        pluginInterface!.SavePluginConfig(this);
    }
}