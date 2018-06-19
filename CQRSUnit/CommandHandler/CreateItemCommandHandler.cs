using CQRSUnit.Commands;
using CQRSUnit.Commands.Impl;
using CQRSUnit.Domain;
using CQRSUnit.Services;
using CQRSUnit.Services.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CQRSUnit.CommandHandler
{
	public class CreateItemCommandHandler<TCommand> : ICommandHandler<TCommand> where TCommand:Command
	{
		ICommandService<DataItemEntity> _commandService;
		public CreateItemCommandHandler()
		{
			_commandService = new CommandService<DataItemEntity>();
		}
		public void Execute(TCommand command)
		{
			CreateItemCommand createCommand = command as CreateItemCommand;
			var dataItem = new DataItemEntity()
			{
				Id = createCommand.Id.ToString(),
				Title = createCommand.Title,
				Description = createCommand.Description,
				From = createCommand.From,
				To =createCommand.To
			};
			_commandService.Add(dataItem);
			_commandService.Commit();
		}
	}
}