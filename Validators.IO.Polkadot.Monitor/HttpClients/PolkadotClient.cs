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
	public class PolkadotClient : RpcClient
	{
		public PolkadotClient(string nodeUrl) : base(nodeUrl){}

		public async Task<SystemHealth> GetSystemHealth()
		{
			return await Query<SystemHealth>("system_health");
		}
		public async Task<string> GetSystemNameAndVersion()
		{
			var name = await Query<string>("system_name");
			var version = await Query<string>("system_version");

			return name + " " + version;
		}
		public async Task<string> GetSystemChain()
		{
			return await Query<string>("system_chain");
		}

	}
}
