using System;
using System.Text;

using MvbaCore;

namespace MvbaMapper
{
	internal class RandomValueType : NamedConstant<RandomValueType>
	{
		private static readonly Random Random = new Random();
		public static readonly RandomValueType Bool = new RandomValueType(typeof(bool).FullName, () => true);
		public static readonly RandomValueType DateTime = new RandomValueType(typeof(DateTime).FullName, () => new DateTime(Random.Next(1950, 2013), Random.Next(1, 13), Random.Next(1, 29)));
		public static readonly RandomValueType Decimal = new RandomValueType(typeof(Decimal).FullName, () => (decimal)Random.NextDouble());
		public static readonly RandomValueType Int = new RandomValueType(typeof(int).FullName, () => Random.Next(1, Int32.MaxValue));
		public static readonly RandomValueType String = new RandomValueType(typeof(string).FullName, () =>
		                                                                                             	{
		                                                                                             		var builder = new StringBuilder();
		                                                                                             		var random = Random;
		                                                                                             		var size = random.Next(2, 10);
		                                                                                             		for (var i = 0; i < size; i++)
		                                                                                             		{
		                                                                                             			var offset = random.Next(0, 36);
		                                                                                             			var ch = "0123456789abcdefghijklmnopqrstuvwxyz"[offset];
		                                                                                             			builder.Append(ch);
		                                                                                             		}
		                                                                                             		return builder.ToString();
		                                                                                             	});

		private readonly Func<object> _createRandomValue;

		private RandomValueType(string key, Func<object> createRandomValue)
		{
			_createRandomValue = createRandomValue;
			Add(key, this);
		}

		public object CreateRandomValue()
		{
			return _createRandomValue.Invoke();
		}
	}
}