using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Validators.IO.Polkadot.Monitor.HttpClients.Dtos
{
	public class RpcResponse
	{
		public string Jsonrpc { get; set; }

		public int Id { get; set; }

		public JToken Result { get; set; }

	}
}
