Mapper ReadMe
==
### Description

Mapper is a simple property mapper for c# Converter classes. We started with AutoMapper but decided it required configuration that turned into black magic.

Here we present a simple mapper that copies values from properties in type A to type B if the names and property types match. We will add to this project as our needs grow.

### Samples

	User source = new User();
	Customer destination = source.MapTo<Customer>();

	User source = new User();
	Customer destination = new Customer();
	destination.MapFrom(source);

	User source = new User();
	Customer destination = new Customer();
	new SimpleMapper().Map(source, destination);

### Testing class mappings

	const int age = 26;
	const string name = "James";
	var user = new User { Name = name, Age = age };
	var customer = _someConverter.Convert(user);
	var expectedCustomer = new Customer { Name = name, Age = age };
	
	var tester = new MappingTester<Customer>();
	var result = tester.Verify(customer, expectedCustomer)
	result.IsValid.ShouldBeTrue();

### How To Build:

The build script requires Ruby with rake installed.

1. Run `InstallGems.bat` to get the ruby dependencies (only needs to be run once per computer)
1. open a command prompt to the root folder and type `rake` to execute rakefile.rb

If you do not have ruby:

1. You need to create a src\CommonAssemblyInfo.cs file. Go.bat will copy src\CommonAssemblyInfo.cs.default to src\CommonAssemblyInfo.cs
1. open src\StarterTree.sln with Visual Studio and build the solution
### License

[MIT License][mitlicense]

This project is part of [MVBA's Open Source Projects][MvbaLawGithub].

[MvbaLawGithub]: http://mvbalaw.github.io/
[mitlicense]: http://www.opensource.org/licenses/mit-license.php
