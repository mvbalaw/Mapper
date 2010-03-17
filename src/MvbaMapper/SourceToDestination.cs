using System;

namespace MvbaMapper
{
	public class SourceToDestination
	{
		public Func<object, object> GetValueFromSource { get; set; }
		public Action<object, object> SetValueToDestination { get; set; }
	}
}