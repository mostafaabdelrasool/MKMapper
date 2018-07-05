using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MKMapper;
using MKMapperTest.Mocks;
using System.Collections.Generic;
using FizzWare.NBuilder;

namespace MKMapperTest
{
    [TestClass]
    public class TestMapping
    {
        [TestMethod]
        public void TestSourceIsObject()
        {
            MapProperties map = new MapProperties();
            map.OnAssigning = OnAssigning;
            var xx = map.Map<ObjectMock, ObjectMock>(Builder<ObjectMock>.CreateNew().Build(),
                new ObjectMock());
        }
        [TestMethod]
        public void TestSameNameDifferentType()
        {
            MapProperties map = new MapProperties();
            map.OnAssigning = OnAssigning;
            var xx = map.Map<ObjectMock, ObjectMock3>(Builder<ObjectMock>.CreateNew().Build(),
                new ObjectMock3());
        }
        [TestMethod]
        public void TestWithoutAssign()
        {
            MapProperties map = new MapProperties();
            var xx = map.Map<ObjectMock, ObjectMock3>(Builder<ObjectMock>.CreateNew().Build(),
                new ObjectMock3());
        }
        [TestMethod]
        public void TestChildExclude()
        {
            MapProperties map = new MapProperties();
            var mock = Builder<ObjectMock>.CreateNew().Build();
            mock.prop4 = new Mocks.ObjectMock2();
            mock.prop4.exludeProp = "ssss";
           var result = map.Map<ObjectMock, ObjectMock3>(mock,
                new ObjectMock3(),new List<string> { "exludeProp" });
            if (!string.IsNullOrWhiteSpace(result.prop4.exludeProp))
            {
                Assert.Fail();
            }
        }
        private object OnAssigning(AssignedProperty arg)
        {
            if (arg.PropertyName == "prop1")
            {
                return 1;
            }
            return null;
        }
    }
}
