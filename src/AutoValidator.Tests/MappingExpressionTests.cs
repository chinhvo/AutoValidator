﻿using AutoValidator.Impl;
using AutoValidator.Tests.Models;
using FluentAssertions;
using NUnit.Framework;

namespace AutoValidator.Tests
{
    [TestFixture]
    public class MappingExpressionTests
    {

        //TEST validate for each prop / expression type
        [Test]
        public void Custom_Mapping_Valid()
        {
            // arrange
            var model = new Model2
            {
                EmailAddress = "jon@test.com",
                Number = 21,
                Category = "dev"
            };

            var expression = new MappingExpression<Model2>();
            expression
                .ForMember(m => m.Category, cat => !string.IsNullOrWhiteSpace(cat))
                .ForMember(m => m.EmailAddress, email => email.Contains("@"))
                .ForMember(m => m.Number, num => num >= 18);

            // act
            var result = expression.Validate(model);

            // assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Errors.Count.Should().Be(0);
        }

        [Test]
        public void Custom_Mapping_invalid_Email()
        {
            // arrange
            var model = new Model2
            {
                EmailAddress = "jontest.com",
                Number = 21,
                Category = "dev"
            };

            var expression = new MappingExpression<Model2>();
            expression
                .ForMember(m => m.Category, cat => !string.IsNullOrWhiteSpace(cat))
                .ForMember(m => m.EmailAddress, email => email.Contains("@"))
                .ForMember(m => m.Number, num => num >= 18);

            // act
            var result = expression.Validate(model);

            // assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Errors.Count.Should().Be(1);

            result.Errors.ContainsKey("EmailAddress").Should().BeTrue();
            result.Errors["EmailAddress"].Should().Be("m => m.EmailAddress did not pass validation");
        }

        [Test]
        public void Custom_Mapping_invalid_Email_And_Number()
        {
            // arrange
            var model = new Model2
            {
                EmailAddress = "jontest.com",
                Number = 2,
                Category = "dev"
            };

            var expression = new MappingExpression<Model2>();
            expression
                .ForMember(m => m.Category, cat => !string.IsNullOrWhiteSpace(cat))
                .ForMember(m => m.EmailAddress, email => email.Contains("@"))
                .ForMember(m => m.Number, num => num >= 18);

            // act
            var result = expression.Validate(model);

            // assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Errors.Count.Should().Be(2);

            result.Errors.ContainsKey("EmailAddress").Should().BeTrue();
            result.Errors["EmailAddress"].Should().Be("m => m.EmailAddress did not pass validation");

            result.Errors.ContainsKey("Number").Should().BeTrue();
            result.Errors["Number"].Should().Be("m => m.Number did not pass validation");
        }

        [Test]
        public void Custom_Mapping_Can_Be_ReUsed()
        {
            // arrange
            var model1 = new Model2
            {
                EmailAddress = "jon@test.com",
                Number = 21,
                Category = "dev"
            };
            
            // arrange
            var model2 = new Model2
            {
                EmailAddress = "jontest.com",
                Number = 2,
                Category = "dev"
            };

            var expression = new MappingExpression<Model2>();
            expression
                .ForMember(m => m.Category, cat => !string.IsNullOrWhiteSpace(cat))
                .ForMember(m => m.EmailAddress, email => email.Contains("@"))
                .ForMember(m => m.Number, num => num >= 18);

            // act
            var result1 = expression.Validate(model1);
            var result2 = expression.Validate(model2);
            var result3 = expression.Validate(model1);

            // assert
            result1.Should().NotBeNull();
            result1.Success.Should().BeTrue();
            result1.Errors.Count.Should().Be(0);

            result2.Should().NotBeNull();
            result2.Success.Should().BeFalse();
            result2.Errors.Count.Should().Be(2);

            result2.Errors.ContainsKey("EmailAddress").Should().BeTrue();
            result2.Errors["EmailAddress"].Should().Be("m => m.EmailAddress did not pass validation");

            result2.Errors.ContainsKey("Number").Should().BeTrue();
            result2.Errors["Number"].Should().Be("m => m.Number did not pass validation");

            result3.Should().NotBeNull();
            result3.Success.Should().BeTrue();
            result3.Errors.Count.Should().Be(0);
        }


        [Test]

        public void MaxLength_Validator_Expression_valid()
        {
            // arrange
            // arrange
            var model = new ModelSimple
            {
                Name = "Jon Hawkins",
            };

            var expression = new MappingExpression<ModelSimple>();
            expression
                .ForMember(m => m.Name, (n, exp) => exp.MaxLength(n, 999, null));

            // act
            var result = expression.Validate(model);

            // assert
            result.Success.Should().BeTrue();
            result.Errors.Count.Should().Be(0);
        }

        [Test]

        public void MaxLength_Validator_Equal_Length_Expression_valid()
        {
            // arrange
            // arrange
            var model = new ModelSimple
            {
                Name = "123",
            };

            var expression = new MappingExpression<ModelSimple>();
            expression
                .ForMember(m => m.Name, (n, exp) => exp.MaxLength(n, 3, null));

            // act
            var result = expression.Validate(model);

            // assert
            result.Success.Should().BeTrue();
            result.Errors.Count.Should().Be(0);
        }

        [Test]

        public void MaxLength_Validator_Expression_Invalid()
        {
            // arrange
            // arrange
            var model = new ModelSimple
            {
                Name = "Jon Hawkins",
            };

            var expression = new MappingExpression<ModelSimple>();
            expression
                .ForMember(m => m.Name, (n, exp) => exp.MaxLength(n, 3, null));

            // act
            var result = expression.Validate(model);

            // assert
            result.Success.Should().BeFalse();
            result.Errors.ContainsKey("Name").Should().BeTrue();
            result.Errors["Name"].Should().Be("Name should not be longer than 3");
        }

        [Test]

        public void MaxLength_Validator_Expression_Invalid_custom_error()
        {
            // arrange
            // arrange
            var model = new ModelSimple
            {
                Name = "Jon Hawkins",
            };

            var expression = new MappingExpression<ModelSimple>();
            expression
                .ForMember(m => m.Name, (n, exp) => exp.MaxLength(n, 3, "{0} custom error {1} {2}"));

            // act
            var result = expression.Validate(model);

            // assert
            result.Success.Should().BeFalse();
            result.Errors.ContainsKey("Name").Should().BeTrue();
            result.Errors["Name"].Should().Be("3 custom error Name Jon Hawkins");
        }

        [Test]

        public void MinLength_Validator_Expression_Invalid()
        {
            // arrange
            var model = new ModelSimple
            {
                Name = "Jon Hawkins",
            };

            var expression = new MappingExpression<ModelSimple>();
            expression
                .ForMember(m => m.Name, (n, exp) => exp.MinLength(n, 4444, null));

            // act
            var result = expression.Validate(model);

            // assert
            result.Success.Should().BeFalse();
            result.Errors.ContainsKey("Name").Should().BeTrue();
            result.Errors["Name"].Should().Be("Name must be at least 4444");
        }

        [Test]

        public void MinLength_Validator_Expression_Invalid_custom_error()
        {
            // arrange
            var model = new ModelSimple
            {
                Name = "Jon Hawkins",
            };

            var expression = new MappingExpression<ModelSimple>();
            expression
                .ForMember(m => m.Name, (n, exp) => exp.MinLength(n, 4444, "{0} - {1} - {2}"));

            // act
            var result = expression.Validate(model);

            // assert
            result.Success.Should().BeFalse();
            result.Errors.ContainsKey("Name").Should().BeTrue();
            result.Errors["Name"].Should().Be("4444 - Name - Jon Hawkins");
        }

        [Test]

        public void MinLength_Validator_Expression_EqualValue_is_valid()
        {
            // arrange
            var model = new ModelSimple
            {
                Name = "123",
            };

            var expression = new MappingExpression<ModelSimple>();
            expression
                .ForMember(m => m.Name, (n, exp) => exp.MinLength(n, 3, null));

            // act
            var result = expression.Validate(model);

            // assert
            result.Success.Should().BeTrue();
        }

        [Test]

        public void MinLength_Validator_ExpressionGreaterValue_is_valid()
        {
            // arrange
            var model = new ModelSimple
            {
                Name = "123",
            };

            var expression = new MappingExpression<ModelSimple>();
            expression
                .ForMember(m => m.Name, (n, exp) => exp.MinLength(n, 2, null));

            // act
            var result = expression.Validate(model);

            // assert
            result.Success.Should().BeTrue();
        }

        [Test]

        public void NotNullOrEmpty_Empty_String_Is_Not_Valid()
        {
            // arrange
            var model = new ModelSimple();

            var expression = new MappingExpression<ModelSimple>();
            expression
                .ForMember(m => m.Name, (n, exp) => exp.NotNullOrEmpty(n, null));

            // act
            var result = expression.Validate(model);

            // assert
            result.Success.Should().BeFalse();
            result.Errors["Name"].Should().Be("Name can't be null or empty");
        }

        [Test]

        public void NotNullOrEmpty_Empty_String_Is_Not_Valid_Override_Message()
        {
            // arrange
            var model = new ModelSimple();

            var expression = new MappingExpression<ModelSimple>();
            expression
                .ForMember(m => m.Name, (n, exp) => exp.NotNullOrEmpty(n, "Test {0} Test"));

            // act
            var result = expression.Validate(model);

            // assert
            result.Success.Should().BeFalse();
            result.Errors["Name"].Should().Be("Test Name Test");
        }

        [Test]

        public void NotNullOrEmpty_NonEmpty_String_Is_Valid()
        {
            // arrange
            var model = new ModelSimple{ Name = "a" };

            var expression = new MappingExpression<ModelSimple>();
            expression
                .ForMember(m => m.Name, (n, exp) => exp.NotNullOrEmpty(n, null));

            // act
            var result = expression.Validate(model);

            // assert
            result.Success.Should().BeTrue();
        }

        [Test]

        public void IsEmailAddress_Invalid_Email_Error()
        {
            // arrange
            var model = new ModelSimple{ Name = "a.com" };

            var expression = new MappingExpression<ModelSimple>();
            expression
                .ForMember(m => m.Name, (n, exp) => exp.IsEmailAddress(n, null));

            // act
            var result = expression.Validate(model);

            // assert
            result.Success.Should().BeFalse();
            result.Errors["Name"].Should().Be("Invalid Email");
        }

        [Test]

        public void IsEmailAddress_Invalid_Email_Error_Custom_Error()
        {
            // arrange
            var model = new ModelSimple { Name = "a.com" };

            var expression = new MappingExpression<ModelSimple>();
            expression
                .ForMember(m => m.Name, (n, exp) => exp.IsEmailAddress(n, "{0} != email '{1}'"));

            // act
            var result = expression.Validate(model);

            // assert
            result.Success.Should().BeFalse();
            result.Errors["Name"].Should().Be("Name != email 'a.com'");
        }

        [Test]

        public void IsEmailAddress_With_Valid_Email_Return_True()
        {
            var model = new ModelSimple { Name = "a@a.com" };

            var expression = new MappingExpression<ModelSimple>();
            expression
                .ForMember(m => m.Name, (n, exp) => exp.IsEmailAddress(n, "{0} != email '{1}'"));

            // act
            var result = expression.Validate(model);

            // assert
            result.Success.Should().BeTrue();
        }

        [Test]

        public void MinValue_Too_small_Value_Returns_False()
        {
            // arrange
            var model = new Model2 { Number = 10 };
            var expression = new MappingExpression<Model2>();
            expression.ForMember(m => m.Number, (n, exp) => exp.MinValue(n, 11, null));

            // act
            var result = expression.Validate(model);

            // assert
            result.Success.Should().BeFalse();
            result.Errors["Number"].Should().Be("Number should be at least 11");
        }

        [Test]

        public void MinValue_equal_Value_Returns_True()
        {
            // arrange
            var model = new Model2 { Number = 10 };
            var expression = new MappingExpression<Model2>();
            expression.ForMember(m => m.Number, (n, exp) => exp.MinValue(n, 10, null));

            // act
            var result = expression.Validate(model);

            // assert
            result.Success.Should().BeTrue();
        }

        [Test]

        public void MinValue_greater_Value_Returns_True()
        {
            // arrange
            var model = new Model2 { Number = 11 };
            var expression = new MappingExpression<Model2>();
            expression.ForMember(m => m.Number, (n, exp) => exp.MinValue(n, 10, null));

            // act
            var result = expression.Validate(model);

            // assert
            result.Success.Should().BeTrue();
        }

        [Test]

        public void MinValue_Too_small_Value_Returns_False_custom_error()
        {
            // arrange
            var model = new Model2 { Number = 10 };
            var expression = new MappingExpression<Model2>();
            expression.ForMember(m => m.Number, (n, exp) => exp.MinValue(n, 11, "Test {0} should be more than {1} {2}"));

            // act
            var result = expression.Validate(model);

            // assert
            result.Success.Should().BeFalse();
            result.Errors["Number"].Should().Be("Test 11 should be more than Number 10");
        }

    }
}
