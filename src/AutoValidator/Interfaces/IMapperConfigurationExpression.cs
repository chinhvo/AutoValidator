﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutoValidator.Interfaces
{
    public interface IMapperConfigurationExpression
    {
        void AddProfile<TProfile>() where TProfile : IClassValidationProfile, new();

        void AddProfile(Type profileType);

        void AddProfile(Assembly assemblyToScan);

        void AddProfiles(IEnumerable<Assembly> assembliesToScan);

        void AssertConfigurationExpressionIsValid();
    }
}
