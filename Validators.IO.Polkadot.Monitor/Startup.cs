using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Validators.IO.Polkadot.Monitor.Background.Tasks;

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

			//var botOptions = new BotOptions<AlertBot>();
			//botOptions.ApiToken = "620950762:AAEZMMyfWfdvBX7dAwce1GXUqPzvHzkWHkQ";
			//botOptions.BotUserName = "polkadot_testnet_bot";

			//services.AddTelegramBot(botOptions)
			//	 .AddUpdateHandler<StartCommand>()
			//	 .Configure();

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
