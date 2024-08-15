using BlazorAuthTemplate.Client.Models.Enums;
using BlazorAuthTemplate.Models;
using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BlazorAuthTemplate.Data
{
    public static class DataUtility
    {

        public static string? GetConnectionString(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection"); //local connection string
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");   //RailWay connection string
            return string.IsNullOrEmpty(databaseUrl) ? connectionString : BuildConnectionString(databaseUrl);
        }

        private static string BuildConnectionString(string databaseUrl)
        {
            //Provides an object representation of a uniform resource identifier (URI) and easy access to the parts of the URI.
            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');

            var database = Environment.GetEnvironmentVariable("RAILWAY_SERVICE_NAME")
                ?? typeof(DataUtility).Assembly.GetName().Name;

            //Provides a simple way to create and manage the contents of connection strings used by the NpgsqlConnection class.
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/'),
                SslMode = SslMode.Prefer,
            };
            return builder.ToString();
        }

        public static async Task ManageDataAsync(IServiceProvider serviceProvider)
        {
            var dbContextService = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManagerService = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var configService = serviceProvider.GetRequiredService<IConfiguration>();

            await dbContextService.Database.MigrateAsync();

            // Execute the Data methods
            await SeedDemoUserAsync(userManagerService, configService);
            await SeedDemoContactsAsync(userManagerService, dbContextService, configService);
        }

        private static async Task SeedDemoUserAsync(UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            try
            {
                string? demoEmail = config["DemoUserLogin"];
                string? demoPassword = config["DemoUserPassword"];

                if (string.IsNullOrEmpty(demoEmail) || string.IsNullOrEmpty(demoPassword))
                {
                    throw new Exception("Demo User Email and/or Password not found. Seeding aborted");
                }

                ApplicationUser demoUser = new()
                {
                    UserName = demoEmail,
                    Email = demoEmail,
                    FirstName = "Demo",
                    LastName = "Login",
                    EmailConfirmed = true,
                };

                ApplicationUser? user = await userManager.FindByEmailAsync(demoEmail);

                if (user == null) 
                {
                    await userManager.CreateAsync(demoUser, demoPassword);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("********** Error **********");
                Console.WriteLine("Error Seeding Demo Login User");
                Console.WriteLine(ex.Message);
                Console.WriteLine("***************************");

                throw;
            }
        }

        private static async Task SeedDemoContactsAsync(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IConfiguration config)
        {
            string? demoEmail = config["DemoUserLogin"];
            if (string.IsNullOrEmpty(demoEmail)) return;

            var user = await userManager.FindByEmailAsync(demoEmail);
            if (user == null) return;

            var demoContacts = await context.Contacts
                .Where(c => c.AppUserId == user.Id)
                .Include(c => c.Categories)
                .ToListAsync();

            var demoCategories = await context.Categories
                .Where(c => c.AppUserId == user.Id)
                .ToListAsync();

            Random rand = new();

            if (demoContacts.Count == 0)
            {
                var newContacts = new Faker<Contact>()
                    .RuleFor(c => c.LastName, f => f.Name.LastName())
                    .RuleFor(c => c.BirthDate, f => f.Date.Between(
                        DateTime.Now - TimeSpan.FromDays(365 * 60),
                        DateTime.Now - TimeSpan.FromDays(365 * 18)
                    ))
                    .RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber())
                    .RuleFor(c => c.Address1, f => f.Address.StreetAddress())
                    .RuleFor(c => c.City, f => f.Address.City())
                    .RuleFor(c => c.State, f => f.PickRandom<State>())
                    .RuleFor(c => c.ZipCode, f => int.Parse(f.Address.ZipCode("#####")))
                    .RuleFor(c => c.AppUserId, user.Id)
                    .Generate(10);

                Faker faker = new();

                var imageDir = Path.Combine(Directory.GetCurrentDirectory(), "Data/DemoImages/");
                var mensPics = Directory.GetFiles(Path.Combine(imageDir, "Men/")).ToList();
                var womensPics = Directory.GetFiles(Path.Combine(imageDir, "Women/")).ToList();

                for (int i = 0; i < newContacts.Count; i++)
                {
                    Contact contact = newContacts[i];

                    if (i % 2 == 0)
                    {
                        contact.FirstName = faker.Name.FirstName(Bogus.DataSets.Name.Gender.Male);
                        if (mensPics.Count > 0)
                        {
                            var pic = mensPics[rand.Next(0, mensPics.Count)];
                            mensPics.Remove(pic);

                            ImageUpload image = new()
                            {
                                Data = await File.ReadAllBytesAsync(pic),
                                Extension = $"image/{Path.GetExtension(pic).TrimStart('.')}"
                            };

                            contact.Image = image;
                            context.Images.Add(image);
                        }
                    }
                    else
                    {
                        contact.FirstName = faker.Name.FirstName(Bogus.DataSets.Name.Gender.Female);
                        if (womensPics.Count > 0)
                        {
                            var pic = womensPics[rand.Next(0, womensPics.Count)];
                            womensPics.Remove(pic);

                            ImageUpload image = new()
                            {
                                Data = await File.ReadAllBytesAsync(pic),
                                Extension = $"image/{Path.GetExtension(pic).TrimStart('.')}"
                            };

                            contact.Image = image;
                            context.Images.Add(image);
                        }
                    }

                    contact.Email = faker.Internet.Email(contact.FirstName, contact.LastName, "mailinator.com");
                    if (rand.Next() % 2 == 0) contact.Address2 = new Faker().Address.SecondaryAddress();
                }

                demoContacts.AddRange(newContacts);
            }

            if (demoCategories.Count == 0)
            {
                demoCategories = [
                    new() { Name = "Family", AppUserId = user.Id },
                    new() { Name = "Friends", AppUserId = user.Id },
                    new() { Name = "Coworkers", AppUserId = user.Id },
                    new() { Name = "Clients", AppUserId = user.Id },
                    new() { Name = "School", AppUserId = user.Id },
                    new() { Name = "Gaming", AppUserId = user.Id },
                    new() { Name = "Favorites", AppUserId = user.Id },
                ];

                context.Categories.AddRange(demoCategories);
            }

            foreach (var contact in demoContacts.Where(c => c.Categories.Count == 0))
            {
                int numCategories = rand.Next(1, 5);
                var categories = demoCategories
                    .OrderBy(c => Guid.NewGuid())
                    .Take(numCategories);

                contact.Categories = [.. categories];
                context.Update(contact);
            }

            await context.SaveChangesAsync();
        }

    }

}
