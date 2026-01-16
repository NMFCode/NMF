using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models.Repository;
using NMF.Interop.Ecore;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;

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

        [TestMethod]
        public void SmartGridSchemaGeneratesSuccessfully()
        {
            GenerateAndAssertEcore("schema.ecore");
        }

        [TestMethod]
        public void KDMGeneratesSuccessfully()
        {
            GenerateAndAssertEcore("KDM.ecore");
        }

        [TestMethod]
        public void NameClashesResolvedSuccessfully()
        {
            GenerateAndAssertEcore("NameClashes.ecore");
        }

        [TestMethod]
        public void DefaultValueExampleGeneratesAndInstanceCanBeLoaded()
        {
            GenerateAndAssertEcore("DefaultValueTest.ecore");
        }

        [TestMethod]
        public void OperationsTestGeneratedSuccessfully()
        {
            GenerateAndAssertEcore("OperationTest.ecore");
        }

        private void GenerateAndAssertEcore(string modelPath)
        {
            string log;
            string errorLog;
            var package = EcoreInterop.LoadPackageFromFile(modelPath);
            var ns = EcoreInterop.Transform2Meta(package);
            var codeFile = Path.ChangeExtension(modelPath, ".cs");
            var result = CodeGenerationTest.GenerateAndCompile(ns, null, ".", Path.ChangeExtension(modelPath, ".csproj"), codeFile, out errorLog, out log);
            Console.WriteLine(errorLog);
            Assert.AreEqual(0, result, log + errorLog);

            var codeFileText = File.ReadAllText(codeFile);
            var reference = File.ReadAllText(Path.Combine("References", codeFile));

            AssertTextEqual(EliminateWhitespaces(codeFileText), EliminateWhitespaces(reference));
        }

        private static void AssertTextEqual(string actual, string expected)
        {
            Assert.IsNotNull(actual, "First string is null");
            Assert.IsNotNull(expected, "Second string is null");
            var index = 0;
            while (index < actual.Length && index < expected.Length)
            {
                if (actual[index] != expected[index]) break;
                index++;
            }
            if (index < actual.Length || index < expected.Length)
            {
                index = Math.Max(0, index - 50);
                Assert.Fail($"Strings are not equal and differ at index {index}.\n Expected '{expected.Substring(index, Math.Min(100, expected.Length - index))}'\n but received '{actual.Substring(index, Math.Min(100, actual.Length - index))}'");
            }
        }

        private static string EliminateWhitespaces(string input)
        {
            return Regex.Replace(Regex.Replace(input, "///?[^/].*", string.Empty), @"\s+", string.Empty);
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
