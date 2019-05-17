using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NodaTime;
using System;
using System.Linq;
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

				using (var db = new AppDatabase())
				{
					var nodes = db.Nodes;

					foreach (var node in nodes.FindAll().ToList())
					{
						using (var client = new PolkadotClient(node.Url))
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
						}
					}

					var bot = db.Bots.FindOne(b => b.Id == 1);
					if (bot != null)
					{
						if (bot.IsEnabled)
						{
							var botClient = new TelegramBotClient(bot.AccessToken);

							var message = "Monitor updated for " + nodes.FindAll().Count() + " nodes";
							await botClient.SendTextMessageAsync(bot.ChatId, message);
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

