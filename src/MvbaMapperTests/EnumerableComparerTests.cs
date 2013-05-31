using System.Collections;
using System.Collections.Generic;

using FluentAssert;

using JetBrains.Annotations;

using MvbaMapper;

using NUnit.Framework;

namespace MvbaMapperTests
{
	[UsedImplicitly]
	public class EnumerableComparerTests
	{
		[TestFixture]
		public class When_asked_if_two_inputs_have_the_same_contents
		{
			private List<int> _listA;
			private ArrayList _listB;
			private bool _result;

			[Test]
			public void Given_inputs_with_different_total_items()
			{
				Test.Given(new EnumerableComparer())
					.When(Asked_if_two_inputs_have_the_same_contents)
					.With(Different_total_items)
					.Should(Return_false)
					.Verify();
			}

			[Test]
			public void Given_inputs_with_same_total_items_and_same_distinct_items_that_occur_a_different_number_of_times()
			{
				Test.Given(new EnumerableComparer())
					.When(Asked_if_two_inputs_have_the_same_contents)
					.With(Same_distinct_items_that_occur_a_different_number_of_times)
					.Should(Return_false)
					.Verify();
			}

			[Test]
			public void Given_inputs_with_same_total_items_and_same_distinct_items_that_occur_in_a_different_order()
			{
				Test.Given(new EnumerableComparer())
					.When(Asked_if_two_inputs_have_the_same_contents)
					.With(Same_number_and_kind_of_items_in_different_order)
					.Should(Return_true)
					.Verify();
			}

			[Test]
			public void Given_inputs_with_same_total_items_but_different_distinct_items()
			{
				Test.Given(new EnumerableComparer())
					.When(Asked_if_two_inputs_have_the_same_contents)
					.With(Same_total_items_but_different_distinct_items)
					.Should(Return_false)
					.Verify();
			}

			private void Asked_if_two_inputs_have_the_same_contents(EnumerableComparer obj)
			{
				_result = obj.HaveSameContents(_listA, _listB);
			}

			private void Different_total_items(EnumerableComparer obj)
			{
				_listA = new List<int>
					{
						4,
						5,
						6
					};

				_listB = new ArrayList
					{
						5,
						6
					};
			}

			private void Return_false(EnumerableComparer obj)
			{
				_result.ShouldBeFalse();
			}

			private void Return_true(EnumerableComparer obj)
			{
				_result.ShouldBeTrue();
			}

			private void Same_distinct_items_that_occur_a_different_number_of_times(EnumerableComparer obj)
			{
				_listA = new List<int>
					{
						4,
						5,
						5,
						6
					};

				_listB = new ArrayList
					{
						4,
						5,
						6,
						6
					};
			}

			private void Same_number_and_kind_of_items_in_different_order(EnumerableComparer obj)
			{
				_listA = new List<int>
					{
						4,
						5,
						6
					};

				_listB = new ArrayList
					{
						6,
						5,
						4
					};
			}

			private void Same_total_items_but_different_distinct_items(EnumerableComparer obj)
			{
				_listA = new List<int>
					{
						4,
						5,
						6
					};

				_listB = new ArrayList
					{
						5,
						6,
						7
					};
			}
		}
	}
}