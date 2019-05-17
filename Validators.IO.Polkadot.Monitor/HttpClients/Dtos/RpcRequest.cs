using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Validators.IO.Polkadot.Monitor.HttpClients.Dtos
{
	public class RpcRequest
	{
		public RpcRequest(string method, List<string> parameters)
		{
			Id = 1;
			Jsonrpc = "2.0";
			Method = method;
			Params = parameters;
		}

		[JsonProperty("jsonrpc")]
		public string Jsonrpc { get; set; }

		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("method")]
		public string Method { get; set; }

		[JsonProperty("params")]
		public List<string> Params { get; set; }

	}
}
