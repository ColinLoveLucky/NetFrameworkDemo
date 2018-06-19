using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecKill.domain
{
	public class SecKillToken:Aggregator
	{
		public string Token
		{
			get;set;
		}
		public string ProductName
		{
			get;set;
		}

		public DateTime ExpireStartTime
		{
			get;set;
		}
		public DateTime ExpireEndTime
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
		public int IsBuy
		{
			get;set;
		}
	}
}