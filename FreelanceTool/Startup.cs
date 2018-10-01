using System;
using System.Collections.Generic;
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
using Microsoft.Extensions.Options;

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
			services.AddDbContext<ApplicationDbContext>(
				options =>
				{
					options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
				});

			// Add authentication service
			services.AddIdentity<ApplicationUser, IdentityRole>(
				options =>
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
					//options.User.RequireUniqueEmail = true;
				})
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			// Add application custom services.
			services.AddTransient<IEmailSender, EmailSender>();
			services.AddTransient<AppLocalizer>();

			// Add and configure supported cultures and localization options.
			services.AddLocalization(options =>
			{
				options.ResourcesPath = "Resources";
			});
			services.Configure<RequestLocalizationOptions>(options =>
			{
				options.DefaultRequestCulture = GetDefaultCulture();
				options.SupportedCultures = GetSupportedCultures();
				options.SupportedUICultures = GetSupportedCultures();
			});

			// Add and configure MVC
			services.AddMvc(config =>
				{
					var policy = new AuthorizationPolicyBuilder()
						.RequireAuthenticatedUser()
						.Build();
					config.Filters.Add(new AuthorizeFilter(policy));

					//config.Filters.Add(new ValidateAntiForgeryTokenAttribute());
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

			// Instead of again specifying options for the UseRequestLocalization
			// middleware we can use the same options specified above when the
			// RequestLocalizationOptions option was configured in services.
			/*app.UseRequestLocalization(new RequestLocalizationOptions
			{
				DefaultRequestCulture = GetDefaultCulture(),
				// Formatting numbers, dates, etc.
				SupportedCultures = GetSupportedCultures(),
				// UI strings that we have localized.
				SupportedUICultures = GetSupportedCultures()
			});*/
			app.UseRequestLocalization(
				app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value);

			app.UseStaticFiles();

			app.UseAuthentication();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Applicants}/{action=Create}/{id?}");
			});
		}

		private List<CultureInfo> GetSupportedCultures()
		{
			return new List<CultureInfo>
			{
				new CultureInfo("en"),
				new CultureInfo("de"),
				new CultureInfo("fr")
			};
		}

		private RequestCulture GetDefaultCulture()
		{
			return new RequestCulture("de");
		}
	}
}
