using System;
using System.Globalization;
using FreelanceTool.Data;
using FreelanceTool.Helpers;
using FreelanceTool.Models;
using FreelanceTool.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FreelanceTool
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// Add data context
			services.AddDbContext<ApplicationDataContext>(
				options =>
				{
					options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
				});

			// Add authentication service
			services.AddIdentity<ApplicationUser, IdentityRole>(options =>
				{
					// Lockout settings
					options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
					options.Lockout.MaxFailedAccessAttempts = 3;
					options.Lockout.AllowedForNewUsers = true;

					// Password settings
					options.Password.RequireDigit = false;
					options.Password.RequireNonAlphanumeric = false;
					options.Password.RequireUppercase = false;

					// User settings
					options.User.RequireUniqueEmail = true;
				})
				.AddEntityFrameworkStores<ApplicationDataContext>()
				.AddDefaultTokenProviders();

			// Add application custom services.
			services.AddTransient<IEmailSender, EmailSender>();
			services.AddTransient<AppLocalizer>();

			// Add and configure supported cultures and localization options
			services.AddLocalization(options => options.ResourcesPath = "Resources");
			services.Configure<RequestLocalizationOptions>(options =>
			{
				var enCulture = "en";
				var supportedCultures = new[]
				{
					new CultureInfo(enCulture),
					new CultureInfo("de"),
					new CultureInfo("fr")
				};
				options.DefaultRequestCulture = new RequestCulture(enCulture);
				options.SupportedCultures = supportedCultures;
				options.SupportedUICultures = supportedCultures;
			});

			// Add and configure MVC
			services.AddMvc(config =>
				{
					var policy = new AuthorizationPolicyBuilder()
						.RequireAuthenticatedUser()
						.Build();
					config.Filters.Add(new AuthorizeFilter(policy));
				})
				.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
				.AddDataAnnotationsLocalization();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			// Configure error handling
			if (env.IsDevelopment())
			{
				//app.UseBrowserLink();
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}
			app.UseStatusCodePages();

			// Configure cultures and localization options
			var enCulture = "en";
			var supportedCultures = new[]
			{
				new CultureInfo(enCulture),
				new CultureInfo("de"),
				new CultureInfo("fr")
			};
			app.UseRequestLocalization(new RequestLocalizationOptions
			{
				DefaultRequestCulture = new RequestCulture(enCulture),
				// Formatting numbers, dates, etc.
				SupportedCultures = supportedCultures,
				// UI strings that we have localized.
				SupportedUICultures = supportedCultures
			});

			//app.UseRewriter(new RewriteOptions().AddRedirectToHttpsPermanent());
			app.UseStaticFiles();

			//app.UseCookiePolicy();
			app.UseAuthentication();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Applicants}/{action=Create}/{id?}");
			});
		}
	}
}
