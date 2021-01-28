using Microsoft.EntityFrameworkCore;
using SampleApplication.Database;
using SampleApplication.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SampleApplication.DataSeeds
{
    public class Seed
    {
        public static async Task SeedUsers(DatabaseContext databaseContext)
        {
            if(!await databaseContext.Users.AnyAsync())
            {
                var userData = await File.ReadAllTextAsync("DataSeeds/UserSeed.json");

                var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

                foreach (var user in users)
                {
                    using var hmac = new HMACSHA512();

                    user.UserName = user.UserName.ToLower();
                    user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                    user.PasswordSalt = hmac.Key;

                    databaseContext.Users.Add(user);
                }

                await databaseContext.SaveChangesAsync();
            }

            return;
        }
    }
}