using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.ContactProgram
{
    public class ContactPro
    {
        public void PreContract(int age)
        {
            Contract.Requires(age > 0 && age < 99, "年龄不合法");
            Console.WriteLine(string.Format("输出的年龄{0}", age));
        }
        public void AssertContract(Random rng)
        {
            Contract.Requires(rng is Random, "Random Is NUll");
            Contract.Assert(rng != null);
            int firstRoll = rng.Next(1, 7);
            Contract.Assume(firstRoll >= 1);
            Contract.Assume(firstRoll <= 6);
            Console.WriteLine(firstRoll);
        }
        public int ResultContract(int age)
        {
            Contract.Requires(age > 0 && age < 99, "年龄不合法");
            Contract.Ensures(Contract.Result<int>() > 0, "输出的结果不正确");

            if (age > 10)
                return 40;
            if (age > 30 && age < 40)
                return 50;
            else
                return 60;
        }
        List<string> _array = new List<string>();
        public void OldValueContract(string value)
        {
            Contract.Ensures(_array.Count == Contract.OldValue(_array.Count) + 1);
            _array.Add(value);
        }

        public void ReturnValueContract()
        {
            int returnValue = 10;

            Contract.Ensures(Contract.ValueAtReturn<int>(out returnValue) == 20);
        }
    }

    [ContractClass(typeof(ICastConvertContracts))]
    public interface ICastConvert
    {
        string Convert(string text);
    }

    [ContractClassFor(typeof(ICastConvert))]
    public abstract class ICastConvertContracts:ICastConvert
    {
        public string Convert(string text)
        {
            Contract.Requires(!string.IsNullOrEmpty(text));

            Contract.Ensures(Contract.Result<string>() != null);

            return default(string);
        }
    }

    public class CurrentCultureUpperCastFormater:ICastConvert
    {

        public string Convert(string text)
        {
            return text.ToUpper(CultureInfo.CurrentCulture);
        }
    }
}
