using CQRSUnit.Commands.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CQRSUnit.Message
{
	public interface ICommandBus
	{
		void Send<T>(T command) where T : Command;
	}
}