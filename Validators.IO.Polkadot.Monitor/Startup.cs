using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Validators.IO.Polkadot.Monitor.Authentication;
using Validators.IO.Polkadot.Monitor.Background.Tasks;

/// <summary>
/// Author: Validators.com
/// Use with own risk.
/// Open source. 
/// </summary>
namespace Validators.IO.Polkadot.Monitor
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
			// AppSettings.json
			//
			services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

			// Background tasks
			//
			services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, MonitorTask>();
			services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, BotTask>();

			// Cookies
			//
			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{
					options.Cookie.Expiration = TimeSpan.FromHours(24);
				});

			// Simple user store for admin login. Uses "AdminPassword" from appsettings.json
			//
			services.AddDefaultIdentity<IdentityUser>()
				.AddUserStore<InMemoryUserStore>()
				.AddDefaultTokenProviders();

			// Website
			//
			services.AddMvc(config =>
			{
				// All MvcControllers are hereby "Authorize". Use AllowAnonymous to give anon access
				//
				var policy = new AuthorizationPolicyBuilder()
				 .RequireAuthenticatedUser()
				 .Build();
				config.Filters.Add(new AuthorizeFilter(policy));
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");

				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}


			app.UseHttpsRedirection();

			app.UseStaticFiles();
			app.UseAuthentication();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
