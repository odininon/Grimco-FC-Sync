using System.Security.Claims;
using GrimcoDatabase.Context;
using GrimcoLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DutiesController : ControllerBase
{
    private readonly GrimcoDatabaseContext _context;

    public DutiesController(GrimcoDatabaseContext context)
    {
        _context = context;
    }

    [HttpGet("all")]
    public async Task<List<Duty>> GetAllDuties()
    {
        return await _context.Duties.Include(duty => duty.Quest).ToListAsync();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create(CharacterDutyUnlocks characterDutyUnlocks)
    {
        var character =
            _context.Characters.FirstOrDefault(character => character.Name == characterDutyUnlocks.Character);
        if (character == null && characterDutyUnlocks.Duties == null)
        {
            return BadRequest();
        }

        var unlockedDuties = characterDutyUnlocks.Duties!
            .Select(dutyNam => _context.Duties.First(duty => duty.Name == dutyNam));

        foreach (var duty in unlockedDuties)
        {
            var unlockedDuty = _context.UnlockedDuties.FirstOrDefault(unlockedDuty =>
                unlockedDuty.DutyId == duty.DutyId && unlockedDuty.CharacterId == character!.CharacterId);

            if (unlockedDuty != null) continue;

            unlockedDuty = new UnlockedDuty
            {
                CharacterId = character!.CharacterId,
                DutyId = duty.DutyId
            };
            _context.UnlockedDuties.Add(unlockedDuty);
            _context.SaveChanges();
        }

        return Ok("testing");
    }

    [HttpGet("me")]
    [Authorize]
    public Task<List<CharacterDutyUnlocks>> GetCharacterDuties()
    {
        var result = new List<CharacterDutyUnlocks>();

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var characters = _context.Characters.Where(character => character.UserId == userId).ToList();

        foreach (var character in characters)
        {
            var unlockedDuties = _context.UnlockedDuties.Where(duty => duty.CharacterId == character.CharacterId)
                .Select(duty => duty.DutyId)
                .Select(dutyId => _context.Duties.First(duty => duty.DutyId == dutyId))
                .Select(duty => duty.Name)
                .ToList();

            var unlocks = new CharacterDutyUnlocks
            {
                Character = character.Name,
                Duties = unlockedDuties
            };

            result.Add(unlocks);
        }

        return Task.FromResult(result);
    }
}