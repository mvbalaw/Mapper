using System.Collections.Generic;

using FluentAssert;

using MvbaCore;

using MvbaMapper;

using MvbaMapperTests.TestClasses;

using NUnit.Framework;

namespace MvbaMapperTests
{
	public class MappingTesterTests
	{
		[TestFixture]
		public class When_asked_to_verify_an_object_mapping
		{
			private string _expectedDifferenceMessage;
			private Notification _notification;

			[Test]
			public void Given_objects_where_only_a_boolean_property_is_different()
			{
				Test.Given(new MappingTester<OutputClass>())
					.WithContext(new OutputClassContext())
					.When(Verify_is_called)
					.With(A_boolean_property_difference)
					.Should(Detect_the_difference)
					.Should(Give_the_name_of_the_property_that_has_the_value_difference)
					.Verify();
			}

			[Test]
			public void Given_objects_where_only_a_DateTime_property_is_different()
			{
				Test.Given(new MappingTester<OutputClass>())
					.WithContext(new OutputClassContext())
					.When(Verify_is_called)
					.With(A_DateTime_property_difference)
					.Should(Detect_the_difference)
					.Should(Give_the_name_of_the_property_that_has_the_value_difference)
					.Verify();
			}

			[Test]
			public void Given_objects_where_only_a_decimal_property_is_different()
			{
				Test.Given(new MappingTester<OutputClass>())
					.WithContext(new OutputClassContext())
					.When(Verify_is_called)
					.With(A_decimal_property_difference)
					.Should(Detect_the_difference)
					.Should(Give_the_name_of_the_property_that_has_the_value_difference)
					.Verify();
			}

			[Test]
			public void Given_objects_where_only_a_List_property_is_different()
			{
				Test.Given(new MappingTester<OutputListClass>())
					.WithContext(new OutputListClassContext())
					.When(Verify_is_called)
					.With(A_list_property_difference)
					.Should(Detect_the_difference)
					.Should(Give_the_name_of_the_property_that_has_the_value_difference)
					.Verify();
			}

			[Test]
			public void Given_objects_where_only_a_string_property_is_different()
			{
				Test.Given(new MappingTester<OutputClass>())
					.WithContext(new OutputClassContext())
					.When(Verify_is_called)
					.With(A_string_property_difference)
					.Should(Detect_the_difference)
					.Should(Give_the_name_of_the_property_that_has_the_value_difference)
					.Verify();
			}

			[Test]
			public void Given_objects_where_only_an_int_property_is_different()
			{
				Test.Given(new MappingTester<OutputClass>())
					.WithContext(new OutputClassContext())
					.When(Verify_is_called)
					.With(An_int_property_difference)
					.Should(Detect_the_difference)
					.Should(Give_the_name_of_the_property_that_has_the_value_difference)
					.Verify();
			}

			private void A_boolean_property_difference(MappingTester<OutputClass> obj, OutputClassContext context)
			{
				var source = new ClassFiller<InputClass>().Source;
				context.Expected = new OutputClass
					{
						BooleanProperty = source.BooleanProperty,
						IntegerProperty = source.IntegerProperty,
						StringProperty = source.StringProperty,
						DecimalProperty = source.DecimalProperty,
						DateTimeProperty = source.DateTimeProperty
					};

				context.Destination = new OutputClass
					{
						BooleanProperty = !context.Expected.BooleanProperty,
						StringProperty = context.Expected.StringProperty,
						DecimalProperty = context.Expected.DecimalProperty,
						DateTimeProperty = context.Expected.DateTimeProperty,
						IntegerProperty = context.Expected.IntegerProperty
					};

				_expectedDifferenceMessage = "BooleanProperty";
			}

			private void A_DateTime_property_difference(MappingTester<OutputClass> obj, OutputClassContext context)
			{
				var source = new ClassFiller<InputClass>().Source;
				context.Expected = new OutputClass
					{
						BooleanProperty = source.BooleanProperty,
						IntegerProperty = source.IntegerProperty,
						StringProperty = source.StringProperty,
						DecimalProperty = source.DecimalProperty,
						DateTimeProperty = source.DateTimeProperty
					};
				context.Destination = new OutputClass
					{
						BooleanProperty = context.Expected.BooleanProperty,
						StringProperty = context.Expected.StringProperty,
						DecimalProperty = context.Expected.DecimalProperty,
						DateTimeProperty = context.Expected.DateTimeProperty.AddDays(1),
						IntegerProperty = context.Expected.IntegerProperty
					};
				_expectedDifferenceMessage = "DateTimeProperty";
			}

			private void A_decimal_property_difference(MappingTester<OutputClass> obj, OutputClassContext context)
			{
				var source = new ClassFiller<InputClass>().Source;
				context.Expected = new OutputClass
					{
						BooleanProperty = source.BooleanProperty,
						IntegerProperty = source.IntegerProperty,
						StringProperty = source.StringProperty,
						DecimalProperty = source.DecimalProperty,
						DateTimeProperty = source.DateTimeProperty
					};
				context.Destination = new OutputClass
					{
						BooleanProperty = context.Expected.BooleanProperty,
						StringProperty = context.Expected.StringProperty,
						DecimalProperty = context.Expected.DecimalProperty - 1,
						DateTimeProperty = context.Expected.DateTimeProperty,
						IntegerProperty = context.Expected.IntegerProperty
					};
				_expectedDifferenceMessage = "DecimalProperty";
			}

			private void A_list_property_difference(MappingTester<OutputListClass> obj, OutputListClassContext context)
			{
				context.Destination = new OutputListClass
					{
						Items = new List<int>
							{
								4,
								5,
								6
							}
					};
				context.Expected = new OutputListClass
					{
						Items = new List<int>
							{
								4,
								5,
								6,
								7
							}
					};
				_expectedDifferenceMessage = "Items";
			}

			private void A_string_property_difference(MappingTester<OutputClass> obj, OutputClassContext context)
			{
				var source = new ClassFiller<InputClass>().Source;
				context.Expected = new OutputClass
					{
						BooleanProperty = source.BooleanProperty,
						IntegerProperty = source.IntegerProperty,
						StringProperty = source.StringProperty,
						DecimalProperty = source.DecimalProperty,
						DateTimeProperty = source.DateTimeProperty
					};
				context.Destination = new OutputClass
					{
						BooleanProperty = context.Expected.BooleanProperty,
						StringProperty = context.Expected.StringProperty.ToUpper(),
						DecimalProperty = context.Expected.DecimalProperty,
						DateTimeProperty = context.Expected.DateTimeProperty,
						IntegerProperty = context.Expected.IntegerProperty
					};
				_expectedDifferenceMessage = "StringProperty";
			}

			private void An_int_property_difference(MappingTester<OutputClass> obj, OutputClassContext context)
			{
				var source = new ClassFiller<InputClass>().Source;
				context.Expected = new OutputClass
					{
						BooleanProperty = source.BooleanProperty,
						IntegerProperty = source.IntegerProperty,
						StringProperty = source.StringProperty,
						DecimalProperty = source.DecimalProperty,
						DateTimeProperty = source.DateTimeProperty
					};
				context.Destination = new OutputClass
					{
						BooleanProperty = context.Expected.BooleanProperty,
						StringProperty = context.Expected.StringProperty,
						DecimalProperty = context.Expected.DecimalProperty,
						DateTimeProperty = context.Expected.DateTimeProperty,
						IntegerProperty = context.Expected.IntegerProperty - 1
					};

				_expectedDifferenceMessage = "IntegerProperty";
			}

			private void Detect_the_difference(MappingTester<OutputClass> obj, OutputClassContext context)
			{
				_notification.IsValid.ShouldBeFalse();
			}

			private void Detect_the_difference(MappingTester<OutputListClass> obj, OutputListClassContext context)
			{
				_notification.IsValid.ShouldBeFalse();
			}

			private void Give_the_name_of_the_property_that_has_the_value_difference(MappingTester<OutputClass> obj, OutputClassContext context)
			{
				Give_the_name_of_the_property_that_has_the_value_difference();
			}

			private void Give_the_name_of_the_property_that_has_the_value_difference()
			{
				_notification.ToString().ShouldBeEqualTo(_expectedDifferenceMessage);
			}

			private void Give_the_name_of_the_property_that_has_the_value_difference(MappingTester<OutputListClass> obj, OutputListClassContext context)
			{
				Give_the_name_of_the_property_that_has_the_value_difference();
			}

			private void Verify_is_called(MappingTester<OutputClass> obj, OutputClassContext context)
			{
				_notification = obj.Verify(context.Destination, context.Expected);
			}

			private void Verify_is_called(MappingTester<OutputListClass> obj, OutputListClassContext context)
			{
				_notification = obj.Verify(context.Destination, context.Expected);
			}
		}
	}
}