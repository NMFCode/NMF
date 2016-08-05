using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMF.Models;
using NMF.Expressions.Linq;
using NMF.Utilities;

namespace NMF.Synchronizations.Example
{
    public class FamiliesToPersons : ReflectiveSynchronization
    {
        public class FamilyModel2PersonsModel : SynchronizationRule<Model, Model>
        {
            public override void DeclareSynchronization()
            {
                SynchronizeManyLeftToRightOnly(SyncRule<Member2Male>(),
                    familyModel => familyModel.Descendants()
                                              .OfType<Families.Family>()
                                              .SelectMany(fam => fam.Sons.Concat(fam.Father)),
                    personModel => personModel.RootElements.OfType<IModelElement, Persons.Male>());

                SynchronizeManyLeftToRightOnly(SyncRule<Member2Female>(),
                    familyModel => familyModel.Descendants()
                                              .OfType<Families.Family>()
                                              .SelectMany(fam => fam.Daughters.Concat(fam.Mother)),
                    personModel => personModel.RootElements.OfType<IModelElement, Persons.Female>());
            }
        }

        public class Member2Male : SynchronizationRule<Families.IMember, Persons.Male>
        {
            public override void DeclareSynchronization()
            {
                SynchronizeLeftToRightOnly(
                    member => member.FirstName + " " + (member.Parent as Families.Family).LastName,
                    person => person.FullName);
            }
        }

        public class Member2Female : SynchronizationRule<Families.IMember, Persons.Female>
        {
            public override void DeclareSynchronization()
            {
                SynchronizeLeftToRightOnly(
                    member => member.FirstName + " " + (member.Parent as Families.Family).LastName,
                    person => person.FullName);
            }
        }
    }
}
