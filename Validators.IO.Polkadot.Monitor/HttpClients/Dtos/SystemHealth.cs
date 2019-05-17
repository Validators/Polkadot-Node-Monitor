using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Validators.IO.Polkadot.Monitor.HttpClients.Dtos
{
	public class SystemHealth
	{
		public bool IsSyncing { get; set; }
		public int Peers { get; set; }
		public bool ShouldHavePeers { get; set; }

	}
}
