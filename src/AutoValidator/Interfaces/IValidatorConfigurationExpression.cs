﻿using System;
using System.Collections.Generic;
using System.Reflection;
using AutoValidator.Models;

namespace AutoValidator.Interfaces
{
    public interface IValidatorConfigurationExpression
    {
        void AddProfile<TProfile>() where TProfile : IClassValidationProfile, new();

        void AddProfile(Type profileType);

        void AddProfiles(Assembly assemblyToScan);

        void AddProfiles(IEnumerable<Assembly> assembliesToScan);

        List<ProfileExpressionValidationResult> GetConfigurationExpressionValidation();
    }
}
