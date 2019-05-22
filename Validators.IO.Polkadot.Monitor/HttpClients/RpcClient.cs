using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Validators.IO.Polkadot.Monitor.HttpClients.Dtos;

namespace Validators.IO.Polkadot.Monitor.HttpClients
{
	public class RpcClient : JsonClient
	{
		public RpcClient(string nodeUrl) : base(nodeUrl)
		{
		}

		internal async Task<T> Query<T>(string method)
		{
			return await Query<T>(method, new List<string>());
		}

		internal async Task<T> Query<T>(string method, List<string> parameters)
		{
			var data = new RpcRequest(method, parameters);

			var response = await client.PostAsJsonAsync("", data);

			if (response?.IsSuccessStatusCode == false)
			{
				response.EnsureSuccessStatusCode();
			}

			var responseContent = await response.Content.ReadAsAsync<RpcResponse>();

			return responseContent.Result.ToObject<T>();
		}
	}
}
