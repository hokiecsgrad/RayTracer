using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Xunit;

namespace RayTracer.Tests
{
    public class YamlTests
    {
        public class YamlClass 
        {
            public string Name { get; set; } = string.Empty;
        }

        [Fact]
        public void LoadingATestClass_ShouldInstantiateATestObject()
        {
            var document = @"Name: Ryan";

            var input = new StringReader(document);
            var deserializer = new Deserializer();
            var test = deserializer.Deserialize<YamlClass>(input);

            Assert.NotNull(test);
            Assert.IsType<YamlClass>(test);
        }
    }
}