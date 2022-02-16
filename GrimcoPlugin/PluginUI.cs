using System;
using System.Collections.Generic;
using System.Numerics;
using GrimcoLib;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;

namespace GrimcoPlugin;

// It is good to have this be disposable in general, in case you ever need it
// to do any cleanup
internal class PluginUI : IDisposable
{
    private readonly Unlocks _unlocks;
    private readonly WebServiceClient _webServiceClient;
    private readonly Configuration configuration;

    private bool settingsVisible;

    // this extra bool exists for ImGui, since you can't ref a property

    // passing in the image here just for simplicity
    public PluginUI(Plugin plugin, Configuration configuration, WebServiceClient webServiceClient, Unlocks unlocks)
    {
        Plugin = plugin;
        this.configuration = configuration;
        _webServiceClient = webServiceClient;
        _unlocks = unlocks;
    }

    public Plugin Plugin { get; }

    public bool Visible { get; set; } = false;

    public bool SettingsVisible
    {
        get => settingsVisible;
        set => settingsVisible = value;
    }

    public void Dispose()
    {
    }

    public void Draw()
    {
        // This is our only draw handler attached to UIBuilder, so it needs to be
        // able to draw any windows we might have open.
        // Each method checks its own visibility/state to ensure it only draws when
        // it actually makes sense.
        // There are other ways to do this, but it is generally best to keep the number of
        // draw delegates as low as possible.

        DrawMainWindow();
        DrawSettingsWindow();
    }

    public void DrawMainWindow()
    {
        if (!Visible) return;

        ImGui.SetNextWindowSize(new Vector2(375, 330), ImGuiCond.FirstUseEver);
        ImGui.SetNextWindowSizeConstraints(new Vector2(375, 330), new Vector2(float.MaxValue, float.MaxValue));
        if (ImGui.Button("Update Duties"))
        {
            var instanceQuests = _unlocks.Duties();

            var duties = new List<ContentFinderCondition>();

            foreach (var instanceQuest in instanceQuests)
                if (Plugin.Common.Functions.Journal.IsQuestCompleted(instanceQuest.Quest))
                    duties.Add(instanceQuest.Instance);

            _webServiceClient.UpdateDuties(Plugin.ClientState.LocalPlayer!, duties);
        }

        ImGui.End();
    }

    public void DrawSettingsWindow()
    {
        if (!SettingsVisible) return;

        ImGui.SetNextWindowSize(new Vector2(232, 75), ImGuiCond.Always);
        if (ImGui.Begin("Grimco Configuration Window", ref settingsVisible,
                ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar |
                ImGuiWindowFlags.NoScrollWithMouse))
        {
            // can't ref a property, so use a local copy
            var apiKey = configuration.ApiKey;
            if (ImGui.InputText("ApiKey", ref apiKey, 36))
            {
                configuration.ApiKey = apiKey;
                configuration.Save();
            }

            var endpoint = configuration.EndPoint;
            if (ImGui.InputText("EndPoint", ref endpoint, 36))
            {
                configuration.EndPoint = endpoint;
                configuration.Save();
            }
        }

        ImGui.End();
    }
}