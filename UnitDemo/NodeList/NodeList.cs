using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.NodeList
{
    public class NodeListDemo
    {
        public static LinkedList<string> _linkList = new LinkedList<string>();

        public static void AddNode(string name)
        {
            // _linkList.AddFirst(name);

            //_linkList.AddFirst()

            _linkList.AddFirst(new LinkedListNode<string>(name));
        }

        public static void AddLasNode(string name)
        {
            _linkList.AddLast(new LinkedListNode<string>(name));
        }

        public static void AddBeforeNode(LinkedListNode<string> node, string value)
        {
            _linkList.AddBefore(node, new LinkedListNode<string>(value));
        }

        public static void AddAfterNode(LinkedListNode<string> node, string value)
        {
            _linkList.AddAfter(node, new LinkedListNode<string>(value));
        }

        public static string GetFirstNode()
        {
            return _linkList.First();
        }

        public static string GetLastNode()
        {
            return _linkList.Last();
        }

        public static string GetNextNode()
        {
            return _linkList.First.Next.Value;
        }
    }

    public class SingleNodeList<T>
    {
        public class Node<T>
        {
            public Node()
            {

            }
            public Node(T item)
            {
                NodeValue = item;
            }
            public T NodeValue { get; set; }

            public Node<T> PriNode { get; set; }

            public Node<T> Next { get; set; }
        }

        private Node<T> _head;

        public Node<T> Head
        {
            get
            {
                return _head;
            }
            set
            {
                _head = value;
            }
        }

        public int GetLength()
        {
            Node<T> p = _head;

            int len = 0;

            while (p != null)
            {
                ++len;

                p = p.Next;
            }

            return len;
        }

        public void Clear()
        {
            _head = null;
        }

        public bool IsEmpty()
        {
            if (_head == null)
                return true;
            else
                return false;
        }

        public void Append(T item)
        {
            Node<T> q = new Node<T>(item);

            Node<T> p = new Node<T>();

            if (_head == null)
            {
                _head = q;
            }

            p = _head;

            while (p.Next != null)
            {
                p = p.Next;
            }

            p.Next = q;
        }

        public void InsertBefore(T item, int i)
        {
            if (IsEmpty() || i < 1 || i > GetLength())
                throw new Exception("Single Node is Empty");
            if (i == 1)
            {
                Node<T> q = new Node<T>(item);
                q.Next = _head;
                _head = q;
            }

            Node<T> p = _head;

            Node<T> r = new Node<T>();

            int j = 1;

            while (p.Next != null && j < i)
            {
                r = p;
                p = p.Next;
                ++j;
            }

            if (j == i)
            {
                Node<T> q = new Node<T>(item);

                q.Next = p;

                r.Next = q;
            }
        }

        public void InsertAfter(T item, int i)
        {
            if (IsEmpty() || i < 1 || i > GetLength())
                throw new Exception("Single NodeLink is Empty");

            if (i == 1)
            {
                Node<T> q = new Node<T>(item);

                q.Next = _head.Next;

                _head.Next = q;
            }

            Node<T> p = _head;
            int j = 1;
            while (p != null && j > i)
            {
                p = p.Next;
                ++j;
            }

            if (j == i)
            {
                Node<T> q = new Node<T>(item);

                q.Next = p.Next;

                p.Next = q;
            }
        }

        public void Delete(int i)
        {
            if (IsEmpty() || i < 0 || i > GetLength())
                throw new Exception("Node Link Is Empty");

            Node<T> q = new Node<T>();

            if (i == 1)
            {
                q = _head.Next;

                _head = q;
            }

            Node<T> p = _head;

            int j = 1;

            while (p != null && j < i)
            {
                ++j;
                q = p;
                p = p.Next;
            }

            if (j == i)
            {
                q.Next = p.Next;
            }

        }

        public T GetItem(int i)
        {
            if (IsEmpty() || i < 0)
                throw new Exception("null");

            Node<T> p = _head;
            int j = 0;
            while (p != null && j < 1)
            {
                ++j;
                p = p.Next;
            }
            if (j == i)
                return p.NodeValue;
            else
                return default(T);

        }

        public int Find(T value)
        {
            if (IsEmpty())
                throw new Exception("null");
            Node<T> p = _head;
            int i = 1;
            while (p.NodeValue.Equals(value) && p != null)
            {
                p = p.Next;
                ++i;
            }

            return i;

        }

        public void Display()
        {
            Node<T> p = _head;

            while (p != null)
            {
                Console.WriteLine("NodeValue {0}", p.NodeValue);
                p = p.Next;
            }
        }

    }
}
