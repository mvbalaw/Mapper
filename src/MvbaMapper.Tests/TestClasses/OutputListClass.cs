using System.Collections.Generic;

namespace MvbaMapper.Tests.TestClasses
{
	public class OutputListClass
	{
		public List<int> Items { get; set; }
	}

	public class OutputListClassWithoutSetter
	{
		public OutputListClassWithoutSetter(List<int> items)
		{
			Items = items;
		}

		public List<int> Items { get; private set; }
	}

}