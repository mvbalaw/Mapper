using System.Collections.Generic;

using MvbaCore.CodeQuery;

namespace MvbaMapper
{
	public class ClassFiller<TSource>
		where TSource : class, new()
	{
		private readonly Dictionary<string, object> _propertyValues = new Dictionary<string, object>();
		private TSource _source;

		public TSource Source
		{
			get
			{
				if (_source == null)
				{
					_source = new TSource();
					Populate(_source);
				}
				return _source;
			}
		}

		private void Populate(TSource source)
		{
			foreach (var propertyInfo in source.GetType()
				.GetProperties()
				.ThatHaveAGetter()
				.ThatHaveASetter())
			{
// ReSharper disable once PossibleNullReferenceException
				var value = RandomValueType.GetFor(propertyInfo.PropertyType.FullName).CreateRandomValue();
				_propertyValues.Add(propertyInfo.Name, value);
				propertyInfo.SetValue(source, value, null);
			}
		}
	}
}