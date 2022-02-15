using System.Linq;
using CommandLine;
using Dalamud;
using Dalamud.Game.Text.Sanitizer;
using GrimcoDatabase.Context;
using GrimcoLib;
using GrimcoLib.Models;
using Lumina;

namespace ConsoleTool.Commands;

[Verb("update", HelpText = "Update db to latest FFXIV excel")]
public class UpdateDBCommand : ICommand
{
    public void Execute()
    {
        using var db = new GrimcoDatabaseContext();
        using var transaction = db.Database.BeginTransaction();

        var data = new GameData("F:/SquareEnix/FINAL FANTASY XIV - A Realm Reborn/game/sqpack");
        var cleaner = new Cleaner(new Sanitizer(ClientLanguage.English));

        var unlocks = new Unlocks(data);
        var instances = unlocks.Duties();

        foreach (var instance in instances)
        {
            var questName = cleaner.Convert(instance.Quest.Name).TextValue;
            var questDao = db.Quests.FirstOrDefault(q => q.Name == questName);

            if (questDao == null)
            {
                questDao = new Quest
                {
                    Name = questName
                };
                db.Quests.Add(questDao);
            }

            var instanceName = cleaner.Convert(instance.Instance.Name).TextValue;
            var instanceDao = db.Duties.FirstOrDefault(q => q.Name == instanceName);

            if (instanceDao == null)
            {
                instanceDao = new Duty
                {
                    Name = instanceName,
                    Quest = questDao
                };
                db.Duties.Add(instanceDao);
            }

            db.SaveChanges();
        }

        transaction.Commit();
    }
}