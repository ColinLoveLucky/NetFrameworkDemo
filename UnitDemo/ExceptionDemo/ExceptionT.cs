using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.ExceptionDemo
{
    public class ExceptionT
    {
        public void ThrowExcept()
        {
            try
            {
                int i =Convert.ToInt32("123e");
            }
            catch
            {
                throw;
            }
        }

        public void ThrowNewExept()
        {
            try
            {
                int i = Convert.ToInt32("123e");
            }
            catch (Exception ex)
            {
                throw new Exception("Hello World");
            }
        }

        public void ThrowExcEx()
        {
            try
            {
                int i = Convert.ToInt32("123e");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
