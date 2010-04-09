using System;
using System.Collections;

namespace MvbaMapper
{
	public class EnumerableComparer
	{
		public bool HaveSameContents(IEnumerable listA, IEnumerable listB)
		{
			if (listA == null)
			{
				throw new ArgumentNullException("listA", "enumerable being compared cannot be null");
			}
			var nonNullA = new ArrayList();
			foreach (var item in listA)
			{
				if (item != null)
				{
					nonNullA.Add(item);
				}
			}
			var nonNullB = new ArrayList();
			foreach (var item in listB)
			{
				if (item != null)
				{
					nonNullB.Add(item);
				}
			}

			if (nonNullA.Count != nonNullB.Count)
			{
				return false;
			}
			foreach (var item in nonNullA)
			{
				int index = nonNullB.IndexOf(item);
				if (index == -1)
				{
					return false;
				}
				nonNullB.RemoveAt(index);
			}
			return true;
		}
	}
}