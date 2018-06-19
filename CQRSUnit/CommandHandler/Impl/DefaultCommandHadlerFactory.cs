using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CQRSUnit.Commands.Impl;

namespace CQRSUnit.CommandHandler.Impl
{
	public class DefaultCommandHadlerFactory : ICommandHandlerFactory
	{
		public ICommandHandler<T> GetHandler<T>() where T : Command
		{
			return new CreateItemCommandHandler<T>();
			//throw new NotImplementedException();
		}
	}
}