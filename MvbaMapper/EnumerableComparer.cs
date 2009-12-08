using System;
using System.Collections;

namespace MvbaMapper
{
	public class EnumerableComparer
	{
		private readonly ArrayList _listA;
		private readonly ArrayList _listB;

		public EnumerableComparer(IEnumerable listA, IEnumerable listB)
		{
			if (listA == null)
			{
				throw new ArgumentNullException("listA", "enumerable being compared cannot be null");
			}
			_listA = new ArrayList();
			foreach (var item in listA)
			{
				if (item != null)
				{
					_listA.Add(item);
				}
			}
			_listB = new ArrayList();
			foreach (var item in listB)
			{
				if (item != null)
				{
					_listB.Add(item);
				}
			}
		}

		public bool HaveSameContents()
		{
			if (_listA.Count != _listB.Count)
			{
				return false;
			}
			foreach (var item in _listA)
			{
				int index = _listB.IndexOf(item);
				if (index == -1)
				{
					return false;
				}
				_listB.RemoveAt(index);
			}
			return true;
		}
	}
}