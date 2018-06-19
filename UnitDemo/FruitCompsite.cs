using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo
{
	public class FruitCompsite
	{
		private Dictionary<string, int> _fruit;
		public FruitCompsite()
		{
			_fruit = new Dictionary<string, int>();
			_fruit.Add("Apple", 10);
			_fruit.Add("banana", 12);
			_fruit.Add("orange", 38);
			_fruit.Add("strawberry", 100);
			_fruit.Add("durian", 760);
			_fruit.Add("pitaya", 460);
			_fruit.Add("pineapple", 70);
			_fruit.Add("ddd", 10);
			_fruit.Add("XXX", 10);
		}
		public FruitCompsite(Dictionary<string, int> fruit)
		{
			_fruit = fruit;
		}
		public void StaticFruitCompositeByNDollar(int dollar)
		{
			var fruitLen = _fruit.Count();
			List<Dictionary<string, int>> listFruitComposite = new List<Dictionary<string, int>>();
			listFruitComposite.Add(_fruit);
			for (var index = 1; index < fruitLen; index++)
			{
				listFruitComposite.Add(new Dictionary<string, int>());
			}
			for (var compositeStep = 1; compositeStep < fruitLen; compositeStep++)
			{
				RecuFruitComposite(compositeStep, listFruitComposite);
			}
			var compositeResult = new Dictionary<string, int>();
			var currentEqualValue = new List < KeyValuePair<string, int>>();
			foreach (var item in listFruitComposite)
			{
				currentEqualValue.AddRange(item.Where(x => x.Value == dollar));
			}
			foreach (var item in currentEqualValue)
			{
				Console.WriteLine("fruit composite {0} price {1}", item.Key,item.Value);
			}
		}
		private void RecuFruitComposite(int step, List<Dictionary<string, int>> result)
		{
			var key = string.Empty;
			var currentDictionary = new Dictionary<string, int>();
			if (step < _fruit.Count)
			{
				foreach (var resultItem in result[step - 1])
				{
					foreach (var currentItem in _fruit)
					{
						if (!resultItem.Key.Split('_').Contains(currentItem.Key))
						{
							key = resultItem.Key + "_" + currentItem.Key;
							if (!currentDictionary.ContainsKey(key))
							{
								result[step].Add(key, resultItem.Value + currentItem.Value);
							}
						}
					}
				}
			}
		}
	}
}
