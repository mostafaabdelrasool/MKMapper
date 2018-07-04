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
