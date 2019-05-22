using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Validators.IO.Polkadot.Monitor
{
	public class AppSettings
	{
		public string AdminPassword { get; set; }

		public int MonitorTaskFrequencyInSeconds { get; set; }
		public int BotTaskFrequencyInSeconds { get; set; }
		public int AlertForMinimumPeers { get; set; }

		public string LocalNodeEndpoint { get; set; }
		public string PublicNodeEndpoint { get; set; }
		public string PolkadotExplorerEndpoint { get; set; }

	}
}
