using EfResposity.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace EfResposity.UnitOfWorkInfranstrure
{
	//public interface IUnitOfWork
	//{
	//	DbContext CurrentDbContext { get; set; }
	//	void RegisterAdded(EntityBase entityBase, IUnitOfWorkResposity unitOfWorkResposity);
	//	void RegisterChanged(EntityBase entityBase, IUnitOfWorkResposity unitOfWorkResposity);
	//	void RegisterRemoved(EntityBase entityBase, IUnitOfWorkResposity unitOfWorkResposity);
	//	void Commit();
	//}
	//public interface IUnitOfWorkResposity
	//{
	//	void PersistNewItem(EntityBase entityBase);
	//	void PersistUpdatedItem(EntityBase entityBase);
	//	void PersistDeletedItem(EntityBase entityBase);
	//}
	//public class UnitOfWork : IUnitOfWork
	//{
	//	private Dictionary<EntityBase, IUnitOfWorkResposity> addedEntities;
	//	private Dictionary<EntityBase, IUnitOfWorkResposity> changedEntities;
	//	private Dictionary<EntityBase, IUnitOfWorkResposity> removedEntities;
	//	public UnitOfWork()
	//	{
	//		addedEntities = new Dictionary<EntityBase, IUnitOfWorkResposity>();
	//		changedEntities = new Dictionary<EntityBase, IUnitOfWorkResposity>();
	//		removedEntities = new Dictionary<EntityBase, IUnitOfWorkResposity>();
	//	}
	//	public DbContext CurrentDbContext { get; set; }
	//	public void Commit()
	//	{
	//		using (TransactionScope transactionScope = new TransactionScope())
	//		{
	//			foreach (var entity in addedEntities.Keys)
	//			{
	//				addedEntities[entity].PersistNewItem(entity);
	//			}
	//			foreach (var entity in changedEntities.Keys)
	//			{
	//				changedEntities[entity].PersistUpdatedItem(entity);
	//			}
	//			foreach (var entity in removedEntities.Keys)
	//			{
	//				removedEntities[entity].PersistDeletedItem(entity);
	//			}
	//			CurrentDbContext.SaveChanges();

	//			transactionScope.Complete();
	//		}
	//	}
	//	public void RegisterAdded(EntityBase entityBase, IUnitOfWorkResposity unitOfWorkResposity)
	//	{
	//		this.addedEntities.Add(entityBase, unitOfWorkResposity);
	//	}
	//	public void RegisterChanged(EntityBase entityBase, IUnitOfWorkResposity unitOfWorkResposity)
	//	{
	//		this.changedEntities.Add(entityBase, unitOfWorkResposity);
	//	}
	//	public void RegisterRemoved(EntityBase entityBase, IUnitOfWorkResposity unitOfWorkResposity)
	//	{
	//		this.removedEntities.Add(entityBase, unitOfWorkResposity);
	//	}
	//}
	//public interface IUserResposity
	//{
	//	void Save(Ef_User user);
	//	void Add(Ef_User user);
	//	void Remove(Ef_User user);
	//}
	//public class UserResposity : IUserResposity, IUnitOfWorkResposity
	//{
	//	private IUnitOfWork _unitOfWork;
	//	public UserResposity(IUnitOfWork unitOfWork)
	//	{
	//		this._unitOfWork = unitOfWork;
	//	}
	//	public void PersistDeletedItem(EntityBase entityBase)
	//	{
	//		_unitOfWork.CurrentDbContext.Set<EntityBase>().Remove(entityBase);
	//	}
	//	public void PersistNewItem(EntityBase entityBase)
	//	{
	//		_unitOfWork.CurrentDbContext.Set<EntityBase>().Add(entityBase);
	//	}
	//	public void PersistUpdatedItem(EntityBase entityBase)
	//	{
	//		_unitOfWork.CurrentDbContext.Entry(entityBase).State = EntityState.Modified;
	//	}
	//	public void Add(Ef_User user)
	//	{
	//		this._unitOfWork.RegisterAdded(user, this);
	//	}
	//	public void Remove(Ef_User user)
	//	{
	//		_unitOfWork.RegisterRemoved(user, this);
	//	}
	//	public void Save(Ef_User user)
	//	{
	//		_unitOfWork.RegisterChanged(user, this);
	//	}

	//}

	//public class UserService
	//{
	//	private IUserResposity _userResposity;
	//	private IUnitOfWork _unitOfWork;
	//	public UserService(IUserResposity userResposity, IUnitOfWork unitOfWork)
	//	{
	//		this._unitOfWork = unitOfWork;
	//		this._userResposity = userResposity;
	//	}
	//	public void AddUser(Ef_User user)
	//	{
	//		_userResposity.Add(user);
	//		_unitOfWork.Commit();
	//	}

	//	public void GetAllUser()
	//	{
		
	//	}
	//}
}
