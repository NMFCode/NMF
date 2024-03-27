using System;
using System.Linq;
using NMF.Utilities;

using Ps = NMF.Transformations.Example.Persons;
using Fam = NMF.Transformations.Example.FamilyRelations;

using NMF.Transformations.Linq;
using NMF.Transformations.Core;

namespace NMF.Transformations.Example
{
    public class Persons2FamilyRelations : ReflectiveTransformation
    {
        public class Root2Root : TransformationRule<Ps.Root, Fam.Root> { }

        public class Person2Female : TransformationRule<Ps.Person, Fam.Female>
        {
            public override void Transform(Ps.Person input, Fam.Female output, ITransformationContext context)
            {
                output.Husband = context.Trace.ResolveIn(Rule<Person2Male>(), input.Spouse);

                foreach (var child in context.Trace.ResolveManyIn(Rule<Person2Person>(), input.Children))
                {
                    child.Mother = output;
                }
            }

            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<Person2Person>(), p => p.Gender == Ps.Gender.Female);
            }
        }

        public class Person2Male : TransformationRule<Ps.Person, Fam.Male>
        {
            public override void Transform(Ps.Person input, Fam.Male output, ITransformationContext context)
            {
                output.Wife = context.Trace.ResolveIn(Rule<Person2Female>(), input.Spouse);

                foreach (var child in context.Trace.ResolveManyIn(Rule<Person2Person>(), input.Children))
                {
                    child.Father = output;
                }
            }

            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<Person2Person>(), p => p.Gender == Ps.Gender.Male);
            }
        }

        public class Person2Person : AbstractTransformationRule<Ps.Person, Fam.Person>
        {
            public override void Transform(Ps.Person input, Fam.Person output, ITransformationContext context)
            {
                output.LastName = input.Name;
                output.FirstName = input.FirstName;

                var daughters = context.Trace.ResolveManyIn(Rule<Person2Female>(), input.Children.Where(child => child.Gender == Ps.Gender.Female));
                var sons = context.Trace.ResolveManyIn(Rule<Person2Male>(), input.Children.Where(child => child.Gender == Ps.Gender.Male));

                output.Daughters.AddRange(daughters);
                output.Sons.AddRange(sons);

                foreach (var daughter in daughters)
                {
                    daughter.Sisters.AddRange(daughters.Except(daughter));
                    daughter.Brothers.AddRange(sons);
                }

                foreach (var son in sons)
                {
                    son.Sisters.AddRange(daughters);
                    son.Brothers.AddRange(sons.Except(son));
                }
            }

            public override void RegisterDependencies()
            {
                CallForEach<Ps.Root, Fam.Root>(
                    root => root.Persons,
                    (root, people) => root.People.AddRange(people));

                CallOutputSensitive(Rule<SetRelatives>(), (ps, fam) => fam);
            }
        }

        public class HouseHold
        {
            public Fam.Male Mother { get; set; }
            public Fam.Male Dad { get; set; }
        }

        public class Households : TransformationRule<Ps.Person, Ps.Person, Households>
        {
            public override void RegisterDependencies()
            {
                var possibleMoms = Rule<Person2Female>().ToComputationSource();
                var possibleDads = Rule<Person2Male>().ToComputationSource();

                WithPattern(from mom in possibleMoms
                            from dad in possibleDads
                            where mom.Input.Spouse == dad.Input
                            select Tuple.Create(mom.Input, dad.Input));
            }
        }

        public class SetRelatives : InPlaceTransformationRule<Fam.Person>
        {
            public override void Transform(Fam.Person person, ITransformationContext context)
            {
                if (person.Father != null)
                {
                    person.Uncles.AddRange(person.Father.Brothers);
                    person.Aunts.AddRange(person.Father.Sisters);
                }
                if (person.Mother != null)
                {
                    person.Uncles.AddRange(person.Mother.Brothers);
                    person.Aunts.AddRange(person.Mother.Sisters);
                }
            }

            public override void RegisterDependencies()
            {
                TransformationDelayLevel = 1;
            }
        }
    }
}
