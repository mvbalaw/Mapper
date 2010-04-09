using System;

namespace MvbaMapperTests.TestClasses
{
	public class OutputClass
	{
		public bool BooleanProperty { get; set; }
		public DateTime DateTimeProperty { get; set; }
		public DateTime? DateTimeToNullable { get; set; }
		public decimal DecimalProperty { get; set; }
		public int IntegerProperty { get; set; }
		public string StringProperty { get; set; }
	}
}