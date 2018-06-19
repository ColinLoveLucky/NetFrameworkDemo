using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitDemo.Event
{
    public interface SayHello
    {
        int SayHi(string name);
    }
    public class ChineseSayHi : SayHello
    {
        public int SayHi(string name)
        {
            Thread.Sleep(3000);

            Console.WriteLine("早上好{0}", name);

            return 1;
        }
    }
    public class EnglishSayHi : SayHello
    {
        public int SayHi(string name)
        {
            Console.WriteLine("Good Mroning {0}", name);

            return 1;
        }
    }
    public class EventDemo
    {
        //public event EventHandler SayHelloWorld;
        public delegate void EventHandlerDemo(object sender, EventArgs e);
        private EventHandlerDemo EventDele;
        public void Add_EventDele(EventHandlerDemo value)
        {
            EventDele = (EventHandlerDemo)Delegate.Combine(EventDele, value);
        }
        public void Remove_EventDele(EventHandlerDemo value)
        {
            EventDele = (EventHandlerDemo)Delegate.Remove(EventDele, value);
        }
        public void ExcuteInvokeList()
        {
            EventSaiHiEventArgs e = new EventSaiHiEventArgs("zhangsan");

            foreach (Delegate item in EventDele.GetInvocationList())
            {
                Console.WriteLine(item.DynamicInvoke(this, e));
            }

        }
        public event EventHandlerDemo EventDemoAttr
        {
            add
            {
                EventDele = (EventHandlerDemo)Delegate.Combine(EventDele, value);
            }
            remove
            {
                EventDele = (EventHandlerDemo)Delegate.Remove(EventDele, value);
            }
        }
        public string Name { get; set; }
        public void SayChineseHi(string name)
        {
            EventSaiHiEventArgs e = new EventSaiHiEventArgs(name);
            if (EventDele != null)
            {
                EventDele(this, e);
            }
        }
        public void SayChinese(object sender, EventArgs e)
        {
            Console.WriteLine((e as EventSaiHiEventArgs).Name);
        }
        public void SayEnglish(object sender, EventArgs e)
        {
            Console.WriteLine((e as EventSaiHiEventArgs).Name);
        }
        public class EventSaiHiEventArgs : EventArgs
        {
            public string Name
            {
                get;
                set;
            }
            public EventSaiHiEventArgs(string name)
            {
                this.Name = name;
            }
        }
    }
    public class EventHadlerTest//subject
    {
        public event EventHandler SayHi;
        public void SayChineseHi(object sender, EventArgs e)
        {
           UnitDemo.Event.EventDemo.EventSaiHiEventArgs ee=new EventDemo.EventSaiHiEventArgs("zhangsan");

            if(SayHi!=null)
            {
                SayHi(this, ee);
            }
        }
        public void SayHiEvent(string name)
        {
            UnitDemo.Event.EventDemo.EventSaiHiEventArgs e = new UnitDemo.Event.EventDemo.EventSaiHiEventArgs(name);
            if (SayHi != null)
            {
                SayHi(this, e);
            }
        }
    }
    public abstract class Observer
    {
        public abstract void Update();
    }
    public abstract class Subject
    {
        public string ConcerteState { get; set; }
        private IList<Observer> observers = new List<Observer>();
        public void Attach(Observer observer)
        {
            observers.Add(observer);
        }
        public void Detach(Observer observer)
        {
            observers.Remove(observer);
        }
        //public static void operator --(Observer lhs)
        //{

        //}
        //public static IList<Observer> operator +(Observer value)
        //{
        //    observers.Add(value);
        //}

        //Subject+=new Observer();
        //Subject +=new Observer();
        //Subject-=new Observer();
        public void Notify()
        {
            foreach (Observer item in observers)
                item.Update();
        }
    }
    public class ConcerteSuject : Subject
    {
        public string SubjectState { get; set; }
    }
    public class ConcerteObserver : Observer
    {
        private string name;

        private string observerState;
        public ConcerteSuject Subject { get; set; }

        private IList<Subject> _subject = new List<Subject>();

        public void AddSubject(Subject value)
        {
            _subject.Add(value);
        }

        public ConcerteObserver(ConcerteSuject subject, string name)
        {
            this.name = name;
            this.Subject = subject;
        }
        public override void Update()
        {

            if (_subject.Count > 0)
            {
                foreach (Subject subject in _subject)
                {
                    var state = subject.ConcerteState;
                }
            }
            else
            {
                observerState = Subject.SubjectState;
                Console.WriteLine("The observer's state of {0} is {1}", name, observerState);

            }
        }
    }
    public class TestObserver
    {
        public void Show()
        {
            ConcerteSuject subject = new ConcerteSuject();

            ConcerteObserver observer = new ConcerteObserver(subject, "observer1");

            observer.AddSubject(subject);

            subject.Notify();

            // subject += observer;

            // subject+=
        }
    }

}
