﻿using MeControla.Core.Tests.Mocks.Datas.Entities;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace MeControla.Core.Tests.Mocks.Primitives
{
    public class IConfigurationMock
    {
        public static IConfiguration CreateEmpty()
            => CreateConfigurationInstance(new Dictionary<string, string>());

        public static IConfiguration Create()
            => CreateConfigurationInstance(DataSettings());

        private static Dictionary<string, string> DataSettings()
            => new()
            {
                { $"{nameof(ClassTest)}:{nameof(ClassTest.FieldInClass1)}", $"{DataMock.VALUE_DEFAULT_5}" },
                { $"{nameof(ClassTest)}:{nameof(ClassTest.FieldInClass2)}", $"{DataMock.VALUE_DEFAULT_9}" }
            };

        private static IConfiguration CreateConfigurationInstance(IDictionary<string, string> dataSettings)
            => new ConfigurationBuilder().AddInMemoryCollection(dataSettings)
                                         .Build();
    }
}