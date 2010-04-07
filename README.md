We wanted a simple property mapper for c# Converter classes. We started with AutoMapper but decided it required configuration that turned into black magic.

Here we present a simple mapper that copies values from properties in type A to type B if the names and property types match. We will add to this project as our needs grow.

## Sample

   User source = new User();
   Customer destination = source.MapTo<Customer>();

   User source = new User();
   Customer destination = new Customer();
   destination.MapFrom(source);

   User source = new User();
   Customer destination = new Customer();
   new SimpleMapper().Map(source, destination);

## Testing class mappings

   const int age = 26;
   const string name = "James";
   var user = new User { Name = name, Age = age };
   var customer = _someConverter.Convert(user);
   var expectedCustomer = new Customer { Name = name, Age = age };
   
   var tester = new MappingTester<Customer>();
   var result = tester.Verify(customer, expectedCustomer)
   result.IsValid.ShouldBeTrue();

## License		

[MIT License][mitlicense]

This project is part of [MVBA Law Commons][mvbalawcommons].

[mvbalawcommons]: http://code.google.com/p/mvbalaw-commons/
[mitlicense]: http://www.opensource.org/licenses/mit-license.php   
