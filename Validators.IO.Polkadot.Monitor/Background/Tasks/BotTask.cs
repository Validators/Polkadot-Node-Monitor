using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Validators.IO.Polkadot.Monitor.Database;

namespace Validators.IO.Polkadot.Monitor.Background.Tasks
{
	public class BotTask : ScheduledProcessor
	{
		private readonly AppSettings settings;
		private readonly ILogger logger;

		public BotTask(IServiceScopeFactory serviceScopeFactory, IOptions<AppSettings> settings, ILogger<BotTask> logger)
			 : base(serviceScopeFactory, "*/" + settings.Value.BotTaskFrequencyInSeconds + " * * * * *")
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
				logger.LogError(ex, "BotTask exception");
			}
		}

		private TelegramBotClient botClient;

		public async Task Executing(IServiceProvider serviceProvider)
		{
			try
			{
				if (botClient != null)
					return;

				using (var db = new AppDatabase())
				{
					var bot = db.Bots.FindOne(b => b.Id != 0);

					if (bot != null)
					{
						botClient = new TelegramBotClient(bot.AccessToken);

						botClient.OnMessage += Bot_OnMessage;
						botClient.StartReceiving();
					}
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private async void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
		{
			if (e.Message.Text != null)
			{
				if (e.Message.Text != "/start" && e.Message.Text != "/stop")
				{
					await botClient.SendTextMessageAsync(
					  chatId: e.Message.Chat,
					  text: "Thank you for the message. In order to start receiving alerts from this Polkadot Monitor, please send the message /start right here. Have a good day master."
					);
					return;
				}

				var chatIdChanged = false;

				using (var db = new AppDatabase())
				{
					var bot = db.Bots.FindOne(b => b.Id == 1);

					if (bot != null)
					{
						if (bot.ChatId != 0 && bot.ChatId != e.Message.Chat.Id)
							chatIdChanged = true;

						if (e.Message.Text == "/start")
							bot.IsEnabled = true;
						if (e.Message.Text == "/stop")
							bot.IsEnabled = false;

						bot.ChatId = e.Message.Chat.Id;

						if(bot.Username == null)
						{
							var me = await botClient.GetMeAsync();
							bot.Username = me.Username;
						}

						db.Bots.Update(bot);
					}
				}

				var changeState = "saved";
				if (chatIdChanged)
					changeState = "changed";

				if (e.Message.Text == "/start")
				{
					await botClient.SendTextMessageAsync(
						chatId: e.Message.Chat,
						text: "Thank you, chat Id " + changeState + " to " + e.Message.Chat.Id + ". Any alerts will now be posted here. If you want to stop alerts then send the message /stop in this chat. Have a good day master."
				);
				}
				if (e.Message.Text == "/stop")
				{
					await botClient.SendTextMessageAsync(
						chatId: e.Message.Chat,
						text: "Alerts have now been disabled. If you want to start alerts again then send the message /start in this chat. Have a good day master."
				);
				}
			}
		}
	}
}

