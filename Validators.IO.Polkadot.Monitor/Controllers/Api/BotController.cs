using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Validators.IO.Polkadot.Monitor.Controllers.Api.Models;
using Validators.IO.Polkadot.Monitor.Database;
using Validators.IO.Polkadot.Monitor.Database.Entities;

namespace Validators.IO.Polkadot.Monitor.Controllers.Api
{
	[Route("api/bots")]
	[ApiController]
	public class BotController : Controller
	{
		[HttpGet]
		public ActionResult Get()
		{
			using (var db = new AppDatabase())
			{
				var bot = db.GetBot();

				return Ok(bot ?? new Bot());
			}
		}
		[HttpPost]
		public ActionResult Post(BotModel model)
		{
			using (var db = new AppDatabase())
			{
				var bot = db.GetBot();

				if(bot == null)
				{
					db.Bots.Upsert(new Database.Entities.Bot
					{
						Id = 1,
						IsEnabled = false,
						AccessToken = model.AccessToken,
						LastUpdatedUtc = DateTime.UtcNow
					});
				}
				else
				{
					// Reset all other parameters if token changes
					//
					if(bot.AccessToken != model.AccessToken)
					{
						bot.Username = null;
						bot.ChatId = 0;
						bot.IsEnabled = false;
						bot.LastUpdatedUtc = DateTime.UtcNow;
						bot.AccessToken = model.AccessToken;
						db.Bots.Update(bot);
					}
				}

				return Ok();
			}
		}

		[HttpDelete]
		public ActionResult Delete()
		{
			using (var db = new AppDatabase())
			{
				db.Bots.Delete(n => n.Id == 1);
			}
			return Ok();
		}
	}
}
