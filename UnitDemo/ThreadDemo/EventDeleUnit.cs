using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.ThreadDemo
{
	public delegate void SayHi(string name);
	public class EventDeleUnit
	{
		public event SayHi SayHiHandle;
		public void FireEvent()
		{
			if (SayHiHandle != null)
			{
				SayHiHandle.Invoke("Zhangsan");
			}
		}
		public void RegisterEvent()
		{
			SayHiHandle += EventDeleUnit_SayHiHandle;
		}
		private void EventDeleUnit_SayHiHandle(string name)
		{
			Console.WriteLine("Hi {0}", name);
		}
	}
	public class EventHandlerClassUnit
	{
		private static readonly Object _evnetLock = new object();
		private EventHandler<NewMailEventArgs> _mNewMail;
		private EventHandlerList _events;
		protected bool HasEvents()
		{
			return (_events != null);
		}
		protected EventHandlerList Events
		{
			get
			{
				if (_events == null)
				{
					_events = new EventHandlerList();
				}
				return _events;
			}
		}
		public event EventHandler<NewMailEventArgs> NewMail
		{
			add
			{
				lock (_evnetLock)
				{
					Events.AddHandler("NewMai", value);
					//_mNewMail=Delegate.Combine(value) as EventHandler<NewMailEventArgs>;
				}
			}
			remove
			{
				lock (_evnetLock)
				{
					Events.RemoveHandler("NewMai", value);
					//_mNewMail = Delegate.Remove(_mNewMail,value) as EventHandler<NewMailEventArgs>; ;
				}
			}
		}
		public void OnFireMail(NewMailEventArgs e)
		{

			//if (NewMail. != null)
			//{
			//	_mNewMail(this, e);
			//}
			if (HasEvents())
			{
				EventHandler<NewMailEventArgs> handler = _events["NewMai"] as EventHandler<NewMailEventArgs>;
				if (handler != null)
				{
					handler(this, e);
				}
			}
		}
	}
	public class NewMailEventArgs : EventArgs
	{
		public string Name
		{
			get; set;
		}
	}
}
