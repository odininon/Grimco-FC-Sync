namespace GrimcoLib.Models;

public class Duty
{
    public int DutyId { get; set; }
    public string Name { get; set; }

    public Quest Quest { get; set; }
}