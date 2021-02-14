using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mercurial.Net;
using NUnit.Framework;

namespace Mercurial.Net.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class CommandClassesTests
    {
        public static IEnumerable<Type> CommandClassTypes()
        {
            return from type in typeof(Repository).Assembly.GetTypes()
                   where
                       !type.IsAbstract && type.BaseType != null && type.BaseType.IsGenericType &&
                       (type.BaseType.GetGenericTypeDefinition() == typeof(MercurialCommandBase<>) ||
                        type.BaseType.GetGenericTypeDefinition() == typeof(IncludeExcludeCommandBase<>)) && !type.IsGenericType
                   select type;
        }

        public static IEnumerable<object[]> CommandClassTypesWithProperties()
        {
            IEnumerable<Type> types = from type in typeof(Repository).Assembly.GetTypes()
                                      where
                                          !type.IsAbstract && type.BaseType != null && type.BaseType.IsGenericType &&
                                          (type.BaseType.GetGenericTypeDefinition() == typeof(MercurialCommandBase<>) ||
                                           type.BaseType.GetGenericTypeDefinition() == typeof(IncludeExcludeCommandBase<>)) && !type.IsGenericType
                                      select type;

            return from type in types
                   from prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                   where prop.CanRead && prop.CanWrite && prop.Name != "Result"
                   select new object[]
                   {
                       type, prop.Name
                   };
        }

        [TestCaseSource(nameof(CommandClassTypes))]
        [Test]
        [Category("API")]
        public void AllCommandClassesMustBeSealed(Type commandClassType)
        {
            Assert.That(commandClassType.IsSealed, Is.True);
        }
    }
}