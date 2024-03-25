using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMF.Models;
using NMF.Expressions.Linq;
using NMF.Utilities;
using NMF.Expressions;

namespace NMF.Synchronizations.Example
{
    /// <summary>
    /// An example of a model synchronization between two models, taken from TTC 2017 Families to Persons
    /// </summary>
    public class FamiliesToPersons : ReflectiveSynchronization
    {
        /// <summary>
        /// The root of the synchronization
        /// </summary>
        public class FamilyModel2PersonsModel : SynchronizationRule<Model, Model>
        {
            /// <inheritdoc />
            public override void DeclareSynchronization()
            {
                SynchronizeManyLeftToRightOnly(SyncRule<Member2Male>(),
                    familyModel => familyModel.Descendants()
                                              .OfType<Families.Member>()
                                              .Where(member => !member.IsFemale()),
                    personModel => personModel.RootElements.OfType<IModelElement, Persons.Male>());

                SynchronizeManyLeftToRightOnly(SyncRule<Member2Female>(),
                    familyModel => familyModel.Descendants()
                                              .OfType<Families.Member>()
                                              .Where(member => member.IsFemale()),
                    personModel => personModel.RootElements.OfType<IModelElement, Persons.Female>());
            }
        }

        /// <summary>
        /// Rule to synchronize males
        /// </summary>
        public class Member2Male : SynchronizationRule<Families.IMember, Persons.Male>
        {
            /// <inheritdoc />
            public override void DeclareSynchronization()
            {
                SynchronizeLeftToRightOnly(
                    member => member.FirstName + " " + (member.Parent as Families.Family).LastName,
                    person => person.FullName);
            }
        }

        /// <summary>
        /// Rule to synchronize females
        /// </summary>
        public class Member2Female : SynchronizationRule<Families.IMember, Persons.Female>
        {
            /// <inheritdoc />
            public override void DeclareSynchronization()
            {
                SynchronizeLeftToRightOnly(
                    member => member.FirstName + " " + (member.Parent as Families.Family).LastName,
                    person => person.FullName);
            }
        }
    }

    /// <summary>
    /// Helpers
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Determines whether the member is a female
        /// </summary>
        /// <param name="member">the member</param>
        /// <returns>true or false</returns>
        [ObservableProxy(typeof(Proxies), "IsFemale")]
        public static bool IsFemale(this Families.Member member)
        {
            return member.FamilyMother != null || member.FamilyDaughter != null;
        }

        /// <summary>
        /// Proxy class
        /// </summary>
        private static class Proxies
        {
            private static readonly ObservingFunc<Families.Member, bool> isFemaleFunc = new ObservingFunc<Families.Member, bool>(member => member.FamilyMother != null ? true : member.FamilyDaughter != null);

            /// <summary>
            /// Incremental version of IsFemale
            /// </summary>
#pragma warning disable S3218 // Inner class members should not shadow outer class "static" or type members
            public static INotifyValue<bool> IsFemale(INotifyValue<Families.Member> member)
#pragma warning restore S3218 // Inner class members should not shadow outer class "static" or type members
            {
                return isFemaleFunc.Observe(member);
            }
        }
    }
}
