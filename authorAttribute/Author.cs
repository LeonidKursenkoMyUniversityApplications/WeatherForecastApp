using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authorAttribute
{
    [System.AttributeUsage(System.AttributeTargets.Class |
        System.AttributeTargets.Struct)]
    public class Author : System.Attribute
    {
        private string name;
        public double version;

        public Author(string name)
        {
            this.name = name;
            version = 1.0;
        }

        public string GetName()
        {
            return name;
        }

    }

}
