using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo
{
	public class Descartes
	{
		/// <summary>  
		/// 笛卡尔积  
		/// </summary>  
		/// <param name="dimvalue">将每个维度的集合的元素视为List<string>,多个集合构成List<List<string>> dimvalue作为输入</param>  
		/// <param name="result">将多维笛卡尔乘积的结果放到List<string> result之中作为输出</param>  
		/// <param name="layer">int layer 只是两个中间过程的参数携带变量</param>  
		/// <param name="curstring"> string curstring只是两个中间过程的参数携带变量,传递""就行</param>  
		public static void run(List<List<string>> dimvalue, List<string> result, int layer = 0, string curstring = "")
		{
			if (layer < dimvalue.Count - 1)
			{
				if (dimvalue[layer].Count == 0)
					run(dimvalue, result, layer + 1, curstring);
				else
				{
					for (int i = 0; i < dimvalue[layer].Count; i++)
					{
						StringBuilder s1 = new StringBuilder();
						s1.Append(curstring);
						s1.Append(dimvalue[layer][i]);
						run(dimvalue, result, layer + 1, s1.ToString());
					}
				}
			}
			else if (layer == dimvalue.Count - 1)
			{
				if (dimvalue[layer].Count == 0) result.Add(curstring);
				else
				{
					for (int i = 0; i < dimvalue[layer].Count; i++)
					{
						result.Add(curstring + dimvalue[layer][i]);
					}
				}
			}
		}
	}
}
