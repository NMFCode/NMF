using System.Collections.Generic;

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
