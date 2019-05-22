using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot;
using Validators.IO.Polkadot.Monitor.Database;
using Validators.IO.Polkadot.Monitor.HttpClients;

namespace Validators.IO.Polkadot.Monitor.Background.Tasks
{
	public class MonitorTask : ScheduledProcessor
	{
		private readonly AppSettings settings;
		private readonly ILogger logger;
		private DateTime? lastAlert;

		public MonitorTask(IServiceScopeFactory serviceScopeFactory, IOptions<AppSettings> settings, ILogger<MonitorTask> logger)
			 : base(serviceScopeFactory, "*/" + settings.Value.MonitorTaskFrequencyInSeconds + " * * * * *")
		{
			this.settings = settings.Value;
			this.logger = logger;
		}

		public override async Task ProcessInScope(IServiceProvider serviceProvider)
		{
			try
			{
				await Executing(serviceProvider);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "MonitorTask exception");
			}
		}

		public async Task Executing(IServiceProvider serviceProvider)
		{
			try
			{
				var now = SystemClock.Instance.GetCurrentInstant();
				var alerts = new List<string>();

				using (var db = new AppDatabase())
				{
					var nodes = db.Nodes;

					foreach (var node in nodes.FindAll().ToList())
					{
						using (var client = new PolkadotClient(node.Url))
						{
							try
							{
								var health = await client.GetSystemHealth();
								var chain = await client.GetSystemChain();
								var nameAndVersion = await client.GetSystemNameAndVersion();

								node.IsSyncing = health.IsSyncing;
								node.Peers = health.Peers;
								node.Chain = chain;
								node.NameAndVersion = nameAndVersion;

								node.LastUpdatedUtc = now.InUtc().ToDateTimeUtc();
								nodes.Update(node);


								if (health.ShouldHavePeers && health.Peers <= settings.AlertForMinimumPeers)
								{
									alerts.Add("Peers are below " + settings.AlertForMinimumPeers + " (" + health.Peers + ") for " + node.Url);
								}
							}
							catch (HttpRequestException ex)
							{
								alerts.Add("Node url not reachable: " + node.Url + " Message: " + ex.Message);
							}
							catch (Exception ex)
							{
								throw ex;
							}
						}
					}

					var nextAlertDate = lastAlert.GetValueOrDefault(DateTime.UtcNow.AddMinutes(-20)).AddMinutes(10);

					if (nextAlertDate < DateTime.UtcNow && alerts.Any())
					{
						var bot = db.Bots.FindOne(b => b.Id == 1);
						if (bot != null)
						{
							if (bot.IsEnabled)
							{
								var botClient = new TelegramBotClient(bot.AccessToken);

								var message = "";

								var counter = 0;
								foreach (var alert in alerts)
								{
									counter++;
									message += counter + ") " + alert + Environment.NewLine;
								}

								message += Environment.NewLine + Environment.NewLine + "Next alert will trigger in 10 minutes.";

								await botClient.SendTextMessageAsync(bot.ChatId, message);
								lastAlert = DateTime.UtcNow;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}

