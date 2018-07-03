using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKMapper
{
    public class AssignedProperty
    {
        public object SourceValue { get; set; }
        public string PropertyName { get; set; }
        public string SourceName { get; set; }
        public Type PropertyType { get; set; }


    }
}
