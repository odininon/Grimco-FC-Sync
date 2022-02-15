using Dalamud.Data;
using Dalamud.Game.ClientState;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using GrimcoLib;
using XivCommon;

namespace GrimcoPlugin;

public sealed class Plugin : IDalamudPlugin
{
    public string Name => "Gricmo GrimcoPlugin";

    private const string commandName = "/grimco";

    private DalamudPluginInterface PluginInterface { get; init; }
    private CommandManager CommandManager { get; init; }
    private Configuration Configuration { get; init; }
    private DataManager DataManager { get; init; }
    public ClientState ClientState { get; }
    private PluginUI PluginUi { get; init; }
    public XivCommonBase Common { get; set; }

    public Plugin(
        [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
        [RequiredVersion("1.0")] CommandManager commandManager,
        [RequiredVersion("1.0")] DataManager dataManager,
        [RequiredVersion("1.0")] ClientState clientState)
    {
        this.DataManager = dataManager;
        ClientState = clientState;
        this.PluginInterface = pluginInterface;
        this.CommandManager = commandManager;
        this.Common = new XivCommonBase();

        this.Configuration = this.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        this.Configuration.Initialize(this.PluginInterface);
        var webServiceClient = new WebServiceClient(new Cleaner(pluginInterface.Sanitizer), this.Configuration);
        var unlocks = new Unlocks(dataManager.GameData);

        // you might normally want to embed resources and load them from the manifest stream
        this.PluginUi = new PluginUI(this, this.Configuration, webServiceClient, unlocks);

        this.CommandManager.AddHandler(commandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "A useful message to display in /xlhelp"
        });

        this.PluginInterface.UiBuilder.Draw += DrawUI;
        this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
    }


    public void Dispose()
    {
        this.PluginUi.Dispose();
        this.Common.Dispose();
        this.CommandManager.RemoveHandler(commandName);
    }

    private void OnCommand(string command, string args)
    {
        // in response to the slash command, just display our main ui
        this.PluginUi.Visible = true;
    }

    private void DrawUI()
    {
        this.PluginUi.Draw();
    }

    private void DrawConfigUI()
    {
        this.PluginUi.SettingsVisible = true;
    }
}