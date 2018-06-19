using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecKill.domain
{
	public class Stock:Aggregator
	{
		public string ProductName
		{
			get;set;
		}

		public long SurplusNum
		{
			get;set;
		}

		public double SecKillPrice
		{
			get;set;
		}

		public DateTime StartTime
		{
			get;set;
		}
		public DateTime EndTime
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