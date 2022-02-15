using System;
using System.Linq;
using CommandLine;
using GrimcoDatabase.Context;
using GrimcoLib.Models;

namespace ConsoleTool.Commands;

[Verb("generate-key", HelpText = "Generate key for a user")]
public class GenerateAPIKeyCommand : ICommand
{
    [Option('u', "userName", Required = true, HelpText = "Username to generate the apikey for")]
    public string Username { get; set; }

    public void Execute()
    {
        using var db = new GrimcoDatabaseContext();

        var user = db.Users.FirstOrDefault(user => user.Name == Username);

        if (user == null)
        {
            user = new User()
            {
                Name = Username
            };
            db.Users.Add(user);
            db.SaveChanges();

            user = db.Users.FirstOrDefault(user1 => user1.Name == Username);
        }

        Console.WriteLine($"Generating API Key for {user!.Name}");

        var apiKey = new ApiKey
        {
            UserId = user.UserId,
            Key = Guid.NewGuid().ToString()
        };

        db.ApiKeys.Add(apiKey);
        db.SaveChanges();

        Console.WriteLine($"ApiKey: {apiKey.Key}");
    }
}