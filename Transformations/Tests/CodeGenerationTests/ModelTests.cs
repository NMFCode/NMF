using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models.Repository;
using NMF.Interop.Ecore;

namespace NMF.CodeGenerationTests
{
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public void ClassModelGeneratesSuccessfully()
        {
            GenerateAndAssertEcore("Class.ecore");
        }

        [TestMethod]
        public void ArchitectureCRAModelGeneratesSuccessfully()
        {
            GenerateAndAssertEcore("architectureCRA.ecore");
        }

        [TestMethod]
        public void FamiliesModelGeneratesSuccessfully()
        {
            GenerateAndAssertEcore("Families.ecore");
        }

        [TestMethod]
        public void PersonsModelGeneratesSuccessfully()
        {
            GenerateAndAssertEcore("Persons.ecore");
        }

        [TestMethod]
        public void RailwayModelGeneratesSuccessfully()
        {
            GenerateAndAssertEcore("railway.ecore");
        }

        [TestMethod]
        public void RelationalModelGeneratesSuccessfully()
        {
            GenerateAndAssertEcore("Relational.ecore");
        }

        private void GenerateAndAssertEcore(string modelPath)
        {
            string log;
            string errorLog;
            var package = EcoreInterop.LoadPackageFromFile(modelPath);
            var ns = EcoreInterop.Transform2Meta(package);
            var result = CodeGenerationTest.GenerateAndCompile(ns, out errorLog, out log);
            Assert.AreEqual(0, result, log + errorLog);
        }
    }
}
