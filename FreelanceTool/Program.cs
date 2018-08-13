using System;
using FreelanceTool.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FreelanceTool
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var host = BuildWebHost(args);

			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				var context = services.GetRequiredService<ApplicationDataContext>();
				context.Database.Migrate();
				var config = services.GetRequiredService<IConfiguration>();
				var defaultUserPassword = config["seededUserPwd"];
				try
				{
					DbInitializer.Init(services, defaultUserPassword).Wait();
					//var context = services.GetRequiredService<ApplicationDataContext>();
					//DbInitializer.SeedData(context);
				}
				catch (Exception ex)
				{
					//var logger = services.GetRequiredService<ILogger<Program>>();
					//logger.LogError(ex, "An error occurred while seeding the database.");
					throw ex;
				}
			}

			host.Run();
		}

		public static IWebHost BuildWebHost(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>()
				.Build();
	}
}
