using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NodaTime;

namespace Validators.IO.Polkadot.Monitor.Database.Entities
{
	public class Node
	{
		public int Id { get; set; }
		public string Name { get; set; }


		public DateTime LastUpdatedUtc { get; set; }

		/// <summary>
		/// 5xxxxx address of your session account
		/// </summary>
		public string Address { get; set; }

		/// <summary>
		/// The IP or Domain url of the node
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		/// Balance on chain
		/// </summary>
		public string Balance { get; set; }

		public bool IsSyncing { get; set; }

		public int Peers { get; set; }
		public string NameAndVersion { get; set; }
		public string Chain { get; internal set; }
	}
}
