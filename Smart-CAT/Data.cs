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
    using System.IO;

    /// <summary>
    /// A data.
    /// </summary>
    public static class Data
    {
        #region Fields

        /// <summary>
        /// This variable allows access to functions of the Exceptions class.
        /// </summary>
        internal static Exceptions aExceptions = new Exceptions();

        /// <summary>
        /// Stores all data and observables found at the game logs file.
        /// </summary>
        internal static Tuple<string[], string[][]> AllGameLogs = Tuple.Create<string[], string[][]>(new string[] { }, new string[][] { });

        /// <summary>
        /// Stores information whether the data for the given facets and competencies is labelled to
        /// decide ML approach.
        /// </summary>
        internal static Tuple<bool[][], bool[][][]> CheckLabels = Tuple.Create<bool[][], bool[][][]>(new bool[][] { }, new bool[][][] { });

        /// <summary>
        /// Stores all the elements declared at the Competency Model.
        /// </summary>
        internal static Tuple<string[], string[][]> CompetencyModel = Tuple.Create<string[], string[][]>(new String[] { }, new string[][] { });

        internal static String[] UniCompetencyModel = new string[] { };

        /// <summary>
        /// The correlations (Competencies, Facets).
        /// </summary>
        internal static Tuple<double[][], double[][][]> Correlations = Tuple.Create<double[][], double[][][]>(new double[][] { }, new double[][][] { });

        /// <summary>
        /// The instance.
        /// </summary>
        internal static Tuple<double[][][][], double[][][][]> Inst = new Tuple<double[][][][], double[][][][]>(new double[][][][] { }, new double[][][][] { });

        /// <summary>
        /// Stores instances per statistical submodel.
        /// </summary>
        internal static double[][][][] Instances = new double[][][][] { };

        /// <summary>
        /// Stores the labelling data for the given facets and competencies.
        /// </summary>
        internal static Tuple<int[][], int[][][]> LabelledData = Tuple.Create<int[][], int[][][]>(new int[][] { }, new int[][][] { });

        /// <summary>
        /// The labeled output competencies.
        /// </summary>
        internal static Tuple<int[][], int[][][], int[][]> LabelledOutputC = Tuple.Create<int[][], int[][][], int[][]>(new int[][] { }, new int[][][] { }, new int[][] { });

        /// <summary>
        /// The labeled output facets.
        /// </summary>
        internal static Tuple<int[][], int[][][], int[][][]> LabelledOutputF = Tuple.Create<int[][], int[][][], int[][][]>(new int[][] { }, new int[][][] { }, new int[][][] { });

        /// <summary>
        /// Options for controlling the miles.
        /// </summary>
        internal static IMLOptions MLOptions = new NaiveBayesAlgorithm();

        /// <summary>
        /// The output labels.
        /// </summary>
        internal static Tuple<int[][], int[][][]> OutputLabels = Tuple.Create<int[][], int[][][]>(new int[][] { }, new int[][][] { });

        /// <summary>
        /// The performance.
        /// </summary>
        internal static IPerformance Performance;

        /// <summary>
        /// Stores the pruned observables and the correlation analysis results.
        /// </summary>
        internal static Tuple<string[][][][], double[][][][][]> PrunedResults = Tuple.Create<string[][][][], double[][][][][]>(new string[][][][] { }, new double[][][][][] { });

        /// <summary>
        /// Stores the external data for the given facets and competencies.
        /// </summary>
        internal static Tuple<int[][], int[][][]> RandomLabelledData = Tuple.Create<int[][], int[][][]>(new int[][] { }, new int[][][] { });

        /// <summary>
        /// Stores the Statistical Submodel .
        /// </summary>
        internal static string[][][] StatisticalSubmodel = new string[][][] { };

        internal static string[][] UniEvidenceModel = new string[][] { };

        /// <summary>
        /// Options for controlling the validation and verification.
        /// </summary>
        internal static VandVOptions VandVOptions = new VandVOptions(Double.NaN, Double.NaN);

        internal static Tuple<double[][][], double[][][]> InstUni = new Tuple<double[][][], double[][][]>(new double[][][] { }, new double[][][] { });

        internal static double[][][] InstancesUni = new double[][][] { };

        internal static bool[][] CheckLabelsUni = new bool[][] { };

        internal static int[][] LabelledDataUni = new int[][] { };

        internal static Tuple<int[][], int[][], int[][]> UniLabelledOutputC = Tuple.Create<int[][], int[][], int[][]>(new int[][] { }, new int[][] { }, new int[][] { });

        internal static Tuple<string[], double[][]> spearmansMulti = new Tuple<string[], double[][]>(new string[] { }, new double[][] { });

        internal static Tuple<string[], double[][]> spearmansUni = new Tuple<string[], double[][]>(new string[] { }, new double[][] { });

        internal static Tuple<string[], double[], string[][], double[][]> cronbachAlphaMulti = new Tuple<string[], double[], string[][], double[][]>(new string[] { }, new double[] { }, new string[][] { }, new double[][] { }) { };

        internal static Tuple<string[], double[]> cronbachAlphaUni = new Tuple<string[], double[]>(new string[] { }, new double[] { }) { };

        internal static double pSignificance = new double();

        internal static Tuple<string[], string[][]> ExternalData = Tuple.Create<string[], string[][]>(new string[] { }, new string[][] { });

        #endregion Fields

        #region Methods

        /// <summary>
        /// Saves the data as default.
        /// </summary>
        internal static void SaveDataAsDefault(String filename = null)
        {
            Logger.Info($"Saving ECD to: '{Utils.MakePathRelative(filename)}'.");

            //! 1) CompetencyModel.
            ECD.SaveCompetencyModel(CompetencyModel, filename);

            //! 2) Statistical Submodel.
            ECD.SaveEvidenceModel(Data.StatisticalSubmodel, filename);

            //! 3) Load Observables.
            //! NOTE: Observables might be empty (they are extracted from the data file).
            ECD.SaveObservables(Data.AllGameLogs.Item1, filename);

            //! 4) Uni CompetencyModel.
            ECD.SaveUniCompetencyModel(Data.UniCompetencyModel, filename);

            //! 5) Uni Statistical Submodel.
            ECD.SaveUniEvidenceModel(Data.UniEvidenceModel, filename);
        }

        /// <summary>
        /// Loads data as default.
        /// </summary>
        ///
        /// <param name="filename"> (Optional) Filename of the file. </param>
        internal static void LoadDataAsDefault(String filename = null)
        {
            if (File.Exists(filename))
            {
                Logger.Info($"Loading ECD from: '{Utils.MakePathRelative(filename)}'.");

                //! 1) CompetencyModel.
                Data.CompetencyModel = ECD.LoadCompetencyModel(filename);

                //! 2) Statistical Submodel.
                Data.StatisticalSubmodel = ECD.LoadEvidenceModel(filename).Item2;

#warning Can't load & assign Observables directly, dimensions might mismatch.

                //! 3) ObservablesModel.
                Data.AllGameLogs = new Tuple<string[], string[][]>(ECD.LoadObservables(filename), Data.AllGameLogs.Item2);

                //! 4) Uni CompetencyModel.
                Data.UniCompetencyModel = ECD.LoadUniCompetencyModel(filename);

                //! 5) Uni Statistical Submodel.
                Data.UniEvidenceModel = ECD.LoadUniEvidenceModel(filename);
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
            //ConsoleDialog.HighVideo();
            //Debug.WriteLine($"[{Extensions.GetCurrentMethod()}]");
            //ConsoleDialog.NormVideo();

            //foreach (String o in Data.AllGameLogs.Item1)
            //{
            //    Debug.WriteLine($"{o}");
            //}

            //Debug.WriteLine(String.Empty);
        }

        #endregion Methods
    }
}