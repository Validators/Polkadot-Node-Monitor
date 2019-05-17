using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Validators.IO.Polkadot.Monitor.Database.Entities;

namespace Validators.IO.Polkadot.Monitor.Database
{
	public class AppDatabase : LiteDatabase
	{		
		public AppDatabase() : base(new ConnectionString(@"Filename=LiteDatabase.db;Utc=true;"))
		{
		}

		//public void SeedData()
		//{
		//	var bot = GetTelegramBot();

		//	if (bot == null)
		//	{
		//		Bots.Upsert(new Bot
		//		{					
		//		});
		//	}
		//}

		public Node GetNode(string address)
		{
			return Nodes.FindOne(n => n.Address == address);
		}

		public Bot GetBot()
		{
			return Bots.FindOne(b => b.Id == 1);
		}

		public LiteCollection<Node> Nodes { get { return GetCollection<Node>("nodes"); } }
		public LiteCollection<Bot> Bots { get { return GetCollection<Bot>("bots"); } }
	}
}
