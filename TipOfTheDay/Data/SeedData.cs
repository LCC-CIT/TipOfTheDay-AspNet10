using Microsoft.EntityFrameworkCore;
using TipOfTheDay.Models;
using System.Linq;

namespace TipOfTheDay.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Look for any tips.
                if (context.Tip.Any())
                {
                    return;   // DB has been seeded
                }

                AppUser appUser = new() { UserName = "A. Member" };
                context.Users.Add(appUser);

                Tip tip1 = new() { TipText = "The first tip", Member = appUser };
                Tip tip2 = new() { TipText = "Another tip", Member = appUser };
                Tip tip3 = new() { TipText = "Yet another tip", Member = appUser };
                context.Tip.AddRange(tip1, tip2, tip3);

                Tag tag1 = new() { Category = "Cat-A" };
                Tag tag2 = new() { Category = "Cat-B" };
                Tag tag3 = new() { Category = "Cat-C" };
                context.Tag.AddRange(tag1, tag2, tag3);

                context.SaveChanges();
            }
        }
    }
}
