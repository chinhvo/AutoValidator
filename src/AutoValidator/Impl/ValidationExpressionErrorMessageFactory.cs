﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoValidator.Constants;
using AutoValidator.Interfaces;

namespace AutoValidator.Impl
{
    public class ValidationExpressionErrorMessageFactory : IValidationExpressionErrorMessageFactory
    {
        public Tuple<string, List<object>> Get<TMember>(Expression<Func<TMember, IValidatorExpression, bool>> exp, string propName)
        {
            //TODO for each method name, collect up all variables and put in list and find the correct error message format string / override.

            var methodCall = exp.Body as MethodCallExpression;
            var methodSignature = methodCall.Method.ToString();

            string format = string.Empty;
            string messageOverride = string.Empty;
            var variables = new List<object>();

            switch (methodSignature)
            {
                case "Boolean IsEmailAddress(System.String, System.String)":
                    messageOverride = GetArgumentMessageOverride(methodCall, 1);
                    format = !string.IsNullOrWhiteSpace(messageOverride) ? messageOverride : ValidationMessageConstStrings.InvalidEmail;
                    break;

                case "Boolean NotNullOrEmpty(System.String, System.String)":
                    messageOverride = GetArgumentMessageOverride(methodCall, 1);
                    format = !string.IsNullOrWhiteSpace(messageOverride) ? messageOverride : ValidationMessageConstStrings.StringNotNullOrEmpty;
                    break;

                case "Boolean MaxLength(System.String, Int32, System.String)":
                    messageOverride = GetArgumentMessageOverride(methodCall, 2);
                    format = !string.IsNullOrWhiteSpace(messageOverride) ? messageOverride :  ValidationMessageConstStrings.StringMaxLength;

                    variables.Add(GetArgumentValue(methodCall.Arguments[1]));
                    break;

                case "Boolean MinLength(System.String, Int32, System.String)":
                    messageOverride = GetArgumentMessageOverride(methodCall, 2);
                    format = !string.IsNullOrWhiteSpace(messageOverride) ? messageOverride : ValidationMessageConstStrings.StringMinLength;

                    variables.Add(GetArgumentValue(methodCall.Arguments[1]));
                    break;

                case "Boolean MinValue(Int32, Int32, System.String)":
                    messageOverride = GetArgumentMessageOverride(methodCall, 2);
                    format = !string.IsNullOrWhiteSpace(messageOverride) ? messageOverride : ValidationMessageConstStrings.IntMinValue;

                    variables.Add(GetArgumentValue(methodCall.Arguments[1]));
                    break;

                case "Boolean Ignore()":
                    format = ValidationMessageConstStrings.Ignore;
                    break;

                default:
                    throw new ArgumentOutOfRangeException("unknown IValidatorExpression expression method");
            }

            variables.Add(propName);
            
            return new Tuple<string, List<object>>(format, variables);
        }

        private string GetArgumentMessageOverride(MethodCallExpression methodCall, int index)
        {
            var argValue = GetArgumentValue(methodCall.Arguments[index]);
            return argValue != null ? argValue.ToString() : string.Empty;
        }

        private object GetArgumentValue(Expression methodExpression)
        {
            var constExp = methodExpression as ConstantExpression;
            return constExp?.Value;
        }

        public string Get(string value, string propName, object[] values)
        {
            // TODO do the same as above

            // Get calling method name
            //Console.WriteLine(stackTrace.GetFrame(1).GetMethod().Name);

            // from calling method name can figure out the expression and call into same structure as above.
            throw new NotImplementedException();
        }
    }
}
