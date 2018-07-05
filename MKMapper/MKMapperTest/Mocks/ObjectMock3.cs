using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKMapperTest.Mocks
{
    public class ObjectMock3
    {
        public int prop1 { get; set; }
        public string prop2 { get; set; }
        public DateTime prop3 { get; set; }

        public ObjectMock2 prop4 { get; set; }
        public List<ObjectMock2> prop5 { get; set; }

    }
}
