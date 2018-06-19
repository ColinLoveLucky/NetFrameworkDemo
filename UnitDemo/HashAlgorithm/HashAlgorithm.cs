using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.HashAlgorithm
{
    public class HashAlgorithmDemo
    {
        public static int AddHash(string key, int prime)
        {
            int hash, i;

            var charArray = key.ToCharArray();

            for (hash = key.Length, i = 0; i < charArray.Length; i++)

                hash += charArray[i] * 123;

            return (hash % prime);
        }

        public static string ConvertToTwoJingZhi(string key)
        {
            
            byte[] data = Encoding.Unicode.GetBytes(key);

            StringBuilder result = new StringBuilder(data.Length * 8);

            foreach (byte b in data)
            {
                result.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }
            return result.ToString();
        }

        public static string ErJingZhiToString(string s)
        {
            System.Text.RegularExpressions.CaptureCollection cs =
                 System.Text.RegularExpressions.Regex.Match(s, @"([01]{8})+").Groups[1].Captures;
            byte[] data = new byte[cs.Count];
            for (int i = 0; i < cs.Count; i++)
            {
                data[i] = Convert.ToByte(cs[i].Value, 2);
            }
            return Encoding.Unicode.GetString(data, 0, data.Length);
        }
    }
}
