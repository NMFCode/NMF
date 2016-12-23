using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;
using ConsoleApplication1.ArchitectureCRA;
using NMF.Models.Repository;
using NMF.Utilities;
//using TTC2016.ArchitectureCRA.ArchitectureCRA;
using NMF.Expressions;
using NMF.Expressions.Linq;

namespace ClassDiagramOptimization
{
    class IncrementalProgram
    {
        static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var repository = new ModelRepository();
            var model = repository.Resolve(args[0]);
            var classModel = model.RootElements[0] as ClassModel;
            stopwatch.Stop();
            Console.WriteLine("Loading model took {0}ms", stopwatch.ElapsedMilliseconds);

            var dataDependencyCounts = new Dictionary<IAttribute, List<Method>>();
            var functionalDependencyCounts = new Dictionary<IMethod, List<Method>>();

            stopwatch.Restart();
            foreach (var feature in classModel.Features.OfType<Method>())
            {
                var featureClass = new Class()
                {
                    Name = "C" + feature.Name
                };
                featureClass.Encapsulates.Add(feature);
                classModel.Classes.Add(featureClass);
                foreach (var dataDependency in feature.DataDependency)
                {
                    List<Method> methods;
                    if (!dataDependencyCounts.TryGetValue(dataDependency, out methods))
                    {
                        methods = new List<Method>();
                        dataDependencyCounts.Add(dataDependency, methods);
                    }
                    methods.Add(feature);
                }
                foreach (var functionalDependency in feature.FunctionalDependency)
                {
                    List<Method> methods;
                    if (!functionalDependencyCounts.TryGetValue(functionalDependency, out methods))
                    {
                        methods = new List<Method>();
                        functionalDependencyCounts.Add(functionalDependency, methods);
                    }
                    methods.Add(feature);
                }
                if (!functionalDependencyCounts.ContainsKey(feature)) functionalDependencyCounts.Add(feature, new List<Method>());
            }
            foreach (var attribute in classModel.Features.OfType<IAttribute>())
            {
                List<Method> dependingMethods;
                if (dataDependencyCounts.TryGetValue(attribute, out dependingMethods))
                {
                    if (dependingMethods.Count == 1)
                    {
                        dependingMethods[0].IsEncapsulatedBy.Encapsulates.Add(attribute);
                        continue;
                    }
                }
                else
                {
                    dependingMethods = new List<Method>();
                    dataDependencyCounts.Add(attribute, dependingMethods);
                }
                var featureClass = new Class()
                {
                    Name = "C" + attribute.Name
                };
                featureClass.Encapsulates.Add(attribute);
                classModel.Classes.Add(featureClass);
            }

            var combinationCount = new Func<int, int>(i => i * (i - 1));
            var divideOrZero = new Func<double, double, double>((a, b) => b == 0 ? 0 : a / b);

            var M = new MemoizedFunc<IClass, int>(cl => cl == null ? 0 : cl.Encapsulates.OfType<Method>().Count());
            var A = new MemoizedFunc<IClass, int>(cl => cl == null ? 0 : cl.Encapsulates.OfType<IAttribute>().Count());

            var MAI = new MemoizedFunc<IClass, IClass, double>((cl_i, cl_j) =>
                cl_i == null || cl_j == null ? 0 : 
                cl_i.Encapsulates.OfType<Method>()
                    .Sum(m => m.DataDependency.Intersect(cl_j.Encapsulates).Count()));

            var MMI = new MemoizedFunc<IClass, IClass, double>((cl_i, cl_j) =>
                cl_i == null || cl_j == null ? 0 :
                cl_i.Encapsulates.OfType<Method>()
                    .Sum(m => m.FunctionalDependency.Intersect(cl_j.Encapsulates).Count()));

            var dataDependingClassesCoupling = new ObservingFunc<IClass, IClass, double>(
                (cl_i, cl_j) =>
                    (from att in cl_i.Encapsulates.OfType<IAttribute>().Concat(cl_j.Encapsulates.OfType<IAttribute>())
                     from meth in dataDependencyCounts[att]
                     select meth.IsEncapsulatedBy).Distinct().Sum(ck => 
                        ck == cl_i || ck == cl_j ? 0
                            : (divideOrZero(MAI[ck, cl_i], M[ck] * A[cl_i]) + 
                               divideOrZero(MAI[ck, cl_j], A[cl_j] * M[ck]) - 
                               divideOrZero(MAI[ck, cl_i] + MAI[ck, cl_j], (A[cl_i] + A[cl_j]) * M[ck]))
                     ));
            var dataDependingClassesCouplingMemo = new MemoizedFunc<IClass, IClass, INotifyValue<double>>(
                (cl_i, cl_j) => dataDependingClassesCoupling.Observe(cl_i, cl_j));

            var functionalDependingClassesCoupling = new ObservingFunc<IClass, IClass, double>(
                (cl_i, cl_j) =>
                    (from m2 in cl_i.Encapsulates.OfType<Method>().Concat(cl_j.Encapsulates.OfType<Method>())
                     from meth in functionalDependencyCounts[m2]
                     select meth.IsEncapsulatedBy).Distinct().Sum(ck =>
                        ck == cl_i || ck == cl_j ? 0
                           : (divideOrZero(MMI[ck, cl_i], (M[cl_i] - 1) * M[ck]) +
                              divideOrZero(MMI[ck, cl_j], (M[cl_j] - 1) * M[ck]) -
                              divideOrZero(MMI[ck, cl_i] + MMI[ck, cl_j], (M[cl_i] + M[cl_j] - 1) * M[ck]))
                     ));

            var functionalDependingClassesCouplingMemo = new MemoizedFunc<IClass, IClass, INotifyValue<double>>(
                (cl_i, cl_j) => functionalDependingClassesCoupling.Observe(cl_i, cl_j));

            var dataDependentClassesCoupling = new ObservingFunc<IClass, IClass, double>(
                (cl_i, cl_j) =>
                    (from meth in cl_i.Encapsulates.OfType<Method>().Concat(cl_j.Encapsulates.OfType<Method>())
                     from dataDep in meth.DataDependency
                     select dataDep.IsEncapsulatedBy).Distinct().Sum(ck =>
                        divideOrZero(MAI[cl_i, ck], M[cl_i] * A[ck]) + 
                        divideOrZero(MAI[cl_j, ck], M[cl_j] * A[ck]) - 
                        divideOrZero(MAI[cl_i, ck] + MAI[cl_j, ck], (M[cl_i] + M[cl_j]) * A[ck])
                     ));
            var dataDependentClassesCouplingMemo = new MemoizedFunc<IClass, IClass, INotifyValue<double>>(
                (cl_i, cl_j) => dataDependentClassesCoupling.Observe(cl_i, cl_j));

            var functionalDependentClassesCoupling = new ObservingFunc<IClass, IClass, double>(
                (cl_i, cl_j) =>
                    (from meth in cl_i.Encapsulates.OfType<Method>().Concat(cl_j.Encapsulates.OfType<Method>())
                     from functionalDep in meth.FunctionalDependency
                     select functionalDep.IsEncapsulatedBy).Distinct().Sum(ck =>
                        ck == cl_i || ck == cl_j ? 0
                           : divideOrZero(MMI[cl_i, ck], M[cl_i] * M[ck]) + 
                             divideOrZero(MMI[cl_j, ck], M[cl_j] * M[ck]) - 
                             divideOrZero(MMI[cl_i, ck] + MMI[cl_j, ck], (M[cl_i] + M[cl_j]) * M[ck])
                     ));

            var functionalDependentClassesCouplingMemo = new MemoizedFunc<IClass, IClass, INotifyValue<double>>(
                (cl_i, cl_j) => functionalDependingClassesCoupling.Observe(cl_i, cl_j));



            var Effect = new Func<IClass, IClass, double>((cl_i, cl_j) =>
            {
                var M_i = M[cl_i];
                var M_j = M[cl_j];
                var A_i = A[cl_i];
                var A_j = A[cl_j];
                var MAI_i = MAI[cl_i, cl_i];
                var MAI_j = MAI[cl_j, cl_j];
                var MAI_ij = MAI[cl_i, cl_j];
                var MAI_ji = MAI[cl_j, cl_i];
                var MMI_i = MMI[cl_i, cl_i];
                var MMI_j = MMI[cl_j, cl_j];
                var MMI_ij = MMI[cl_i, cl_j];
                var MMI_ji = MMI[cl_j, cl_i];
                var deltaCohesionDataDep = // Delta of Cohesion based on data dependencies
                        divideOrZero(MAI_i + MAI_ij + MAI_ji + MAI_j, (M_i + M_j) * (A_i + A_j)) - divideOrZero(MAI_i, M_i * A_i) - divideOrZero(MAI_j, M_j * A_j);
                var deltaCohesionFunctionalDep = // Delta of Cohesion based on functional dependencies
                        divideOrZero(MMI_i + MMI_ij + MMI_ji + MMI_j, combinationCount(M_i + M_j)) - divideOrZero(MMI_i, combinationCount(M_i)) - divideOrZero(MMI_j, combinationCount(M_j));
                var deltaCouplingij = // Delta of Coupling between C_i and C_j
                        divideOrZero(MAI_ij, M_i * A_j) + divideOrZero(MAI_ji, M_j * A_i) + divideOrZero(MMI_ij, M_i * (M_j - 1)) + divideOrZero(MMI_ji, M_j * (M_i - 1));

                var deltaCouplingFromOthers = // Coupling from other classes
                        dataDependingClassesCouplingMemo[cl_i, cl_j].Value + functionalDependingClassesCouplingMemo[cl_i, cl_j].Value;
                var deltaCouplingToOthers = dataDependentClassesCouplingMemo[cl_i, cl_j].Value + functionalDependentClassesCouplingMemo[cl_i, cl_j].Value;
                return deltaCohesionDataDep + deltaCohesionFunctionalDep + deltaCouplingij + deltaCouplingToOthers + deltaCouplingFromOthers;
            });

            var allClasses = false;
            var prioritizedMerges = (from cl_i in classModel.Classes
                                     where allClasses || cl_i.Encapsulates.All(f => f is IAttribute)
                                     from cl_j in classModel.Classes
                                     where cl_i.Name.CompareTo(cl_j.Name) < 0
                                     select new
                                     {
                                         Cl_i = cl_i,
                                         Cl_j = cl_j,
                                         Effect = Effect(cl_i, cl_j)
                                     }).OrderByDescending(m => m.Effect);

            var nextMerge = prioritizedMerges.FirstOrDefault();
            var classCounter = 1;
            while (nextMerge != null && nextMerge.Effect > 0)
            {
                Console.WriteLine("Now merging {0} and {1}, should have effect {2}", nextMerge.Cl_i.Name, nextMerge.Cl_j.Name, nextMerge.Effect);
                // We need to save the features from these classes as they will be dropped as soon as we delete the encapsulating classes
                var newFeatures = nextMerge.Cl_i.Encapsulates.Concat(nextMerge.Cl_j.Encapsulates).ToList();
                classModel.Classes.Remove(nextMerge.Cl_i);
                classModel.Classes.Remove(nextMerge.Cl_j);
                var newClass = new Class() { Name = "C" + (classCounter++).ToString() };
                newClass.Encapsulates.AddRange(newFeatures);
                classModel.Classes.Add(newClass);
                nextMerge = prioritizedMerges.FirstOrDefault();
            }
            allClasses = true;
            nextMerge = prioritizedMerges.FirstOrDefault();
            while (nextMerge != null && nextMerge.Effect > 0)
            {
                Console.WriteLine("Now merging {0} and {1}, should have effect {2}", nextMerge.Cl_i.Name, nextMerge.Cl_j.Name, nextMerge.Effect);
                // We need to save the features from these classes as they will be dropped as soon as we delete the encapsulating classes
                var newFeatures = nextMerge.Cl_i.Encapsulates.Concat(nextMerge.Cl_j.Encapsulates).ToList();
                classModel.Classes.Remove(nextMerge.Cl_i);
                classModel.Classes.Remove(nextMerge.Cl_j);
                var newClass = new Class() { Name = "C" + (classCounter++).ToString() };
                newClass.Encapsulates.AddRange(newFeatures);
                classModel.Classes.Add(newClass);
                nextMerge = prioritizedMerges.FirstOrDefault();
            }

            stopwatch.Stop();
            Console.WriteLine("Model optimization took {0}ms", stopwatch.ElapsedMilliseconds);
            using (var writer = File.AppendText("results.csv"))
            {
                writer.WriteLine("Incremental;{0};{1}", stopwatch.ElapsedMilliseconds, args[0]);
            }

            Console.WriteLine("Output contains {0} classes", classModel.Classes.Count);
            stopwatch.Restart();
            classModel.Name = "Optimized Class Model";
            repository.Save(classModel, Path.ChangeExtension(args[0], ".Output.xmi"));
            stopwatch.Stop();

            Console.WriteLine("Serializing result model took {0}ms", stopwatch.ElapsedMilliseconds);
        }
    }
}
