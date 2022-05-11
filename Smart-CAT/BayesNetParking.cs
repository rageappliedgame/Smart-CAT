using Accord.MachineLearning;
using Accord.Math.Distances;
using Accord.Statistics.Distributions.DensityKernels;
using Accord.Statistics.Distributions.Fitting;
using Accord.Statistics.Distributions.Multivariate;
using ArffTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealthAssessmentWizard
{
    public static partial class BayesNet
    {
        /// <summary>
        /// Generates .arff files for correlation analysis.
        /// </summary>
        ///
        /// <param name="MyProject">          my project. </param>
        /// <param name="CompetencyModel">    The competency model. </param>
        /// <param name="RandomLabelledData"> Information describing the random labelled. </param>
        /// <param name="OutputLabels">       The output labels. </param>

        [Obsolete]
        internal static void GenerateArffFilesForCA(
                    String MyProject,
                    Tuple<string[], string[][]> CompetencyModel,
                    Tuple<int[][], int[][][]> RandomLabelledData,
                    Tuple<int[][], int[][][]> OutputLabels)
        {
            //! Declaring Variables.
            double[][] vals = Array.Empty<double[]>();

            //! Generated .arff files for the declared competencies.
            for (int x = 0; x < CompetencyModel.Item1.Length; x++)
            {

                //! Added extra SafeGuard for empty arff files.

                if (RandomLabelledData.Item1[x] != null && RandomLabelledData.Item1[x].Length != 0)
                {
                    String arff_c = Path.Combine(MyProject, @"data\CA\" + CompetencyModel.Item1[x] + ".arff");

                    Directory.CreateDirectory(Path.GetDirectoryName(arff_c));

                    using (ArffWriter arffWriter = new ArffWriter(arff_c))
                    {
                        arffWriter.WriteRelationName(CompetencyModel.Item1[x]);

                        //! Set up attributes
                        arffWriter.WriteAttribute(new ArffAttribute("External data for item " + CompetencyModel.Item1[x], ArffAttributeType.Numeric));
                        arffWriter.WriteAttribute(new ArffAttribute("Labelled data for item " + CompetencyModel.Item1[x], ArffAttributeType.Numeric));

                        Int32 AttrCount = 2;

                        //! Fill with data
                        vals = new double[OutputLabels.Item1[x].Length][];
                        for (int a = 0; a < vals.Length; a++)
                        {
                            vals[a] = new double[AttrCount];
                        }

                        for (int i = 0; i < OutputLabels.Item1[x].Length; i++)
                        {
                            for (int a = 0; a < AttrCount; a++)
                            {
                                if (a == 0)
                                {
                                    vals[i][a] = RandomLabelledData.Item1[x][i];
                                }
                                else if (a == 1)
                                {
                                    vals[i][a] = OutputLabels.Item1[x][i];
                                }
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
                else
                {
                    Logger.Warn("No external data was found.");
                };

                //! Generated .arff files for the declared facets.
                for (int y = 0; y < CompetencyModel.Item2[x].Length; y++)
                {

                    //! Added extra SafeGuard for empty arff files.

                    if (RandomLabelledData.Item2[x][y] != null && RandomLabelledData.Item2[x][y].Length != 0)
                    {
                        String arff_f = Path.Combine(MyProject, @"data\CA\" + CompetencyModel.Item1[x] + @"\" + CompetencyModel.Item2[x][y] + ".arff");

                        Directory.CreateDirectory(Path.GetDirectoryName(arff_f));

                        //! 1)
                        using (ArffWriter arffWriter = new ArffWriter(arff_f))
                        {
                            //! Set up attributes
                            arffWriter.WriteAttribute(new ArffAttribute("External data for item " + CompetencyModel.Item2[x][y], ArffAttributeType.Numeric));
                            arffWriter.WriteAttribute(new ArffAttribute("Labelled data for item " + CompetencyModel.Item2[x][y], ArffAttributeType.Numeric));

                            Int32 AttrCount = 2;

                            //! Fill with data
                            vals = new double[OutputLabels.Item2[x][y].Length][];
                            for (int a = 0; a < vals.Length; a++)
                            {
                                vals[a] = new double[AttrCount];
                            }

                            for (int i = 0; i < OutputLabels.Item2[x][y].Length; i++)
                            {
                                for (int a = 0; a < AttrCount; a++)
                                {
                                    if (a == 0)
                                    {
                                        vals[i][a] = RandomLabelledData.Item2[x][y][i];
                                    }
                                    else if (a == 1)
                                    {
                                        vals[i][a] = OutputLabels.Item2[x][y][i];
                                    }
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
                    else
                    {
                        Logger.Warn("No external data was found.");
                    };
                }
            }
        }

        /// <summary>
        /// Runs Gaussian Mixture Model clustering from the Accord library.
        /// </summary>
        ///
        /// <param name="MyProject">  my project. </param>
        /// <param name="reader">     The reader. </param>
        /// <param name="Competency"> The competency. </param>
        /// <param name="Facet">      The facet. </param>
        ///
        /// <returns>
        /// An int[].
        /// </returns>
        ///
        /// ### <param name="insts"> The insts. </param>
        [Obsolete]
        public static int[] GaussianMixtureModel(
            String MyProject,
            ArffReader reader,
            string Competency,
            string Facet)
        {
            Accord.Math.Random.Generator.Seed = 1;

            //Instances randData = new Instances(insts);

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

            //! Create a new Gaussian Mixture Model
            GaussianMixtureModel gmm = new GaussianMixtureModel(kmeans: kmeans);

            //! Create a multivariate Gaussian for N dimensions (depending on no. of observables)
            var normal = new MultivariateNormalDistribution(header.Attributes.Count);

            //! Specify a regularization constant in the fitting options
            gmm.Options = new NormalOptions() { Regularization = 1e-10 };

            //! Fit the distribution to the data
            normal.Fit(GameLogs, gmm.Options);

            //! Estimate the Gaussian Mixture
            var cl = gmm.Learn(GameLogs);

            //! Predict cluster labels for each sample
            int[] predicted = cl.Decide(GameLogs);

            //! Save BayesNet results in .txt file
            using (StreamWriter file = new StreamWriter(Path.Combine(MyProject, @"data\" + Competency + @"\" + Facet + "_GaussianMixtureModelAccord.txt")))
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
                    file.WriteLine("Label: {0}", predicted[x]);
                }

                Logger.Info("Gaussian Mixture Model results from Accord saved successfully.");
            }

            return predicted;
        }

        /// <summary>
        /// Runs Mean Shift clustering from the Accord library.
        /// </summary>
        ///
        /// <param name="MyProject">  my project. </param>
        /// <param name="reader">     The reader. </param>
        /// <param name="Competency"> The competency. </param>
        /// <param name="Facet">      The facet. </param>
        ///
        /// <returns>
        /// An int[].
        /// </returns>
        [Obsolete]
        public static int[] MeanShift(
            String MyProject,
            ArffReader reader,
            String Competency,
            String Facet)
        {
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

            //! Create a new Mean-Shift algorithm for 3 dimensional samples
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

            MeanShift meanShift = new MeanShift()
            {
                //! Use a uniform kernel density
                Kernel = new GaussianKernel(dimension: 3),
                Bandwidth = 3,

                //! We will compute the mean-shift algorithm until the means
                //! change less than 0.05 between two iterations of the algorithm
                Tolerance = tolerance,

                MaxIterations = 10
            };

            //! Learn a data partitioning using the Mean Shift algorithm
            MeanShiftClusterCollection clustering = meanShift.Learn(GameLogs);

            //! Predict group labels for each point
            int[] labels = clustering.Decide(GameLogs);

            //! Save BayesNet results in .txt file
            using (StreamWriter file = new StreamWriter(Path.Combine(MyProject, @"data\" + Competency + @"\" + Facet + "_MeanShiftAccord.txt")))
            {
                for (int x = 0; x < GameLogs.Length; x++)
                {
                    file.Write("Instance {0}: ", x);
                    for (int y = 0; y < GameLogs[x].Length; y++)
                    {
                        file.Write("{0} ", GameLogs[x][y]);
                    }
                    file.WriteLine("Label: {0}", labels[x]);
                }

                Logger.Info("Mean Shift results from Accord saved successfully.");
            }

            return labels;
        }

        /// <summary>
        /// Generates an arff files for randomization.
        /// </summary>
        ///
        /// <param name="MyProject">       my project. </param>
        /// <param name="CompetencyModel"> The competency model. </param>
        /// <param name="ExternalDataCF">  The external data cf. </param>
        ///
        /// ### <param name="competencyModel"> The competency model. </param>
        ///
        /// ### <param name="externalDataCF"> The external data cf. </param>
        [Obsolete]
        internal static void GenerateArffFilesforRandomization(
            String MyProject,
            Tuple<string[], string[][]> CompetencyModel,
            Tuple<int[][], int[][][]> ExternalDataCF)
        {
            //! Declaring Variables.
            double[][] vals = Array.Empty<double[]>();

            //! Generated .arff files for the declared competencies.
            for (int x = 0; x < CompetencyModel.Item1.Length; x++)
            {
                if (ExternalDataCF.Item1[x] != null)
                {
                    String arff_c = Path.Combine(MyProject, @"data\CA\Randomized\" + CompetencyModel.Item1[x] + ".arff");

                    Directory.CreateDirectory(Path.GetDirectoryName(arff_c));

                    //! 1)
                    using (ArffWriter arffWriter = new ArffWriter(arff_c))
                    {
                        arffWriter.WriteRelationName(CompetencyModel.Item1[x]);

                        Int32 AttrCount = 0;

                        //! Set up attributes
                        arffWriter.WriteAttribute(new ArffAttribute("External data for item " + CompetencyModel.Item1[x], ArffAttributeType.Numeric));
                        AttrCount++;

                        //! Fill with data
                        vals = new double[ExternalDataCF.Item1[x].Length][];
                        for (int a = 0; a < vals.Length; a++)
                        {
                            vals[a] = new double[AttrCount];
                        }

                        for (int i = 0; i < ExternalDataCF.Item1[x].Length; i++)
                        {
                            for (int a = 0; a < AttrCount; a++)
                            {
                                vals[i][a] = ExternalDataCF.Item1[x][i];
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
                else
                {
                    Logger.Warn("No external data was found.");
                };

                //===R
                //
                //! Generated .arff files for the declared facets.
                for (int y = 0; y < CompetencyModel.Item2[x].Length; y++)
                {
                    if (ExternalDataCF.Item2[x][y] != null)
                    {
                        String arff_f = Path.Combine(MyProject, @"data\CA\Randomized\" + CompetencyModel.Item1[x] + @"\" + CompetencyModel.Item2[x][y] + ".arff");

                        Directory.CreateDirectory(Path.GetDirectoryName(arff_f));

                        //! 1)
                        using (ArffWriter arffWriter = new ArffWriter(arff_f))
                        {
                            arffWriter.WriteRelationName(CompetencyModel.Item2[x][y]);

                            Int32 AttrCount = 0;

                            //! Set up attributes
                            arffWriter.WriteAttribute(new ArffAttribute("External data for item " + CompetencyModel.Item2[x][y], ArffAttributeType.Numeric));
                            AttrCount++;

                            //! Fill with data
                            vals = new double[ExternalDataCF.Item2[x][y].Length][];
                            for (int a = 0; a < vals.Length; a++)
                            {
                                vals[a] = new double[AttrCount];
                            }

                            for (int i = 0; i < ExternalDataCF.Item2[x][y].Length; i++)
                            {
                                for (int a = 0; a < AttrCount; a++)
                                {
                                    vals[i][a] = ExternalDataCF.Item2[x][y][i];
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
                    else
                    {
                        Logger.Warn("No external data was found.");
                    };
                }
            }
        }

        [Obsolete]
        internal static void GenerateArffFilesForUniCompetencies(
            String MyProject,
            String[] UniCompetencyModel,
            String[][] UniEvidenceModel,
            bool[][] CheckLabelsUni,
            int[][] LabelledDataUni)
        {
            //! Declaring Variables.
            double[][] vals = Array.Empty<double[]>();
            string[][] valsClass = Array.Empty<string[]>();
            string[] Class = new string[3] { "0", "1", "2" };

            for (int x = 0; x < UniCompetencyModel.Length; x++)
            {
                if (CheckLabelsUni[x][0] == true)
                {
                    Logger.Info($"Labeled data has been found for {UniCompetencyModel[x]}.");

                    String arff_c = Path.Combine(MyProject, @"data\" + UniCompetencyModel[x] + ".arff");

                    Directory.CreateDirectory(Path.GetDirectoryName(arff_c));

                    //! 1)
                    using (ArffWriter arffWriter = new ArffWriter(arff_c))
                    {
                        arffWriter.WriteRelationName(UniCompetencyModel[x]);

                        Int32 AttrCount = 0;

                        //! Set up attributes
                        for (int i = 0; i < UniCompetencyModel[x].Length; i++)
                        {
                            //! 2)
                            arffWriter.WriteAttribute(new ArffAttribute("Data for item " + UniCompetencyModel[i], ArffAttributeType.Numeric));

                            AttrCount++;
                        }

                        //! 3)
                        arffWriter.WriteAttribute(new ArffAttribute("Labelled Data", ArffAttributeType.Nominal(Class.ToArray())));
                        AttrCount++;

                        //! Fill with data
                        vals = new double[LabelledDataUni[x].Length][];
                        valsClass = new string[LabelledDataUni[x].Length][];
                        for (int a = 0; a < vals.Length; a++)
                        {
                            vals[a] = new double[AttrCount - 1];
                            valsClass[a] = new string[1];
                        }

                        for (int i = 0; i < LabelledDataUni.Length; i++)
                        {
                            for (int a = 0; a < LabelledDataUni[x].Length; a++)
                            {
                                if (a != AttrCount - 1)
                                {
                                    vals[i][a] = LabelledDataUni[x][a];
                                }
                            }
                            valsClass[i][0] = Convert.ToString(LabelledDataUni[x][i]);
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
                    Logger.Warn($"No labeled data has been found for '{UniCompetencyModel[x]}'.");

                    String arff_c = Path.Combine(MyProject, @"data\" + UniCompetencyModel[x] + ".arff");

                    Directory.CreateDirectory(Path.GetDirectoryName(arff_c));

                    using (ArffWriter arffWriter = new ArffWriter(arff_c))
                    {
                        arffWriter.WriteRelationName(UniCompetencyModel[x]);

                        Int32 AttrCount = 0;

                        //! Set up attributes
                        for (int i = 0; i < UniCompetencyModel[x].Length; i++)
                        {
                            arffWriter.WriteAttribute(new ArffAttribute("Data for item " + UniCompetencyModel[x][i], ArffAttributeType.Numeric));

                            AttrCount++;
                        }

                        //! Fill with data
                        vals = new double[LabelledDataUni[x].Length][];
                        for (int a = 0; a < vals.Length; a++)
                        {
                            vals[a] = new double[AttrCount];
                        }

                        for (int i = 0; i < LabelledDataUni[x].Length; i++)
                        {
                            for (int a = 0; a < LabelledDataUni[x].Length; a++)
                            {
                                vals[i][a] = LabelledDataUni[x][a];
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
        /// Loads instances external facets competencies.
        /// </summary>
        ///
        /// <param name="AllExternalMeasurementData"> Information describing all external measurement. </param>
        /// <param name="CompetencyModel">            The competency model. </param>
        ///
        /// <returns>
        /// The instances external facets competencies.
        /// </returns>
        [Obsolete]
        internal static Tuple<int[][], int[][][]> LoadInstancesExternalFacetsCompetencies(
            Tuple<string[], string[][]> AllExternalMeasurementData,
            Tuple<string[], string[][]> CompetencyModel)
        {
            //! Stores the external data for the given competencies.
            int[][] ExternalDataCompetencies = new int[CompetencyModel.Item1.Length][];

            //! Stores the external data for the given facets.
            int[][][] ExternalDataFacets = new int[CompetencyModel.Item1.Length][][];

            //! Stores the external data for the given facets and competencies.
            Tuple<int[][], int[][][]> ExternalDataCF = new Tuple<int[][], int[][][]>(ExternalDataCompetencies, ExternalDataFacets);

            //! Initialization of arrays.

            //! check if external data exists for a given competency.
            bool flagC = false;

            //! check if data exists for a given facet.
            bool flagF = false;

            for (int x = 0; x < CompetencyModel.Item1.Length; x++)
            {
                ExternalDataFacets[x] = new int[CompetencyModel.Item2[x].Length][];
                for (int y = 0; y < AllExternalMeasurementData.Item1.Length; y++)
                {
                    if (CompetencyModel.Item1[x] == AllExternalMeasurementData.Item1[y])
                    {
                        ExternalDataCompetencies[x] = new int[AllExternalMeasurementData.Item2[y].Length];
                        flagC = true;
                    }
                }

                if (!flagC)
                {
                    ExternalDataCompetencies[x] = new int[1];
                }

                for (int y = 0; y < CompetencyModel.Item2[x].Length; y++)
                {
                    for (int i = 0; i < AllExternalMeasurementData.Item1.Length; i++)
                    {
                        if (CompetencyModel.Item2[x][y] == AllExternalMeasurementData.Item1[i])
                        {
                            ExternalDataFacets[x][y] = new int[AllExternalMeasurementData.Item2[i].Length];
                            flagF = true;
                        }
                    }

                    if (!flagF)
                    {
                        ExternalDataFacets[x][y] = new int[1];
                    }
                }
            }

            //! Store external data for competencies.
            for (int x = 0; x < CompetencyModel.Item1.Length; x++)
            {
                for (int y = 0; y < AllExternalMeasurementData.Item1.Length; y++)
                {
                    if (CompetencyModel.Item1[x] == AllExternalMeasurementData.Item1[y])
                    {
                        for (int i = 0; i < AllExternalMeasurementData.Item2[y].Length; i++)
                        {
                            ExternalDataCompetencies[x][i] = Convert.ToInt32(AllExternalMeasurementData.Item2[y][i]);
                        }
                    }
                }

                //! Store external data for facets.
                for (int y = 0; y < CompetencyModel.Item2[x].Length; y++)
                {
                    for (int i = 0; i < AllExternalMeasurementData.Item1.Length; i++)
                    {
                        if (CompetencyModel.Item2[x][y] == AllExternalMeasurementData.Item1[i])
                        {
                            for (int k = 0; k < AllExternalMeasurementData.Item2[i].Length; k++)
                            {
                                ExternalDataFacets[x][y][k] = Convert.ToInt32(AllExternalMeasurementData.Item2[i][k]);
                            }
                        }
                    }
                }
            }

            return ExternalDataCF;
        }

    }
}
