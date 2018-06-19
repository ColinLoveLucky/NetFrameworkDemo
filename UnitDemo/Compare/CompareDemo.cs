using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.Compare
{
    public class StudentCompare : IComparable<StudentCompare>, IEquatable<StudentCompare>
    {
        public int Age
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }

        public static int operator +(StudentCompare a, StudentCompare b)
        {
           int result=0;
           if (a != null && b != null)
               result = a.Age + b.Age;
           return result;
        }
        public int CompareTo(StudentCompare other)
        {
            int result = 0;
            if (other != null)
            {
                if (this.Age > other.Age)
                    result = 1;
                else if (this.Age == other.Age)
                    result = 0;
                else if (this.Age < other.Age)
                    result = -1;
            }
            return result;
        }

        public bool Equals(StudentCompare other)
        {
            bool result = false;

            if (other != null)
            {
                if (this.Age == other.Age &&
                    string.Compare(this.Name, other.Name) == 0)
                    result = true;
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }


    public class DicKey:IEquatable<DicKey
        >
    {
        public int Id { get; set; }

        public int SubId { get; set; }

        public bool Equals(DicKey other)
        {
            return this.Id == other.Id && this.SubId == other.SubId;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return base.Equals(obj);
            if (obj is DicKey)
                return Equals(obj as DicKey);
            else
                throw new Exception("Argument is not a DicKey");
        }

        public override int GetHashCode()
        {
            //return base.GetHashCode();

           // return 0;

            return Id.GetHashCode() ^ SubId.GetHashCode();
        }
    }


}
