using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecKill.domain
{
	public class Order : Aggregator
	{
		public string Phone
		{
			get; set;
		}
		public virtual Product ProductEntity
		{
			get;set;
		}
		public DateTime CreateTime
		{
			get;set;
		}
		public DateTime UpdateTime
		{
			get;set;
		}

	
	}
}