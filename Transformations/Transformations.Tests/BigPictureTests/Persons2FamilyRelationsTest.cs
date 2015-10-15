using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Transformations.Example;
using Fam = NMF.Transformations.Example.FamilyRelations;
using Ps = NMF.Transformations.Example.Persons;
using NMF.Transformations;

using NMF.Tests;
using NMF.Transformations.Core;

namespace NMF.Transformations.Tests.BigPictureTests
{
    [TestClass]
    public class Persons2FamilyRelationsTest
    {
        private Persons2FamilyRelations transformation = new Persons2FamilyRelations();

        [TestMethod]
        public void Transformations_Persons2FamilyRelations1()
        {
            var root = CreateSimpsons();
            var context = new TransformationContext(transformation);
            var simpsons = TransformationEngine.Transform<Ps.Root, Fam.Root>(root, context);

            Assert.AreEqual(root, context.Input[0]);
            Assert.AreEqual(simpsons, context.Output);

            var abraham = LookupMale("Abraham", context);
            var mona = LookupFemale("Mona", context);
            var clancy = LookupMale("Clancy", context);
            var jackeline = LookupFemale("Jackeline", context);
            var herb = LookupMale("Herb", context);
            var homer = LookupMale("Homer", context);
            var marge = LookupFemale("Marge", context);
            var patty = LookupFemale("Patty", context);
            var selma = LookupFemale("Selma", context);
            var bart = LookupMale("Bart", context);
            var lisa = LookupFemale("Lisa", context);
            var maggie = LookupFemale("Maggie", context);
            var ling = LookupFemale("Ling", context);

            abraham.AssertNotNull();
            mona.AssertNotNull();
            clancy.AssertNotNull();
            jackeline.AssertNotNull();
            herb.AssertNotNull();
            homer.AssertNotNull();
            marge.AssertNotNull();
            patty.AssertNotNull();
            selma.AssertNotNull();
            bart.AssertNotNull();
            lisa.AssertNotNull();
            maggie.AssertNotNull();
            ling.AssertNotNull();

            //abraham
            Assert.AreEqual(mona, abraham.Wife);
            abraham.Father.AssertNull();
            abraham.Mother.AssertNull();
            abraham.Sisters.AssertEmpty();
            abraham.Brothers.AssertEmpty();
            abraham.Uncles.AssertEmpty();
            abraham.Aunts.AssertEmpty();
            abraham.Sons.AssertContainsOnly(herb, homer);
            abraham.Daughters.AssertEmpty();
            //mona
            Assert.AreEqual(abraham, mona.Husband);
            mona.Father.AssertNull();
            mona.Mother.AssertNull();
            mona.Sisters.AssertEmpty();
            mona.Brothers.AssertEmpty();
            mona.Aunts.AssertEmpty();
            mona.Uncles.AssertEmpty();
            mona.Sons.AssertContainsOnly(herb, homer);
            mona.Daughters.AssertEmpty();
            //clancy
            Assert.AreEqual(jackeline, clancy.Wife);
            clancy.Father.AssertNull();
            clancy.Mother.AssertNull();
            clancy.Sisters.AssertEmpty();
            clancy.Brothers.AssertEmpty();
            clancy.Aunts.AssertEmpty();
            clancy.Uncles.AssertEmpty();
            clancy.Sons.AssertEmpty();
            clancy.Daughters.AssertContainsOnly(marge, patty, selma);
            //jackeline
            Assert.AreEqual(clancy, jackeline.Husband);
            jackeline.Father.AssertNull();
            jackeline.Mother.AssertNull();
            jackeline.Sisters.AssertEmpty();
            jackeline.Brothers.AssertEmpty();
            jackeline.Aunts.AssertEmpty();
            jackeline.Uncles.AssertEmpty();
            jackeline.Daughters.AssertContainsOnly(marge, patty, selma);
            jackeline.Sons.AssertEmpty();
            //herb
            herb.Wife.AssertNull();
            Assert.AreEqual(abraham, herb.Father);
            Assert.AreEqual(mona, herb.Mother);
            herb.Brothers.AssertContainsOnly(homer);
            herb.Sisters.AssertEmpty();
            herb.Aunts.AssertEmpty();
            herb.Uncles.AssertEmpty();
            herb.Daughters.AssertEmpty();
            herb.Sons.AssertEmpty();
            //homer
            Assert.AreEqual(marge, homer.Wife);
            Assert.AreEqual(abraham, homer.Father);
            Assert.AreEqual(mona, homer.Mother);
            homer.Brothers.AssertContainsOnly(herb);
            homer.Sisters.AssertEmpty();
            homer.Aunts.AssertEmpty();
            homer.Uncles.AssertEmpty();
            homer.Sons.AssertContainsOnly(bart);
            homer.Daughters.AssertContainsOnly(lisa, maggie);
            //marge
            Assert.AreEqual(homer, marge.Husband);
            Assert.AreEqual(clancy, marge.Father);
            Assert.AreEqual(jackeline, marge.Mother);
            marge.Brothers.AssertEmpty();
            marge.Sisters.AssertContainsOnly(patty, selma);
            marge.Aunts.AssertEmpty();
            marge.Uncles.AssertEmpty();
            marge.Sons.AssertContainsOnly(bart);
            marge.Daughters.AssertContainsOnly(lisa, maggie);
            //patty
            patty.Husband.AssertNull();
            Assert.AreEqual(clancy, patty.Father);
            Assert.AreEqual(jackeline, patty.Mother);
            patty.Sisters.AssertContainsOnly(marge, selma);
            patty.Brothers.AssertEmpty();
            patty.Aunts.AssertEmpty();
            patty.Uncles.AssertEmpty();
            patty.Daughters.AssertEmpty();
            patty.Sons.AssertEmpty();
            //selma
            selma.Husband.AssertNull();
            Assert.AreEqual(clancy, selma.Father);
            Assert.AreEqual(jackeline, selma.Mother);
            selma.Sisters.AssertContainsOnly(marge, patty);
            selma.Brothers.AssertEmpty();
            selma.Aunts.AssertEmpty();
            selma.Uncles.AssertEmpty();
            selma.Sons.AssertEmpty();
            selma.Daughters.AssertContainsOnly(ling);
            //bart
            bart.Wife.AssertNull();
            Assert.AreEqual(homer, bart.Father);
            Assert.AreEqual(marge, bart.Mother);
            bart.Sisters.AssertContainsOnly(lisa, maggie);
            bart.Brothers.AssertEmpty();
            bart.Uncles.AssertContainsOnly(herb);
            bart.Aunts.AssertContainsOnly(patty, selma);
            bart.Daughters.AssertEmpty();
            bart.Sons.AssertEmpty();
            //lisa
            lisa.Husband.AssertNull();
            Assert.AreEqual(homer, lisa.Father);
            Assert.AreEqual(marge, lisa.Mother);
            lisa.Sisters.AssertContainsOnly(maggie);
            lisa.Brothers.AssertContainsOnly(bart);
            lisa.Uncles.AssertContainsOnly(herb);
            lisa.Aunts.AssertContainsOnly(patty, selma);
            lisa.Daughters.AssertEmpty();
            lisa.Sons.AssertEmpty();
            //maggie
            maggie.Husband.AssertNull();
            Assert.AreEqual(homer, maggie.Father);
            Assert.AreEqual(marge, maggie.Mother);
            maggie.Sisters.AssertContainsOnly(lisa);
            maggie.Brothers.AssertContainsOnly(bart);
            maggie.Aunts.AssertContainsOnly(patty, selma);
            maggie.Uncles.AssertContainsOnly(herb);
            maggie.Sons.AssertEmpty();
            maggie.Daughters.AssertEmpty();
            //Ling
            ling.Husband.AssertNull();
            Assert.AreEqual(selma, ling.Mother);
            ling.Father.AssertNull();
            ling.Sisters.AssertEmpty();
            ling.Brothers.AssertEmpty();
            ling.Aunts.AssertContainsOnly(marge, patty);
            ling.Uncles.AssertEmpty();
            ling.Daughters.AssertEmpty();
            ling.Sons.AssertEmpty();
        }

        private Fam.Male LookupMale(string name, ITransformationContext context)
        {
            return context.Trace.FindWhere<Ps.Person, Fam.Male>(m => m.FirstName == name).FirstOrDefault();
        }
        private Fam.Female LookupFemale(string name, ITransformationContext context)
        {
            return context.Trace.FindWhere<Ps.Person, Fam.Female>(f => f.FirstName == name).FirstOrDefault();
        }

        public static Ps.Root CreateSimpsons()
        {
            Ps.Root root = new Ps.Root();

            var abraham = new Ps.Person()
            {
                FirstName = "Abraham",
                Gender = Ps.Gender.Male
            };

            var mona = new Ps.Person()
            {
                FirstName = "Mona",
                Gender = Ps.Gender.Female
            };

            var clancy = new Ps.Person()
            {
                FirstName = "Clancy",
                Gender = Ps.Gender.Male
            };

            var jackeline = new Ps.Person()
            {
                FirstName = "Jackeline",
                Gender = Ps.Gender.Female
            };

            var herb = new Ps.Person()
            {
                FirstName = "Herb",
                Gender = Ps.Gender.Male
            };

            var homer = new Ps.Person()
            {
                FirstName = "Homer",
                Gender = Ps.Gender.Male
            };

            var marge = new Ps.Person()
            {
                FirstName = "Marge",
                Gender = Ps.Gender.Female
            };

            var patty = new Ps.Person()
            {
                FirstName = "Patty",
                Gender = Ps.Gender.Female
            };

            var selma = new Ps.Person()
            {
                FirstName = "Selma",
                Gender = Ps.Gender.Female
            };

            var bart = new Ps.Person()
            {
                FirstName = "Bart",
                Gender = Ps.Gender.Male
            };

            var lisa = new Ps.Person()
            {
                FirstName = "Lisa",
                Gender = Ps.Gender.Female
            };

            var maggie = new Ps.Person()
            {
                FirstName = "Maggie",
                Gender = Ps.Gender.Female
            };

            var ling = new Ps.Person()
            {
                FirstName = "Ling",
                Gender = Ps.Gender.Female
            };

            abraham.Spouse = mona;
            mona.Spouse = abraham;

            abraham.Children.Add(herb);
            abraham.Children.Add(homer);

            mona.Children.Add(herb);
            mona.Children.Add(homer);

            clancy.Spouse = jackeline;
            jackeline.Spouse = clancy;

            clancy.Children.Add(marge);
            clancy.Children.Add(patty);
            clancy.Children.Add(selma);

            jackeline.Children.Add(marge);
            jackeline.Children.Add(patty);
            jackeline.Children.Add(selma);

            homer.Spouse = marge;
            marge.Spouse = homer;

            homer.Children.Add(bart);
            homer.Children.Add(lisa);
            homer.Children.Add(maggie);

            marge.Children.Add(bart);
            marge.Children.Add(lisa);
            marge.Children.Add(maggie);

            selma.Children.Add(ling);

            root.Persons.Add(abraham);
            root.Persons.Add(mona);
            root.Persons.Add(clancy);
            root.Persons.Add(jackeline);
            root.Persons.Add(herb);
            root.Persons.Add(homer);
            root.Persons.Add(marge);
            root.Persons.Add(patty);
            root.Persons.Add(selma);
            root.Persons.Add(bart);
            root.Persons.Add(lisa);
            root.Persons.Add(maggie);
            root.Persons.Add(ling);

            return root;
        }
    }
}
