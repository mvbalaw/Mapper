using System;
using System.Text;

using FluentAssert;

namespace MvbaMapperTests
{
	public class RandomValueType : NamedConstant<RandomValueType>
	{
		public static readonly RandomValueType Bool = new RandomValueType(typeof(bool).FullName, () => true);
		public static readonly RandomValueType Int = new RandomValueType(typeof(int).FullName, () => new Random().Next(1, Int32.MaxValue));
		public static readonly RandomValueType String = new RandomValueType(typeof(string).FullName, () =>
		                                                                                             	{
		                                                                                             		StringBuilder builder = new StringBuilder();
		                                                                                             		Random random = new Random();
		                                                                                             		int size = random.Next(2, 10);
		                                                                                             		for (int i = 0; i < size; i++)
		                                                                                             		{
		                                                                                             			int offset = random.Next(0, 36);
		                                                                                             			char ch = "0123456789abcdefghijklmnopqrstuvwxyz"[offset];
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

		public static RandomValueType GetFor(string key)
		{
			return Get(key);
		}
	}
}