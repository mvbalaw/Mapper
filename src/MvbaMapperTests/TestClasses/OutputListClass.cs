using System.Collections.Generic;

namespace MvbaMapperTests.TestClasses
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