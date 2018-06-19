using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CQRSUnit.Commands.Impl
{
	public class Command : ICommand
	{
		public Guid Id
		{
			get;
			private set;
		}
		public int Version { get; private set; }
		public Command(Guid id, int version)
		{
			Id = id;
			Version = version;
		}
	}
}