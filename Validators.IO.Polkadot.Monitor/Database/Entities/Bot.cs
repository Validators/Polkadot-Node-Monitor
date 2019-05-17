using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NodaTime;

namespace Validators.IO.Polkadot.Monitor.Database.Entities
{
	/// <summary>
	/// Telegram Bot
	/// </summary>
	public class Bot
	{
		public int Id { get; set; }
		public long ChatId { get; set; }

		public string AccessToken { get; set; }

		/// <summary>
		/// If the message "/start" has been sent. If the message "/stop" then its disabled.
		/// </summary>
		public bool IsEnabled { get; set; }

		public string Username { get; set; }

		public DateTime LastUpdatedUtc { get; set; }
	}
}
