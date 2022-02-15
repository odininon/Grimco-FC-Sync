using System.Collections.Generic;

namespace GrimcoLib.Models;

public class Character
{
    public int CharacterId { get; set; }
    public int UserId { get; set; }

    public string Name { get; set; }

    public List<UnlockedDuty> DutiesUnlocked { get; set; }
}