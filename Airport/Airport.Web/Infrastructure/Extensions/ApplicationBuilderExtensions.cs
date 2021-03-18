namespace Airport.Web.Infrastructure.Extensions
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using Airport.Common.Constants;
    using Airport.Common.Enums;
    using Airport.Data;
    using Airport.Data.Enums;
    using Airport.Data.Models;
    using System.Linq;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDatabaseMigration(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<AirportDbContext>().Database.Migrate();

                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                Task.Run(async () =>
                {
                    foreach (var roleName in Enum.GetNames(typeof(Role)))
                    {
                        var roleExists = await roleManager.RoleExistsAsync(roleName);
                        if (!roleExists)
                        {
                            await roleManager.CreateAsync(new IdentityRole()
                            {
                                Name = roleName
                            });
                        }
                    }

                    var adminRole = Role.Administrator.ToString();
                    var adminUser = await userManager.FindByEmailAsync(AdminConstants.Email);

                    if (adminUser == null)
                    {
                        adminUser = new RegularUser()
                        {
                            Email = AdminConstants.Email,
                            FirstName = AdminConstants.FirstName,
                            LastName = AdminConstants.LastName,
                            Gender = Gender.Male,
                            UserName = AdminConstants.Username   
                        };

                        await userManager.CreateAsync(adminUser, AdminConstants.Password);
                        await userManager.AddToRoleAsync(adminUser, adminRole);
                    }

                })
                .GetAwaiter()
                .GetResult();
            }

            return app;
        }

        public static IApplicationBuilder Seed(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetRequiredService<AirportDbContext>();
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                SeedTowns(db);
                SeedAirports(db);
                SeedCompanies(db, userManager);
                SeedUsers(db, userManager);
                SeedTickets(db);
            }


                return app;
        }

        private static void SeedTowns(AirportDbContext db)
        {
            if (File.Exists(WebConstants.FilePath.Towns))
            {
                var towns = File.ReadAllText(WebConstants.FilePath.Towns)
                    .Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                for (int i = 1; i < towns.Length; i++)
                {
                    var town = new Town()
                    {
                        Name = towns[i]
                    };

                    Task.Run(async () =>
                    {
                        var townExists = await db.Towns.AnyAsync(t => t.Name.ToLower() == towns[i].ToLower());

                        if (!townExists)
                        {
                            await db.Towns.AddAsync(town);
                            await db.SaveChangesAsync();
                        }
                    })
                     .Wait();
                }
            }
        }

        private static void SeedAirports(AirportDbContext db)
        {
            if (File.Exists(WebConstants.FilePath.Airports))
            {
                var airports = File.ReadAllText(WebConstants.FilePath.Airports)
                    .Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                for (int i = 1; i < airports.Length; i++)
                {
                    var airportInfo = airports[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var airportName = airportInfo[0];
                    var townId = int.Parse(airportInfo[1]);
                    var phone = airportInfo[2];

                    var airport = new Airport()
                    {
                        Name = airportName,
                        TownId = townId,
                        Phone = phone
                    };

                    Task.Run(async () =>
                    {
                        var townExists = await db.Towns.AnyAsync(t => t.Id == townId);

                        if (townExists)
                        {
                            var town = db.Towns.Include(t => t.Airports).FirstOrDefault(t => t.Id == townId);

                            var airportExists = town.Airports.Any(s => s.Name.ToLower() == airportName.ToLower() && s.Phone == phone);

                            if (!airportExists)
                            {
                                await db.Airports.AddAsync(airport);
                                await db.SaveChangesAsync();
                            }

                        }
                    })
                        .Wait();
                }

            }
        }

        private static void SeedCompanies(AirportDbContext db, UserManager<User> userManager)
        {
            if (File.Exists(WebConstants.FilePath.Companies))
            {
                var companies = File.ReadAllText(WebConstants.FilePath.Companies)
                    .Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                var random = new Random();
                var townsCount = db.Towns.Count();
                var firstTownId = db.Towns.First().Id;
                var airportsCount = db.Airports.Count();
                var firstAirportId = db.Airports.First().Id;

                for (int i = 1; i < companies.Length; i++)
                {
                    var companyInfo = companies[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var password = companyInfo[2];

                    var company = new Company()
                    {
                        UserName = companyInfo[0],
                        Email = companyInfo[1],
                        Name = companyInfo[3],
                        Logo = File.ReadAllBytes(WebConstants.FilePath.CompaniesImages + companyInfo[4]),
                        Description = companyInfo[5],
                        UniqueReferenceNumber = companyInfo[6],
                        ChiefFirstName = companyInfo[7],
                        ChiefLastName = companyInfo[8],
                        Address = companyInfo[9],
                        PhoneNumber = companyInfo[10],
                        TownId = random.Next(firstTownId, townsCount),
                        RegistrationDate = DateTime.UtcNow.ToLocalTime()
                    };

                    Task.Run(async () => 
                    {
                        var companyExists = await db.Companies
                            .AnyAsync(c => c.Name.ToLower() == company.Name.ToLower() && c.UserName == company.UserName);

                        if (!companyExists)
                        {
                            await userManager.CreateAsync(company, password);
                            await userManager.AddToRoleAsync(company, Role.Company.ToString());

                            var currentCompany = db.Companies
                                .Where(c => c.UserName == company.UserName).Include(c => c.Routes).FirstOrDefault();

                            for (int r = 1; r <= airportsCount * 4; r++)
                            {
                                var startAirportId = random.Next(firstAirportId, firstAirportId + airportsCount);
                                var endAirportId = random.Next(firstAirportId, firstAirportId + airportsCount);
                                var departureTime = new TimeSpan(random.Next(0, 23), random.Next(0, 59), 0);
                                var duration = new TimeSpan(random.Next(0, 23), random.Next(0, 59), 0);

                                if (startAirportId == endAirportId)
                                {
                                    continue;
                                }

                                var route = new Route()
                                {
                                    AircraftType = r % 2 == 0 ? AircraftType.Mini : AircraftType.Standart,
                                    StartAirportId = startAirportId,
                                    EndAirportId = endAirportId,
                                    Price = random.Next(DataConstants.Route.PriceMinValue, DataConstants.Route.PriceMaxValue),
                                    DepartureTime = departureTime,
                                    Duration = duration,
                                    IsActive = true
                                };

                                if (!currentCompany.Routes
                                    .Any(cr => cr.StartAirportId == startAirportId && cr.EndAirportId == endAirportId && cr.DepartureTime == departureTime))
                                {
                                    currentCompany.Routes.Add(route);
                                    db.SaveChanges();
                                }
                            }
                        }

                     })
                        .Wait();
                }

            }
        }

        private static void SeedUsers(AirportDbContext db, UserManager<User> userManager)
        {
            if (File.Exists(WebConstants.FilePath.Users))
            {
                var users = File.ReadAllText(WebConstants.FilePath.Users)
                    .Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                for (int i = 1; i < users.Length; i++)
                {
                    var userInfo = users[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var password = userInfo[4];
                    var username = userInfo[0];
                    var email = userInfo[1];

                    var user = new RegularUser()
                    {
                        UserName = username,
                        Email = email,
                        FirstName = userInfo[2],
                        LastName = userInfo[3]
                    };

                    Task.Run(async () =>
                    {
                        var userExists = await db.Users
                            .AnyAsync(u => u.UserName.ToLower() == username.ToLower() && u.Email.ToLower() == email.ToLower());

                        if (!userExists)
                        {
                            await userManager.CreateAsync(user, password);
                        }
                    })
                        .Wait();
                }
            }
        }


        private static void SeedTickets(AirportDbContext db)
        {
            var userIds = db.RegularUsers.Select(u => u.Id).ToList();

            var companiesRoutes = db.Companies.Select(c => new
            {
                Routes = c.Routes.Select(r => new
                {
                    Id = r.Id,
                    BusSeatsCount = (int)r.AircraftType,
                    DepartureTime = r.DepartureTime,
                    IsActive = r.IsActive
                }).ToList()

           }).ToList();

            var random = new Random();

            const int month = 3;
            const int year = 2021;
            const int startDay = 20;
            const int endDay = 23;
            const int maxRoutesCount = 10;

            Task.Run(async () =>
            {
                for (int i = 0; i < companiesRoutes.Count; i++)
                {
                    for (int r = 1; r <= maxRoutesCount; r++)
                    {
                        if (!companiesRoutes[i].Routes.Any())
                        {
                            continue;
                        }

                        var firstRouteId = companiesRoutes[i].Routes.First().Id;
                        var routeId = random.Next(firstRouteId, firstRouteId + companiesRoutes[i].Routes.Count);

                        var route = companiesRoutes[i].Routes.FirstOrDefault(ro => ro.Id == routeId);

                        if (!route.IsActive || route == null)
                        {
                            continue;
                        }

                        var totalDays = random.Next(startDay, endDay);

                        for (int day = startDay; day <= totalDays; day++)
                        {
                            var ticketDepartureTime = new DateTime(year, month, day, route.DepartureTime.Hours, route.DepartureTime.Minutes, route.DepartureTime.Seconds);

                            var totalSeats = random.Next(1, route.BusSeatsCount);

                            for (int seat = 1; seat <= totalSeats; seat++)
                            {
                                var userIdIndex = random.Next(0, userIds.Count);
                                var userId = userIds[userIdIndex];

                                if (!db.Tickets.Any(t => t.RouteId == routeId && t.DepartureTime == ticketDepartureTime && t.SeatNumber == seat ))
                                {
                                    db.Tickets.Add(new Ticket()
                                    {
                                        DepartureTime = ticketDepartureTime,
                                        IsCancelled = false,
                                        RouteId = routeId,
                                        SeatNumber = seat,
                                        UserId = userId
                                    });

                                    await db.SaveChangesAsync();
                                }
                            }
                        }
                    }
                }
            })
                .Wait();

        }




    }
}
