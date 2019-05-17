using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Args;
using Validators.IO.Polkadot.Monitor.Database;
using Validators.IO.Polkadot.Monitor.Models;

namespace Validators.IO.Polkadot.Monitor.Controllers
{
	public class HomeController : Controller
	{
		public async Task<IActionResult> Index()
		{
			return View();

	//		var accesstoken = "620950762:AAEZMMyfWfdvBX7dAwce1GXUqPzvHzkWHkQ";

			using (var db = new AppDatabase())
			{
				var nodes = db.Nodes.FindAll().ToList();
				var bot = db.Bots.FindOne(b => b.Id == 1);

				//if(bot == null)
				//{
				//	db.Bots.Insert(new Database.Entities.Bot
				//	{
				//		Id = 1,
				//		AccessToken = accesstoken,
				//		IsEnabled = false,
				//		LastUpdatedUtc = DateTime.UtcNow
				//	});
				//}

				return Content("Nodes: " + nodes.Count + " " + JsonConvert.SerializeObject(bot));
			}



			//var token = "620950762:AAEZMMyfWfdvBX7dAwce1GXUqPzvHzkWHkQ";

			//var botClient = new TelegramBotClient(token);
			//var me = botClient.GetMeAsync().Result;

			//botClient.OnMessage += Bot_OnMessage;
			//botClient.StartReceiving();
			//Thread.Sleep(int.MaxValue);

			//return Content($"Hello, World! I am user {me.Id} and my name is {me.FirstName}.");
		}

		//static async void Bot_OnMessage(object sender, MessageEventArgs e)
		//{
		//	if (e.Message.Text != null)
		//	{
		//		Console.WriteLine($"Received a text message in chat {e.Message.Chat.Id}.");

		//		await botClient.SendTextMessageAsync(
		//		  chatId: e.Message.Chat,
		//		  text: "You said:\n" + e.Message.Text
		//		);
		//	}
		//}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
