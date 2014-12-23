using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimplestDI;

namespace SimplestDI.Tests
{   
    public class TestStub {

    }

    public class TestStubDependency
    {
        public TestStub Stub {get;set;}

        public TestStubDependency(TestStub stub)
        {
            Stub = stub;
        }
    }

    [TestClass]
    public class BasicTests
    {
        [TestMethod]
        public void Register_Test()
        {
            var container = new SimplestContainer();
            container.Register<TestStub>();
            
            var result = container.Create<TestStub>();
            Assert.IsInstanceOfType(result, typeof(TestStub));
        }

        [TestMethod]
        public void Register_Constructor_DI_Test()
        {
            var container = new SimplestContainer();
            container.Register<TestStub>();
            container.Register<TestStubDependency>();

            var result = container.Create<TestStubDependency>();
            Assert.IsInstanceOfType(result.Stub, typeof(TestStub));

        }

           
        


    }
}
