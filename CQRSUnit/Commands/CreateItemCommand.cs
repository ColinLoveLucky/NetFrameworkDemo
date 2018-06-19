using CQRSUnit.Commands.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CQRSUnit.Commands
{
	public class CreateItemCommand:Command
	{
		public string Title { get; internal set; }
		public string Description { get; internal set; }
		public DateTime From { get; private set; }
		public DateTime To { get; private set; }
		public CreateItemCommand(Guid aggregateId, string title,
			string description, int version, 
			DateTime from, DateTime to):base(aggregateId,version)
		{
			Title = title;
			Description = description;
			From = from;
			To = to;
		}
	}
}