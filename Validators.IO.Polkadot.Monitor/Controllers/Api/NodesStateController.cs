using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Validators.IO.Polkadot.Monitor.Controllers.Api.Models;
using Validators.IO.Polkadot.Monitor.Database;

namespace Validators.IO.Polkadot.Monitor.Controllers.Api
{
	[Route("api/nodes/state")]
	[ApiController]
	public class NodesStateController : Controller
	{
		[HttpPost]
		public ActionResult Post(NodeStateModel model)
		{
			using (var db = new AppDatabase())
			{
				var node = db.GetNode(model.Address);

				if(node == null)
					return BadRequest("Node not found");


				node.Balance = model.Balance;
				db.Nodes.Update(node);

				return Ok();
			}
		}
	}
}
