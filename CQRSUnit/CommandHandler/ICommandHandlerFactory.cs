using CQRSUnit.Commands.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CQRSUnit.CommandHandler
{
	public interface ICommandHandlerFactory
	{
		ICommandHandler<T> GetHandler<T>() where T : Command;
	}
}