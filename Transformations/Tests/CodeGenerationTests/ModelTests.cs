using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models.Repository;
using NMF.Interop.Ecore;
using System.Reflection;

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
        public void FromSchemaModelGeneratesSuccessfully()
        {
            GenerateAndAssertEcore("FromSchemaEcore.ecore");
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

        [TestMethod]
        public void Iso61850GeneratesSuccessfully()
        {
            GenerateAndAssertEcore("61850.ecore");
        }

        [TestMethod]
        public void CosemModelGeneratesSuccessfully()
        {
            GenerateAndAssertEcore("COSEM.ecore");
        }

        //[TestMethod]
        //public void SmartGridSchemaGeneratesSuccessfully()
        //{
        //    GenerateAndAssertEcore("schema.ecore");
        //}

        [TestMethod]
        public void DefaultValueExampleGeneratesAndInstanceCanBeLoaded()
        {
            GenerateAndAssertEcore("DefaultValueTest.ecore");
        }

        private void GenerateAndAssertEcore(string modelPath)
        {
            string log;
            string errorLog;
            var package = EcoreInterop.LoadPackageFromFile(modelPath);
            var ns = EcoreInterop.Transform2Meta(package);
            var result = CodeGenerationTest.GenerateAndCompile(ns, null, out errorLog, out log);
            Assert.AreEqual(0, result, log + errorLog);
        }

        private void GenerateAndLoadModel(string metamodelPath, string modelPath)
        {
            string log;
            string errorLog;
            var package = EcoreInterop.LoadPackageFromFile(metamodelPath);
            var ns = EcoreInterop.Transform2Meta(package);

            var loadModel = new Action<string, int>((path, buildExit) =>
            {
                if (buildExit != 0) return;
                var repository = new ModelRepository();
                var assemblyPath = System.IO.Path.Combine(path, "bin", "project.dll");
                Assembly.LoadFile(assemblyPath);
                repository.Resolve(modelPath);
            });

            var result = CodeGenerationTest.GenerateAndCompile(ns, loadModel, out errorLog, out log);
            Assert.AreEqual(0, result, log + errorLog);
        }
    }
}
