/*
* Copyright 2022 Open University of the Netherlands (OUNL)
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

    using Accord.IO;
    using Accord.MachineLearning;
    using Accord.MachineLearning.Bayes;
    using Accord.MachineLearning.DecisionTrees.Learning;
    using Accord.MachineLearning.DecisionTrees.Rules;
    using Accord.Math.Distances;
    using Accord.Statistics.Analysis;
    using Accord.Statistics.Distributions.DensityKernels;
    using Accord.Statistics.Distributions.Fitting;
    using Accord.Statistics.Distributions.Multivariate;
    using Accord.Statistics.Distributions.Univariate;

    using ArffTools;

    using RDotNet;

    using Swiss;

#if false

    using Debug = System.Diagnostics.Debug;
    using Environment = System.Environment;
    using java.io;
    using weka.core;

#endif

    public static partial class BayesNet
    {
        #region Methods

        /// <summary>
        /// Correlation analysis between outputs and external measurement data on multi-competencies.
        /// level.
        /// </summary>
        ///
        /// <param name="CompetencyModel"> The competency model. </param>
        ///
        /// <returns>
        /// A (double[][],double[][][])
        /// </returns>
        public static (string[], double[][]) CorrelationAnalysisMulti(
            string[] MultiCompetencies,
            int[][] Output,
            (string[], string[][]) ExternalData)
        {
            (string[], double[][]) spearmans = (new string[MultiCompetencies.Length], new double[MultiCompetencies.Length][]);

            // Check if same legends exist for the multicompetencies in external data.
            for (int i = 0; i < MultiCompetencies.Length; i++)
            {
                for (int x = 0; x < ExternalData.Item1.Length; x++)
                {
                    if (string.Equals(MultiCompetencies[i], ExternalData.Item1[x]) == true)
                    {
                        // Initialize arrays.
#warning check if size of arrays is same.

                        int[] classifications = new int[Output[i].Length];

                        for (int y = 0; y < Output[i].Length; y++)
                        {
                            classifications[y] = Output[i][y];
                        }

                        int[] external = new int[ExternalData.Item2[x].Length];

                        for (int k = 0; k < ExternalData.Item2[x].Length; k++)
                        {
                            external[k] = Convert.ToInt32(ExternalData.Item2[x][k]);
                        }

                        //Perform Validation.
                        IntegerVector classi = Utils.engine.CreateIntegerVector(classifications);
                        Utils.engine.SetSymbol("classi", classi);

                        IntegerVector ext = Utils.engine.CreateIntegerVector(external);
                        Utils.engine.SetSymbol("ext", ext);

                        //! RRSE
                        NumericVector correl = Utils.engine.Evaluate("cor(classi, ext, method='spearman')").AsNumeric();

                        spearmans.Item1[i] = MultiCompetencies[i];
                        spearmans.Item2[i] = new double[1];
                        spearmans.Item2[i][0] = Math.Round(correl[0], 4);
                    }
#warning deal with non-found validation data.
                }
            }

            return spearmans;
        }

        /// <summary>
        /// Correlation analysis between outputs and external measurement data on multi-competencies.
        /// level.
        /// </summary>
        ///
        /// <param name="CompetencyModel"> The competency model. </param>
        ///
        /// <returns>
        /// A (double[][],double[][][])
        /// </returns>
        public static (string[], double[][]) CorrelationAnalysisUni(
            string[] UniCompetencies,
            int[][] Output,
            (string[], string[][]) ExternalData)
        {
            //int[] classifications = Array.Empty<int>();
            //int[] external = Array.Empty<int>();

            (string[], double[][]) spearmans = (new string[UniCompetencies.Length], new double[UniCompetencies.Length][]);

            //Check if same legends exist for the multi-competecies in external data.
            for (int i = 0; i < UniCompetencies.Length; i++)
            {
                for (int x = 0; x < ExternalData.Item1.Length; x++)
                {
                    if (string.Equals(UniCompetencies[i], ExternalData.Item1[x]) == true)
                    {
                        //Initialize arrays.
#warning check if size of arrays is same.

                        int[] classifications = new int[Output[i].Length];
                        for (int y = 0; y < Output[i].Length; y++)
                        {
                            classifications[y] = Output[i][y];
                        }

                        int[] external = new int[ExternalData.Item2[x].Length];
                        for (int k = 0; k < ExternalData.Item2[x].Length; k++)
                        {
                            external[k] = Convert.ToInt32(ExternalData.Item2[x][k]);
                        }

                        //Perform Validation.
                        IntegerVector classi = Utils.engine.CreateIntegerVector(classifications);
                        Utils.engine.SetSymbol("classi", classi);

                        IntegerVector ext = Utils.engine.CreateIntegerVector(external);
                        Utils.engine.SetSymbol("ext", ext);

                        //! RRSE
                        NumericVector correl = Utils.engine.Evaluate("cor(classi, ext, method='spearman')").AsNumeric();

                        spearmans.Item1[i] = UniCompetencies[i];
                        spearmans.Item2[i] = new double[1];
                        spearmans.Item2[i][0] = Math.Round(correl[0], 4);
                    }
#warning deal with non-found validation data.
                }
            }

            return spearmans;
        }

        /// <summary>
        /// Runs Decision Trees C45 from the Accord library.
        /// </summary>
        ///
        /// <param name="reader">     The reader. </param>
        /// <param name="Competency"> The competency. </param>
        ///
        /// <returns>
        /// An int[].
        /// </returns>
        public static int[] DecisionTreesAccord_C(
            String MyData,
            ArffReader reader,
            String Competency)
        {
            int[] predicted = Array.Empty<int>();

            ArffHeader header = reader.ReadHeader();
            Object[][] randData = reader.ReadAllInstances();

            //! set the sizes of the training and testing datasets according to the 66% split rule.
            int trainSize = (int)Math.Round((double)randData.Length * (Data.MLOptions as DecisionTreesAlgorithm).PercentSplit / 100);
            int testSize = randData.Length - trainSize;

            //! Initialize the training and testing datasets.
            int[] checks = new int[3];

            Int32 cnt = header.Attributes.Count;

            //! Add instances in training and testing data
            try
            {
                //! Add instances in training and testing data
                checks[0] = randData.Count(p => p[cnt - 1].Equals(0));
                checks[1] = randData.Count(p => p[cnt - 1].Equals(1));
                checks[2] = randData.Count(p => p[cnt - 1].Equals(2));

                if (checks[0] * checks[1] * checks[2] == 0)
                {
                    Logger.Error("Missing Values in Training and Testing Data $.");

                    return predicted;
                }

                //! Initialize arrays.
                double[][] GameLogs = new double[trainSize][];
                int[] LabelledData = new int[trainSize];

                double[][] GameLogsTest = new double[testSize][];
                int[] expected = new int[testSize];

                for (int i = 0; i < trainSize; i++)
                {
                    GameLogs[i] = new double[cnt - 1];
                }

                for (int i = 0; i < testSize; i++)
                {
                    GameLogsTest[i] = new double[cnt - 1];
                }

                //! Load data on arrays.
                for (int i = 0; i < trainSize; i++)
                {
                    for (int k = 0; k < cnt; k++)
                    {
                        if (k != cnt - 1)
                        {
                            GameLogs[i][k] = Convert.ToDouble(randData[i + 0][k]);
                        }
                        else
                        {
                            LabelledData[i] = Convert.ToInt32(randData[i + 0][k]);
                        }

                    }
                }

                for (int i = 0; i < testSize; i++)
                {
                    for (int k = 0; k < cnt; k++)
                    {
                        if (k != cnt - 1)
                        {
                            GameLogsTest[i][k] = Convert.ToDouble(randData[i + trainSize][k]);
                        }
                        else
                        {
                            expected[i] = Convert.ToInt32(randData[i + trainSize][k]);
                        }
                    }
                }

                //! And we can use the C4.5 for learning:
                C45Learning teacher = new C45Learning();

                //! Finally induce the tree from the data:
                var tree = teacher.Learn(GameLogs, LabelledData);

                //! To get the estimated class labels, we can use
                predicted = tree.Decide(GameLogsTest);

                //! Moreover, we may decide to convert our tree to a set of rules:
                DecisionSet rules = tree.ToRules();

                using (IniFile ini = new IniFile(Path.Combine(MyData, Competency + "_DecisionTreesAccord.ini")))
                {
                    ini.Clear();

                    //! Save BayesNet results in .txt file
                    using (StreamWriter file = new StreamWriter(Path.Combine(MyData, Competency + "_DecisionTreesAccord.txt")))
                    {
                        //! We can get different performance measures to assess how good our model was at
                        //! predicting the true, expected, ground-truth labels for the decision problem:
                        var cm = new GeneralConfusionMatrix(classes: 3, expected: expected, predicted: predicted);

                        //! We can obtain the proper confusion matrix using:
                        int[,] matrix = cm.Matrix;

                        //! And multiple performance measures:
                        double accuracy = cm.Accuracy;
                        double error = cm.Error;
                        double kappa = cm.Kappa;

                        file.WriteLine("{0}", Math.Round(accuracy, 4));
                        file.WriteLine("{0}", Math.Round(error, 4));
                        file.WriteLine("{0}", Math.Round(kappa, 4));

                        ini.WriteDouble("Performance", "Accuracy", Math.Round(accuracy, 4));
                        ini.WriteDouble("Performance", "Error", Math.Round(error, 4));
                        ini.WriteDouble("Performance", "Kappa", Math.Round(kappa, 4));

                        double MAE = 0;
                        double RMSE = 0;
                        double RAE = 0;
                        double RRSE = 0;
                        (double, double, double, double) Performace = (MAE, RMSE, RAE, RRSE);
                        Performace = PerformaceStatistics(expected, predicted);

                        file.WriteLine("{0}", Math.Round(Performace.Item1, 4));
                        file.WriteLine("{0}", Math.Round(Performace.Item2, 4));
                        file.WriteLine("{0}%", Math.Round(Performace.Item3, 4));
                        file.WriteLine("{0}%", Math.Round(Performace.Item4, 4));
                        file.WriteLine();

                        ini.WriteDouble("Performance", "MAE", Math.Round(Performace.Item1, 4));
                        ini.WriteDouble("Performance", "RMSE", Math.Round(Performace.Item2, 4));
                        ini.WriteDouble("Performance", "RAE(%)", Math.Round(Performace.Item3, 4), "%");
                        ini.WriteDouble("Performance", "RRSE(%)", Math.Round(Performace.Item4, 4), "%");

                        Data.Performance = new DecisionTreesPerformanceModel(Competency, String.Empty, accuracy, error, kappa, MAE, RMSE, RAE, RRSE);

                        for (Int32 i = 0; i < 3; i++)
                        {
                            ini.WriteInteger("Summary", $"Classified as {i}", predicted.Count(p => p == i));
                        }

                        for (int x = 0; x < predicted.Length; x++)
                        {
                            String section = $"Instance_{x + 1}";

                            ini.WriteList(section, "Tests", GameLogsTest[x].ToList(), ',');

                            file.Write("Instance {0}:", x + 1);
                            for (int y = 0; y < GameLogsTest[x].Length; y++)
                            {
                                file.Write("{0} ", GameLogsTest[x][y]);
                            }

                            ini.WriteDouble(section, "Classified as", predicted[x]);

                            file.WriteLine("Classified as:{0} ", predicted[x]);
                        }

                        file.WriteLine();
                        file.WriteLine("Rules: {0}", rules.ToString());

                        ini.WriteString("Rules", "Rules", rules.ToString());

                        Logger.Info("Decision Trees results from Accord saved successfully.");
                    }

                    ini.UpdateFile();
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (ArgumentException)
            {
                Logger.Error("Attribute is constant. Changing seed.");
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
#pragma warning restore CA1031 // Do not catch general exception types

            return predicted;
        }

        /// <summary>
        /// Runs Decision Trees C45 from the Accord library.
        /// </summary>
        ///
        /// <param name="MyData">     Information describing my. </param>
        /// <param name="reader">     The reader. </param>
        /// <param name="Competency"> The competency. </param>
        /// <param name="Facet">      The facet. </param>
        ///
        /// <returns>
        /// An int[].
        /// </returns>
        public static int[] DecisionTreesAccord_F(
            String MyData,
            ArffReader reader,
            String Competency,
            String Facet)
        {
            ArffHeader header = reader.ReadHeader();
            Object[][] randData = reader.ReadAllInstances();

            //! Set the sizes of the training and testing datasets according to the 66% split rule.
            int trainSize = (int)Math.Round((double)randData.Length * (Data.MLOptions as DecisionTreesAlgorithm).PercentSplit / 100);
            int testSize = randData.Length - trainSize;

            //! Initialize the training and testing datasets.
            int[] checks = new int[3];

            Int32 cnt = header.Attributes.Count;

            //! Add instances in training and testing data
            checks[0] = randData.Count(p => p[cnt - 1].Equals(0));
            checks[1] = randData.Count(p => p[cnt - 1].Equals(1));
            checks[2] = randData.Count(p => p[cnt - 1].Equals(2));

            if (checks[0] * checks[1] * checks[2] == 0)
            {
                Logger.Error("Missing Values in Training and Testing Data.");
            }

            //! Initialize arrays.
            double[][] GameLogs = new double[trainSize][];
            int[] LabelledData = new int[trainSize];

            double[][] GameLogsTest = new double[testSize][];
            int[] expected = new int[testSize];

            for (int i = 0; i < trainSize; i++)
            {
                GameLogs[i] = new double[cnt - 1];
            }

            for (int i = 0; i < testSize; i++)
            {
                GameLogsTest[i] = new double[cnt - 1];
            }

            //! Load data on arrays.
            for (int i = 0; i < trainSize; i++)
            {
                for (int k = 0; k < cnt; k++)
                {
                    if (k != cnt - 1)
                    {
                        GameLogs[i][k] = Convert.ToDouble(randData[i + 0][k]);
                    }
                    else
                    {
                        LabelledData[i] = Convert.ToInt32(randData[i + 0][k]);
                    }

                }
            }

            for (int i = 0; i < testSize; i++)
            {
                for (int k = 0; k < cnt; k++)
                {
                    if (k != cnt - 1)
                    {
                        GameLogsTest[i][k] = Convert.ToDouble(randData[i + trainSize][k]);
                    }
                    else
                    {
                        expected[i] = Convert.ToInt32(randData[i + trainSize][k]);
                    }
                }
            }

            //! And we can use the C4.5 for learning:
            C45Learning teacher = new C45Learning();

            //! Finally induce the tree from the data:
            var tree = teacher.Learn(GameLogs, LabelledData);

            //! To get the estimated class labels, we can use
            int[] predicted = tree.Decide(GameLogsTest);

            //! Moreover, we may decide to convert our tree to a set of rules:
            DecisionSet rules = tree.ToRules();

            using (IniFile ini = new IniFile(Path.Combine(MyData, Competency, Facet + "_DecisionTreesAccord.ini")))
            {
                ini.Clear();

                //! Save BayesNet results in .txt file
                using (StreamWriter file = new StreamWriter(Path.Combine(MyData, Competency, Facet + "_DecisionTreesAccord.txt")))
                {
                    //! We can get different performance measures to assess how good our model was at
                    //! predicting the true, expected, ground-truth labels for the decision problem:
                    var cm = new GeneralConfusionMatrix(classes: 3, expected: expected, predicted: predicted);

                    //! We can obtain the proper confusion matrix using:
                    int[,] matrix = cm.Matrix;

                    //! And multiple performance measures:
                    double accuracy = cm.Accuracy;
                    double error = cm.Error;
                    double kappa = cm.Kappa;

                    file.WriteLine("{0}", Math.Round(accuracy, 4));
                    file.WriteLine("{0}", Math.Round(error, 4));
                    file.WriteLine("{0}", Math.Round(kappa, 4));

                    ini.WriteDouble("Performance", "Accuracy", Math.Round(accuracy, 4));
                    ini.WriteDouble("Performance", "Error", Math.Round(error, 4));
                    ini.WriteDouble("Performance", "Kappa", Math.Round(kappa, 4));

                    double MAE = 0;
                    double RMSE = 0;
                    double RAE = 0;
                    double RRSE = 0;
                    (double, double, double, double) Performace = (MAE, RMSE, RAE, RRSE);
                    Performace = PerformaceStatistics(expected, predicted);

                    file.WriteLine("{0}", Math.Round(Performace.Item1, 4));
                    file.WriteLine("{0}", Math.Round(Performace.Item2, 4));
                    file.WriteLine("{0}%", Math.Round(Performace.Item3, 4));
                    file.WriteLine("{0}%", Math.Round(Performace.Item4, 4));
                    file.WriteLine();

                    ini.WriteDouble("Performance", "MAE", Math.Round(Performace.Item1, 4));
                    ini.WriteDouble("Performance", "RMSE", Math.Round(Performace.Item2, 4));
                    ini.WriteDouble("Performance", "RAE(%)", Math.Round(Performace.Item3, 4), "%");
                    ini.WriteDouble("Performance", "RRSE(%)", Math.Round(Performace.Item4, 4), "%");

                    Data.Performance = new DecisionTreesPerformanceModel(Competency, Facet, accuracy, error, kappa, MAE, RMSE, RAE, RRSE);

                    for (Int32 i = 0; i < 3; i++)
                    {
                        ini.WriteInteger("Summary", $"Classified as {i}", predicted.Count(p => p == i));
                    }

                    for (int x = 0; x < predicted.Length; x++)
                    {
                        String section = $"Instance_{x + 1}";

                        ini.WriteList(section, "Tests", GameLogsTest[x].ToList(), ',');

                        file.Write("Instance {0}:", x + 1);
                        for (int y = 0; y < GameLogsTest[x].Length; y++)
                        {
                            file.Write("{0} ", GameLogsTest[x][y]);
                        }

                        ini.WriteDouble(section, "Classified as", predicted[x]);

                        file.WriteLine("Classified as:{0} ", predicted[x]);
                    }

                    file.WriteLine();
                    file.WriteLine("Rules: {0}", rules.ToString());

                    ini.WriteString("Rules", "Rules", rules.ToString());

                    System.Console.WriteLine("Decision Trees results from Accord saved successfully.");
                }

                ini.UpdateFile();
            }

            return predicted;
        }

        /// <summary>
        /// Runs K-Means clustering from the Accord library.
        /// </summary>
        ///
        /// <param name="MyData">     Information describing my. </param>
        /// <param name="reader">     The reader. </param>
        /// <param name="Competency"> The competency. </param>
        /// <param name="Facet">      The facet. </param>
        ///
        /// <returns>
        /// An int[].
        /// </returns>
        public static int[] KMeans(
            String MyData,
            ArffReader reader,
            String Competency,
            String Facet)
        {
            Accord.Math.Random.Generator.Seed = 1;

            ArffHeader header = reader.ReadHeader();
            object[][] randData = reader.ReadAllInstances();

            //! Initialize arrays.
            double[][] GameLogs = new double[randData.Length][];

            for (int i = 0; i < randData.Length; i++)
            {
                GameLogs[i] = new double[header.Attributes.Count];
            }

            //! Load data on arrays.
            for (int i = 0; i < randData.Length; i++)
            {
                for (int k = 0; k < header.Attributes.Count; k++)
                {
                    GameLogs[i][k] = Convert.ToDouble(randData[i][k]);
                }
            }

            //! Create a new K-Means algorithm
            Double tolerance = 0.05;
            if (Data.MLOptions != null)
            {
                switch (Data.MLOptions.Algorithm)
                {
                    case MLAlgorithms.NaiveBayes:
                        tolerance = (Data.MLOptions as NaiveBayesAlgorithm).Tolerance;
                        break;
                    case MLAlgorithms.DecisionTrees:
                        tolerance = (Data.MLOptions as DecisionTreesAlgorithm).Tolerance;
                        break;
                }
            }

            KMeans kmeans = new KMeans(k: 3)
            {
                Distance = new SquareEuclidean(),

                //! We will compute the K-Means algorithm until cluster centroids
                //! change less than 0.5 between two iterations of the algorithm
                Tolerance = tolerance
            };

            //! Compute and retrieve the data centroids
            var clusters = kmeans.Learn(GameLogs);

            //! Ordering the clusters
            int count = 0;
            double[] sumVar = new double[clusters.NumberOfClasses];

            foreach (var cluster in clusters)
            {
                count++;
                double[] tempVar = cluster.Centroid;
                double sum = 0;

                for (int x = 0; x < tempVar.Length; x++)
                {
                    sum += tempVar[x];
                }

                sumVar[count - 1] = sum / tempVar.Length;
            }

            double Max = sumVar.Max();
            double Min = sumVar.Min();
            string[] clOrder = new string[clusters.NumberOfClasses];

            for (int v = 0; v < sumVar.Length; v++)
            {
                if (sumVar[v] == Max)
                {
                    clOrder[v] = "High";
                }
                else if (sumVar[v] == Min)
                {
                    clOrder[v] = "Low";
                }
                else
                {
                    clOrder[v] = "Medium";
                }
            }

            //! Use the centroids to partition all the data
            int[] labels = clusters.Decide(GameLogs);
            int[] newlabels = new int[randData.Length];

            //! Save BayesNet results in .txt file
            using (StreamWriter file = new StreamWriter(Path.Combine(MyData, Competency, Facet + "_KMeansAccord.txt")))
            {
                int noCl = 0;
                foreach (var value in clusters)
                {
                    file.WriteLine("Label {0} is: {1}", noCl, clOrder[noCl]);
                    noCl++;
                    double[] tempVar = value.Centroid;
                    for (int x = 0; x < tempVar.Length; x++)
                    {
                        file.WriteLine("Centroid of label {0} for variable {1}: {2}", noCl - 1, x + 1, tempVar[x].ToString());
                    }
                }

                file.WriteLine();

                for (int x = 0; x < GameLogs.Length; x++)
                {
                    file.Write("Instance {0}: ", x);
                    for (int y = 0; y < GameLogs[x].Length; y++)
                    {
                        file.Write("{0} ", GameLogs[x][y]);
                    }
                    file.WriteLine("Old Label: {0} ", labels[x]);

                    //! Assigning new labels
                    if (labels[x] == 0)
                    {
                        if (clOrder[0] == "Low")
                        {
                            newlabels[x] = 0;
                        }
                        else if (clOrder[0] == "Medium")
                        {
                            newlabels[x] = 1;
                        }
                        else if (clOrder[0] == "High")
                        {
                            newlabels[x] = 2;
                        }
                    }
                    else if (labels[x] == 1)
                    {
                        if (clOrder[1] == "Low")
                        {
                            newlabels[x] = 0;
                        }
                        else if (clOrder[1] == "Medium")
                        {
                            newlabels[x] = 1;
                        }
                        else if (clOrder[1] == "High")
                        {
                            newlabels[x] = 2;
                        }
                    }
                    else if (labels[x] == 2)
                    {
                        if (clOrder[2] == "Low")
                        {
                            newlabels[x] = 0;
                        }
                        else if (clOrder[2] == "Medium")
                        {
                            newlabels[x] = 1;
                        }
                        else if (clOrder[2] == "High")
                        {
                            newlabels[x] = 2;
                        }
                    }
                    file.WriteLine("New Label: {0} ", newlabels[x]);

                }

                Logger.Info("K-Means results from Accord saved successfully.");
            }

            return newlabels;
        }

        /// <summary>
        /// Naive Bayes accord for competencies.
        /// </summary>
        ///
        /// <param name="reader">     The reader. </param>
        /// <param name="Competency"> The competency. </param>
        ///
        /// <returns>
        /// An int[].
        /// </returns>
        public static int[] NaiveBayesAccord_C(
            String MyData,
            ArffReader reader,
            String Competency)
        {
            var cv = new NaiveBayesLearning<NormalDistribution>();
            cv.Options.InnerOption = new NormalOptions { Regularization = 1e-5 }; // to avoid zero variances

            int[] predicted = Array.Empty<int>();

            ArffHeader header = reader.ReadHeader();
            Object[][] randData = reader.ReadAllInstances();

            //! Set the sizes of the training and testing datasets according to the 66% split rule.
            int trainSize = (int)Math.Round((double)randData.Length * (Data.MLOptions as NaiveBayesAlgorithm).PercentSplit / 100);
            int testSize = randData.Length - trainSize;

            //! Initialize the training and testing datasets.
            int[] checks = new int[3];

            Int32 cnt = header.Attributes.Count;

            try
            {
                checks[0] = randData.Count(p => p[cnt - 1].Equals(0));
                checks[1] = randData.Count(p => p[cnt - 1].Equals(1));
                checks[2] = randData.Count(p => p[cnt - 1].Equals(2));

                if (checks[0] * checks[1] * checks[2] == 0)
                {
                    Logger.Error("Missing Values in Training and Testing Data.");

                    return predicted;
                }

                //! Initialize arrays.
                double[][] GameLogs = new double[trainSize][];
                double[][] GameLogsTest = new double[testSize][];
                int[] expected = new int[testSize];

                for (int i = 0; i < trainSize; i++)
                {
                    GameLogs[i] = new double[cnt - 1];
                }

                for (int i = 0; i < testSize; i++)
                {
                    GameLogsTest[i] = new double[cnt - 1];
                }

                int[] LabelledData = new int[trainSize];

                //! Load data on arrays.
                for (int i = 0; i < trainSize; i++)
                {
                    for (int k = 0; k < cnt; k++)
                    {
                        if (k != cnt - 1)
                        {
                            GameLogs[i][k] = Convert.ToDouble(randData[i + 0][k]);
                        }
                        else
                        {
                            LabelledData[i] = Convert.ToInt32(randData[i + 0][k]);
                        }

                    }
                }

                for (int i = 0; i < testSize; i++)
                {
                    for (int k = 0; k < cnt; k++)
                    {
                        if (k != cnt - 1)
                        {
                            GameLogsTest[i][k] = Convert.ToDouble(randData[i + trainSize][k]);
                        }
                        else
                        {
                            expected[i] = Convert.ToInt32(randData[i + trainSize][k]);
                        }
                    }
                }

                //! performance of Naive Bayes on the above data set:
                NaiveBayes<NormalDistribution> nb = cv.Learn(GameLogs, LabelledData);

                nb.Save(Path.Combine(MyData, "NATIVE.TXT"), SerializerCompression.None);

                predicted = nb.Decide(GameLogsTest);
                double[][] probs = nb.Probabilities(GameLogsTest);

                using (IniFile ini = new IniFile(Path.Combine(MyData, Competency + "_BayesAccord.ini")))
                {
                    ini.Clear();

                    //! Save BayesNet results in .txt file
                    using (StreamWriter file = new StreamWriter(Path.Combine(MyData, Competency + "_BayesAccord.txt")))
                    {
                        //! We can get different performance measures to assess how good our model was at
                        //! predicting the true, expected, ground-truth labels for the decision problem:
                        var cm = new GeneralConfusionMatrix(classes: 3, expected: expected, predicted: predicted);

                        //! We can obtain the proper confusion matrix using:
                        int[,] matrix = cm.Matrix;

                        //! And multiple performance measures:
                        double accuracy = cm.Accuracy;
                        double error = cm.Error;
                        double kappa = cm.Kappa;

                        file.WriteLine("{0}", Math.Round(accuracy, 4));
                        file.WriteLine("{0}", Math.Round(error, 4));
                        file.WriteLine("{0}", Math.Round(kappa, 4));

                        ini.WriteDouble("Performance", "Accuracy", Math.Round(accuracy, 4));
                        ini.WriteDouble("Performance", "Error", Math.Round(error, 4));
                        ini.WriteDouble("Performance", "Kappa", Math.Round(kappa, 4));

                        double MAE = 0;
                        double RMSE = 0;
                        double RAE = 0;
                        double RRSE = 0;
                        (double, double, double, double) Performace = (MAE, RMSE, RAE, RRSE);
                        Performace = PerformaceStatistics(expected, predicted);

                        file.WriteLine("{0}", Math.Round(Performace.Item1, 4));
                        file.WriteLine("{0}", Math.Round(Performace.Item2, 4));
                        file.WriteLine("{0}%", Math.Round(Performace.Item3, 4));
                        file.WriteLine("{0}%", Math.Round(Performace.Item4, 4));
                        file.WriteLine();

                        ini.WriteDouble("Performance", "MAE", Math.Round(Performace.Item1, 4));
                        ini.WriteDouble("Performance", "RMSE", Math.Round(Performace.Item2, 4));
                        ini.WriteDouble("Performance", "RAE(%)", Math.Round(Performace.Item3, 4), "%");
                        ini.WriteDouble("Performance", "RRSE(%)", Math.Round(Performace.Item4, 4), "%");

                        Data.Performance = new NaiveBayesPerformanceModel(Competency, String.Empty, accuracy, error, kappa, MAE, RMSE, RAE, RRSE);

                        for (Int32 i = 0; i < 3; i++)
                        {
                            ini.WriteInteger("Summary", $"Classified as {i}", predicted.Count(p => p == i));
                        }

                        for (int x = 0; x < predicted.Length; x++)
                        {
                            String section = $"Instance_{x + 1}";

                            ini.WriteList(section, "Tests", GameLogsTest[x].ToList(), ',');

                            file.Write("Instance {0}:", x + 1);
                            for (int y = 0; y < GameLogsTest[x].Length; y++)
                            {
                                file.Write("{0} ", GameLogsTest[x][y]);
                            }

                            ini.WriteDouble(section, "Classified as", predicted[x]);

                            file.WriteLine("Classified as:{0} ", predicted[x]);

                            //for (int y = 0; y < probs[x].Length; y++)
                            //{
                            //    ini.WriteDouble(section, $"Prob for class {y}", Math.Round(probs[x][y], 4));
                            //    file.WriteLine("Prob for class {0}:{1} ", y, Math.Round(probs[x][y], 4));
                            //}
                        }

                        Logger.Info("Naive Bayes results from Accord saved successfully.");
                    }

                    ini.UpdateFile();
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (ArgumentException)
            {
                Logger.Warn("Attribute is constant. Changing seed.");
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
#pragma warning restore CA1031 // Do not catch general exception types

            return predicted;
        }

        /// <summary>
        /// Naive Bayes accord facets.
        /// </summary>
        ///
        /// <param name="MyData">     my project. </param>
        /// <param name="reader">     The reader. </param>
        /// <param name="Competency"> The competency. </param>
        /// <param name="Facet">      The facet. </param>
        ///
        /// <returns>
        /// An int[].
        /// </returns>
        public static int[] NaiveBayesAccord_F(
            String MyData,
            ArffReader reader,
            String Competency,
            String Facet)
        {
            var cv = new NaiveBayesLearning<NormalDistribution>();

            //! to avoid zero variances
            cv.Options.InnerOption = new NormalOptions
            {
                Regularization = 1e-5
            };

            ArffHeader header = reader.ReadHeader();
            Object[][] randData = reader.ReadAllInstances();

            //! Test Code of added ArffReader Methods.
            //
            //insts.Reset();
            //Int64 len = insts.Count();

            //! set the sizes of the training and testing datasets according to the PercentSplit% split rule.
            int trainSize = (int)Math.Round((double)randData.Length * (Data.MLOptions as NaiveBayesAlgorithm).PercentSplit / 100);
            int testSize = randData.Length - trainSize;

            //! Initialize the training and testing datasets.
            int[] checks = new int[3];

            Int32 cnt = header.Attributes.Count;

            //! Add instances in training and testing data
            checks[0] = randData.Count(p => p[cnt - 1].Equals(0));
            checks[1] = randData.Count(p => p[cnt - 1].Equals(1));
            checks[2] = randData.Count(p => p[cnt - 1].Equals(2));

            if (checks[0] * checks[1] * checks[2] == 0)
            {
                Logger.Error("Missing Values in Training and Testing Data $.");
            }

            //! Initialize arrays.
            double[][] GameLogs = new double[trainSize][];
            int[] LabelledData = new int[trainSize];

            double[][] GameLogsTest = new double[testSize][];
            int[] expected = new int[testSize];

            for (int i = 0; i < trainSize; i++)
            {
                GameLogs[i] = new double[cnt - 1];
            }

            for (int i = 0; i < testSize; i++)
            {
                GameLogsTest[i] = new double[cnt - 1];
            }

            //! Load data on arrays.
            //
            for (int i = 0; i < trainSize; i++)
            {
                for (int k = 0; k < cnt; k++)
                {
                    if (k != cnt - 1)
                    {
                        GameLogs[i][k] = Convert.ToDouble(randData[i + 0][k]);
                    }
                    else
                    {
                        LabelledData[i] = Convert.ToInt32(randData[i + 0][k]);
                    }
                }
            }

            for (int i = 0; i < testSize; i++)
            {
                for (int k = 0; k < cnt; k++)
                {
                    if (k != cnt - 1)
                    {
                        GameLogsTest[i][k] = Convert.ToDouble(randData[i + trainSize][k]);
                    }
                    else
                    {
                        expected[i] = Convert.ToInt32(randData[i + trainSize][k]);
                    }
                }
            }

            //===R
            //
            //! Performance of Naive Bayes on the above data set:

            NaiveBayes<NormalDistribution> nb = cv.Learn(GameLogs, LabelledData);

            nb.Save(Path.Combine(MyData, "NATIVE.TXT"), SerializerCompression.None);
            NaiveBayes<NormalDistribution> nb2 = Serializer.Load<NaiveBayes<NormalDistribution>>(Path.Combine(MyData, "NATIVE.TXT"));

            int[] predicted = nb2.Decide(GameLogsTest);
            double[][] probs = nb2.Probabilities(GameLogsTest);

            using (IniFile ini = new IniFile(Path.Combine(MyData, Competency, Facet + "_BayesAccord.ini")))
            {
                ini.Clear();

                //! Save BayesNet results in .txt file
                using (StreamWriter file = new StreamWriter(Path.Combine(MyData, Competency, Facet + "_BayesAccord.txt")))
                {
                    //! We can get different performance measures to assess how good our model was at
                    //! predicting the true, expected, ground-truth labels for the decision problem:
                    var cm = new GeneralConfusionMatrix(classes: 3, expected: expected, predicted: predicted);

                    //! We can obtain the proper confusion matrix using:
                    int[,] matrix = cm.Matrix;

                    //! And multiple performance measures:
                    double accuracy = cm.Accuracy;
                    double error = cm.Error;
                    double kappa = cm.Kappa;

                    file.WriteLine("{0}", Math.Round(accuracy, 4));
                    file.WriteLine("{0}", Math.Round(error, 4));
                    file.WriteLine("{0}", Math.Round(kappa, 4));

                    ini.WriteDouble("Performance", "Accuracy", Math.Round(accuracy, 4));
                    ini.WriteDouble("Performance", "Error", Math.Round(error, 4));
                    ini.WriteDouble("Performance", "Kappa", Math.Round(kappa, 4));

                    double MAE = 0;
                    double RMSE = 0;
                    double RAE = 0;
                    double RRSE = 0;
                    (double, double, double, double) Performace = (MAE, RMSE, RAE, RRSE);
                    Performace = PerformaceStatistics(expected, predicted);

                    file.WriteLine("{0}", Math.Round(Performace.Item1, 4));
                    file.WriteLine("{0}", Math.Round(Performace.Item2, 4));
                    file.WriteLine("{0}%", Math.Round(Performace.Item3, 4));
                    file.WriteLine("{0}%", Math.Round(Performace.Item4, 4));
                    file.WriteLine();

                    ini.WriteDouble("Performance", "MAE", Math.Round(Performace.Item1, 4));
                    ini.WriteDouble("Performance", "RMSE", Math.Round(Performace.Item2, 4));
                    ini.WriteDouble("Performance", "RAE(%)", Math.Round(Performace.Item3, 4), "%");
                    ini.WriteDouble("Performance", "RRSE(%)", Math.Round(Performace.Item4, 4), "%");

                    Data.Performance = new NaiveBayesPerformanceModel(Competency, Facet, accuracy, error, kappa, MAE, RMSE, RAE, RRSE);

                    for (Int32 i = 0; i < 3; i++)
                    {
                        ini.WriteInteger("Summary", $"Classified as {i}", predicted.Count(p => p == i));
                    }

                    for (int x = 0; x < predicted.Length; x++)
                    {
                        String section = $"Instance_{x + 1}";

                        ini.WriteList(section, "Tests", GameLogsTest[x].ToList(), ',');

                        file.Write("Instance {0}:", x + 1);
                        for (int y = 0; y < GameLogsTest[x].Length; y++)
                        {
                            file.Write("{0} ", GameLogsTest[x][y]);
                        }

                        ini.WriteDouble(section, "Classified as", predicted[x]);

                        file.WriteLine("Classified as:{0} ", predicted[x]);
                    }

                    Logger.Info("Naive Bayes results from Accord saved successfully.");
                }

                ini.UpdateFile();
            }

            return predicted;
        }

        /// <summary>
        /// Calculate the classifier's performance statistics using R.
        /// </summary>
        ///
        /// <param name="expected">  The expected. </param>
        /// <param name="predicted"> The predicted. </param>
        ///
        /// <returns>
        /// A (double,double,double,double)
        /// </returns>
        public static (double, double, double, double) PerformaceStatistics(
            int[] expected,
            int[] predicted)
        {
            //! .NET Framework array to R vector.
            //
            double[] Expected = new double[expected.Length];
            for (int x = 0; x < expected.Length; x++)
            {
                Expected[x] = Convert.ToDouble(expected[x]);
            }
            NumericVector exp = Utils.engine.CreateNumericVector(Expected);
            Utils.engine.SetSymbol("expected", exp);

            double[] Predicted = new double[predicted.Length];
            for (int x = 0; x < predicted.Length; x++)
            {
                Predicted[x] = Convert.ToDouble(predicted[x]);
            }

            NumericVector pre = Utils.engine.CreateNumericVector(Predicted);
            Utils.engine.SetSymbol("predicted", pre);

            //! MAE
            GenericVector mae = Utils.engine.Evaluate("mae(expected, predicted)").AsList();
            double MAE = mae.AsNumeric().First();

            //! RMSE
            GenericVector rmse = Utils.engine.Evaluate("rmse(expected, predicted)").AsList();
            double RMSE = rmse.AsNumeric().First();

            //! RAE
            GenericVector rae = Utils.engine.Evaluate("rae(expected, predicted)").AsList();
            double RAE = rae.AsNumeric().First() * 100;

            //! RRSE
            GenericVector rrse = Utils.engine.Evaluate("rrse(expected, predicted)").AsList();
            double RRSE = rrse.AsNumeric().First() * 100;

            return (MAE, RMSE, RAE, RRSE);
        }

        /// <summary>
        /// Reliability analysis multi.
        /// </summary>
        ///
        /// <param name="CompetencyModel">     The competency model. </param>
        /// <param name="StatisticalSubmodel"> The statistical submodel. </param>
        /// <param name="Insts">               The insts. </param>
        ///
        /// <returns>
        /// A (string[],double[],string[][],double[][])
        /// </returns>
        public static (string[], double[], string[][], double[][]) ReliabilityAnalysisMulti(
            (string[] competencies, string[][] facets) CompetencyModel,
            string[][][] StatisticalSubmodel,
            double[][][][] Insts)
        {
            double[] CronAlphaComp = new double[CompetencyModel.competencies.Length];
            double[][] CronAlphaFacet = new double[CompetencyModel.competencies.Length][];

            string[][] ObsPerComp = new string[CompetencyModel.competencies.Length][];
            double[][][] InstObsPerComp = new double[CompetencyModel.competencies.Length][][];
            int noCount = 0;

            //! Initialize ObsPerComp.
            for (int i = 0; i < CompetencyModel.competencies.Length; i++)
            {
                for (int x = 0; x < CompetencyModel.facets[i].Length; x++)
                {
                    for (int y = 0; y < StatisticalSubmodel[i][x].Length; y++)
                    {
                        noCount++;
                    }
                }
                ObsPerComp[i] = new string[noCount];
                InstObsPerComp[i] = new double[noCount][];
                noCount = 0;

                for (int x = 0; x < CompetencyModel.facets[i].Length; x++)
                {
                    for (int y = 0; y < StatisticalSubmodel[i][x].Length; y++)
                    {
                        ObsPerComp[i][noCount] = StatisticalSubmodel[i][x][y];
                        InstObsPerComp[i][noCount] = new double[Insts[i][x][y].Length];
                        for (int d = 0; d < Insts[i][x][y].Length; d++)
                        {
                            InstObsPerComp[i][noCount][d] = Insts[i][x][y][d];
                        }
                        noCount++;
                    }
                }
                noCount = 0;
            }

            //! Initialize data frame per competency.
            for (int i = 0; i < CompetencyModel.competencies.Length; i++)
            {
                //Initialize data frame per facet.
                //set column names of data frame per facet.
                string[] columnNamesFacets = new string[ObsPerComp[i].Length];
                CronAlphaFacet[i] = new double[CompetencyModel.facets[i].Length];
                IEnumerable<double>[] columnsFacet = new IEnumerable<double>[ObsPerComp[i].Length];
                //Transpose InstancesUni

                for (int x = 0; x < CompetencyModel.facets[i].Length; x++)
                {
                    //set column names and columns of data frame per observable.
                    string[] columnNamesObs = new string[StatisticalSubmodel[i][x].Length];

                    for (int y = 0; y < StatisticalSubmodel[i][x].Length; y++)
                    {
                        columnNamesObs[y] = StatisticalSubmodel[i][x][y];
                    }

                    IEnumerable<double>[] columnsObs = new IEnumerable<double>[StatisticalSubmodel[i][x].Length];
                    for (int y = 0; y < StatisticalSubmodel[i][x].Length; y++)
                    {
                        //set columns data per observable.
                        double[] tempData = new double[Insts[i][x][y].Length];

                        for (int c = 0; c < Insts[i][x][y].Length; c++)
                        {
                            tempData[c] = Insts[i][x][y][c];

                        }
                        columnsObs[y] = tempData;
                    }

                    //set data frame per facet.
                    DataFrame dfFacet = Utils.engine.CreateDataFrame(columnsObs, columnNamesObs);
                    Utils.engine.SetSymbol("dfFacet", dfFacet);

                    //Run reliability analysis per facet if more than one observables found.
                    if (StatisticalSubmodel[i][x].Length > 1)
                    {
                        GenericVector alphaFacet = Utils.engine.Evaluate("alpha(dfFacet, keys=NULL,cumulative=FALSE, title=NULL, max=10,na.rm = TRUE, check.keys = FALSE, n.iter = 1, delete = TRUE, use = 'pairwise', warnings = TRUE, n.obs = NULL)").AsList();

                        //get cronbach's a per facet
                        GenericVector RelRes = alphaFacet[0].AsList();
                        NumericVector nvRelRes = RelRes[0].AsNumeric();
                        CronAlphaFacet[i][x] = Math.Round(nvRelRes.First(), 4);
                    }
                    else
                    {
                        CronAlphaFacet[i][x] = Double.NaN;
                    }
                }

                for (int x = 0; x < ObsPerComp[i].Length; x++)
                {
                    columnNamesFacets[x] = ObsPerComp[i][x];

                    //set columns per facet
                    double[] TempDataFacet = new double[InstObsPerComp[i][x].Length];
                    for (int y = 0; y < InstObsPerComp[i][x].Length; y++)
                    {
                        TempDataFacet[y] = InstObsPerComp[i][x][y];
                    }
                    columnsFacet[x] = TempDataFacet;
                }

                DataFrame dfComp = Utils.engine.CreateDataFrame(columnsFacet, columnNamesFacets);
                Utils.engine.SetSymbol("dfComp", dfComp);

                //Run reliability analysis per competency if more than one facets found.
                if (CompetencyModel.competencies[i].Length > 1)
                {
                    GenericVector alphaComp = Utils.engine.Evaluate("alpha(dfComp, keys=NULL,cumulative=FALSE, title=NULL, max=10,na.rm = TRUE, check.keys = FALSE, n.iter = 1, delete = TRUE, use = 'pairwise', warnings = TRUE, n.obs = NULL)").AsList();

                    //get cronbach's a per facet
                    GenericVector RelRes = alphaComp[0].AsList();
                    NumericVector nvRelRes = RelRes[0].AsNumeric();
                    CronAlphaComp[i] = Math.Round(nvRelRes.First(), 4);
                }
                else
                {
                    CronAlphaComp[i] = Double.NaN;
                }
            }

            return (CompetencyModel.competencies, CronAlphaComp, CompetencyModel.facets, CronAlphaFacet);
        }

        public static (string[], double[]) ReliabilityAnalysisUni(
            string[] CompetencyModel,
            double[][][] InstUni,
            string[][] UniEvidenceModel)
        {
            double[] CronAlphaComp = new double[CompetencyModel.Length];

            //! Initialize data frame per competency.
            for (int i = 0; i < CompetencyModel.Length; i++)
            {
                //Initilize data frame per facet.
                //set column names of data frame per facet.
                string[] columnNamesFacets = new string[UniEvidenceModel[i].Length];
                IEnumerable<double>[] columnsFacet = new IEnumerable<double>[UniEvidenceModel[i].Length];

                for (int x = 0; x < UniEvidenceModel[i].Length; x++)
                {
                    columnNamesFacets[x] = UniEvidenceModel[i][x];

                    //set columns per facet
                    double[] TempDataFacet = new double[InstUni[i][x].Length];
                    for (int y = 0; y < InstUni[i][x].Length; y++)
                    {
                        TempDataFacet[y] = InstUni[i][x][y];
                    }
                    columnsFacet[x] = TempDataFacet;
                }

                DataFrame dfComp = Utils.engine.CreateDataFrame(columnsFacet, columnNamesFacets);
                Utils.engine.SetSymbol("dfComp", dfComp);

                //Run reliability analysis per competency if more than one facets found.

                GenericVector alphaComp = Utils.engine.Evaluate("alpha(dfComp, keys=NULL,cumulative=FALSE, title=NULL, max=10,na.rm = TRUE, check.keys = FALSE, n.iter = 1, delete = TRUE, use = 'pairwise', warnings = TRUE, n.obs = NULL)").AsList();

                //get cronbach's a per facet
                GenericVector RelRes = alphaComp[0].AsList();
                NumericVector nvRelRes = RelRes[0].AsNumeric();
                CronAlphaComp[i] = Math.Round(nvRelRes.First(), 4);

            }

            return (CompetencyModel, CronAlphaComp);
        }

        public static int[] UniDecisionTreesAccord_C(
            String MyData,
            ArffReader reader,
            String Competency)
        {
            ArffHeader header = reader.ReadHeader();
            Object[][] randData = reader.ReadAllInstances();

            //! Set the sizes of the training and testing datasets according to the 66% split rule.
            int trainSize = (int)Math.Round((double)randData.Length * (Data.MLOptions as DecisionTreesAlgorithm).PercentSplit / 100);
            int testSize = randData.Length - trainSize;

            //! Initialize the training and testing datasets.
            int[] checks = new int[3];

            Int32 cnt = header.Attributes.Count;

            //! Add instances in training and testing data
            checks[0] = randData.Count(p => p[cnt - 1].Equals(0));
            checks[1] = randData.Count(p => p[cnt - 1].Equals(1));
            checks[2] = randData.Count(p => p[cnt - 1].Equals(2));

            if (checks[0] * checks[1] * checks[2] == 0)
            {
                Logger.Error("Missing Values in Training and Testing Data.");
            }

            //! Initialize arrays.
            double[][] GameLogs = new double[trainSize][];
            int[] LabelledData = new int[trainSize];

            double[][] GameLogsTest = new double[testSize][];
            int[] expected = new int[testSize];

            for (int i = 0; i < trainSize; i++)
            {
                GameLogs[i] = new double[cnt - 1];
            }

            for (int i = 0; i < testSize; i++)
            {
                GameLogsTest[i] = new double[cnt - 1];
            }

            //! Load data on arrays.
            for (int i = 0; i < trainSize; i++)
            {
                for (int k = 0; k < cnt; k++)
                {
                    if (k != cnt - 1)
                    {
                        GameLogs[i][k] = Convert.ToDouble(randData[i + 0][k]);
                    }
                    else
                    {
                        LabelledData[i] = Convert.ToInt32(randData[i + 0][k]);
                    }

                }
            }

            for (int i = 0; i < testSize; i++)
            {
                for (int k = 0; k < cnt; k++)
                {
                    if (k != cnt - 1)
                    {
                        GameLogsTest[i][k] = Convert.ToDouble(randData[i + trainSize][k]);
                    }
                    else
                    {
                        expected[i] = Convert.ToInt32(randData[i + trainSize][k]);
                    }
                }
            }

            //! And we can use the C4.5 for learning:
            C45Learning teacher = new C45Learning();

            //! Finally induce the tree from the data:
            var tree = teacher.Learn(GameLogs, LabelledData);

            //! To get the estimated class labels, we can use
            int[] predicted = tree.Decide(GameLogsTest);

            //! Moreover, we may decide to convert our tree to a set of rules:
            DecisionSet rules = tree.ToRules();

            using (IniFile ini = new IniFile(Path.Combine(MyData, Competency + "_DecisionTreesAccord.ini")))
            {
                ini.Clear();

                //! Save BayesNet results in .txt file
                using (StreamWriter file = new StreamWriter(Path.Combine(MyData, Competency + "_DecisionTreesAccord.txt")))
                {
                    //! We can get different performance measures to assess how good our model was at
                    //! predicting the true, expected, ground-truth labels for the decision problem:
                    var cm = new GeneralConfusionMatrix(classes: 3, expected: expected, predicted: predicted);

                    //! We can obtain the proper confusion matrix using:
                    int[,] matrix = cm.Matrix;

                    //! And multiple performance measures:
                    double accuracy = cm.Accuracy;
                    double error = cm.Error;
                    double kappa = cm.Kappa;

                    file.WriteLine("{0}", Math.Round(accuracy, 4));
                    file.WriteLine("{0}", Math.Round(error, 4));
                    file.WriteLine("{0}", Math.Round(kappa, 4));

                    ini.WriteDouble("Performance", "Accuracy", Math.Round(accuracy, 4));
                    ini.WriteDouble("Performance", "Error", Math.Round(error, 4));
                    ini.WriteDouble("Performance", "Kappa", Math.Round(kappa, 4));

                    double MAE = 0;
                    double RMSE = 0;
                    double RAE = 0;
                    double RRSE = 0;
                    (double, double, double, double) Performace = (MAE, RMSE, RAE, RRSE);
                    Performace = PerformaceStatistics(expected, predicted);

                    file.WriteLine("{0}", Math.Round(Performace.Item1, 4));
                    file.WriteLine("{0}", Math.Round(Performace.Item2, 4));
                    file.WriteLine("{0}%", Math.Round(Performace.Item3, 4));
                    file.WriteLine("{0}%", Math.Round(Performace.Item4, 4));
                    file.WriteLine();

                    ini.WriteDouble("Performance", "MAE", Math.Round(Performace.Item1, 4));
                    ini.WriteDouble("Performance", "RMSE", Math.Round(Performace.Item2, 4));
                    ini.WriteDouble("Performance", "RAE(%)", Math.Round(Performace.Item3, 4), "%");
                    ini.WriteDouble("Performance", "RRSE(%)", Math.Round(Performace.Item4, 4), "%");

                    Data.Performance = new DecisionTreesPerformanceModel(Competency, "na", accuracy, error, kappa, MAE, RMSE, RAE, RRSE);

                    for (Int32 i = 0; i < 3; i++)
                    {
                        ini.WriteInteger("Summary", $"Classified as {i}", predicted.Count(p => p == i));
                    }

                    for (int x = 0; x < predicted.Length; x++)
                    {
                        String section = $"Instance_{x + 1}";

                        ini.WriteList(section, "Tests", GameLogsTest[x].ToList(), ',');

                        file.Write("Instance {0}:", x + 1);
                        for (int y = 0; y < GameLogsTest[x].Length; y++)
                        {
                            file.Write("{0} ", GameLogsTest[x][y]);
                        }

                        ini.WriteDouble(section, "Classified as", predicted[x]);

                        file.WriteLine("Classified as:{0} ", predicted[x]);
                    }

                    file.WriteLine();
                    file.WriteLine("Rules: {0}", rules.ToString());

                    ini.WriteString("Rules", "Rules", rules.ToString());

                    System.Console.WriteLine("Decision Trees results from Accord saved successfully.");
                }

                ini.UpdateFile();
            }

            return predicted;
        }

        /// <summary>
        /// Uni naive bayes accord c.
        /// </summary>
        ///
        /// <param name="MyData">  my project. </param>
        /// <param name="reader">     The reader. </param>
        /// <param name="Competency"> The competency. </param>
        ///
        /// <returns>
        /// An int[].
        /// </returns>
        public static int[] UniNaiveBayesAccord_C(
            String MyData,
            ArffReader reader,
            String Competency)
        {
            var cv = new NaiveBayesLearning<NormalDistribution>();

            //! to avoid zero variances
            cv.Options.InnerOption = new NormalOptions
            {
                Regularization = 1e-5
            };

            ArffHeader header = reader.ReadHeader();
            Object[][] randData = reader.ReadAllInstances();

            //! Test Code of added ArffReader Methods.
            //
            //insts.Reset();
            //Int64 len = insts.Count();

            //! set the sizes of the training and testing datasets according to the PercentSplit% split rule.
            int trainSize = (int)Math.Round((double)randData.Length * (Data.MLOptions as NaiveBayesAlgorithm).PercentSplit / 100);
            int testSize = randData.Length - trainSize;

            //! Initialize the training and testing datasets.
            int[] checks = new int[3];

            Int32 cnt = header.Attributes.Count;

            //! Add instances in training and testing data
            checks[0] = randData.Count(p => p[cnt - 1].Equals(0));
            checks[1] = randData.Count(p => p[cnt - 1].Equals(1));
            checks[2] = randData.Count(p => p[cnt - 1].Equals(2));

            if (checks[0] * checks[1] * checks[2] == 0)
            {
                Logger.Error("Missing Values in Training and Testing Data $.");
            }

            //! Initialize arrays.
            double[][] GameLogs = new double[trainSize][];
            int[] LabelledData = new int[trainSize];

            double[][] GameLogsTest = new double[testSize][];
            int[] expected = new int[testSize];

            for (int i = 0; i < trainSize; i++)
            {
                GameLogs[i] = new double[cnt - 1];
            }

            for (int i = 0; i < testSize; i++)
            {
                GameLogsTest[i] = new double[cnt - 1];
            }

            //! Load data on arrays.
            //
            for (int i = 0; i < trainSize; i++)
            {
                for (int k = 0; k < cnt; k++)
                {
                    if (k != cnt - 1)
                    {
                        GameLogs[i][k] = Convert.ToDouble(randData[i + 0][k]);
                    }
                    else
                    {
                        LabelledData[i] = Convert.ToInt32(randData[i + 0][k]);
                    }
                }
            }

            for (int i = 0; i < testSize; i++)
            {
                for (int k = 0; k < cnt; k++)
                {
                    if (k != cnt - 1)
                    {
                        GameLogsTest[i][k] = Convert.ToDouble(randData[i + trainSize][k]);
                    }
                    else
                    {
                        expected[i] = Convert.ToInt32(randData[i + trainSize][k]);
                    }
                }
            }

            //===R
            //
            //! Performance of Naive Bayes on the above data set:

            NaiveBayes<NormalDistribution> nb = cv.Learn(GameLogs, LabelledData);

            nb.Save(Path.Combine(MyData, "NATIVE.TXT"), SerializerCompression.None);
            NaiveBayes<NormalDistribution> nb2 = Serializer.Load<NaiveBayes<NormalDistribution>>(Path.Combine(MyData, "NATIVE.TXT"));

            int[] predicted = nb2.Decide(GameLogsTest);
            double[][] probs = nb2.Probabilities(GameLogsTest);

            using (IniFile ini = new IniFile(Path.Combine(MyData, Competency + "_BayesAccord.ini")))
            {
                ini.Clear();

                //! Save BayesNet results in .txt file
                using (StreamWriter file = new StreamWriter(Path.Combine(MyData, Competency + "_BayesAccord.txt")))
                {
                    //! We can get different performance measures to assess how good our model was at
                    //! predicting the true, expected, ground-truth labels for the decision problem:
                    var cm = new GeneralConfusionMatrix(classes: 3, expected: expected, predicted: predicted);

                    //! We can obtain the proper confusion matrix using:
                    int[,] matrix = cm.Matrix;

                    //! And multiple performance measures:
                    double accuracy = cm.Accuracy;
                    double error = cm.Error;
                    double kappa = cm.Kappa;

                    file.WriteLine("{0}", Math.Round(accuracy, 4));
                    file.WriteLine("{0}", Math.Round(error, 4));
                    file.WriteLine("{0}", Math.Round(kappa, 4));

                    ini.WriteDouble("Performance", "Accuracy", Math.Round(accuracy, 4));
                    ini.WriteDouble("Performance", "Error", Math.Round(error, 4));
                    ini.WriteDouble("Performance", "Kappa", Math.Round(kappa, 4));

                    double MAE = 0;
                    double RMSE = 0;
                    double RAE = 0;
                    double RRSE = 0;
                    (double, double, double, double) Performace = (MAE, RMSE, RAE, RRSE);
                    Performace = PerformaceStatistics(expected, predicted);

                    file.WriteLine("{0}", Math.Round(Performace.Item1, 4));
                    file.WriteLine("{0}", Math.Round(Performace.Item2, 4));
                    file.WriteLine("{0}%", Math.Round(Performace.Item3, 4));
                    file.WriteLine("{0}%", Math.Round(Performace.Item4, 4));
                    file.WriteLine();

                    ini.WriteDouble("Performance", "MAE", Math.Round(Performace.Item1, 4));
                    ini.WriteDouble("Performance", "RMSE", Math.Round(Performace.Item2, 4));
                    ini.WriteDouble("Performance", "RAE(%)", Math.Round(Performace.Item3, 4), "%");
                    ini.WriteDouble("Performance", "RRSE(%)", Math.Round(Performace.Item4, 4), "%");

                    Data.Performance = new NaiveBayesPerformanceModel(Competency, "na", accuracy, error, kappa, MAE, RMSE, RAE, RRSE);

                    for (Int32 i = 0; i < 3; i++)
                    {
                        ini.WriteInteger("Summary", $"Classified as {i}", predicted.Count(p => p == i));
                    }

                    for (int x = 0; x < predicted.Length; x++)
                    {
                        String section = $"Instance_{x + 1}";

                        ini.WriteList(section, "Tests", GameLogsTest[x].ToList(), ',');

                        file.Write("Instance {0}:", x + 1);
                        for (int y = 0; y < GameLogsTest[x].Length; y++)
                        {
                            file.Write("{0} ", GameLogsTest[x][y]);
                        }

                        ini.WriteDouble(section, "Classified as", predicted[x]);

                        file.WriteLine("Classified as:{0} ", predicted[x]);
                    }

                    Logger.Info("Naive Bayes results from Accord saved successfully.");
                }

                ini.UpdateFile();
            }

            return predicted;
        }

        /// <summary>
        /// All labeled.
        /// </summary>
        ///
        /// <param name="CheckLabels"> The check labels. </param>
        ///
        /// <returns>
        /// True if it succeeds, false if it fails.
        /// </returns>
        internal static bool AllLabelled(
            (bool[][] competencies, bool[][][] facets) CheckLabels)
        {
            //! Count missing competency labels.
            //
            if (CheckLabels.competencies.Count(p => p[0] == false) != 0)
            {
                return false;
            }

            //! Count missing facet labels.
            //
            foreach (bool[][] f in CheckLabels.facets)
            {
                if (f.Count(p => p[0] == false) != 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check labeling.
        /// </summary>
        ///
        /// <param name="observables">     all game logs. </param>
        /// <param name="CompetencyModel"> The competency model. </param>
        ///
        /// <returns>
        /// A (bool[][],bool[][][])
        /// </returns>
        internal static (bool[][] competencies, bool[][][] facets) CheckLabelling(
            Observables observables,
            (string[] competencies, string[][] facets) CompetencyModel)
        {
            //! Stores information whether the data for the given competencies is labeled to decide ML approach.
            bool[][] CheckLabellingCompetencies = new bool[CompetencyModel.competencies.Length][];

            //! Stores information whether the data for the given facets of the declared competencies is labeled to decide ML approach.
            bool[][][] CheckLabellingFacets = new bool[CompetencyModel.competencies.Length][][];

            //! Stores information whether the data for the given facets and competencies is labeled to decide ML approach.
            (bool[][] competencies, bool[][][] facets) CheckLabels = (competencies: CheckLabellingCompetencies, facets: CheckLabellingFacets);

            //! Initialisation of arrays.
            for (int x = 0; x < CompetencyModel.competencies.Length; x++)
            {
                CheckLabellingCompetencies[x] = new bool[1];
                CheckLabellingFacets[x] = new bool[CompetencyModel.facets[x].Length][];
                for (int y = 0; y < CompetencyModel.facets[x].Length; y++)
                {
                    CheckLabellingFacets[x][y] = new bool[1];
                }
            }

            //! Check if labels exists for all declared competencies and facets.
            for (int x = 0; x < CompetencyModel.competencies.Length; x++)
            {
                CheckLabellingCompetencies[x][0] = observables.Any(p => p.ObservableName.Equals(CompetencyModel.competencies[x]));

                for (int y = 0; y < CompetencyModel.facets[x].Length; y++)
                {
                    CheckLabellingFacets[x][y][0] = observables.Any(p => p.ObservableName.Equals(CompetencyModel.facets[x][y]));
                }
            }

            return CheckLabels;
        }

        internal static bool[][] CheckLabellingUni(
            Observables observables,
            String[] UniCompetencyModel)
        {
            //! Stores information whether the data for the given competencies is labeled to decide ML approach.
            bool[][] CheckLabellingCompetencies = new bool[UniCompetencyModel.Length][];

            //! Initialisation of arrays.
            for (int x = 0; x < UniCompetencyModel.Length; x++)
            {
                CheckLabellingCompetencies[x] = new bool[1];
            }

            //! Check if labels exists for all declared competencies and facets.
            for (int x = 0; x < UniCompetencyModel.Length; x++)
            {
                CheckLabellingCompetencies[x][0] = observables.Any(p => p.ObservableName.Equals(UniCompetencyModel[x]));
            }

            return CheckLabellingCompetencies;
        }

        /// <summary>
        /// Generates an arff files for competencies.
        /// </summary>
        ///
        /// <param name="MyData">              my project. </param>
        /// <param name="CompetencyModel">     The competency model. </param>
        /// <param name="StatisticalSubmodel"> The statistical submodel. </param>
        /// <param name="CheckLabels">         The check labels. </param>
        /// <param name="LabelledData">        Information describing the labeled data. </param>
        internal static void GenerateArffFilesForCompetencies(
            String MyData,
            (string[] competencies, string[][] facets) CompetencyModel,
            string[][][] StatisticalSubmodel,
            (bool[][] competencies, bool[][][] facets) CheckLabels,
            (int[][] competencies, int[][][] facets) LabelledData)
        {
            //! Declaring Variables.
            double[][] vals = Array.Empty<double[]>();
            string[][] valsClass = Array.Empty<string[]>();
            string[] Class = new string[3] { "0", "1", "2" };

            for (int x = 0; x < CompetencyModel.competencies.Length; x++)
            {
                if (CheckLabels.competencies[x][0] == true)
                {
                    Logger.Info($"Labeled data has been found for {CompetencyModel.competencies[x]}.");

                    String arff_c = Path.Combine(MyData, CompetencyModel.competencies[x] + ".arff");

                    Directory.CreateDirectory(Path.GetDirectoryName(arff_c));

                    //! 1)
                    using (ArffWriter arffWriter = new ArffWriter(arff_c))
                    {
                        arffWriter.WriteRelationName(CompetencyModel.competencies[x]);

                        Int32 AttrCount = 0;

                        //! Set up attributes
                        for (int i = 0; i < CompetencyModel.facets[x].Length; i++)
                        {
                            //! 2)
                            arffWriter.WriteAttribute(new ArffAttribute("Data for item " + CompetencyModel.facets[x][i], ArffAttributeType.Numeric));

                            AttrCount++;
                        }

                        //! 3)
                        arffWriter.WriteAttribute(new ArffAttribute("Labelled Data", ArffAttributeType.Nominal(Class.ToArray())));
                        AttrCount++;

                        //! Fill with data
                        vals = new double[LabelledData.competencies[x].Length][];
                        valsClass = new string[LabelledData.competencies[x].Length][];
                        for (int a = 0; a < vals.Length; a++)
                        {
                            vals[a] = new double[AttrCount - 1];
                            valsClass[a] = new string[1];
                        }

                        for (int i = 0; i < LabelledData.competencies[x].Length; i++)
                        {
                            for (int a = 0; a < LabelledData.facets[x].Length; a++)
                            {
                                if (a != AttrCount - 1)
                                {
                                    vals[i][a] = LabelledData.facets[x][a][i];
                                }
                            }
                            valsClass[i][0] = Convert.ToString(LabelledData.competencies[x][i]);
                        }

                        //! Add data
                        for (int k = 0; k < vals.Length; k++)
                        {
                            List<Object> instance = new List<Object>();

                            for (int v = 0; v < AttrCount; v++)
                            {
                                instance.Add(
                                          (v != AttrCount - 1)
                                          ? (object)vals[k][v]
                                          : (object)valsClass[k][0]);
                            }

                            arffWriter.WriteInstance(instance.ToArray());
                        }

                        // Save data
                        arffWriter.Flush();

                        Logger.Info($"'{Utils.MakePathRelative(arff_c)}' file successfully.");
                    }
                }
                else
                {
                    Logger.Warn($"No labeled data has been found for '{CompetencyModel.competencies[x]}'.");

                    String arff_c = Path.Combine(MyData, CompetencyModel.competencies[x] + ".arff");

                    Directory.CreateDirectory(Path.GetDirectoryName(arff_c));

                    using (ArffWriter arffWriter = new ArffWriter(arff_c))
                    {
                        arffWriter.WriteRelationName(CompetencyModel.competencies[x]);

                        Int32 AttrCount = 0;

                        //! Set up attributes
                        for (int i = 0; i < CompetencyModel.facets[x].Length; i++)
                        {
                            arffWriter.WriteAttribute(new ArffAttribute("Data for item " + CompetencyModel.facets[x][i], ArffAttributeType.Numeric));

                            AttrCount++;
                        }

                        //! Fill with data
                        vals = new double[LabelledData.competencies[x].Length][];
                        for (int a = 0; a < vals.Length; a++)
                        {
                            vals[a] = new double[AttrCount];
                        }

                        for (int i = 0; i < LabelledData.competencies[x].Length; i++)
                        {
                            for (int a = 0; a < LabelledData.facets[x].Length; a++)
                            {
                                vals[i][a] = LabelledData.facets[x][a][i];
                            }
                        }

                        //! Add data
                        for (int k = 0; k < vals.Length; k++)
                        {
                            arffWriter.WriteInstance(vals[k].Select(p => (Object)p).ToArray());
                        }

                        //! Save data
                        arffWriter.Flush();

                        Logger.Info($"'{Utils.MakePathRelative(arff_c)}' file successfully.");
                    }
                }
            }
        }

        /// <summary>
        /// Generates an arff files for facets.
        /// </summary>
        ///
        /// <param name="MyData">              The competency model. </param>
        /// <param name="CompetencyModel">     The statistical submodel. </param>
        /// <param name="StatisticalSubmodel"> The instances. </param>
        /// <param name="Instances">           The check labels. </param>
        /// <param name="CheckLabels">         Information describing the labeled data. </param>
        /// <param name="LabelledData">        Information describing the labelled. </param>
        internal static void GenerateArffFilesForFacets(
            String MyData,
            (string[] competencies, string[][] facets) CompetencyModel,
            String[][][] StatisticalSubmodel,
            double[][][][] Instances,
            (bool[][] competencies, bool[][][] facets) CheckLabels,
            (int[][] competencies, int[][][] facets) LabelledData)
        {
            //! Declaring Variables.
            double[][] vals = Array.Empty<double[]>();
            string[][] valsClass = Array.Empty<string[]>();
            string[] Class = new string[3] { "0", "1", "2" };

            for (int x = 0; x < CompetencyModel.competencies.Length; x++)
            {
                if (CheckLabels.competencies[x][0] == true)
                {
                    Logger.Info($"Labeled data has been found for '{CompetencyModel.competencies[x]}'.");

                    for (int y = 0; y < CompetencyModel.facets[x].Length; y++)
                    {
                        //! 0)
                        String arff_f = Path.Combine(MyData, CompetencyModel.competencies[x], CompetencyModel.facets[x][y] + ".arff");

                        Directory.CreateDirectory(Path.GetDirectoryName(arff_f));

                        if (CheckLabels.facets[x][y][0] == true)
                        {
                            Logger.Info($"Labeled data has been found for '{CompetencyModel.facets[x][y]}'.");

                            //! 1)
                            using (ArffWriter arffWriter = new ArffWriter(arff_f))
                            {
                                arffWriter.WriteRelationName(CompetencyModel.facets[x][y]);

                                Int32 AttrCount = 0;

                                //! Set up attributes
                                for (int i = 0; i < StatisticalSubmodel[x][y].Length; i++)
                                {
                                    //! 2)
                                    arffWriter.WriteAttribute(new ArffAttribute("Data for item " + StatisticalSubmodel[x][y][i], ArffAttributeType.Numeric));

                                    AttrCount++;
                                }

                                //! 3)
                                arffWriter.WriteAttribute(new ArffAttribute("Labelled Data", ArffAttributeType.Nominal(Class.ToArray())));
                                AttrCount++;

                                //! Fill with data
                                vals = new double[Instances[x][y].Length][];
                                valsClass = new string[Instances[x][y].Length][];

                                for (int a = 0; a < vals.Length; a++)
                                {
                                    vals[a] = new double[AttrCount - 1];
                                    valsClass[a] = new string[1];
                                }

                                for (int i = 0; i < Instances[x][y].Length; i++)
                                {
                                    for (int a = 0; a < AttrCount; a++)
                                    {
                                        if (a != AttrCount - 1)
                                        {
                                            vals[i][a] = Instances[x][y][i][a];
                                        }
                                        else
                                        {
                                            valsClass[i][0] = Convert.ToString(LabelledData.facets[x][y][i]);
                                        }
                                    }
                                }

                                //! Add data
                                for (int k = 0; k < vals.Length; k++)
                                {
                                    List<Object> instance = new List<Object>();

                                    for (int v = 0; v < AttrCount; v++)
                                    {
                                        instance.Add(
                                            (v != AttrCount - 1)
                                            ? (Object)vals[k][v]
                                            : (Object)valsClass[k][0]);
                                    }

                                    arffWriter.WriteInstance(instance.ToArray());
                                }

                                //! Save data
                                arffWriter.Flush();

                                Logger.Info($"'{Utils.MakePathRelative(arff_f)}' file successfully.");
                            }
                        }
                        else
                        {
                            Logger.Warn($"No labeled data has been found for '{CompetencyModel.facets[x][y]}'.");

                            using (ArffWriter arffWriter = new ArffWriter(arff_f))
                            {
                                arffWriter.WriteRelationName(CompetencyModel.facets[x][y]);

                                //! #Attributes will be StatisticalSubmodel[x][y].Length (Items) +0 (Labels).
                                Int32 AttrCount = 0;

                                //! Set up attributes
                                for (int i = 0; i < StatisticalSubmodel[x][y].Length; i++)
                                {
                                    //! 2)
                                    arffWriter.WriteAttribute(new ArffAttribute("Data for item " + StatisticalSubmodel[x][y][i], ArffAttributeType.Numeric));

                                    AttrCount++;
                                }

                                //! Fill with data
                                vals = new double[Instances[x][y].Length][];
                                for (int a = 0; a < vals.Length; a++)
                                {
                                    vals[a] = new double[AttrCount];
                                }

                                for (int i = 0; i < vals.Length; i++)
                                {
                                    for (int a = 0; a < vals[i].Length; a++)
                                    {
                                        vals[i][a] = Instances[x][y][i][a];
                                    }
                                }

                                //! Add data
                                for (int k = 0; k < vals.Length; k++)
                                {
                                    //! Or as a one-liner:
                                    //
                                    arffWriter.WriteInstance(vals[k].Select(p => (Object)p).ToArray());
                                }

                                //! Save data
                                arffWriter.Flush();

                                Logger.Info($"'{Utils.MakePathRelative(arff_f)}' file successfully.");
                            }
                        }
                    }
                }
                else
                {
                    // Debug.WriteLine($"No labeled data has been found for {CompetencyModel.Item1[x]}.");
                    for (int y = 0; y < CompetencyModel.facets[x].Length; y++)
                    {
                        //! 0)
                        String arff_f = Path.Combine(MyData, CompetencyModel.competencies[x], CompetencyModel.facets[x][y] + ".arff");

                        Directory.CreateDirectory(Path.GetDirectoryName(arff_f));

                        if (CheckLabels.facets[x][y][0] == true)
                        {
                            Logger.Info($"Labeled data has been found for '{CompetencyModel.facets[x][y]}'.");

                            //! 1)
                            using (ArffWriter arffWriter = new ArffWriter(arff_f))
                            {
                                arffWriter.WriteRelationName(CompetencyModel.facets[x][y]);

                                //! #Attributes will be StatisticalSubmodel[x][y].Length (Items) +1 (Labels).
                                Int32 AttrCount = 0;

                                //! Set up attributes
                                for (int i = 0; i < StatisticalSubmodel[x][y].Length; i++)
                                {
                                    //! 2)
                                    arffWriter.WriteAttribute(new ArffAttribute("Data for item " + StatisticalSubmodel[x][y][i], ArffAttributeType.Numeric));

                                    AttrCount++;
                                }

                                arffWriter.WriteAttribute(new ArffAttribute("Labelled Data", ArffAttributeType.Nominal(Class.ToArray())));
                                AttrCount++;

                                //! Fill with data
                                vals = new double[Instances[x][y].Length][];
                                valsClass = new string[Instances[x][y].Length][];
                                for (int a = 0; a < vals.Length; a++)
                                {
                                    vals[a] = new double[AttrCount - 1];
                                    valsClass[a] = new string[1];
                                }

                                for (int i = 0; i < Instances[x][y].Length; i++)
                                {
                                    for (int a = 0; a < AttrCount; a++)
                                    {
                                        if (a != AttrCount - 1)
                                        {
                                            vals[i][a] = Instances[x][y][i][a];
                                        }
                                        else
                                        {
                                            valsClass[i][0] = Convert.ToString(LabelledData.facets[x][y][i]);
                                        }
                                    }
                                }

                                //! Add data
                                for (int k = 0; k < vals.Length; k++)
                                {
                                    List<Object> instance = new List<Object>();

                                    for (int v = 0; v < AttrCount; v++)
                                    {
                                        instance.Add(
                                                    (v != AttrCount - 1)
                                                    ? (Object)vals[k][v]
                                                    : (Object)valsClass[k][0]);
                                    }

                                    arffWriter.WriteInstance(instance.ToArray());
                                }

                                //! Save data
                                arffWriter.Flush();

                                Logger.Info($"'{Utils.MakePathRelative(arff_f)}' file successfully.");
                            }
                        }
                        else
                        {
                            Logger.Warn($"No labeled data has been found for '{CompetencyModel.facets[x][y]}'.");

                            //! 1)
                            using (ArffWriter arffWriter = new ArffWriter(arff_f))
                            {
                                arffWriter.WriteRelationName(CompetencyModel.facets[x][y]);

                                Int32 AttrCount = 0;

                                //! Set up attributes
                                for (int i = 0; i < StatisticalSubmodel[x][y].Length; i++)
                                {
                                    arffWriter.WriteAttribute(new ArffAttribute("Data for item " + StatisticalSubmodel[x][y][i], ArffAttributeType.Numeric));

                                    AttrCount++;
                                }

                                //! Fill with data
                                vals = new double[Instances[x][y].Length][];
                                for (int a = 0; a < vals.Length; a++)
                                {
                                    vals[a] = new double[AttrCount];
                                }

                                for (int i = 0; i < vals.Length; i++)
                                {
                                    for (int a = 0; a < vals[i].Length; a++)
                                    {
                                        vals[i][a] = Instances[x][y][i][a];
                                    }
                                }

                                //! Add data
                                for (int k = 0; k < vals.Length; k++)
                                {
                                    arffWriter.WriteInstance(vals[k].Select(p => (Object)p).ToArray());
                                }

                                //! Save data
                                arffWriter.Flush();

                                Logger.Info($"'{Utils.MakePathRelative(arff_f)}' file successfully.");
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Generates an arff files for uni competencies.
        /// </summary>
        ///
        /// <param name="MyData">             my project. </param>
        /// <param name="UniCompetencyModel"> The uni competency model. </param>
        /// <param name="UniEvidenceModel">   The uni evidence model. </param>
        /// <param name="InstancesUni">       The instances uni. </param>
        /// <param name="CheckLabelsUni">     The check labels uni. </param>
        /// <param name="LabelledDataUni">    The labeled data uni. </param>
        internal static void GenerateArffFilesForUniCompetencies(
            String MyData,
            String[] UniCompetencyModel,
            String[][] UniEvidenceModel,
            double[][][] InstancesUni,
            bool[][] CheckLabelsUni,
            int[][] LabelledDataUni)
        {
            //! Declaring Variables.
            string[] Class = new string[3] { "0", "1", "2" };

            for (int x = 0; x < UniCompetencyModel.Length; x++)
            {
                if (CheckLabelsUni[x][0] == true)
                {
                    Logger.Info($"Labeled data has been found for '{UniCompetencyModel[x]}'.");

                    String arff_f = Path.Combine(MyData, UniCompetencyModel[x] + ".arff");

                    Directory.CreateDirectory(Path.GetDirectoryName(arff_f));

                    //! 1)
                    using (ArffWriter arffWriter = new ArffWriter(arff_f))
                    {
                        arffWriter.WriteRelationName(UniCompetencyModel[x]);

                        Int32 AttrCount = 0;

                        //! Set up attributes
                        for (int i = 0; i < UniEvidenceModel[x].Length; i++)
                        {
                            //! 2)
                            arffWriter.WriteAttribute(new ArffAttribute("Data for item " + UniEvidenceModel[x][i], ArffAttributeType.Numeric));

                            AttrCount++;
                        }

                        //! 3)
                        arffWriter.WriteAttribute(new ArffAttribute("Labelled Data", ArffAttributeType.Nominal(Class.ToArray())));
                        AttrCount++;

                        //! Fill with data
                        double[][] vals = new double[InstancesUni[x].Length][];
                        string[][] valsClass = new string[InstancesUni[x].Length][];

                        for (int a = 0; a < vals.Length; a++)
                        {
                            vals[a] = new double[AttrCount - 1];
                            valsClass[a] = new string[1];
                        }

                        for (int i = 0; i < InstancesUni[x].Length; i++)
                        {
                            for (int a = 0; a < AttrCount; a++)
                            {
                                if (a != AttrCount - 1)
                                {
                                    vals[i][a] = InstancesUni[x][i][a];
                                }
                                else
                                {
                                    valsClass[i][0] = Convert.ToString(LabelledDataUni[x][i]);
                                }
                            }
                        }

                        //! Add data
                        for (int k = 0; k < vals.Length; k++)
                        {
                            List<Object> instance = new List<Object>();

                            for (int v = 0; v < AttrCount; v++)
                            {
                                instance.Add(
                                    (v != AttrCount - 1)
                                    ? (Object)vals[k][v]
                                    : (Object)valsClass[k][0]);
                            }

                            arffWriter.WriteInstance(instance.ToArray());
                        }

                        //! Save data
                        arffWriter.Flush();

                        Logger.Info($"'{Utils.MakePathRelative(arff_f)}' file successfully.");
                    }

                }
                else
                {

                    //! 0)
                    String arff_f = Path.Combine(MyData, UniCompetencyModel[x] + ".arff");

                    Directory.CreateDirectory(Path.GetDirectoryName(arff_f));

                    //! 1)
                    using (ArffWriter arffWriter = new ArffWriter(arff_f))
                    {
                        arffWriter.WriteRelationName(UniCompetencyModel[x]);

                        //! #Attributes will be StatisticalSubmodel[x][y].Length (Items) +1 (Labels).
                        Int32 AttrCount = 0;

                        //! Set up attributes
                        for (int i = 0; i < UniEvidenceModel[x].Length; i++)
                        {
                            //! 2)
                            arffWriter.WriteAttribute(new ArffAttribute("Data for item " + UniEvidenceModel[x][i], ArffAttributeType.Numeric));

                            AttrCount++;
                        }

                        arffWriter.WriteAttribute(new ArffAttribute("Labelled Data", ArffAttributeType.Nominal(Class.ToArray())));
                        AttrCount++;

                        //! Fill with data
                        double[][] vals = new double[InstancesUni[x].Length][];
                        string[][] valsClass = new string[InstancesUni[x].Length][];

                        for (int a = 0; a < vals.Length; a++)
                        {
                            vals[a] = new double[AttrCount - 1];
                            valsClass[a] = new string[1];
                        }

                        for (int i = 0; i < InstancesUni[x].Length; i++)
                        {
                            for (int a = 0; a < AttrCount; a++)
                            {
                                if (a != AttrCount - 1)
                                {
                                    vals[i][a] = InstancesUni[x][i][a];
                                }
                                else
                                {
                                    valsClass[i][0] = Convert.ToString(LabelledDataUni[x][i]);
                                }
                            }
                        }

                        //! Add data
                        for (int k = 0; k < vals.Length; k++)
                        {
                            List<Object> instance = new List<Object>();

                            for (int v = 0; v < AttrCount; v++)
                            {
                                instance.Add(
                                            (v != AttrCount - 1)
                                            ? (Object)vals[k][v]
                                            : (Object)valsClass[k][0]);
                            }

                            arffWriter.WriteInstance(instance.ToArray());
                        }

                        //! Save data
                        arffWriter.Flush();

                        Logger.Info($"'{Utils.MakePathRelative(arff_f)}' file successfully.");
                    }

                }
            }
        }

        /// <summary>
        /// Gets labeled data.
        /// </summary>
        ///
        /// <param name="CompetencyModel"> The competency model. </param>
        /// <param name="observables">     all game logs. </param>
        ///
        /// <returns>
        /// The labeled data.
        /// </returns>
        internal static (int[][] competencies, int[][][] facets) GetLabelledData(
            (string[] competencies, string[][] facets) CompetencyModel,
            Observables observables)
        {
            //! Stores the labeling data for the given competencies.
            int[][] LabelledDataCompetencies = new int[CompetencyModel.competencies.Length][];

            //! Stores the labeling data for the given facets.
            int[][][] LabelledDataFacets = new int[CompetencyModel.competencies.Length][][];

            //! Stores the labeling data for the given facets and competencies.
            (int[][] competencies, int[][][] facets) LabelledData = (competencies: LabelledDataCompetencies, facets: LabelledDataFacets);

            //! Initialisation of arrays.
            for (int x = 0; x < CompetencyModel.competencies.Length; x++)
            {
                for (int i = 0; i < observables.Count; i++)
                {
                    //! PROBLEM HERE IS THAT Item1 has a length of 18 and Item2 (used for the size of the LabelledDataCompetencies) has a length of 11.
                    //! SEEMS Item2 does not contain the calculated facets & competencies yet.
                    //! Observables (after loading the ini) contains 18 and 11 items in it's two arrays.
                    if (CompetencyModel.competencies[x] == observables[i].ObservableName)
                    {
                        LabelledDataCompetencies[x] = new int[observables[i].Length];
                        break;
                    }
                    else
                    {
                        LabelledDataCompetencies[x] = new int[observables[i].Length];
                    }
                }
                LabelledDataFacets[x] = new int[CompetencyModel.facets[x].Length][];

                for (int y = 0; y < CompetencyModel.facets[x].Length; y++)
                {
                    for (int i = 0; i < observables.Count; i++)
                    {
                        if (CompetencyModel.facets[x][y] == observables[i].ObservableName)
                        {
#warning out of range error (i: 0..17 bit Item2 is only 11 items (the data excluding labels).
                            LabelledDataFacets[x][y] = new int[observables[i].Length];
                            break;
                        }
                        else
                        {
#warning out of range error (i: 0..17 bit Item2 is only 11 items (the data excluding labels).
                            LabelledDataFacets[x][y] = new int[observables[i].Length];
                        }
                    }
                }
            }

            //! Store labelled data.
            for (int x = 0; x < CompetencyModel.competencies.Length; x++)
            {
                for (int i = 0; i < observables.Count; i++)
                {
                    if (CompetencyModel.competencies[x] == observables[i].ObservableName)
                    {
                        for (int k = 0; k < observables[i].Length; k++)
                        {
                            LabelledDataCompetencies[x][k] = Convert.ToInt32(observables[i][k]);
                        }
                        break;
                    }
                }

                for (int y = 0; y < CompetencyModel.facets[x].Length; y++)
                {
                    for (int i = 0; i < observables.Count; i++)
                    {
                        if (CompetencyModel.facets[x][y] == observables[i].ObservableName)
                        {
                            for (int k = 0; k < observables[i].Length; k++)
                            {
                                LabelledDataFacets[x][y][k] = Convert.ToInt32(observables[i][k]);
                            }
                            break;
                        }
                    }
                }
            }

            return LabelledData;
        }

        internal static int[][] GetLabelledDataUni(
            String[] UniCompetencyModel,
            Observables observables)
        {
            //! Stores the labeling data for the given competencies.
            int[][] LabelledDataCompetencies = new int[UniCompetencyModel.Length][];

            //! Initialisation of arrays.
            for (int x = 0; x < UniCompetencyModel.Length; x++)
            {
                for (int i = 0; i < observables.Count; i++)
                {
                    if (UniCompetencyModel[x] == observables[i].ObservableName)
                    {
                        LabelledDataCompetencies[x] = new int[observables[i].Length];
                        break;
                    }
                    else
                    {
                        LabelledDataCompetencies[x] = new int[observables[i].Length];
                    }
                }
            }

            //! Store labelled data.
            for (int x = 0; x < UniCompetencyModel.Length; x++)
            {
                for (int i = 0; i < observables.Count; i++)
                {
                    if (UniCompetencyModel[x] == observables[i].ObservableName)
                    {
                        for (int k = 0; k < observables[i].Length; k++)
                        {
                            LabelledDataCompetencies[x][k] = Convert.ToInt32(observables[i][k]);
                        }
                        break;
                    }
                }
            }

            return LabelledDataCompetencies;
        }

        /// <summary>
        /// Runs K-Means clustering from the Accord library.
        /// </summary>
        ///
        /// <param name="MyData">     Information describing my. </param>
        /// <param name="reader">     The reader. </param>
        /// <param name="Competency"> The competency. </param>
        ///
        /// <returns>
        /// An int[].
        /// </returns>
        internal static int[] KMeans(
            String MyData,
            ArffReader reader,
            String Competency)
        {
            Accord.Math.Random.Generator.Seed = 1;

            ArffHeader header = reader.ReadHeader();
            object[][] randData = reader.ReadAllInstances();

            //! Initialize arrays.
            double[][] GameLogs = new double[randData.Length][];

            for (int i = 0; i < randData.Length; i++)
            {
                GameLogs[i] = new double[header.Attributes.Count];
            }

            //! Load data on arrays.
            for (int i = 0; i < randData.Length; i++)
            {
                for (int k = 0; k < header.Attributes.Count; k++)
                {
                    GameLogs[i][k] = Convert.ToDouble(randData[i][k]);
                }
            }

            //! Create a new K-Means algorithm
            Double tolerance = 0.05;
            if (Data.MLOptions != null)
            {
                switch (Data.MLOptions.Algorithm)
                {
                    case MLAlgorithms.NaiveBayes:
                        tolerance = (Data.MLOptions as NaiveBayesAlgorithm).Tolerance;
                        break;
                    case MLAlgorithms.DecisionTrees:
                        tolerance = (Data.MLOptions as DecisionTreesAlgorithm).Tolerance;
                        break;
                }
            }

            KMeans kmeans = new KMeans(k: 3)
            {
                Distance = new SquareEuclidean(),

                //! We will compute the K-Means algorithm until cluster centroids
                //! change less than 0.5 between two iterations of the algorithm
                Tolerance = tolerance
            };

            //!  Compute and retrieve the data centroids
            var clusters = kmeans.Learn(GameLogs);

            //! Ordering the clusters
            int count = 0;
            double[] sumVar = new double[clusters.NumberOfClasses];

            foreach (var cluster in clusters)
            {
                count++;
                double[] tempVar = cluster.Centroid;
                double sum = 0;

                for (int x = 0; x < tempVar.Length; x++)
                {
                    sum += tempVar[x];
                }

                sumVar[count - 1] = sum / tempVar.Length;
            }

            double Max = sumVar.Max();
            double Min = sumVar.Min();
            string[] clOrder = new string[clusters.NumberOfClasses];

            for (int v = 0; v < sumVar.Length; v++)
            {
                if (sumVar[v] == Max)
                {
                    clOrder[v] = "High";
                }
                else if (sumVar[v] == Min)
                {
                    clOrder[v] = "Low";
                }
                else
                {
                    clOrder[v] = "Medium";
                }
            }

            //!  Use the centroids to parition all the data
            int[] labels = clusters.Decide(GameLogs);
            int[] newlabels = new int[randData.Length];

            //! Save BayesNet results in .txt file
            using (StreamWriter file = new StreamWriter(Path.Combine(MyData, Competency + "_KMeansAccord.txt")))
            {
                int noCl = 0;
                foreach (var value in clusters)
                {
                    file.WriteLine("Label {0} is: {1}", noCl, clOrder[noCl]);
                    noCl++;
                    double[] tempVar = value.Centroid;
                    for (int x = 0; x < tempVar.Length; x++)
                    {
                        file.WriteLine("Centroid of label {0} for variable {1}: {2}", noCl - 1, x + 1, tempVar[x].ToString());
                    }
                }

                file.WriteLine();

                for (int x = 0; x < GameLogs.Length; x++)
                {
                    file.Write("Instance {0}: ", x);
                    for (int y = 0; y < GameLogs[x].Length; y++)
                    {
                        file.Write("{0} ", GameLogs[x][y]);
                    }
                    file.WriteLine("Old Label: {0} ", labels[x]);

                    //assigning new labels
                    if (labels[x] == 0)
                    {
                        if (clOrder[0] == "Low")
                        {
                            newlabels[x] = 0;
                        }
                        else if (clOrder[0] == "Medium")
                        {
                            newlabels[x] = 1;
                        }
                        else if (clOrder[0] == "High")
                        {
                            newlabels[x] = 2;
                        }
                    }
                    else if (labels[x] == 1)
                    {
                        if (clOrder[1] == "Low")
                        {
                            newlabels[x] = 0;
                        }
                        else if (clOrder[1] == "Medium")
                        {
                            newlabels[x] = 1;
                        }
                        else if (clOrder[1] == "High")
                        {
                            newlabels[x] = 2;
                        }
                    }
                    else if (labels[x] == 2)
                    {
                        if (clOrder[2] == "Low")
                        {
                            newlabels[x] = 0;
                        }
                        else if (clOrder[2] == "Medium")
                        {
                            newlabels[x] = 1;
                        }
                        else if (clOrder[2] == "High")
                        {
                            newlabels[x] = 2;
                        }
                    }
                    file.WriteLine("New Label: {0} ", newlabels[x]);
                }

                Logger.Info("K-Means results from Accord saved successfully.");
            }

            return newlabels;
        }

        /// <summary>
        /// Loads all data.
        /// </summary>
        ///
        /// <param name="filename"> Filename of the file. </param>
        ///
        /// <returns>
        /// all data.
        /// </returns>
        internal static Observables LoadAllData(string filename)
        {
            return Excel.EPPlus(filename);
        }

        /// <summary>
        /// Loads the instances.
        /// </summary>
        ///
        /// <param name="observables">         all game logs. </param>
        /// <param name="CompetencyModel">     The competency model. </param>
        /// <param name="StatisticalSubmodel"> The statistical submodel. </param>
        ///
        /// <returns>
        /// The instances.
        /// </returns>
        internal static (double[][][][] facets, double[][][][] observables) LoadInstances(
            Observables observables,
            (string[] competencies, string[][] facets) CompetencyModel,
            String[][][] StatisticalSubmodel)
        {
            //! Stores instances per declared observable in the statistical sub models.
            double[][][][] InstancesPerObservable = new double[CompetencyModel.competencies.Length][][][];

            //! Stores instances per declared facet in the competency model.
            double[][][][] Instances = new double[CompetencyModel.competencies.Length][][][];

            (double[][][][] facets, double[][][][] observables) Inst = (facets: Instances, observables: InstancesPerObservable);

            //! Initialisation of arrays.
            for (int x = 0; x < CompetencyModel.competencies.Length; x++)
            {
                InstancesPerObservable[x] = new double[CompetencyModel.facets[x].Length][][];
                Instances[x] = new double[CompetencyModel.facets[x].Length][][];
                for (int y = 0; y < CompetencyModel.facets[x].Length; y++)
                {
                    InstancesPerObservable[x][y] = new double[StatisticalSubmodel[x][y].Length][];
                    for (int i = 0; i < StatisticalSubmodel[x][y].Length; i++)
                    {
                        for (int k = 0; k < observables.Count; k++)
                        {
                            if (StatisticalSubmodel[x][y][i] == observables[k].ObservableName)
                            {
                                InstancesPerObservable[x][y][i] = new double[observables[k].Length];
                                Instances[x][y] = new double[observables[k].Length][];
                                break;
                            }
                        }
                    }
                }
            }

            //! Store Instances per declared observable in the statistical submodels.
            for (int x = 0; x < CompetencyModel.competencies.Length; x++)
            {
                for (int y = 0; y < CompetencyModel.facets[x].Length; y++)
                {
                    for (int i = 0; i < StatisticalSubmodel[x][y].Length; i++)
                    {
                        for (int k = 0; k < observables.Count; k++)
                        {
                            if (StatisticalSubmodel[x][y][i] == observables[k].ObservableName)
                            {
                                for (int a = 0; a < observables[k].Length; a++)
                                {
                                    observables[k][a] = observables[k][a].Replace(".", ",");
                                    InstancesPerObservable[x][y][i][a] = Convert.ToDouble(observables[k][a]);
                                }
                            }
                        }
                    }
                }
            }

            //! Initialisation of array Instances.
            for (int x = 0; x < CompetencyModel.competencies.Length; x++)
            {
                for (int y = 0; y < CompetencyModel.facets[x].Length; y++)
                {
                    for (int i = 0; i < Instances[x][y].Length; i++)
                    {
                        Instances[x][y][i] = new double[StatisticalSubmodel[x][y].Length];
                    }
                }
            }

            //! Stores instances per declared facet in the competency model.
            for (int x = 0; x < CompetencyModel.competencies.Length; x++)
            {
                for (int y = 0; y < CompetencyModel.facets[x].Length; y++)
                {
                    for (int i = 0; i < Instances[x][y].Length; i++)
                    {
                        for (int k = 0; k < Instances[x][y][i].Length; k++)
                        {
                            Instances[x][y][i][k] = InstancesPerObservable[x][y][k][i];
                        }
                    }
                }
            }

            return Inst;
        }

        /// <summary>
        /// Loads instances uni.
        /// </summary>
        ///
        /// <param name="observables">        The observables. </param>
        /// <param name="UniCompetencyModel"> The uni competency model. </param>
        /// <param name="UniEvidenceModel">   The uni evidence model. </param>
        ///
        /// <returns>
        /// The instances uni.
        /// </returns>
        internal static (double[][][], double[][][]) LoadInstancesUni(
            Observables observables, string[] UniCompetencyModel,
            String[][] UniEvidenceModel)
        {
            //! Stores instances per declared observable in the statistical submodels.
            double[][][] InstancesPerObservable = new double[UniCompetencyModel.Length][][];

            //! Stores instances per declared facet in the competency model.
            double[][][] Instances = new double[UniCompetencyModel.Length][][];
            (double[][][], double[][][]) Inst = (Instances, InstancesPerObservable);

            //! Initialisation of arrays.
            for (int x = 0; x < UniCompetencyModel.Length; x++)
            {
                InstancesPerObservable[x] = new double[UniEvidenceModel[x].Length][];
                for (int i = 0; i < UniEvidenceModel[x].Length; i++)
                {
                    for (int k = 0; k < observables.Count; k++)
                    {
                        if (UniEvidenceModel[x][i] == observables[k].ObservableName)
                        {
                            InstancesPerObservable[x][i] = new double[observables[k].Length];
                            Instances[x] = new double[observables[k].Length][];
                            break;
                        }
                    }
                }
            }

            //! Store Instances per declared observable in the statistical submodels.
            for (int x = 0; x < UniCompetencyModel.Length; x++)
            {
                for (int i = 0; i < UniEvidenceModel[x].Length; i++)
                {
                    for (int k = 0; k < observables.Count; k++)
                    {
                        if (UniEvidenceModel[x][i] == observables[k].ObservableName)
                        {
                            for (int a = 0; a < observables[k].Length; a++)
                            {
                                observables[k][a] = observables[k][a].Replace(".", ",");
                                InstancesPerObservable[x][i][a] = Convert.ToDouble(observables[k][a]);
                            }
                        }
                    }
                }
            }

            //! Initialization of array Instances.
            for (int x = 0; x < UniCompetencyModel.Length; x++)
            {
                for (int i = 0; i < Instances[x].Length; i++)
                {
                    Instances[x][i] = new double[UniEvidenceModel[x].Length];
                }
            }

            //! Stores instances per declared facet in the competency model.
            for (int x = 0; x < UniCompetencyModel.Length; x++)
            {
                for (int i = 0; i < Instances[x].Length; i++)
                {
                    for (int k = 0; k < Instances[x][i].Length; k++)
                    {
                        Instances[x][i][k] = InstancesPerObservable[x][k][i];
                    }
                }
            }

            return Inst;
        }

        /// <summary>
        /// Normalisations the given item 1.
        /// </summary>
        ///
        /// <param name="Instances"> The first item. </param>
        ///
        /// <returns>
        /// A double[][][][].
        /// </returns>
        internal static double[][][][] Normalisation(
            double[][][][] Instances)
        {
            double[][][][] InstancesNorm = new double[Instances.Length][][][];
            double[][][][] sample = new double[Instances.Length][][][];

            //! Initialization of arrays.
            for (int x = 0; x < Instances.Length; x++)
            {
                sample[x] = new double[Instances[x].Length][][];
                InstancesNorm[x] = new double[Instances[x].Length][][];
                for (int y = 0; y < Instances[x].Length; y++)
                {
                    InstancesNorm[x][y] = new double[Instances[x][y].Length][];
                    for (int i = 0; i < Instances[x][y].Length; i++)
                    {
                        InstancesNorm[x][y][i] = new double[Instances[x][y][i].Length];
                        sample[x][y] = new double[Instances[x][y][i].Length][];
                    }

                    for (int i = 0; i < sample[x][y].Length; i++)
                    {
                        sample[x][y][i] = new double[Instances[x][y].Length];
                    }
                }
            }

            //! Transpose array to get data per observable.
            for (int x = 0; x < sample.Length; x++)
            {
                for (int y = 0; y < sample[x].Length; y++)
                {
                    for (int i = 0; i < sample[x][y].Length; i++)
                    {
                        for (int k = 0; k < sample[x][y][i].Length; k++)
                        {
                            sample[x][y][i][k] = Instances[x][y][k][i];
                        }
                    }
                }
            }

            //! Normalisation of data per declared observable.
            for (int x = 0; x < sample.Length; x++)
            {
                for (int y = 0; y < sample[x].Length; y++)
                {
                    for (int i = 0; i < sample[x][y].Length; i++)
                    {
                        double[] temp = new double[sample[x][y][i].Length];
                        for (int k = 0; k < sample[x][y][i].Length; k++)
                        {
                            temp[k] = sample[x][y][i][k];
                        }

                        NumericVector sampleVC = Utils.engine.CreateNumericVector(temp);
                        Utils.engine.SetSymbol("Sample", sampleVC);

                        GenericVector Normalized = Utils.engine.Evaluate("normalized = (Sample-min(Sample))/(max(Sample)-min(Sample))").AsList();
                        double[] NormalizedSample = Normalized.AsNumeric().ToArray();
                        for (int k = 0; k < NormalizedSample.Length; k++)
                        {
                            InstancesNorm[x][y][k][i] = NormalizedSample[k];
                        }
                    }
                }
            }

            return InstancesNorm;
        }

        /// <summary>
        /// Normalization uni.
        /// </summary>
        ///
        /// <param name="InstancesUni"> The instances uni. </param>
        ///
        /// <returns>
        /// A double[][][].
        /// </returns>
        internal static double[][][] NormalisationUni(
            double[][][] InstancesUni)
        {
            double[][][] InstancesNorm = new double[InstancesUni.Length][][];
            double[][][] sample = new double[InstancesUni.Length][][];

            //! Initialization of arrays.
            for (int x = 0; x < InstancesUni.Length; x++)
            {
                sample[x] = new double[InstancesUni[x].Length][];
                InstancesNorm[x] = new double[InstancesUni[x].Length][];
                for (int i = 0; i < InstancesUni[x].Length; i++)
                {
                    InstancesNorm[x][i] = new double[InstancesUni[x][i].Length];
                    sample[x] = new double[InstancesUni[x][i].Length][];
                }

                for (int i = 0; i < sample[x].Length; i++)
                {
                    sample[x][i] = new double[InstancesUni[x].Length];
                }
            }

            //! Transpose array to get data per observable.
            for (int x = 0; x < sample.Length; x++)
            {
                for (int i = 0; i < sample[x].Length; i++)
                {
                    for (int k = 0; k < sample[x][i].Length; k++)
                    {
                        sample[x][i][k] = InstancesUni[x][k][i];
                    }
                }
            }

            //! Normalisation of data per declared observable.
            for (int x = 0; x < sample.Length; x++)
            {
                for (int i = 0; i < sample[x].Length; i++)
                {
                    double[] temp = new double[sample[x][i].Length];
                    for (int k = 0; k < sample[x][i].Length; k++)
                    {
                        temp[k] = sample[x][i][k];
                    }

                    NumericVector sampleVC = Utils.engine.CreateNumericVector(temp);
                    Utils.engine.SetSymbol("Sample", sampleVC);

                    GenericVector Normalized = Utils.engine.Evaluate("normalized = (Sample-min(Sample))/(max(Sample)-min(Sample))").AsList();
                    double[] NormalizedSample = Normalized.AsNumeric().ToArray();
                    for (int k = 0; k < NormalizedSample.Length; k++)
                    {
                        InstancesNorm[x][k][i] = NormalizedSample[k];
                    }
                }
            }

            return InstancesNorm;
        }

        /// <summary>
        /// Select labelsfor competencies.
        /// </summary>
        ///
        /// <param name="MyData">          Information describing my. </param>
        /// <param name="CompetencyModel"> The competency model. </param>
        /// <param name="LabelledData">    Information describing the labeled data. </param>
        ///
        /// <returns>
        /// A (int[][],int[][][],int[][])
        /// </returns>
        internal static (int[][] competencies, int[][][] facets, int[][] output) SelectLabelsforCompetencies(
            String MyData,
            (string[] competencies, string[][] facets) CompetencyModel,
            (int[][] competencies, int[][][] facets) LabelledData)
        {
            int[][] output = new int[CompetencyModel.competencies.Length][];

            (int[][] competencies, int[][][] facets, int[][] output) LabelledOutput = (LabelledData.competencies, LabelledData.facets, output);

            for (int x = 0; x < CompetencyModel.competencies.Length; x++)
            {
                Logger.Warn($"No labeled data has been found for '{CompetencyModel.competencies[x]}'.");

                //! Select a clustering algorithm (KMEANS only).

                String arff_c = Path.Combine(MyData, CompetencyModel.competencies[x] + ".arff");

                //! Get data from the .arff file.

                //! Checks user input for an ML algorithm.
                switch (Data.MLOptions.Clustering)
                {
                    //! (1) K-Means [Centroid-based]
                    case MLClustering.KMeans:
                        using (ArffReader arffReader = new ArffReader(arff_c))
                        {
                            int[] labels = KMeans(MyData, arffReader, CompetencyModel.competencies[x]);

                            LabelledData.competencies[x] = new int[labels.Length];
                            output[x] = new int[labels.Length];

                            for (int i = 0; i < labels.Length; i++)
                            {
                                LabelledData.competencies[x][i] = labels[i];
                                output[x][i] = labels[i];
                            }
                        }
                        break;
                }
            }

            return LabelledOutput;
        }

        /// <summary>
        /// Select Labels for facets.
        /// </summary>
        ///
        /// <param name="CompetencyModel"> The competency model. </param>
        /// <param name="CheckLabels">     The check labels. </param>
        /// <param name="LabelledData">    Information describing the labeled data. </param>
        ///
        /// <returns>
        /// A (int[][],int[][][],int[][][])
        /// </returns>
        internal static (int[][] competencies, int[][][] facets, int[][][] output) SelectLabelsforFacets(
            String MyData,
            (string[] competencies, string[][] facets) CompetencyModel,
            (int[][] competencies, int[][][] facets) LabelledData)
        {
            int[][][] output = new int[CompetencyModel.competencies.Length][][];

            (int[][] competencies, int[][][] facets, int[][][] output) LabelledOutput = (LabelledData.competencies, LabelledData.facets, output);

            //! Browse for labeled data for each declared facet and decide which ML algorithm to apply accordingly.
            for (int x = 0; x < CompetencyModel.competencies.Length; x++)
            {
                output[x] = new int[CompetencyModel.facets[x].Length][];

                for (int y = 0; y < CompetencyModel.facets[x].Length; y++)
                {
                    //===R

                    //Debug.WriteLine($"No labeled data has been found for {CompetencyModel.Item2[x][y]}.");

                    String arff_f = Path.Combine(MyData, CompetencyModel.competencies[x], CompetencyModel.facets[x][y] + ".arff");

                    //! Get data from the .arff file.

                    //! Checks user input for an ML algorithm.
                    //
                    switch (Data.MLOptions.Clustering)
                    {
                        //! (1) K-Means [Centroid-based]
                        case MLClustering.KMeans:
                            using (ArffReader arffReader = new ArffReader(arff_f))
                            {
                                int[] labels = KMeans(MyData, arffReader, CompetencyModel.competencies[x], CompetencyModel.facets[x][y]); ;

                                LabelledData.facets[x][y] = new int[labels.Length];
                                output[x][y] = new int[labels.Length];

                                for (int i = 0; i < labels.Length; i++)
                                {
                                    LabelledData.facets[x][y][i] = labels[i];
                                    output[x][y][i] = labels[i];
                                }
                            }
                            break;
                    }
                }
            }

            return LabelledOutput;
        }

        /// <summary>
        /// Select labelsfor uni competencies.
        /// </summary>
        ///
        /// <param name="MyData">             my project. </param>
        /// <param name="UniCompetencyModel"> The uni competency model. </param>
        /// <param name="UniLabelledData">    Information describing the uni labelled. </param>
        ///
        /// <returns>
        /// A (int[][],int[][],int[][])
        /// </returns>
        internal static (int[][], int[][], int[][]) SelectLabelsforUniCompetencies(
            String MyData,
            String[] UniCompetencyModel,
            int[][] UniLabelledData)
        {
            //! This variable allows access to functions of the Exceptions class.
            //var Exceptions = new Exceptions();

            int[][] output = new int[UniCompetencyModel.Length][];

            (int[][], int[][], int[][]) LabelledOutput = (UniLabelledData, UniLabelledData, output);

            //! Browse for labeled data for each declared facet and decide which ML algorithm to apply accordingly.
            for (int x = 0; x < UniCompetencyModel.Length; x++)
            {

                //===R

                //Debug.WriteLine($"No labeled data has been found for {CompetencyModel.Item2[x][y]}.");

                String arff_f = Path.Combine(MyData, UniCompetencyModel[x] + ".arff");

                //! Get data from the .arff file.

                //! Checks user input for an ML algorithm.
                //
                switch (Data.MLOptions.Clustering)
                {
                    //! (1) K-Means [Centroid-based]
                    case MLClustering.KMeans:
                        using (ArffReader arffReader = new ArffReader(arff_f))
                        {
                            int[] labels = KMeans(MyData, arffReader, UniCompetencyModel[x]); ;

                            UniLabelledData[x] = new int[labels.Length];
                            output[x] = new int[labels.Length];

                            for (int i = 0; i < labels.Length; i++)
                            {
                                UniLabelledData[x][i] = labels[i];
                                output[x][i] = labels[i];
                            }
                        }
                        break;
                }

            }

            return LabelledOutput;
        }

        /// <summary>
        /// Select ML for competencies.
        /// </summary>
        ///
        /// <param name="CompetencyModel"> The competency model. </param>
        /// <param name="CheckLabels">     The check labels. </param>
        /// <param name="LabelledData">    Information describing the labeled data. </param>
        ///
        /// <returns>
        /// A (int[][],int[][][],int[][])
        /// </returns>
        ///
        /// ### <param name="competencyModel"> The competency model. </param>
        ///
        /// ### <param name="checkLabels">  The check labels. </param>
        /// ### <param name="labelledData"> Information describing the labeled data. </param>
        internal static (int[][] competencies, int[][][] facets, int[][] output) SelectMLforCompetencies(
            String MyData,
            (string[] competencies, string[][] facets) CompetencyModel,
            (int[][] competencies, int[][][] facets) LabelledData)
        {
            //! This variable allows access to functions of the Exceptions class.
            int[][] output = new int[CompetencyModel.competencies.Length][];
            int[] outputLabels;

            (int[][] competencies, int[][][] facets, int[][] output) LabelledOutput = (LabelledData.competencies, LabelledData.facets, output);

            //! Browse for labeled data for each declared facet and decide which ML algorithm to apply accordingly.
            for (int x = 0; x < CompetencyModel.competencies.Length; x++)
            {
                Logger.Info($"Labeled data has been found for '{CompetencyModel.competencies[x]}'.");

                String arff_c = Path.Combine(MyData, CompetencyModel.competencies[x] + ".arff");

                //! Get data from the .arff file.
                using (ArffReader reader = new ArffReader(arff_c))
                {
                    //! Checks user input for an ML algorithm.
                    switch (Data.MLOptions.Algorithm)
                    {
                        case MLAlgorithms.NaiveBayes:
                            {
                                outputLabels = NaiveBayesAccord_C(MyData, reader, CompetencyModel.competencies[x]);

                                output[x] = new int[outputLabels.Length];
                                for (int i = 0; i < outputLabels.Length; i++)
                                {
                                    output[x][i] = outputLabels[i];
                                }
                            }
                            break;

                        case MLAlgorithms.DecisionTrees:
                            {
                                outputLabels = DecisionTreesAccord_C(MyData, reader, CompetencyModel.competencies[x]);
                                output[x] = new int[outputLabels.Length];
                                for (int i = 0; i < outputLabels.Length; i++)
                                {
                                    output[x][i] = outputLabels[i];
                                }
                            }
                            break;
                    }
                }
            }

            return LabelledOutput;
        }

        /// <summary>
        /// Select ml for facets.
        /// </summary>
        ///
        /// <param name="MyData">          Information describing my. </param>
        /// <param name="CompetencyModel"> The competency model. </param>
        /// <param name="LabelledData">    Information describing the labeled data. </param>
        ///
        /// <returns>
        /// A (int[][],int[][][],int[][][])
        /// </returns>
        internal static (int[][] competencies, int[][][] facets, int[][][] output) SelectMLforFacets(
            String MyData,
            (string[] competencies, string[][] facets) CompetencyModel,
            (int[][] competencies, int[][][] facets) LabelledData)
        {
            //! This variable allows access to functions of the Exceptions class.
            // Exceptions Exceptions = new Exceptions();

            int[][][] output = new int[CompetencyModel.competencies.Length][][];
            int[] outputLabels;

            (int[][] competencies, int[][][] facets, int[][][] output) LabelledOutput = (LabelledData.competencies, LabelledData.facets, output);

            //! Browse for labeled data for each declared facet and decide which ML algorithm to apply accordingly.
            for (int x = 0; x < CompetencyModel.competencies.Length; x++)
            {
                output[x] = new int[CompetencyModel.facets[x].Length][];

                // Debug.WriteLine($"Labeled data has been found for {CompetencyModel.Item1[x]}." );
                for (int y = 0; y < CompetencyModel.facets[x].Length; y++)
                {
                    //===R

                    Logger.Info($"Labeled data has been found for '{CompetencyModel.facets[x][y]}'.");

                    //! Select a ML algorithm.
                    String arff_f = Path.Combine(MyData, CompetencyModel.competencies[x], CompetencyModel.facets[x][y] + ".arff");

                    //! Get data from the .arff file.
                    using (ArffReader reader = new ArffReader(arff_f))
                    {
                        //! Checks user input for an ML algorithm.
                        switch (Data.MLOptions.Algorithm)
                        {
                            //! (1) Naive Bayes
                            case MLAlgorithms.NaiveBayes:
                                {
                                    outputLabels = NaiveBayesAccord_F(MyData, reader, CompetencyModel.competencies[x], CompetencyModel.facets[x][y]);

                                    output[x][y] = new int[outputLabels.Length];
                                    for (int i = 0; i < outputLabels.Length; i++)
                                    {
                                        output[x][y][i] = outputLabels[i];
                                    }
                                }
                                break;

                            //! (2) Decision Trees
                            case MLAlgorithms.DecisionTrees:
                                {
                                    outputLabels = DecisionTreesAccord_F(MyData, reader, CompetencyModel.competencies[x], CompetencyModel.facets[x][y]);
                                    output[x][y] = new int[outputLabels.Length];
                                    for (int i = 0; i < outputLabels.Length; i++)
                                    {
                                        output[x][y][i] = outputLabels[i];
                                    }
                                }
                                break;
                        }
                    }
                }
            }

            return LabelledOutput;
        }

        /// <summary>
        /// Select m lfor uni competencies.
        /// </summary>
        ///
        /// <param name="MyData">             Information describing my. </param>
        /// <param name="UniCompetencyModel"> The uni competency model. </param>
        /// <param name="LabelledDataUni">    The labelled data uni. </param>
        ///
        /// <returns>
        /// A (int[][],int[][],int[][])
        /// </returns>
        internal static (int[][], int[][], int[][]) SelectMLforUniCompetencies(
            String MyData,
            String[] UniCompetencyModel,
            int[][] LabelledDataUni)
        {
            //! This variable allows access to functions of the Exceptions class.
            int[][] output = new int[UniCompetencyModel.Length][];
            int[] outputLabels;

            (int[][], int[][], int[][]) LabelledOutput = (LabelledDataUni, LabelledDataUni, output);

            //! Browse for labelled data for each declared facet and decide which ML algorithm to apply accordingly.
            for (int x = 0; x < UniCompetencyModel.Length; x++)
            {
                Logger.Info($"Labeled data has been found for '{UniCompetencyModel[x]}'.");

                String arff_c = Path.Combine(MyData, UniCompetencyModel[x] + ".arff");

                //! Get data from the .arff file.
                using (ArffReader reader = new ArffReader(arff_c))
                {
                    //! Checks user input for an ML algorithm.
                    switch (Data.MLOptions.Algorithm)
                    {
                        case MLAlgorithms.NaiveBayes:
                            {
                                outputLabels = UniNaiveBayesAccord_C(MyData, reader, UniCompetencyModel[x]);

                                output[x] = new int[outputLabels.Length];
                                for (int i = 0; i < outputLabels.Length; i++)
                                {
                                    output[x][i] = outputLabels[i];
                                }
                            }
                            break;

                        case MLAlgorithms.DecisionTrees:
                            {
                                outputLabels = UniDecisionTreesAccord_C(MyData, reader, UniCompetencyModel[x]);
                                output[x] = new int[outputLabels.Length];
                                for (int i = 0; i < outputLabels.Length; i++)
                                {
                                    output[x][i] = outputLabels[i];
                                }
                            }
                            break;
                    }
                }
            }

            return LabelledOutput;
        }

        #endregion Methods
    }
}