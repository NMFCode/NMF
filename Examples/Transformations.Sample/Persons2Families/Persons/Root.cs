using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Transformations.Example.Persons
{
    public class Root
    {
        public Root()
        {
            Persons = new List<Person>();
        }

        public ICollection<Person> Persons { get; private set; }
    }
}
