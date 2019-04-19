# AutoValidator

## GOAL

AutoValidator's aim is to create a simple, fluent and intiutive framework to validate data and models in your dotnet applications. Simply put an application would validate some data and get a result object.  That object would state sucess or failure and detail the failing data.

There are two basic ways to validate data:

 * Simply call validate each item of data as you need to
 * Define a validation schema for a model then call validate on an instance of that model.

One of the principles AutoValidator follows is that exceptions should only happen in exceptional situations so it will not throw an exception when some data is incorrect, it will simply return a result object detailing what is wrong.

the validation schema process is heavily guided by [AutoMapper](https://github.com/AutoMapper/AutoMapper).

## Creating a validator

There are two ways to create a validator entity;

 1) Directly newing one: `var validator = new Validator();`
 2) Using the factory `IValidatorFactory` => `validatorFactory.Create();`

 ## How does the validation work?

 once you have an instance of a validator you can fluently apply all of your validation rules then finally call `Validate()` to get the validationResult object.

 ```
 validator
    .IsEmailAddress("myemail.com", "email")
    .NotNullOrEmpty("regCode", "registrationCode")
    .Validate();
 ```

 ## Validation methods:

 ```
IValidator IsEmailAddress(string email, string propName = "email", string message = null);
IValidator NotNullOrEmpty(string text, string propName, string message = null);
IValidator MinLength(string text, int minLength, string propName, string message = null);
IValidator MaxLength(string text, int maxLength, string propName, string message = null);
```

##ValidationResult

The result object has two proprties:

 * Boolean - Success
 * Dictionary<string, string> - Errors

 An example error could be:

 ```javascript
 {
	success: false,
	errors: {
		{ 'email', 'invalid email'},
		{ 'firstName', 'must not be null'},
		{ 'age', 'Must be at least 18'}
	}
 }
 ```

## Simple validation

The simplest way to validate is to call each validation method providing all the data required.

`validator.IsEmailAddress("myemail.com", "email").Validate();`

The example above validates the email address `myemail.com` for the property name of `email` and will use the default error message.  Lets break this down a little:

 * The fist property is what we want to validate, i.e. a value that has been passed back from the UI.
 * The second value is the name of the property you wish to validate.  For example in the model or UI it could be called, `email`, `password`, `confirmPassword`.  If there is an error it will be assigned to the value of this property.
 * the last and optional parameter is an error message.  If it is left blank the default will be used.  It uses a `string.Format` taking each of the parameters (excluding the first value, email in this case).  For example `MaxLength` has: `public static string MaxLength => "{1} should not be longer than {0}";` where `{1}` is `propName` and `{0}` is `maxLength`.

 ## Validation Schemas

 -------- todo write this up -------- 


### TODO
 - [x] fix up Teamcity
 - [x] add unit tests to Teamcity
 - [x] write unit tests for all IValidatorExpressions
 - [x] class validation and class expression validation assertion with error messages
 - [x] validation error messages need to be connected to the property name
 - [ ] ValidationResult needs to be able to have multiple errors for each property
 - [ ] static autovalidator initializer for mapping
 - [ ] static autovalidator initializer for assert everything is valid.
 - [ ] be able to create expression and validate inline (without classValidator)
 - [ ] classValidator creation finds mapping and adds expressions
 - [ ] create configuration object for validation schema
 - [x] configuration setup to allow add mappings from assemblies
 - [x] creating custom validation function
 - [ ] allow global CreateMap to be defined in initial setup configuration
 - [ ] ensure all mappings are being saved
 - [ ] factory for class type to get instance of a validator with the defined mappings
 - [ ] be able to use the mapping style in simple validation process
 - [ ] have the ability to clear validation rules for T (only within that instance of a validator)
 - [ ] should be able to have multiple validator errors for one prop
 - [ ] multiple validation errors for one prop should have an array of errors as well as a nicely formatted string represntation.
 - [ ] custom expression error message string format options
 - [ ] standard expression error message string format options
 - [ ] test error message override in classValidator, regular string validator and regular fluent validator
 - [ ] add more IValidatorExpression functions

