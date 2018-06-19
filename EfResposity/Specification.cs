using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EfResposity
{
	public interface ISpecification
	{
		bool IsSpecification(object obj);
	}

	
}
