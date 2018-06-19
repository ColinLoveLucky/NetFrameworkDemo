using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecKill.domain
{
	public class Product : Aggregator
	{
		public string ProductType
		{
			get; set;
		}
		public string ProdcutName
		{
			get; set;
		}
		public double Price
		{
			get; set;
		}
		public DateTime CreateTime
		{
			get; set;
		}
		public DateTime UpdateTime
		{
			get; set;
		}
	}
}