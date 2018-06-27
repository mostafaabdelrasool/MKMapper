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
            map.Map<ObjectMock, ObjectMock>(Builder<ObjectMock>.CreateNew().Build(),
               new ObjectMock());
        }

        private object OnAssigning(object data, string name)
        {
            throw new NotImplementedException();
        }
    }
}
