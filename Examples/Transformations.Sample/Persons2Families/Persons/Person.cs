﻿using System.Collections.Generic;

namespace NMF.Transformations.Example.Persons
{
    public class Person
    {
        public Person()
        {
            Children = new HashSet<Person>();
        }

        public string FirstName { get; set; }
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public Person Spouse { get; set; }
        public ICollection<Person> Children { get; private set; }
    }
}
