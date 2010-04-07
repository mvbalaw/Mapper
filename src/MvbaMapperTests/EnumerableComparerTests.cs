using System.Collections;
using System.Collections.Generic;

using FluentAssert;

using MvbaMapper;

using NUnit.Framework;

namespace MvbaMapperTests
{
	public class EnumerableComparerTests
	{
		[TestFixture]
		public class When_asked_if_two_IEnumerables_have_the_same_contents
		{
			[Test]
			public void Should_return_false_when_IEnumerables_have_same_items_in_different_counts()
			{
				var listA = new List<int>
				{
					4,
					5,
					5,
					6
				};

				var listB = new List<int>
				{
					4,
					5,
					6,
					6
				};

				var comparer = new EnumerableComparer(listA, listB);
				comparer.HaveSameContents().ShouldBeFalse();
			}

			[Test]
			public void Should_return_false_when_one_list_has_more_items_than_the_other()
			{
				var listA = new List<int>
				{
					4,
					5,
					6
				};

				var listB = new ArrayList
				{
					5,
					6
				};

				var comparer = new EnumerableComparer(listA, listB);
				comparer.HaveSameContents().ShouldBeFalse();
			}

			[Test]
			public void Should_return_false_when_same_length_IEnumerables_have_different_contents()
			{
				var listA = new List<int>
				{
					4,
					5,
					6
				};

				var listB = new List<int>
				{
					5,
					6,
					7
				};

				var comparer = new EnumerableComparer(listA, listB);
				comparer.HaveSameContents().ShouldBeFalse();
			}

			[Test]
			public void Should_return_true_when_IEnumerables_have_same_contents_in_different_order()
			{
				var listA = new List<int>
				{
					4,
					5,
					6
				};

				var listB = new List<int>
				{
					6,
					5,
					4
				};

				var comparer = new EnumerableComparer(listA, listB);
				comparer.HaveSameContents().ShouldBeTrue();
			}

			[Test]
			public void Should_return_true_when_same_length_IEnumerables_have_different_contents()
			{
				var listA = new List<int>
				{
					4,
					5,
					6
				};

				var listB = new List<int>
				{
					5,
					6,
					7
				};

				var comparer = new EnumerableComparer(listA, listB);
				comparer.HaveSameContents().ShouldBeFalse();
			}
		}
	}
}