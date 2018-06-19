using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CQRSUnit.Commands
{
	public interface ICommand
	{
		Guid Id { get; }
	}
}