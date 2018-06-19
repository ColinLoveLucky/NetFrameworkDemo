using CQRSUnit.Infranstruture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace CQRSUnit.Services
{
	public interface IUserService<T>:IService<T> 
	{
	}
}