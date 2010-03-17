using System;

namespace MvbaMapper
{
	public class PropertyMappingInfo
	{
		public string Name { get; set; }
		public Type PropertyType { get; set; }
		public Func<object, object> GetValueFromSource { get; set; }
		public Action<object, object> SetValueToDestination { get; set; }
	}
}