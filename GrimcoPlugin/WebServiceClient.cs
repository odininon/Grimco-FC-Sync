using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Logging;
using GrimcoLib;
using GrimcoLib.Models;
using Lumina.Excel.GeneratedSheets;
using Newtonsoft.Json;

namespace GrimcoPlugin;

public class WebServiceClient
{
    private readonly Cleaner _cleaner;
    private readonly Configuration _configuration;
    private readonly HttpClient _httpClient;

    public WebServiceClient(Cleaner cleaner, Configuration configuration)
    {
        _cleaner = cleaner;
        _configuration = configuration;
        _httpClient = new HttpClient();
    }

    public void UpdateDuties(PlayerCharacter character, IEnumerable<ContentFinderCondition> duty)
    {
        var characterDutyUnlocks = new CharacterDutyUnlocks
        {
            Character = character.Name.ToString(),
            Duties = duty.Select(finderCondition => _cleaner.Convert(finderCondition.Name).TextValue).ToList()
        };

        _httpClient.DefaultRequestHeaders.Add("X-Api-Key", _configuration.ApiKey);
        Task.Run(async () =>
        {
            var json = JsonConvert.SerializeObject(characterDutyUnlocks);
            var resp = await _httpClient.PostAsJsonAsync($"{_configuration.EndPoint}/api/Duties", characterDutyUnlocks);
            var output = await resp.Content.ReadAsStringAsync();

            PluginLog.Log(output);
        });
    }
}