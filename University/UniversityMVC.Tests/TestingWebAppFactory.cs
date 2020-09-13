using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityMVC.Data;
using UniversityMVC.Tests.Helpers;

namespace UniversityMVC.Tests
{
    public class TestingWebAppFactory<T> : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<UniversityContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

                services.AddDbContext<UniversityContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryEmployeeTest");
                    options.UseInternalServiceProvider(serviceProvider);
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    //using (var appContext = scope.ServiceProvider.GetRequiredService<UniversityContext>())
                    //{
                    //    try
                    //    {
                    //        appContext.Database.EnsureCreated();
                    //    }
                    //    catch (Exception)
                    //    { //Log errors or do anything you think it's needed throw;
                    //    }
                    //}
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<UniversityContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<TestingWebAppFactory<T>>>();
                    db.Database.EnsureCreated();

                    try
                    {
                        Utilities.InitializeDbForTests(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the " +
                            "database with test messages. Error: {Message}", ex.Message);
                    }
                }
            });
        }
    }
}
