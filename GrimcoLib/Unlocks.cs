#if NET8_0_WINDOWS
using System.Collections.Generic;
using System.Linq;
using Lumina;
using Lumina.Excel.GeneratedSheets;

namespace GrimcoLib;

public class Unlocks(GameData dataManager)
{
    public List<InstanceQuest> Duties()
    {
        var instanceQuests = new List<InstanceQuest>();
        var linkedInstances = new HashSet<ContentFinderCondition>();

        foreach (var quest in dataManager.GetExcelSheet<Quest>()!)
        {
            if (quest.Name.RawString.Length == 0 || quest.RowId == 65536) continue;

            var instances = InstanceUnlocks(quest, linkedInstances);
            if (instances.Count > 0)
            {
                foreach (var instance in instances)
                    instanceQuests.Add(new InstanceQuest
                    {
                        Instance = instance,
                        Quest = quest
                    });

                foreach (var instance in instances) linkedInstances.Add(instance);
            }
        }

        return instanceQuests;
    }

    private HashSet<ContentFinderCondition> InstanceUnlocks(Quest quest,
        ICollection<ContentFinderCondition> others)
    {
        if (quest.IsRepeatable) return new HashSet<ContentFinderCondition>();

        var unlocks = new HashSet<ContentFinderCondition>();

        if (quest.InstanceContentUnlock.Row != 0)
        {
            var cfc = dataManager.GetExcelSheet<ContentFinderCondition>()!.FirstOrDefault(cfc =>
                cfc.Content == quest.InstanceContentUnlock.Row && cfc.ContentLinkType == 1);
            if (cfc != null && cfc.UnlockQuest.Row == 0) unlocks.Add(cfc);
        }

        var instanceRefs = quest.ScriptInstruction
            .Zip(quest.ScriptArg, (ins, arg) => (ins, arg))
            .Where(x => x.ins.RawString.StartsWith("INSTANCEDUNGEON"));

        foreach (var reference in instanceRefs)
        {
            var key = reference.arg;

            var cfc = dataManager.GetExcelSheet<ContentFinderCondition>()!.FirstOrDefault(cfc =>
                cfc.Content == key && cfc.ContentLinkType == 1);
            if (cfc == null || cfc.UnlockQuest.Row != 0 || others.Contains(cfc)) continue;

            if (!quest.ScriptInstruction.Any(i =>
                    i.RawString == "UNLOCK_ADD_NEW_CONTENT_TO_CF" || i.RawString.StartsWith("UNLOCK_DUNGEON")))
                if (quest.ScriptInstruction.Any(i => i.RawString.StartsWith("LOC_ITEM")))
                    continue;

            unlocks.Add(cfc);
        }

        return unlocks;
    }
}
#endif