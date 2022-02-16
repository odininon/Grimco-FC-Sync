using System.Collections.Generic;

namespace GrimcoLib.Models;

public class CharacterDutyUnlocks
{
    public string Character { get; set; }

    public List<string> Duties { get; set; }
}