using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NodaTime;
using Validators.IO.Polkadot.Monitor.Database;
using Validators.IO.Polkadot.Monitor.Models;

namespace Validators.IO.Polkadot.Monitor.Controllers
{
	public class JavascriptController : Controller
	{
		private readonly AppSettings settings;

		public JavascriptController(IOptions<AppSettings> settings)
		{
			this.settings = settings.Value;
		}

		[Route("/javascript/settings.js")]
		[AllowAnonymous]
		[ResponseCache(NoStore = true)]
		public JavascriptResult Settings([FromQuery] string v = "append-version")
		{
			var now = SystemClock.Instance.GetCurrentInstant();


			var script = "var settings = {";

			script += string.Format("nowUtc: '{0}',", now.InUtc().ToDateTimeUtc().ToString("yyyy-MM-ddTHH:mm:ss"));

			script += string.Format("end: '{0}',", "0");
			script += "};";

			return new JavascriptResult(script);
		}
	}

	public class JavascriptResult : ContentResult
	{
		public JavascriptResult(string script)
		{
			this.Content = script;
			this.ContentType = "application/javascript";
		}
	}
}
