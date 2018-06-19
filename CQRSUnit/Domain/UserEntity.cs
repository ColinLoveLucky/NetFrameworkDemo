using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CQRSUnit.Domain
{
	public class UserEntity : Aggregator
	{
		public string Name { get; set; }
		public string Password { get; set; }
	}
}