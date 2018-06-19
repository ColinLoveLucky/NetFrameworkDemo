using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CQRSUnit.Domain
{
	public class DataItemEntity : Aggregator
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime From { get; set; }
		public DateTime To { get; set; }
	}
}