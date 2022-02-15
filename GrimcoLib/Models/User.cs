using System.Collections.Generic;

namespace GrimcoLib.Models;

public class User
{
    public int UserId { get; set; }
    public string Name { get; set; }

    public List<ApiKey> ApiKeys { get; set; }
    public List<Character> Characters { get; set; }
}