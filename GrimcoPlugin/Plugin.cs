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
    private const string commandName = "/grimco";

    public Plugin(
        [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
        [RequiredVersion("1.0")] CommandManager commandManager,
        [RequiredVersion("1.0")] DataManager dataManager,
        [RequiredVersion("1.0")] ClientState clientState)
    {
        DataManager = dataManager;
        ClientState = clientState;
        PluginInterface = pluginInterface;
        CommandManager = commandManager;
        Common = new XivCommonBase();

        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        Configuration.Initialize(PluginInterface);
        var webServiceClient = new WebServiceClient(new Cleaner(pluginInterface.Sanitizer), Configuration);
        var unlocks = new Unlocks(dataManager.GameData);

        // you might normally want to embed resources and load them from the manifest stream
        PluginUi = new PluginUI(this, Configuration, webServiceClient, unlocks);

        CommandManager.AddHandler(commandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "A useful message to display in /xlhelp"
        });

        PluginInterface.UiBuilder.Draw += DrawUI;
        PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
    }

    private DalamudPluginInterface PluginInterface { get; }
    private CommandManager CommandManager { get; }
    private Configuration Configuration { get; }
    private DataManager DataManager { get; }
    public ClientState ClientState { get; }
    private PluginUI PluginUi { get; }
    public XivCommonBase Common { get; set; }
    public string Name => "Gricmo GrimcoPlugin";


    public void Dispose()
    {
        PluginUi.Dispose();
        Common.Dispose();
        CommandManager.RemoveHandler(commandName);
    }

    private void OnCommand(string command, string args)
    {
        // in response to the slash command, just display our main ui
        PluginUi.Visible = true;
    }

    private void DrawUI()
    {
        PluginUi.Draw();
    }

    private void DrawConfigUI()
    {
        PluginUi.SettingsVisible = true;
    }
}