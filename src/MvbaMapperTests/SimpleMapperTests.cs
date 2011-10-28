//  * **************************************************************************
//  * Copyright (c) McCreary, Veselka, Bragg & Allen, P.C.
//  * This source code is subject to terms and conditions of the MIT License.
//  * A copy of the license can be found in the License.txt file
//  * at the root of this distribution. 
//  * By using this source code in any fashion, you are agreeing to be bound by 
//  * the terms of the MIT License.
//  * You must not remove this notice from this software.
//  * **************************************************************************

using System;
using System.Linq.Expressions;

using FluentAssert;

using JetBrains.Annotations;

using MvbaCore;

using MvbaMapper;

using MvbaMapperTests.TestClasses;

using NUnit.Framework;

namespace MvbaMapperTests
{
	[UsedImplicitly]
	public class SimpleMapperTests
	{
		[TestFixture]
		public class When_asked_to_map
		{
			[Test]
			public void Given_a_null_destination()
			{
				Test.Given(new SimpleMapper())
					.WithContext(new ObjectSourceContext())
					.When(Map_is_called)
					.With(A_null_destination)
					.ShouldThrowException<ArgumentNullException>()
					.Verify();
			}

			[Test]
			public void Given_a_null_source()
			{
				Test.Given(new SimpleMapper())
					.WithContext(new LowerCaseContext())
					.When(Map_is_called)
					.With(A_null_source)
					.Should(Not_change_the_destination_object)
					.Verify();
			}

			[Test]
			public void Given_custom_conversions_are_requested()
			{
				Test.Given(new SimpleMapper())
					.WithContext(new SourceContext())
					.When(Map_is_called_with_custom_conversions)
					.With(Inputs_configured_for_custom_conversions)
					.Should(Map_the_property_values)
					.Verify();
			}

			[Test]
			public void Given_inputs_where_the_cases_of_the_property_names_differ()
			{
				Test.Given(new SimpleMapper())
					.WithContext(new LowerCaseContext())
					.When(Map_is_called)
					.With(Inputs_whose_property_names_have_different_casing)
					.Should(Map_the_property_values)
					.Verify();
			}

			[Test]
			public void Given_inputs_where_the_spellings_of_the_property_names_differ()
			{
				Test.Given(new SimpleMapper())
					.WithContext(new AlternateNameContext())
					.When(Map_is_called_with_custom_properties_to_link)
					.With(Inputs_whose_property_names_have_different_spelling)
					.Should(Map_the_property_values)
					.Verify();
			}

			[Test]
			public void Given_properties_to_ignore()
			{
				Test.Given(new SimpleMapper())
					.WithContext(new SourceContext())
					.When(Map_is_called_with_properties_to_ignore)
					.With(Source_initialized)
					.Should(Ignore_the_property_values_that_were_set_ignore)
					.Should(Map_the_property_values)
					.Verify();
			}

			[Test]
			public void Given_source_passed_as_object_type()
			{
				Test.Given(new SimpleMapper())
					.WithContext(new ObjectSourceContext())
					.When(Map_is_called)
					.With(Source_passed_as_object_type)
					.Should(Map_the_property_values)
					.Verify();
			}

			private static void A_null_destination(SimpleMapper arg1, ObjectSourceContext context)
			{
				context.Source = new ClassFiller<InputClass>().Source;
				context.Destination = null;
			}

			private static void A_null_source(SimpleMapper obj, LowerCaseContext context)
			{
				context.Source = null;
				context.Destination = new OutputClassLowerCase();
				context.Expected = new OutputClassLowerCase();
			}

			private static void Ignore_the_property_values_that_were_set_ignore(SourceContext context)
			{
				context.Destination.DecimalProperty.ShouldNotBeEqualTo(context.Source.DecimalProperty);
			}

			private static void Inputs_configured_for_custom_conversions(SimpleMapper obj, SourceContext context)
			{
				context.Source = new ClassFiller<InputClass>().Source;
				context.Destination = new OutputClass();
				context.Expected = new OutputClass
				{
					BooleanProperty = context.Source.BooleanProperty,
					DateTimeProperty = context.Source.DateTimeProperty.AddDays(-1),
					DateTimeToNullable = context.Source.DateTimeToNullable.AddDays(1),
					DecimalProperty = context.Source.DecimalProperty + 1,
					IntegerProperty = context.Source.IntegerProperty + 1,
					StringProperty = context.Source.StringProperty + "!"
				};
			}

			private static void Inputs_whose_property_names_have_different_casing(SimpleMapper obj, LowerCaseContext context)
			{
				context.Source = new ClassFiller<InputClass>().Source;
				context.Destination = new OutputClassLowerCase();
				context.Expected = new OutputClassLowerCase
				{
					strIngPropErty = context.Source.StringProperty,
				};

				// sanity check that the property names are different by case alone
				string inputPropertyName = Reflection.GetPropertyName((InputClass input) => input.StringProperty);
				string actualPropertyName = Reflection.GetPropertyName((OutputClassLowerCase x) => x.strIngPropErty);
				actualPropertyName.ShouldNotBeEqualTo(inputPropertyName);
				String.Compare(actualPropertyName, inputPropertyName, true).ShouldBeEqualTo(0);
			}

			private static void Inputs_whose_property_names_have_different_spelling(SimpleMapper obj, AlternateNameContext context)
			{
				context.Source = new ClassFiller<InputClass>().Source;
				context.Destination = new OutputAlternativeNamesClass();
				context.Expected = new OutputAlternativeNamesClass
				{
					BoolProperty = context.Source.BooleanProperty,
					DateProperty = context.Source.DateTimeProperty,
					NullableDateProperty = context.Source.DateTimeToNullable,
					DecProperty = context.Source.DecimalProperty,
					IntProperty = context.Source.IntegerProperty,
					StrProperty = context.Source.StringProperty,
				};
			}

			private static void Map_is_called(SimpleMapper simpleMapper, ObjectSourceContext context)
			{
				simpleMapper.Map(context.Source, context.Destination);
			}

			private static void Map_is_called(SimpleMapper obj, LowerCaseContext context)
			{
				obj.Map(context.Source, context.Destination);
			}

			private static void Map_is_called_with_custom_conversions(SimpleMapper obj, SourceContext context)
			{
				context.Source.Map()
					.WithCustomConversion((d1, d2) => d1 == typeof(decimal) && d2 == typeof(decimal), value => (decimal)value + 1)
					.WithCustomConverter(new DateTimeToNullableDateTimeConverter())
					.WithCustomConverters(new IntToIntConverter(), new DateTimeToDateTimeConverter(), new StringToStringConverter())
					.To(context.Destination);
			}

			private static void Map_is_called_with_custom_properties_to_link(SimpleMapper obj, AlternateNameContext context)
			{
				context.Source.Map()
					.WithLink<OutputAlternativeNamesClass>(x => x.BooleanProperty, x => x.BoolProperty)
					.WithLink(x => x.DateTimeProperty, x => x.DateProperty)
					.WithLink(x => x.DateTimeToNullable, x => x.NullableDateProperty)
					.WithLink(x => x.DecimalProperty, x => x.DecProperty)
					.WithLink(x => x.IntegerProperty, x => x.IntProperty)
					.WithLink(x => x.StringProperty, x => x.StrProperty)
					.To(context.Destination);
			}

			private static void Map_is_called_with_properties_to_ignore(SimpleMapper simpleMapper, SourceContext context)
			{
				context.Expected.DecimalProperty = 0;
				simpleMapper.Map(context.Source, context.Destination, new Expression<Func<OutputClass, object>>[] { x => x.DecimalProperty });
			}

			private static void Map_the_property_values(LowerCaseContext context)
			{
				var result = new MappingTester<OutputClassLowerCase>().Verify(context.Destination, context.Expected);
				result.IsValid.ShouldBeTrue(result.ToString());
			}

			private static void Map_the_property_values(AlternateNameContext context)
			{
				var result = new MappingTester<OutputAlternativeNamesClass>().Verify(context.Destination, context.Expected);
				result.IsValid.ShouldBeTrue(result.ToString());
			}

			private static void Map_the_property_values(ObjectSourceContext context)
			{
				var result = new MappingTester<OutputClass>().Verify(context.Destination, context.Expected);
				result.IsValid.ShouldBeTrue(result.ToString());
			}

			private static void Map_the_property_values(SourceContext context)
			{
				var result = new MappingTester<OutputClass>().Verify(context.Destination, context.Expected);
				result.IsValid.ShouldBeTrue(result.ToString());
			}

			private static void Not_change_the_destination_object(SimpleMapper obj, LowerCaseContext context)
			{
				var result = new MappingTester<OutputClassLowerCase>().Verify(context.Destination, context.Expected);
				result.IsValid.ShouldBeTrue(result.ToString());
			}

			private static void Source_initialized(SourceContext context)
			{
				var source = new ClassFiller<InputClass>().Source;
				context.Source = source;
				context.Expected = new OutputClass
				{
					BooleanProperty = source.BooleanProperty,
					IntegerProperty = source.IntegerProperty,
					StringProperty = source.StringProperty,
					DecimalProperty = 0,
					DateTimeProperty = source.DateTimeProperty,
					DateTimeToNullable = source.DateTimeToNullable
				};
				context.Destination = new OutputClass();
			}

			private static void Source_passed_as_object_type(SimpleMapper obj, ObjectSourceContext context)
			{
				var source = new ClassFiller<InputClass>().Source;
				context.Source = source;
				context.Expected = new OutputClass
				{
					BooleanProperty = source.BooleanProperty,
					IntegerProperty = source.IntegerProperty,
					StringProperty = source.StringProperty,
					DecimalProperty = source.DecimalProperty,
					DateTimeProperty = source.DateTimeProperty,
					DateTimeToNullable = source.DateTimeToNullable
				};
				context.Destination = new OutputClass();
			}
		}
	}

	internal class DateTimeToNullableDateTimeConverter : ITypeConverter
	{
		public DateTimeToNullableDateTimeConverter()
		{
			CanConvert = (d1, d2) => d1 == typeof(DateTime) && d2 == typeof(DateTime?);
			Convert = value => ((DateTime?)value).Value.AddDays(1);
		}

		public Func<Type, Type, bool> CanConvert { get; private set; }
		public Func<object, object> Convert { get; private set; }
	}

	internal class DateTimeToDateTimeConverter : ITypeConverter
	{
		public DateTimeToDateTimeConverter()
		{
			CanConvert = (d1, d2) => d1 == typeof(DateTime) && d2 == typeof(DateTime);
			Convert = value => ((DateTime?)value).Value.AddDays(-1);
		}

		public Func<Type, Type, bool> CanConvert { get; private set; }
		public Func<object, object> Convert { get; private set; }
	}

	internal class IntToIntConverter : ITypeConverter
	{
		public IntToIntConverter()
		{
			CanConvert = (d1, d2) => d1 == typeof(int) && d2 == typeof(int);
			Convert = value => (int)value + 1;
		}

		public Func<Type, Type, bool> CanConvert { get; private set; }
		public Func<object, object> Convert { get; private set; }
	}

	internal class StringToStringConverter : ITypeConverter
	{
		public StringToStringConverter()
		{
			CanConvert = (d1, d2) => d1 == typeof(string) && d2 == typeof(string);
			Convert = value => (string)value + "!";
		}

		public Func<Type, Type, bool> CanConvert { get; private set; }
		public Func<object, object> Convert { get; private set; }
	}
}