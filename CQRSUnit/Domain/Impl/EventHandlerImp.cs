using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CQRSUnit.Domain.Impl
{
	public delegate void DeleEvent<TEntity>(TEntity entity);
	public class EventHandlerImp<TEntity> : IEventHandler
	{
		private static readonly object _eventLock = new object();
		public DeleEvent< TEntity> _event;
		public event DeleEvent<TEntity> Events
		{
			add
			{
				lock (_eventLock)
					_event += value;
			}
			remove
			{
				lock (_eventLock)
					_event -= value;
			}
		}
		public EventHandlerImp()
		{
		}
		protected bool HasEvents()
		{
			return (_event != null);
		}
		public void OnFire(TEntity entity)
		{
			if (HasEvents())
			{
				_event(entity);	
			}
		}
		//private EventHandlerList _eventHandlerList;
		//protected bool HasEvents()
		//{
		//	return (_eventHandlerList != null);
		//}
		//protected EventHandlerList Events
		//{
		//	get
		//	{
		//		if (_eventHandlerList == null)
		//		{
		//			_eventHandlerList = new EventHandlerList();
		//		}
		//		return _eventHandlerList;
		//	}
		//}
		//public void RegisterEvent<TDelegate>(TKey key, TDelegate del)
		//{
		//	lock (_eventLock)
		//	{
		//		Events.AddHandler(key, del as DeleEvent<TKey, TEntity>);
		//	}
		//}
		//public void RemoveEvent<TDelegate>(TKey key, TDelegate del)
		//{
		//	lock (_eventLock)
		//	{
		//		Events.RemoveHandler(key, del as DeleEvent<TKey, TEntity>);
		//	}
		//}
		//public void OnFire(TKey key, TEntity entity)
		//{
		//	if (HasEvents())
		//	{
		//		DeleEvent<TKey, TEntity> handler = _eventHandlerList[key] as DeleEvent<TKey, TEntity>;
		//		if (handler != null)
		//		{
		//			handler(key, entity);
		//		}
		//	}
		//}



	}
}