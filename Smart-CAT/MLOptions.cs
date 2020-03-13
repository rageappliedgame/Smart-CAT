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
    using System.ComponentModel;

    /// <summary>
    /// Values that represent Miles algorithms.
    /// </summary>
    public enum MLAlgorithms
    {
        /// <summary>
        /// An enum constant representing the naive bayes option.
        /// </summary>
        [Description("Naive Bayes Network")]
        NaiveBayes,

        /// <summary>
        /// An enum constant representing the decision trees option.
        /// </summary>
        [Description("Decision Trees (C45)")]
        DecisionTrees,
    }

    public enum MLClustering
    {
        /// <summary>
        /// An enum constant representing the kmeans option.
        /// </summary>
        [Description("KMeans")]
        KMeans,
    }

    /// <summary>
    /// Interface for iml options.
    /// </summary>
    public interface IMLOptions
    {
        /// <summary>
        /// Gets the algorithm.
        /// </summary>
        ///
        /// <value>
        /// The algorithm.
        /// </value>
        MLAlgorithms Algorithm
        {
            get;
        }

        /// <summary>
        /// Gets the clustering.
        /// </summary>
        ///
        /// <value>
        /// The clustering.
        /// </value>
        MLClustering Clustering
        {
            get;
        }
    }

    /// <summary>
    /// The naive bayes algorithm.
    /// </summary>
    public class NaiveBayesAlgorithm : IMLOptions
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public NaiveBayesAlgorithm()
        {
            //
        }

        /// <summary>
        /// Gets or sets the tolerance.
        /// </summary>
        ///
        /// <value>
        /// The tolerance.
        /// </value>
        [NotifyParentProperty(true)]
        [Category("Algorithm Specific")]
        [Description("The Tolerance for the Naive Bayes ML.")]
        [DefaultValue(0.05)]
        public Double Tolerance { get; set; } = 0.05;

        /// <summary>
        /// Splits the dataset for the Bayesian Network to (PercentSplit)% training and (100-
        /// PercentSplit)% testing.
        /// </summary>
        ///
        /// <value>
        /// The percent split.
        /// </value>
        [Category("Algorithm Specific")]
        [Description("Splits the dataset for the Bayesian Network to (PercentSplit)% for training and (100-PercentSplit)% for testing.")]
        [DisplayName("Split Percentage")]
        [DefaultValue(65)]
        public int PercentSplit { get; set; } = 65;


        /// <summary>
        /// Gets the algorithm.
        /// </summary>
        ///
        /// <value>
        /// The algorithm.
        /// </value>
        [Category("Common")]
        [Description("The Machine Learning Algorithm.")]
        public MLAlgorithms Algorithm => MLAlgorithms.NaiveBayes;

        /// <summary>
        /// Gets the clustering.
        /// </summary>
        ///
        /// <value>
        /// The clustering.
        /// </value>
        [Category("Common")]
        [Description("The Machine Learning Clustering Algorithm.")]
        public MLClustering Clustering => MLClustering.KMeans;
    }

    /// <summary>
    /// A decision trees algorithm.
    /// </summary>
    public class DecisionTreesAlgorithm : IMLOptions
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public DecisionTreesAlgorithm()
        {
            //
        }

        /// <summary>
        /// Gets the algorithm.
        /// </summary>
        ///
        /// <value>
        /// The algorithm.
        /// </value>
        [Category("Common")]
        [Description("The Machine Learning Algorithm.")]
        public MLAlgorithms Algorithm => MLAlgorithms.DecisionTrees;

        /// <summary>
        /// Gets the clustering.
        /// </summary>
        ///
        /// <value>
        /// The clustering.
        /// </value>
        [Category("Common")]
        [Description("The Machine Learning Clustering Algorithm.")]
        public MLClustering Clustering => MLClustering.KMeans;

        /// <summary>
        /// Gets or sets the tolerance.
        /// </summary>
        ///
        /// <value>
        /// The tolerance.
        /// </value>
        [NotifyParentProperty(true)]
        [Category("Algorithm Specific")]
        [Description("The Tolerance for the Naive Bayes ML.")]
        [DefaultValue(0.05)]
        public Double Tolerance { get; set; } = 0.05;

        /// <summary>
        /// Splits the dataset for the Decision Trees to (PercentSplit)% training and (100-PercentSplit)% testing.
        /// </summary>
        [Category("Algorithm Specific")]
        [Description("Splits the dataset for the Decision Trees to (PercentSplit)% for training and (100-PercentSplit)% for testing.")]
        [DisplayName("Split Percentage")]
        [DefaultValue(65)]
        public int PercentSplit { get; set; } = 65;

    }
}
