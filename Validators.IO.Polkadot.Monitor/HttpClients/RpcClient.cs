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
			var responseContent = await response.Content.ReadAsAsync<RpcResponse>();

			if (response?.IsSuccessStatusCode == false)
			{
				// If failed, throw the body as the exception message.
				//
				if (responseContent != null)
				{
					throw new HttpRequestException("Request to " + client.BaseAddress + " failed. Content: " + JsonConvert.SerializeObject(responseContent));
				}
				else
				{
					response.EnsureSuccessStatusCode();
				}
			}

			return responseContent.Result.ToObject<T>();
		}
	}
}
