using System;
using FreelanceTool.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace FreelanceTool
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var host = BuildWebHost(args);

			EnsureDbSeeding(host);

			host.Run();
		}

		public static IWebHost BuildWebHost(string[] args)
		{
			return WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>()
				.Build();
		}

		private static void EnsureDbSeeding(IWebHost host)
		{
			using (var scope = host.Services.CreateScope())
			{
				try
				{
					DbSeeder.Run(scope.ServiceProvider).Wait();
				}
				catch (Exception /*ex*/)
				{
					//var logger = services.GetRequiredService<ILogger<Program>>();
					//logger.LogError(ex, "An error occurred while seeding the database.");
					//throw ex;
				}
			}
		}
	}
}
