using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Validators.IO.Polkadot.Monitor.HttpClients
{
	public class JsonClient : IDisposable
	{
		public HttpClient client { get; }

		public JsonClient(string url)
		{
			client = new HttpClient();
			client.BaseAddress = new Uri(url);
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		public void Dispose()
		{
			client.Dispose();
		}

	}
}
