using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Tests.MSTest;
using NMF.Models.Meta;
using System.Collections.Generic;
using System.Text;

namespace NMF.Models.Tests
{
    [TestClass]
    public class AttributeTests : ModelTest<Attribute> { }

    [TestClass]
    public class ClassTests : ModelTest<Class> { }

    [TestClass]
    public class DataTypeTests : ModelTest<DataType> { }

    [TestClass]
    public class EnumerationTests : ModelTest<Enumeration> { }

    [TestClass]
    public class EventTests : ModelTest<Event> { }

    [TestClass]
    public class LiteralTests : ModelTest<Literal> { }

    [TestClass]
    public class NamespaceTests : ModelTest<Namespace> { }

    [TestClass]
    public class OperationTests : ModelTest<Operation> { }

    [TestClass]
    public class ParameterTests : ModelTest<Parameter> { }

    [TestClass]
    public class PrimitiveTypeTests : ModelTest<PrimitiveType> { }

    [TestClass]
    public class ReferenceTests : ModelTest<Reference> { }

    [TestClass]
    public class AttributeConstraintTests : ModelTest<AttributeConstraint> { }

    [TestClass]
    public class ReferenceConstraintTests : ModelTest<ReferenceConstraint> { }
}
