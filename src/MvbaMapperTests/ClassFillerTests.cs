using FluentAssert;

using JetBrains.Annotations;

using MvbaMapper;

using MvbaMapperTests.TestClasses;

using NUnit.Framework;

namespace MvbaMapperTests
{
	[UsedImplicitly]
	public class ClassFillerTests
	{
		[TestFixture]
		public class When_asked_to_generate_an_object
		{
			private ClassFiller<InputClass> _classFiller;
			private InputClass _source;

			[SetUp]
			public void BeforeEachTest()
			{
				_classFiller = new ClassFiller<InputClass>();
				_source = _classFiller.Source;
			}

			[Test]
			public void Given_a_target_type()
			{
				Test.Given(_classFiller)
					.When(The_desired_object_is_created)
					.Should(Not_return_a_null_object)
					.Should(Initialize_boolean_properties_with_non_default_values)
					.Should(Initialize_boolean_properties_with_non_default_values)
					.Should(Initialize_integer_properties_with_non_default_values)
					.Should(Initialize_string_properties_with_non_default_values)
					.Verify();
			}

			[Test]
			public void Given_a_type_that_was_previously_generated()
			{
				Test.Given(_classFiller)
					.When(The_desired_object_is_created)
					.Should(Get_the_originally_generated_object)
					.Verify();
			}

			private void Get_the_originally_generated_object(ClassFiller<InputClass> obj)
			{
				ReferenceEquals(_source, _classFiller.Source).ShouldBeTrue();
			}

			private void Initialize_boolean_properties_with_non_default_values(ClassFiller<InputClass> obj)
			{
				_source.BooleanProperty.ShouldNotBeEqualTo(default(bool));
			}

			private void Initialize_integer_properties_with_non_default_values(ClassFiller<InputClass> obj)
			{
				_source.IntegerProperty.ShouldNotBeEqualTo(default(int));
			}

			private void Initialize_string_properties_with_non_default_values(ClassFiller<InputClass> obj)
			{
				_source.StringProperty.ShouldNotBeEqualTo(default(string));
				_source.StringProperty.ShouldNotBeEqualTo("");
			}

			private void Not_return_a_null_object(ClassFiller<InputClass> obj)
			{
				_source.ShouldNotBeNull();
			}

			private void The_desired_object_is_created(ClassFiller<InputClass> obj)
			{
				_source = obj.Source;
			}
		}
	}
}