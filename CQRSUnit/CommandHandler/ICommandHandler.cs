using CQRSUnit.Commands.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CQRSUnit.CommandHandler
{
	public interface ICommandHandler<in TCommand> where TCommand : Command
	{
		void Execute(TCommand command);
	}
	
}