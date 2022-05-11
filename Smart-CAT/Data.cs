/*
* Copyright 2020 Open University of the Netherlands (OUNL)
*
* Authors: Konstantinos Georgiadis, Wim van der Vegt.
* Organization: Open University of the Netherlands (OUNL).
* Project: The RAGE project
* Project URL: http://rageproject.eu.
* Task: T2.1 of the RAGE project; Development of assets for assessment. 
* 
* For any questions please contact: 
*
* Konstantinos Georgiadis via konstantinos.georgiadis [AT] ou [DOT] nl
* and/or
* Wim van der Vegt via wim.vandervegt [AT] ou [DOT] nl
* 
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* This project has received funding from the European Union’s Horizon
* 2020 research and innovation programme under grant agreement No 644187.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/
namespace StealthAssessmentWizard
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// A data.
    /// </summary>
    public static class Data
    {
        #region Fields

        /// <summary>
        /// Stores all data and observables found at the game logs file.
        /// </summary>
        internal static Observables observables = new Observables();

        /// <summary>
        /// The unicompetencies.
        /// </summary>
        internal static UniCompetencies unicompetencies = new UniCompetencies();

        /// <summary>
        /// Stores all the elements declared at the Competency Model.
        /// </summary>
        internal static Competencies competencies = new Competencies();

        /// <summary>
        /// This variable allows access to functions of the Exceptions class.
        /// </summary>
        //internal static Exceptions aExceptions = new Exceptions();

        /// <summary>
        /// Stores information whether the data for the given facets and competencies is labeled to
        /// decide ML approach.
        /// 
        /// Item1 [Competencies] -> Label Presence
        /// Item2 [Competences][Facets] -> Label Presence
        /// 
        /// Note: The Label Presence Arrays have a length of 1. (See line 1995 in BayesNet.cs).
        /// Note: Could be 1 dimension flatter.
        /// </summary>
        internal static (bool[][] competencies, bool[][][] facets) CheckLabels = (competencies: Array.Empty<bool[]>(), facets: Array.Empty<bool[][]>());
        //x internal static Tuple<bool[][], bool[][][]> CheckLabels = Tuple.Create<bool[][], bool[][][]>(Array.Empty<bool[]>(), Array.Empty<bool[][]>());

#if NAMED_TUPLE_SYNTAX

#warning TEST OF C#7 NAMED TUPLE SYNTAX (Note: it does not mix with Tuple<T,U>

        //! Old Syntax:        
        //x internal static Tuple<bool[][], bool[][][]> CheckLabels = Tuple.Create<bool[][], bool[][][]>(Array.Empty<bool[]>(), Array.Empty<bool[][]>());

        //! New C# 7 Syntax:
        static (bool[][] competencies, bool[][][] facets) CheckLabels = (competencies: Array.Empty<bool[]>(), facets: Array.Empty<bool[][]>());

        //! Usage:
        static readonly bool c15present = CheckLabelsTEST.competencies[15][0];
#endif

        /// <summary>
        /// The instance.
        /// </summary>
        internal static (double[][][][] facets, double[][][][] observables) Inst = (facets: Array.Empty<double[][][]>(), observables: Array.Empty<double[][][]>());
        //x internal static Tuple<double[][][][], double[][][][]> Inst_ = new Tuple<double[][][][], double[][][][]>(Array.Empty<double[][][]>(), Array.Empty<double[][][]>());

        /// <summary>
        /// Stores instances per statistical submodel.
        /// </summary>
        internal static double[][][][] Instances = Array.Empty<double[][][]>();

        /// <summary>
        /// Stores the labelling data for the given facets and competencies.
        /// </summary>
        internal static (int[][] competencies, int[][][] facets) LabelledData = (competencies: Array.Empty<int[]>(), facets: Array.Empty<int[][]>());
        //x internal static Tuple<int[][], int[][][]> LabelledData = Tuple.Create<int[][], int[][][]>(Array.Empty<int[]>(), Array.Empty<int[][]>());

        /// <summary>
        /// The labeled output competencies.
        /// </summary>
        internal static (int[][] competencies, int[][][] facets, int[][] output) LabelledOutputC = (competencies: Array.Empty<int[]>(), facets: Array.Empty<int[][]>(), output: Array.Empty<int[]>());
        //x internal static Tuple<int[][], int[][][], int[][]> LabelledOutputC = Tuple.Create<int[][], int[][][], int[][]>(Array.Empty<int[]>(), Array.Empty<int[][]>(), Array.Empty<int[]>());

        /// <summary>
        /// The labeled output facets.
        /// </summary>
        internal static (int[][] competencies, int[][][] facets, int[][][] output) LabelledOutputF = (competencies: Array.Empty<int[]>(), facets: Array.Empty<int[][]>(), output: Array.Empty<int[][]>());
        //x internal static Tuple<int[][], int[][][], int[][][]> LabelledOutputF = Tuple.Create<int[][], int[][][], int[][][]>(Array.Empty<int[]>(), Array.Empty<int[][]>(), Array.Empty<int[][]>());

        /// <summary>
        /// Options for controlling the miles.
        /// </summary>
        internal static IMLOptions MLOptions = new NaiveBayesAlgorithm();

        /// <summary>
        /// The output labels.
        /// </summary>
        internal static (int[][] competencies, int[][][] facets) OutputLabels = (competencies: Array.Empty<int[]>(), facets: Array.Empty<int[][]>());
        //x internal static Tuple<int[][], int[][][]> OutputLabels = Tuple.Create<int[][], int[][][]>(Array.Empty<int[]>(), Array.Empty<int[][]>());

        /// <summary>
        /// The performance.
        /// </summary>
        internal static IPerformance Performance;

        internal static Tuple<double[][][], double[][][]> InstUni = new Tuple<double[][][], double[][][]>(Array.Empty<double[][]>(), Array.Empty<double[][]>());

        internal static double[][][] InstancesUni = Array.Empty<double[][]>();

        internal static bool[][] CheckLabelsUni = Array.Empty<bool[]>();

        internal static int[][] LabelledDataUni = Array.Empty<int[]>();

        internal static Tuple<int[][], int[][], int[][]> UniLabelledOutputC = Tuple.Create<int[][], int[][], int[][]>(Array.Empty<int[]>(), Array.Empty<int[]>(), Array.Empty<int[]>());

        internal static Tuple<string[], double[][]> spearmansMulti = new Tuple<string[], double[][]>(Array.Empty<string>(), Array.Empty<double[]>());

        internal static Tuple<string[], double[][]> spearmansUni = new Tuple<string[], double[][]>(Array.Empty<string>(), Array.Empty<double[]>());

        internal static Tuple<string[], double[], string[][], double[][]> cronbachAlphaMulti = new Tuple<string[], double[], string[][], double[][]>(Array.Empty<string>(), Array.Empty<double>(), Array.Empty<string[]>(), Array.Empty<double[]>()) { };

        internal static Tuple<string[], double[]> cronbachAlphaUni = new Tuple<string[], double[]>(Array.Empty<string>(), Array.Empty<double>()) { };

        //x internal static double pSignificance = new double();

        internal static Tuple<string[], string[][]> ExternalData = Tuple.Create<string[], string[][]>(Array.Empty<string>(), Array.Empty<string[]>());

        #endregion Fields

        #region Methods

        /// <summary>
        /// Saves the data as default.
        /// </summary>
        internal static void SaveECD(String filename = null)
        {
            Logger.Info($"Saving Statistical Model to: '{Utils.MakePathRelative(filename)}'.");

            if (!String.IsNullOrEmpty(filename))
            {
                using (Models models = new Models()
                {
                    observables = Data.observables.Names,
                    competencies = Data.competencies,
                    unicompetencies = Data.unicompetencies
                })
                {
                    //! 1) Save ECD.
                    ECD.SaveModelData(models, filename);
                }
            }
        }

        /// <summary>
        /// Saves a model to excel.
        /// </summary>
        ///
        /// <param name="filename"> Filename of the file. </param>
        internal static void SaveModelToExcel(String filename)
        {
            Excel.AddModel(filename);
        }

        /// <summary>
        /// Loads data as default.
        /// </summary>
        ///
        /// <param name="filename"> (Optional) Filename of the file. </param>
        internal static void LoadECD(String filename = null)
        {
            if (File.Exists(filename))
            {
                Logger.Info($"Loading Statistical Model from: '{Utils.MakePathRelative(filename)}'.");

                //! 1) Load ECD.
                using (Models models = ECD.LoadModelData(filename))
                {
                    Data.competencies = models.competencies;
                    Data.unicompetencies = models.unicompetencies;

                    //! 2) Prune Loaded Observables.

                    //! a) Extend Item2 with empty data to the size of obs fails.
                    //! b) However truncating obs to the size of Item2 seems to work OK as
                    //!    this data is loaded later where the sheet matches the size of obs.

                    //! 2) Truncate only (remove surplus of observables)...
                    if (Data.observables.Count != 0)
                    {
                        foreach (String observable in models.observables)
                        {
                            int index = Data.observables.ToList().FindIndex(p => p.ObservableName.Equals(observable));
                            if (index == -1)
                            {
                                Logger.Info($"Removing observable: '{observable}' as no data is found.");
#warning TODO - SUSPICIOUS CODE - TRYING TO REMOVE at -1.
                                //Data.observables.RemoveAt(index);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Dumps the statistical sub model.
        /// </summary>
        internal static void DumpStatisticalSubModels()
        {
            //ConsoleDialog.HighVideo();
            //Debug.WriteLine($"[{Extensions.GetCurrentMethod()}]");
            //ConsoleDialog.NormVideo();

            //for (Int32 i = 0; i < CompetencyModel.Item1.Length; i++)
            //{
            //    Debug.WriteLine($"{CompetencyModel.Item1[i]}");

            //    for (Int32 j = 0; j < CompetencyModel.Item2[i].Length; j++)
            //    {
            //        Debug.WriteLine($"\t{CompetencyModel.Item2[i][j]}");

            //        foreach (String o in StatisticalSubmodel[i][j])
            //        {
            //            Debug.WriteLine($"\t\t{o}");
            //        }
            //    }
            //}

            //for (Int32 i = 0; i < UniCompetencyModel.Length; i++)
            //{
            //    Debug.WriteLine($"{UniCompetencyModel[i]}");

            //    foreach (String o in UniEvidenceModel[i])
            //    {
            //        Debug.WriteLine($"\t{o}");
            //    }
            //}

            //Debug.WriteLine(String.Empty);
        }

        /// <summary>
        /// Dumps the Observables.
        /// </summary>
        internal static void DumpObservables()
        {
            //Debug.WriteLine($"[{Extensions.GetCurrentMethod()}]");

            //foreach (String observable in Data.Observables.Names)
            //{
            //    Debug.WriteLine($"{observable}");
            //}

            //Debug.WriteLine(String.Empty);
        }

        #endregion Methods
    }
}