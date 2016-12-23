using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;
using ConsoleApplication1.ArchitectureCRA;
using NMF.Expressions;
using NMF.Models;
using NMF.Models.Repository;
using NMF.Utilities;

namespace ClassDiagramOptimization
{
    class Program
    {
        private static void BenchmarkRepetitions(int repetitions, string modelPath, string benchmarkName)
        {
            var results = new long[repetitions];
            var stopwatch = new Stopwatch();
            var repository = new ModelRepository();
            
            var dataDependencyCounts = new Dictionary<IAttribute, List<Method>>();
            var functionalDependencyCounts = new Dictionary<IMethod, List<Method>>();

            for (int i = 0; i < repetitions; i++)
            {
                //reset environment
                var model = repository.Resolve(modelPath);
                var classModel = model.RootElements[0] as ClassModel;
                dataDependencyCounts.Clear();
                functionalDependencyCounts.Clear();

                stopwatch.Restart();
                if (benchmarkName.Equals("master"))
                {
                    DoActions(classModel, dataDependencyCounts, functionalDependencyCounts);
                }
                else if (benchmarkName.Equals("executionEngine"))
                {
                    var engine = ExecutionEngine.Current;
                    engine.BeginTransaction();
                    DoActions(classModel, dataDependencyCounts, functionalDependencyCounts);
                    engine.CommitTransaction();
                }
                else if (benchmarkName.Equals("modelTransaction"))
                {
                    using (var transaction = new ModelTransaction(classModel))
                    {
                        DoActions(classModel, dataDependencyCounts, functionalDependencyCounts);
                        transaction.Commit();
                    }
                }
                else
                {
                    throw new ArgumentException($"Benchmark type {benchmarkName} is not defined.");
                }
                
                stopwatch.Stop();

                results[i] = stopwatch.ElapsedMilliseconds;
            }

            using (var writer = File.AppendText("results.csv"))
            {
                writer.WriteLine($"{benchmarkName};{results.Average()};{modelPath}");
            }
        }

        private static void DoActions(ClassModel classModel, Dictionary<IAttribute, List<Method>> dataDependencyCounts, Dictionary<IMethod, List<Method>> functionalDependencyCounts)
        {
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

            var M = new Func<IClass, int>(cl => cl.Encapsulates.OfType<Method>().Count());
            var A = new Func<IClass, int>(cl => cl.Encapsulates.OfType<IAttribute>().Count());

            var MAI = new Func<IClass, IClass, double>((cl_i, cl_j) =>
                cl_i.Encapsulates.OfType<Method>()
                    .Sum(m => m.DataDependency.Intersect(cl_j.Encapsulates).Count()));

            var MMI = new Func<IClass, IClass, double>((cl_i, cl_j) =>
                cl_i.Encapsulates.OfType<Method>()
                    .Sum(m => m.FunctionalDependency.Intersect(cl_j.Encapsulates).Count()));

            var Effect = new Func<IClass, IClass, double>((cl_i, cl_j) =>
            {
                var M_i = M(cl_i);
                var M_j = M(cl_j);
                var A_i = A(cl_i);
                var A_j = A(cl_j);
                var MAI_i = MAI(cl_i, cl_i);
                var MAI_j = MAI(cl_j, cl_j);
                var MAI_ij = MAI(cl_i, cl_j);
                var MAI_ji = MAI(cl_j, cl_i);
                var MMI_i = MMI(cl_i, cl_i);
                var MMI_j = MMI(cl_j, cl_j);
                var MMI_ij = MMI(cl_i, cl_j);
                var MMI_ji = MMI(cl_j, cl_i);
                var deltaCohesionDataDep = // Delta of Cohesion based on data dependencies
                        divideOrZero(MAI_i + MAI_ij + MAI_ji + MAI_j, (M_i + M_j) * (A_i + A_j)) - divideOrZero(MAI_i, M_i * A_i) - divideOrZero(MAI_j, M_j * A_j);
                var deltaCohesionFunctionalDep = // Delta of Cohesion based on functional dependencies
                        divideOrZero(MMI_i + MMI_ij + MMI_ji + MMI_j, combinationCount(M_i + M_j)) - divideOrZero(MMI_i, combinationCount(M_i)) - divideOrZero(MMI_j, combinationCount(M_j));
                var deltaCouplingij = // Delta of Coupling between C_i and C_j
                        divideOrZero(MAI_ij, M_i * A_j) + divideOrZero(MAI_ji, M_j * A_i) + divideOrZero(MMI_ij, M_i * (M_j - 1)) + divideOrZero(MMI_ji, M_j * (M_i - 1));
                var classIAttributes = cl_i.Encapsulates.OfType<IAttribute>();
                var classJAttributes = cl_j.Encapsulates.OfType<IAttribute>();
                var classIMethods = cl_i.Encapsulates.OfType<Method>();
                var classJMethods = cl_j.Encapsulates.OfType<Method>();
                var dataDependingClasses = (from att in classIAttributes.Concat(classJAttributes)
                                            from meth in dataDependencyCounts[att]
                                            select meth.IsEncapsulatedBy).Distinct();
                var functionalDependingClasses = (from m2 in classIMethods.Concat(classJMethods)
                                                  from meth in functionalDependencyCounts[m2]
                                                  select meth.IsEncapsulatedBy).Distinct();
                var deltaCouplingFromOthers = // Coupling from other classes
                        dataDependingClasses.Sum(ck =>
                        {
                            if (ck == cl_i || ck == cl_j) return 0;
                            var mai_ki = MAI(ck, cl_i);
                            var mai_kj = MAI(ck, cl_j);
                            var m_k = M(ck);
                            return (divideOrZero(mai_ki, m_k * A_i) + divideOrZero(mai_kj, A_j * m_k) - divideOrZero(mai_ki + mai_kj, (A_i + A_j) * m_k));
                        })
                        +
                        functionalDependingClasses.Sum(ck =>
                        {
                            if (ck == cl_i || ck == cl_j) return 0;
                            var mmi_ki = MMI(ck, cl_i);
                            var mmi_kj = MMI(ck, cl_j);
                            var m_k = M(ck);
                            return (divideOrZero(mmi_ki, (M_i - 1) * m_k) + divideOrZero(mmi_kj, (M_j - 1) * m_k) - divideOrZero(mmi_ki + mmi_kj, (M_i + M_j - 1) * m_k));
                        });
                var dataDependentClasses = (from meth in classIMethods.Concat(classJMethods)
                                            from dataDep in meth.DataDependency
                                            select dataDep.IsEncapsulatedBy).Distinct();
                var FunctionalDependentClasses = (from meth in classIMethods.Concat(classJMethods)
                                                  from funDep in meth.FunctionalDependency
                                                  select funDep.IsEncapsulatedBy).Distinct();
                var deltaCouplingToOthers = dataDependentClasses.Sum(ck =>
                {
                    var mai_ik = MAI(cl_i, ck);
                    var mai_jk = MAI(cl_j, ck);
                    var a_k = A(ck);
                    return (divideOrZero(mai_ik, M_i * a_k) + divideOrZero(mai_jk, M_j * a_k) - divideOrZero(mai_ik + mai_jk, (M_i + M_j) * a_k));
                })
                        +
                        FunctionalDependentClasses.Sum(ck =>
                        {
                            if (ck == cl_i || ck == cl_j) return 0;
                            var mmi_ik = MMI(cl_i, ck);
                            var mmi_jk = MMI(cl_j, ck);
                            var m_k = M(ck) - 1;
                            return (divideOrZero(mmi_ik, M_i * m_k) + divideOrZero(mmi_jk, M_j * m_k) - divideOrZero(mmi_ik + mmi_jk, (M_i + M_j) * m_k));
                        });
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
                //Console.WriteLine("Now merging {0} and {1}, should have effect {2}", nextMerge.Cl_i.Name, nextMerge.Cl_j.Name, nextMerge.Effect);
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
                //Console.WriteLine("Now merging {0} and {1}, should have effect {2}", nextMerge.Cl_i.Name, nextMerge.Cl_j.Name, nextMerge.Effect);
                // We need to save the features from these classes as they will be dropped as soon as we delete the encapsulating classes
                var newFeatures = nextMerge.Cl_i.Encapsulates.Concat(nextMerge.Cl_j.Encapsulates).ToList();
                classModel.Classes.Remove(nextMerge.Cl_i);
                classModel.Classes.Remove(nextMerge.Cl_j);
                var newClass = new Class() { Name = "C" + (classCounter++).ToString() };
                newClass.Encapsulates.AddRange(newFeatures);
                classModel.Classes.Add(newClass);
                nextMerge = prioritizedMerges.FirstOrDefault();
            }
        }

        private static long BenchmarkModelWithTransactions(string modelPath)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var repository = new ModelRepository();
            var model = repository.Resolve(modelPath);
            var classModel = model.RootElements[0] as ClassModel;
            stopwatch.Stop();
            Console.WriteLine("Loading model took {0}ms", stopwatch.ElapsedMilliseconds);

            var dataDependencyCounts = new Dictionary<IAttribute, List<Method>>();
            var functionalDependencyCounts = new Dictionary<IMethod, List<Method>>();
            
            stopwatch.Restart();
            using (var transaction = new ModelTransaction(classModel))
            {
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

                var M = new Func<IClass, int>(cl => cl.Encapsulates.OfType<Method>().Count());
                var A = new Func<IClass, int>(cl => cl.Encapsulates.OfType<IAttribute>().Count());

                var MAI = new Func<IClass, IClass, double>((cl_i, cl_j) =>
                    cl_i.Encapsulates.OfType<Method>()
                        .Sum(m => m.DataDependency.Intersect(cl_j.Encapsulates).Count()));

                var MMI = new Func<IClass, IClass, double>((cl_i, cl_j) =>
                    cl_i.Encapsulates.OfType<Method>()
                        .Sum(m => m.FunctionalDependency.Intersect(cl_j.Encapsulates).Count()));

                var Effect = new Func<IClass, IClass, double>((cl_i, cl_j) =>
                {
                    var M_i = M(cl_i);
                    var M_j = M(cl_j);
                    var A_i = A(cl_i);
                    var A_j = A(cl_j);
                    var MAI_i = MAI(cl_i, cl_i);
                    var MAI_j = MAI(cl_j, cl_j);
                    var MAI_ij = MAI(cl_i, cl_j);
                    var MAI_ji = MAI(cl_j, cl_i);
                    var MMI_i = MMI(cl_i, cl_i);
                    var MMI_j = MMI(cl_j, cl_j);
                    var MMI_ij = MMI(cl_i, cl_j);
                    var MMI_ji = MMI(cl_j, cl_i);
                    var deltaCohesionDataDep = // Delta of Cohesion based on data dependencies
                            divideOrZero(MAI_i + MAI_ij + MAI_ji + MAI_j, (M_i + M_j) * (A_i + A_j)) - divideOrZero(MAI_i, M_i * A_i) - divideOrZero(MAI_j, M_j * A_j);
                    var deltaCohesionFunctionalDep = // Delta of Cohesion based on functional dependencies
                            divideOrZero(MMI_i + MMI_ij + MMI_ji + MMI_j, combinationCount(M_i + M_j)) - divideOrZero(MMI_i, combinationCount(M_i)) - divideOrZero(MMI_j, combinationCount(M_j));
                    var deltaCouplingij = // Delta of Coupling between C_i and C_j
                            divideOrZero(MAI_ij, M_i * A_j) + divideOrZero(MAI_ji, M_j * A_i) + divideOrZero(MMI_ij, M_i * (M_j - 1)) + divideOrZero(MMI_ji, M_j * (M_i - 1));
                    var classIAttributes = cl_i.Encapsulates.OfType<IAttribute>();
                    var classJAttributes = cl_j.Encapsulates.OfType<IAttribute>();
                    var classIMethods = cl_i.Encapsulates.OfType<Method>();
                    var classJMethods = cl_j.Encapsulates.OfType<Method>();
                    var dataDependingClasses = (from att in classIAttributes.Concat(classJAttributes)
                                                from meth in dataDependencyCounts[att]
                                                select meth.IsEncapsulatedBy).Distinct();
                    var functionalDependingClasses = (from m2 in classIMethods.Concat(classJMethods)
                                                      from meth in functionalDependencyCounts[m2]
                                                      select meth.IsEncapsulatedBy).Distinct();
                    var deltaCouplingFromOthers = // Coupling from other classes
                            dataDependingClasses.Sum(ck =>
                            {
                                if (ck == cl_i || ck == cl_j) return 0;
                                var mai_ki = MAI(ck, cl_i);
                                var mai_kj = MAI(ck, cl_j);
                                var m_k = M(ck);
                                return (divideOrZero(mai_ki, m_k * A_i) + divideOrZero(mai_kj, A_j * m_k) - divideOrZero(mai_ki + mai_kj, (A_i + A_j) * m_k));
                            })
                            +
                            functionalDependingClasses.Sum(ck =>
                            {
                                if (ck == cl_i || ck == cl_j) return 0;
                                var mmi_ki = MMI(ck, cl_i);
                                var mmi_kj = MMI(ck, cl_j);
                                var m_k = M(ck);
                                return (divideOrZero(mmi_ki, (M_i - 1) * m_k) + divideOrZero(mmi_kj, (M_j - 1) * m_k) - divideOrZero(mmi_ki + mmi_kj, (M_i + M_j - 1) * m_k));
                            });
                    var dataDependentClasses = (from meth in classIMethods.Concat(classJMethods)
                                                from dataDep in meth.DataDependency
                                                select dataDep.IsEncapsulatedBy).Distinct();
                    var FunctionalDependentClasses = (from meth in classIMethods.Concat(classJMethods)
                                                      from funDep in meth.FunctionalDependency
                                                      select funDep.IsEncapsulatedBy).Distinct();
                    var deltaCouplingToOthers = dataDependentClasses.Sum(ck =>
                    {
                        var mai_ik = MAI(cl_i, ck);
                        var mai_jk = MAI(cl_j, ck);
                        var a_k = A(ck);
                        return (divideOrZero(mai_ik, M_i * a_k) + divideOrZero(mai_jk, M_j * a_k) - divideOrZero(mai_ik + mai_jk, (M_i + M_j) * a_k));
                    })
                            +
                            FunctionalDependentClasses.Sum(ck =>
                            {
                                if (ck == cl_i || ck == cl_j) return 0;
                                var mmi_ik = MMI(cl_i, ck);
                                var mmi_jk = MMI(cl_j, ck);
                                var m_k = M(ck) - 1;
                                return (divideOrZero(mmi_ik, M_i * m_k) + divideOrZero(mmi_jk, M_j * m_k) - divideOrZero(mmi_ik + mmi_jk, (M_i + M_j) * m_k));
                            });
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
                transaction.Commit();
            }
            stopwatch.Stop();
            Console.WriteLine("Model optimization took {0}ms", stopwatch.ElapsedMilliseconds);

            /*using (var writer = File.AppendText("results.csv"))
            {
                writer.WriteLine("Transaction;{0};{1}", stopwatch.ElapsedMilliseconds, modelPath);

            }*/
            return stopwatch.ElapsedMilliseconds;

            /*Console.WriteLine("Output contains {0} classes", classModel.Classes.Count);
            stopwatch.Restart();
            classModel.Name = "Optimized Class Model";
            repository.Save(classModel, Path.ChangeExtension(modelPath, ".Output.xmi"));
            stopwatch.Stop();

            Console.WriteLine("Serializing result model took {0}ms", stopwatch.ElapsedMilliseconds);
            Console.Read();*/
        }

        private static long BenchmarkModelWithShortTransactions(string modelPath)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var repository = new ModelRepository();
            var model = repository.Resolve(modelPath);
            var classModel = model.RootElements[0] as ClassModel;
            stopwatch.Stop();
            Console.WriteLine("Loading model took {0}ms", stopwatch.ElapsedMilliseconds);

            var dataDependencyCounts = new Dictionary<IAttribute, List<Method>>();
            var functionalDependencyCounts = new Dictionary<IMethod, List<Method>>();

            stopwatch.Restart();
            foreach (var feature in classModel.Features.OfType<Method>())
            {
                using (var transaction = new ModelTransaction(classModel))
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
                    if (!functionalDependencyCounts.ContainsKey(feature))
                        functionalDependencyCounts.Add(feature, new List<Method>());
                    transaction.Commit();
                }
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
                using (var transaction = new ModelTransaction(classModel))
                {
                    featureClass.Encapsulates.Add(attribute);
                    classModel.Classes.Add(featureClass);

                    transaction.Commit();
                }
            }

            var combinationCount = new Func<int, int>(i => i * (i - 1));
            var divideOrZero = new Func<double, double, double>((a, b) => b == 0 ? 0 : a / b);

            var M = new Func<IClass, int>(cl => cl.Encapsulates.OfType<Method>().Count());
            var A = new Func<IClass, int>(cl => cl.Encapsulates.OfType<IAttribute>().Count());

            var MAI = new Func<IClass, IClass, double>((cl_i, cl_j) =>
                cl_i.Encapsulates.OfType<Method>()
                    .Sum(m => m.DataDependency.Intersect(cl_j.Encapsulates).Count()));

            var MMI = new Func<IClass, IClass, double>((cl_i, cl_j) =>
                cl_i.Encapsulates.OfType<Method>()
                    .Sum(m => m.FunctionalDependency.Intersect(cl_j.Encapsulates).Count()));

            var Effect = new Func<IClass, IClass, double>((cl_i, cl_j) =>
            {
                var M_i = M(cl_i);
                var M_j = M(cl_j);
                var A_i = A(cl_i);
                var A_j = A(cl_j);
                var MAI_i = MAI(cl_i, cl_i);
                var MAI_j = MAI(cl_j, cl_j);
                var MAI_ij = MAI(cl_i, cl_j);
                var MAI_ji = MAI(cl_j, cl_i);
                var MMI_i = MMI(cl_i, cl_i);
                var MMI_j = MMI(cl_j, cl_j);
                var MMI_ij = MMI(cl_i, cl_j);
                var MMI_ji = MMI(cl_j, cl_i);
                var deltaCohesionDataDep = // Delta of Cohesion based on data dependencies
                        divideOrZero(MAI_i + MAI_ij + MAI_ji + MAI_j, (M_i + M_j) * (A_i + A_j)) - divideOrZero(MAI_i, M_i * A_i) - divideOrZero(MAI_j, M_j * A_j);
                var deltaCohesionFunctionalDep = // Delta of Cohesion based on functional dependencies
                        divideOrZero(MMI_i + MMI_ij + MMI_ji + MMI_j, combinationCount(M_i + M_j)) - divideOrZero(MMI_i, combinationCount(M_i)) - divideOrZero(MMI_j, combinationCount(M_j));
                var deltaCouplingij = // Delta of Coupling between C_i and C_j
                        divideOrZero(MAI_ij, M_i * A_j) + divideOrZero(MAI_ji, M_j * A_i) + divideOrZero(MMI_ij, M_i * (M_j - 1)) + divideOrZero(MMI_ji, M_j * (M_i - 1));
                var classIAttributes = cl_i.Encapsulates.OfType<IAttribute>();
                var classJAttributes = cl_j.Encapsulates.OfType<IAttribute>();
                var classIMethods = cl_i.Encapsulates.OfType<Method>();
                var classJMethods = cl_j.Encapsulates.OfType<Method>();
                var dataDependingClasses = (from att in classIAttributes.Concat(classJAttributes)
                                            from meth in dataDependencyCounts[att]
                                            select meth.IsEncapsulatedBy).Distinct();
                var functionalDependingClasses = (from m2 in classIMethods.Concat(classJMethods)
                                                    from meth in functionalDependencyCounts[m2]
                                                    select meth.IsEncapsulatedBy).Distinct();
                var deltaCouplingFromOthers = // Coupling from other classes
                        dataDependingClasses.Sum(ck =>
                        {
                            if (ck == cl_i || ck == cl_j) return 0;
                            var mai_ki = MAI(ck, cl_i);
                            var mai_kj = MAI(ck, cl_j);
                            var m_k = M(ck);
                            return (divideOrZero(mai_ki, m_k * A_i) + divideOrZero(mai_kj, A_j * m_k) - divideOrZero(mai_ki + mai_kj, (A_i + A_j) * m_k));
                        })
                        +
                        functionalDependingClasses.Sum(ck =>
                        {
                            if (ck == cl_i || ck == cl_j) return 0;
                            var mmi_ki = MMI(ck, cl_i);
                            var mmi_kj = MMI(ck, cl_j);
                            var m_k = M(ck);
                            return (divideOrZero(mmi_ki, (M_i - 1) * m_k) + divideOrZero(mmi_kj, (M_j - 1) * m_k) - divideOrZero(mmi_ki + mmi_kj, (M_i + M_j - 1) * m_k));
                        });
                var dataDependentClasses = (from meth in classIMethods.Concat(classJMethods)
                                            from dataDep in meth.DataDependency
                                            select dataDep.IsEncapsulatedBy).Distinct();
                var FunctionalDependentClasses = (from meth in classIMethods.Concat(classJMethods)
                                                    from funDep in meth.FunctionalDependency
                                                    select funDep.IsEncapsulatedBy).Distinct();
                var deltaCouplingToOthers = dataDependentClasses.Sum(ck =>
                {
                    var mai_ik = MAI(cl_i, ck);
                    var mai_jk = MAI(cl_j, ck);
                    var a_k = A(ck);
                    return (divideOrZero(mai_ik, M_i * a_k) + divideOrZero(mai_jk, M_j * a_k) - divideOrZero(mai_ik + mai_jk, (M_i + M_j) * a_k));
                })
                        +
                        FunctionalDependentClasses.Sum(ck =>
                        {
                            if (ck == cl_i || ck == cl_j) return 0;
                            var mmi_ik = MMI(cl_i, ck);
                            var mmi_jk = MMI(cl_j, ck);
                            var m_k = M(ck) - 1;
                            return (divideOrZero(mmi_ik, M_i * m_k) + divideOrZero(mmi_jk, M_j * m_k) - divideOrZero(mmi_ik + mmi_jk, (M_i + M_j) * m_k));
                        });
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
                using (var transaction = new ModelTransaction(classModel))
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

                    transaction.Commit();
                }
                
            }
            allClasses = true;
            nextMerge = prioritizedMerges.FirstOrDefault();
            while (nextMerge != null && nextMerge.Effect > 0)
            {
                using (var transaction = new ModelTransaction(classModel))
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

                    transaction.Commit();
                }
                
            }
            stopwatch.Stop();
            Console.WriteLine("Model optimization took {0}ms", stopwatch.ElapsedMilliseconds);

            return stopwatch.ElapsedMilliseconds;
        }

        private static long BenchmarkModelWithEngineTransaction(string modelPath)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var repository = new ModelRepository();
            var model = repository.Resolve(modelPath);
            var classModel = model.RootElements[0] as ClassModel;
            stopwatch.Stop();
            Console.WriteLine("Loading model took {0}ms", stopwatch.ElapsedMilliseconds);

            var dataDependencyCounts = new Dictionary<IAttribute, List<Method>>();
            var functionalDependencyCounts = new Dictionary<IMethod, List<Method>>();

            //changes from here
            stopwatch.Restart();
            var executionEngine = ExecutionEngine.Current;
            executionEngine.BeginTransaction();
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

            var M = new Func<IClass, int>(cl => cl.Encapsulates.OfType<Method>().Count());
            var A = new Func<IClass, int>(cl => cl.Encapsulates.OfType<IAttribute>().Count());

            var MAI = new Func<IClass, IClass, double>((cl_i, cl_j) =>
                cl_i.Encapsulates.OfType<Method>()
                    .Sum(m => m.DataDependency.Intersect(cl_j.Encapsulates).Count()));

            var MMI = new Func<IClass, IClass, double>((cl_i, cl_j) =>
                cl_i.Encapsulates.OfType<Method>()
                    .Sum(m => m.FunctionalDependency.Intersect(cl_j.Encapsulates).Count()));

            var Effect = new Func<IClass, IClass, double>((cl_i, cl_j) =>
            {
                var M_i = M(cl_i);
                var M_j = M(cl_j);
                var A_i = A(cl_i);
                var A_j = A(cl_j);
                var MAI_i = MAI(cl_i, cl_i);
                var MAI_j = MAI(cl_j, cl_j);
                var MAI_ij = MAI(cl_i, cl_j);
                var MAI_ji = MAI(cl_j, cl_i);
                var MMI_i = MMI(cl_i, cl_i);
                var MMI_j = MMI(cl_j, cl_j);
                var MMI_ij = MMI(cl_i, cl_j);
                var MMI_ji = MMI(cl_j, cl_i);
                var deltaCohesionDataDep = // Delta of Cohesion based on data dependencies
                        divideOrZero(MAI_i + MAI_ij + MAI_ji + MAI_j, (M_i + M_j) * (A_i + A_j)) - divideOrZero(MAI_i, M_i * A_i) - divideOrZero(MAI_j, M_j * A_j);
                var deltaCohesionFunctionalDep = // Delta of Cohesion based on functional dependencies
                        divideOrZero(MMI_i + MMI_ij + MMI_ji + MMI_j, combinationCount(M_i + M_j)) - divideOrZero(MMI_i, combinationCount(M_i)) - divideOrZero(MMI_j, combinationCount(M_j));
                var deltaCouplingij = // Delta of Coupling between C_i and C_j
                        divideOrZero(MAI_ij, M_i * A_j) + divideOrZero(MAI_ji, M_j * A_i) + divideOrZero(MMI_ij, M_i * (M_j - 1)) + divideOrZero(MMI_ji, M_j * (M_i - 1));
                var classIAttributes = cl_i.Encapsulates.OfType<IAttribute>();
                var classJAttributes = cl_j.Encapsulates.OfType<IAttribute>();
                var classIMethods = cl_i.Encapsulates.OfType<Method>();
                var classJMethods = cl_j.Encapsulates.OfType<Method>();
                var dataDependingClasses = (from att in classIAttributes.Concat(classJAttributes)
                                            from meth in dataDependencyCounts[att]
                                            select meth.IsEncapsulatedBy).Distinct();
                var functionalDependingClasses = (from m2 in classIMethods.Concat(classJMethods)
                                                    from meth in functionalDependencyCounts[m2]
                                                    select meth.IsEncapsulatedBy).Distinct();
                var deltaCouplingFromOthers = // Coupling from other classes
                        dataDependingClasses.Sum(ck =>
                        {
                            if (ck == cl_i || ck == cl_j) return 0;
                            var mai_ki = MAI(ck, cl_i);
                            var mai_kj = MAI(ck, cl_j);
                            var m_k = M(ck);
                            return (divideOrZero(mai_ki, m_k * A_i) + divideOrZero(mai_kj, A_j * m_k) - divideOrZero(mai_ki + mai_kj, (A_i + A_j) * m_k));
                        })
                        +
                        functionalDependingClasses.Sum(ck =>
                        {
                            if (ck == cl_i || ck == cl_j) return 0;
                            var mmi_ki = MMI(ck, cl_i);
                            var mmi_kj = MMI(ck, cl_j);
                            var m_k = M(ck);
                            return (divideOrZero(mmi_ki, (M_i - 1) * m_k) + divideOrZero(mmi_kj, (M_j - 1) * m_k) - divideOrZero(mmi_ki + mmi_kj, (M_i + M_j - 1) * m_k));
                        });
                var dataDependentClasses = (from meth in classIMethods.Concat(classJMethods)
                                            from dataDep in meth.DataDependency
                                            select dataDep.IsEncapsulatedBy).Distinct();
                var FunctionalDependentClasses = (from meth in classIMethods.Concat(classJMethods)
                                                    from funDep in meth.FunctionalDependency
                                                    select funDep.IsEncapsulatedBy).Distinct();
                var deltaCouplingToOthers = dataDependentClasses.Sum(ck =>
                {
                    var mai_ik = MAI(cl_i, ck);
                    var mai_jk = MAI(cl_j, ck);
                    var a_k = A(ck);
                    return (divideOrZero(mai_ik, M_i * a_k) + divideOrZero(mai_jk, M_j * a_k) - divideOrZero(mai_ik + mai_jk, (M_i + M_j) * a_k));
                })
                        +
                        FunctionalDependentClasses.Sum(ck =>
                        {
                            if (ck == cl_i || ck == cl_j) return 0;
                            var mmi_ik = MMI(cl_i, ck);
                            var mmi_jk = MMI(cl_j, ck);
                            var m_k = M(ck) - 1;
                            return (divideOrZero(mmi_ik, M_i * m_k) + divideOrZero(mmi_jk, M_j * m_k) - divideOrZero(mmi_ik + mmi_jk, (M_i + M_j) * m_k));
                        });
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
            executionEngine.CommitTransaction();
            
            
            stopwatch.Stop();
            Console.WriteLine("Model optimization took {0}ms", stopwatch.ElapsedMilliseconds);

            /*using (var writer = File.AppendText("results.csv"))
            {
                writer.WriteLine("Transaction;{0};{1}", stopwatch.ElapsedMilliseconds, modelPath);

            }*/
            return stopwatch.ElapsedMilliseconds;
        }

        private static long BenchmarkModelWithShortEngineTransactions(string modelPath)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var repository = new ModelRepository();
            var model = repository.Resolve(modelPath);
            var classModel = model.RootElements[0] as ClassModel;
            stopwatch.Stop();
            Console.WriteLine("Loading model took {0}ms", stopwatch.ElapsedMilliseconds);

            var dataDependencyCounts = new Dictionary<IAttribute, List<Method>>();
            var functionalDependencyCounts = new Dictionary<IMethod, List<Method>>();

            //changes from here
            stopwatch.Restart();
            var executionEngine = ExecutionEngine.Current;
            //executionEngine.BeginTransaction();
            foreach (var feature in classModel.Features.OfType<Method>())
            {
                executionEngine.BeginTransaction();
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
                executionEngine.CommitTransaction();
            }
            foreach (var attribute in classModel.Features.OfType<IAttribute>())
            {
                executionEngine.BeginTransaction();
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
                executionEngine.CommitTransaction();
            }

            var combinationCount = new Func<int, int>(i => i * (i - 1));
            var divideOrZero = new Func<double, double, double>((a, b) => b == 0 ? 0 : a / b);

            var M = new Func<IClass, int>(cl => cl.Encapsulates.OfType<Method>().Count());
            var A = new Func<IClass, int>(cl => cl.Encapsulates.OfType<IAttribute>().Count());

            var MAI = new Func<IClass, IClass, double>((cl_i, cl_j) =>
                cl_i.Encapsulates.OfType<Method>()
                    .Sum(m => m.DataDependency.Intersect(cl_j.Encapsulates).Count()));

            var MMI = new Func<IClass, IClass, double>((cl_i, cl_j) =>
                cl_i.Encapsulates.OfType<Method>()
                    .Sum(m => m.FunctionalDependency.Intersect(cl_j.Encapsulates).Count()));

            var Effect = new Func<IClass, IClass, double>((cl_i, cl_j) =>
            {
                var M_i = M(cl_i);
                var M_j = M(cl_j);
                var A_i = A(cl_i);
                var A_j = A(cl_j);
                var MAI_i = MAI(cl_i, cl_i);
                var MAI_j = MAI(cl_j, cl_j);
                var MAI_ij = MAI(cl_i, cl_j);
                var MAI_ji = MAI(cl_j, cl_i);
                var MMI_i = MMI(cl_i, cl_i);
                var MMI_j = MMI(cl_j, cl_j);
                var MMI_ij = MMI(cl_i, cl_j);
                var MMI_ji = MMI(cl_j, cl_i);
                var deltaCohesionDataDep = // Delta of Cohesion based on data dependencies
                        divideOrZero(MAI_i + MAI_ij + MAI_ji + MAI_j, (M_i + M_j) * (A_i + A_j)) - divideOrZero(MAI_i, M_i * A_i) - divideOrZero(MAI_j, M_j * A_j);
                var deltaCohesionFunctionalDep = // Delta of Cohesion based on functional dependencies
                        divideOrZero(MMI_i + MMI_ij + MMI_ji + MMI_j, combinationCount(M_i + M_j)) - divideOrZero(MMI_i, combinationCount(M_i)) - divideOrZero(MMI_j, combinationCount(M_j));
                var deltaCouplingij = // Delta of Coupling between C_i and C_j
                        divideOrZero(MAI_ij, M_i * A_j) + divideOrZero(MAI_ji, M_j * A_i) + divideOrZero(MMI_ij, M_i * (M_j - 1)) + divideOrZero(MMI_ji, M_j * (M_i - 1));
                var classIAttributes = cl_i.Encapsulates.OfType<IAttribute>();
                var classJAttributes = cl_j.Encapsulates.OfType<IAttribute>();
                var classIMethods = cl_i.Encapsulates.OfType<Method>();
                var classJMethods = cl_j.Encapsulates.OfType<Method>();
                var dataDependingClasses = (from att in classIAttributes.Concat(classJAttributes)
                                            from meth in dataDependencyCounts[att]
                                            select meth.IsEncapsulatedBy).Distinct();
                var functionalDependingClasses = (from m2 in classIMethods.Concat(classJMethods)
                                                  from meth in functionalDependencyCounts[m2]
                                                  select meth.IsEncapsulatedBy).Distinct();
                var deltaCouplingFromOthers = // Coupling from other classes
                        dataDependingClasses.Sum(ck =>
                        {
                            if (ck == cl_i || ck == cl_j) return 0;
                            var mai_ki = MAI(ck, cl_i);
                            var mai_kj = MAI(ck, cl_j);
                            var m_k = M(ck);
                            return (divideOrZero(mai_ki, m_k * A_i) + divideOrZero(mai_kj, A_j * m_k) - divideOrZero(mai_ki + mai_kj, (A_i + A_j) * m_k));
                        })
                        +
                        functionalDependingClasses.Sum(ck =>
                        {
                            if (ck == cl_i || ck == cl_j) return 0;
                            var mmi_ki = MMI(ck, cl_i);
                            var mmi_kj = MMI(ck, cl_j);
                            var m_k = M(ck);
                            return (divideOrZero(mmi_ki, (M_i - 1) * m_k) + divideOrZero(mmi_kj, (M_j - 1) * m_k) - divideOrZero(mmi_ki + mmi_kj, (M_i + M_j - 1) * m_k));
                        });
                var dataDependentClasses = (from meth in classIMethods.Concat(classJMethods)
                                            from dataDep in meth.DataDependency
                                            select dataDep.IsEncapsulatedBy).Distinct();
                var FunctionalDependentClasses = (from meth in classIMethods.Concat(classJMethods)
                                                  from funDep in meth.FunctionalDependency
                                                  select funDep.IsEncapsulatedBy).Distinct();
                var deltaCouplingToOthers = dataDependentClasses.Sum(ck =>
                {
                    var mai_ik = MAI(cl_i, ck);
                    var mai_jk = MAI(cl_j, ck);
                    var a_k = A(ck);
                    return (divideOrZero(mai_ik, M_i * a_k) + divideOrZero(mai_jk, M_j * a_k) - divideOrZero(mai_ik + mai_jk, (M_i + M_j) * a_k));
                })
                        +
                        FunctionalDependentClasses.Sum(ck =>
                        {
                            if (ck == cl_i || ck == cl_j) return 0;
                            var mmi_ik = MMI(cl_i, ck);
                            var mmi_jk = MMI(cl_j, ck);
                            var m_k = M(ck) - 1;
                            return (divideOrZero(mmi_ik, M_i * m_k) + divideOrZero(mmi_jk, M_j * m_k) - divideOrZero(mmi_ik + mmi_jk, (M_i + M_j) * m_k));
                        });
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
                executionEngine.BeginTransaction();
                Console.WriteLine("Now merging {0} and {1}, should have effect {2}", nextMerge.Cl_i.Name, nextMerge.Cl_j.Name, nextMerge.Effect);
                // We need to save the features from these classes as they will be dropped as soon as we delete the encapsulating classes
                var newFeatures = nextMerge.Cl_i.Encapsulates.Concat(nextMerge.Cl_j.Encapsulates).ToList();
                classModel.Classes.Remove(nextMerge.Cl_i);
                classModel.Classes.Remove(nextMerge.Cl_j);
                var newClass = new Class() { Name = "C" + (classCounter++).ToString() };
                newClass.Encapsulates.AddRange(newFeatures);
                classModel.Classes.Add(newClass);
                nextMerge = prioritizedMerges.FirstOrDefault();
                executionEngine.CommitTransaction();
            }
            allClasses = true;
            nextMerge = prioritizedMerges.FirstOrDefault();
            while (nextMerge != null && nextMerge.Effect > 0)
            {
                executionEngine.BeginTransaction();
                Console.WriteLine("Now merging {0} and {1}, should have effect {2}", nextMerge.Cl_i.Name, nextMerge.Cl_j.Name, nextMerge.Effect);
                // We need to save the features from these classes as they will be dropped as soon as we delete the encapsulating classes
                var newFeatures = nextMerge.Cl_i.Encapsulates.Concat(nextMerge.Cl_j.Encapsulates).ToList();
                classModel.Classes.Remove(nextMerge.Cl_i);
                classModel.Classes.Remove(nextMerge.Cl_j);
                var newClass = new Class() { Name = "C" + (classCounter++).ToString() };
                newClass.Encapsulates.AddRange(newFeatures);
                classModel.Classes.Add(newClass);
                nextMerge = prioritizedMerges.FirstOrDefault();
                executionEngine.CommitTransaction();
            }
            //executionEngine.CommitTransaction();


            stopwatch.Stop();
            Console.WriteLine("Model optimization took {0}ms", stopwatch.ElapsedMilliseconds);

            return stopwatch.ElapsedMilliseconds;
        }

        private static long BenchmarkModelWithoutTransactions(string modelPath)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var repository = new ModelRepository();
            var model = repository.Resolve(modelPath);
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

            var M = new Func<IClass, int>(cl => cl.Encapsulates.OfType<Method>().Count());
            var A = new Func<IClass, int>(cl => cl.Encapsulates.OfType<IAttribute>().Count());

            var MAI = new Func<IClass, IClass, double>((cl_i, cl_j) =>
                cl_i.Encapsulates.OfType<Method>()
                    .Sum(m => m.DataDependency.Intersect(cl_j.Encapsulates).Count()));

            var MMI = new Func<IClass, IClass, double>((cl_i, cl_j) =>
                cl_i.Encapsulates.OfType<Method>()
                    .Sum(m => m.FunctionalDependency.Intersect(cl_j.Encapsulates).Count()));

            var Effect = new Func<IClass, IClass, double>((cl_i, cl_j) =>
            {
                var M_i = M(cl_i);
                var M_j = M(cl_j);
                var A_i = A(cl_i);
                var A_j = A(cl_j);
                var MAI_i = MAI(cl_i, cl_i);
                var MAI_j = MAI(cl_j, cl_j);
                var MAI_ij = MAI(cl_i, cl_j);
                var MAI_ji = MAI(cl_j, cl_i);
                var MMI_i = MMI(cl_i, cl_i);
                var MMI_j = MMI(cl_j, cl_j);
                var MMI_ij = MMI(cl_i, cl_j);
                var MMI_ji = MMI(cl_j, cl_i);
                var deltaCohesionDataDep = // Delta of Cohesion based on data dependencies
                        divideOrZero(MAI_i + MAI_ij + MAI_ji + MAI_j, (M_i + M_j) * (A_i + A_j)) - divideOrZero(MAI_i, M_i * A_i) - divideOrZero(MAI_j, M_j * A_j);
                var deltaCohesionFunctionalDep = // Delta of Cohesion based on functional dependencies
                        divideOrZero(MMI_i + MMI_ij + MMI_ji + MMI_j, combinationCount(M_i + M_j)) - divideOrZero(MMI_i, combinationCount(M_i)) - divideOrZero(MMI_j, combinationCount(M_j));
                var deltaCouplingij = // Delta of Coupling between C_i and C_j
                        divideOrZero(MAI_ij, M_i * A_j) + divideOrZero(MAI_ji, M_j * A_i) + divideOrZero(MMI_ij, M_i * (M_j - 1)) + divideOrZero(MMI_ji, M_j * (M_i - 1));
                var classIAttributes = cl_i.Encapsulates.OfType<IAttribute>();
                var classJAttributes = cl_j.Encapsulates.OfType<IAttribute>();
                var classIMethods = cl_i.Encapsulates.OfType<Method>();
                var classJMethods = cl_j.Encapsulates.OfType<Method>();
                var dataDependingClasses = (from att in classIAttributes.Concat(classJAttributes)
                                            from meth in dataDependencyCounts[att]
                                            select meth.IsEncapsulatedBy).Distinct();
                var functionalDependingClasses = (from m2 in classIMethods.Concat(classJMethods)
                                                  from meth in functionalDependencyCounts[m2]
                                                  select meth.IsEncapsulatedBy).Distinct();
                var deltaCouplingFromOthers = // Coupling from other classes
                        dataDependingClasses.Sum(ck =>
                        {
                            if (ck == cl_i || ck == cl_j) return 0;
                            var mai_ki = MAI(ck, cl_i);
                            var mai_kj = MAI(ck, cl_j);
                            var m_k = M(ck);
                            return (divideOrZero(mai_ki, m_k * A_i) + divideOrZero(mai_kj, A_j * m_k) - divideOrZero(mai_ki + mai_kj, (A_i + A_j) * m_k));
                        })
                        +
                        functionalDependingClasses.Sum(ck =>
                        {
                            if (ck == cl_i || ck == cl_j) return 0;
                            var mmi_ki = MMI(ck, cl_i);
                            var mmi_kj = MMI(ck, cl_j);
                            var m_k = M(ck);
                            return (divideOrZero(mmi_ki, (M_i - 1) * m_k) + divideOrZero(mmi_kj, (M_j - 1) * m_k) - divideOrZero(mmi_ki + mmi_kj, (M_i + M_j - 1) * m_k));
                        });
                var dataDependentClasses = (from meth in classIMethods.Concat(classJMethods)
                                            from dataDep in meth.DataDependency
                                            select dataDep.IsEncapsulatedBy).Distinct();
                var FunctionalDependentClasses = (from meth in classIMethods.Concat(classJMethods)
                                                  from funDep in meth.FunctionalDependency
                                                  select funDep.IsEncapsulatedBy).Distinct();
                var deltaCouplingToOthers = dataDependentClasses.Sum(ck =>
                {
                    var mai_ik = MAI(cl_i, ck);
                    var mai_jk = MAI(cl_j, ck);
                    var a_k = A(ck);
                    return (divideOrZero(mai_ik, M_i * a_k) + divideOrZero(mai_jk, M_j * a_k) - divideOrZero(mai_ik + mai_jk, (M_i + M_j) * a_k));
                })
                        +
                        FunctionalDependentClasses.Sum(ck =>
                        {
                            if (ck == cl_i || ck == cl_j) return 0;
                            var mmi_ik = MMI(cl_i, ck);
                            var mmi_jk = MMI(cl_j, ck);
                            var m_k = M(ck) - 1;
                            return (divideOrZero(mmi_ik, M_i * m_k) + divideOrZero(mmi_jk, M_j * m_k) - divideOrZero(mmi_ik + mmi_jk, (M_i + M_j) * m_k));
                        });
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
            return stopwatch.ElapsedMilliseconds;
            /*using (var writer = File.AppendText("results.csv"))
            {
                writer.WriteLine("Master;{0};{1}", stopwatch.ElapsedMilliseconds, modelPath);
            }*/

            //Console.WriteLine("Output contains {0} classes", classModel.Classes.Count);
            /*stopwatch.Restart();
            classModel.Name = "Optimized Class Model";
            repository.Save(classModel, Path.ChangeExtension(modelPath, ".Output.xmi"));
            stopwatch.Stop();

            Console.WriteLine("Serializing result model took {0}ms", stopwatch.ElapsedMilliseconds);*/
        }
        static void Main(string[] args)
        {
            var modelIndices = new List<string>() {"A", "B", "C", "D", "E" };
            foreach (var index in modelIndices)
            {
                var modelName = $"InputModels\\TTC_InputRDG_{index}.xmi";
                Console.WriteLine($"Benchmarking model {modelName}.");
                var repetitions = 10;
                var resultsMaster = new long[repetitions];
                var resultsEngine = new long[repetitions];
                var resultsTransaction = new long[repetitions];

                var resultsShortEngine = new long[repetitions];
                var resultsShortTransaction = new long[repetitions];

                //for initializing purposes
                var repository = new ModelRepository();
                var model = repository.Resolve(modelName);
                var classModel = model.RootElements[0] as ClassModel;

                for (var i = 0; i < repetitions; i++)
                {
                    resultsMaster[i] = BenchmarkModelWithoutTransactions(modelName);
                    resultsEngine[i] = BenchmarkModelWithEngineTransaction(modelName);
                    resultsTransaction[i] = BenchmarkModelWithTransactions(modelName);

                    resultsShortEngine[i] = BenchmarkModelWithShortEngineTransactions(modelName);
                    resultsShortTransaction[i] = BenchmarkModelWithShortTransactions(modelName);
                }

                using (var writer = File.AppendText("results.csv"))
                {
                    writer.WriteLine("Master;{0};{1}", resultsMaster.Average(), modelName);
                    writer.WriteLine("ExecutionEngine;{0};{1}", resultsEngine.Average(), modelName);
                    writer.WriteLine("ModelTransaction;{0};{1}", resultsTransaction.Average(), modelName);
                    writer.WriteLine("ShortExecutionEngine;{0};{1}", resultsShortEngine.Average(), modelName);
                    writer.WriteLine("ShortModelTransaction;{0};{1}", resultsShortTransaction.Average(), modelName);
                }
            }
        }
    }
}
