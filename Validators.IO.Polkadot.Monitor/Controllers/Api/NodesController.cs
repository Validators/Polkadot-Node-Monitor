using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Validators.IO.Polkadot.Monitor.Controllers.Api.Models;
using Validators.IO.Polkadot.Monitor.Database;

namespace Validators.IO.Polkadot.Monitor.Controllers.Api
{
	[Route("api/nodes")]
	[ApiController]
	public class NodesController : Controller
	{
		[HttpGet]
		public ActionResult Get()
		{
			using (var db = new AppDatabase())
			{
				var nodes = db.Nodes.FindAll().OrderByDescending(n => n.Id).ToList();

				return Ok(nodes);
			}
		}
		[HttpPost]
		public ActionResult Post(NodeModel model)
		{
			using (var db = new AppDatabase())
			{
				var node = db.GetNode(model.Address);

				if(node == null)
				{
					var nodes = db.Nodes.Upsert(new Database.Entities.Node
					{
						Address = model.Address,
						Name = model.Name,
						Url = model.Url
					});
					db.Nodes.EnsureIndex(n => n.Address, unique: true);
				}
				else
				{
					node.Name = model.Name;
					db.Nodes.Update(node);
				}

				return Ok();
			}
		}
		[HttpDelete]
		public ActionResult Delete(NodeDeleteModel model)
		{
			using (var db = new AppDatabase())
			{
				db.Nodes.Delete(n => n.Address == model.Address);
			}
			return Ok();
		}

		[HttpDelete]
		[Route("delete/all")]
		public ActionResult DeleteAll()
		{

			using (var db = new AppDatabase())
			{
				db.Nodes.Delete(n => n.Address != "");
			}
			return Ok();
		}
	}
}
