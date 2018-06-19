using CQRSUnit.Domain;
using CQRSUnit.Domain.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CQRSUnit.Domain
{
	public class UnitOfManager
	{
		private IUnitOfWork _unitOfWork;

		public IUnitOfWork CurrentUnitOfWork
		{
			get
			{
				_unitOfWork = new UnitOfWork();
				return _unitOfWork;
			}
			set
			{
				_unitOfWork = value;
			}
		}

	
	}
}