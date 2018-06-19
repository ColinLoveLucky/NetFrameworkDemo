using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CQRSUnit.Commands.Impl;
using CQRSUnit.CommandHandler;

namespace CQRSUnit.Message.Impl
{
	public class CommandBus : ICommandBus
	{
		private readonly ICommandHandlerFactory _commandHandlerFactory;
		public CommandBus(ICommandHandlerFactory commadHandlerFactory)
		{
			_commandHandlerFactory = commadHandlerFactory;
		}
		public void Send<T>(T command) where T : Command
		{
			var handler = _commandHandlerFactory.GetHandler<T>();
			if (handler != null)
			{
				handler.Execute(command);
			}
			else
			{
				throw new Exception("unregister handler");
			}
		}
	}
}