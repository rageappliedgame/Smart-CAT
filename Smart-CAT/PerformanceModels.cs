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

    /// <summary>
    /// Interface for performance.
    /// </summary>
    public interface IPerformance
    {
        /// <summary>
        /// Gets the competency.
        /// </summary>
        ///
        /// <value>
        /// The competency.
        /// </value>
        String Competency { get; }

        /// <summary>
        /// Gets the facet.
        /// </summary>
        ///
        /// <value>
        /// The facet.
        /// </value>
        String Facet { get; }
    }

    public class NaiveBayesPerformanceModel : IPerformance
    {
        /// <summary>
        /// Gets or sets the competency.
        /// </summary>
        ///
        /// <value>
        /// The competency.
        /// </value>
        public string Competency { get; private set; }

        /// <summary>
        /// Gets or sets the facet.
        /// </summary>
        ///
        /// <value>
        /// The facet.
        /// </value>
        public string Facet { get; private set; }

        /// <summary>
        /// Gets or sets the accuracy.
        /// </summary>
        ///
        /// <value>
        /// The accuracy.
        /// </value>
        public double Accuracy { get; private set; }

        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        ///
        /// <value>
        /// The error.
        /// </value>
        public double Error { get; private set; }

        /// <summary>
        /// Gets or sets the kappa.
        /// </summary>
        ///
        /// <value>
        /// The kappa.
        /// </value>
        public double Kappa { get; private set; }

        /// <summary>
        /// Gets or sets the mae.
        /// </summary>
        ///
        /// <value>
        /// The mae.
        /// </value>
        public double MAE { get; private set; }

        /// <summary>
        /// Gets or sets the rmse.
        /// </summary>
        ///
        /// <value>
        /// The rmse.
        /// </value>
        public double RMSE { get; private set; }

        /// <summary>
        /// Gets or sets the rae.
        /// </summary>
        ///
        /// <value>
        /// The rae.
        /// </value>
        public double RAE { get; private set; }

        /// <summary>
        /// Gets or sets the rrse.
        /// </summary>
        ///
        /// <value>
        /// The rrse.
        /// </value>
        public double RRSE { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        ///
        /// <param name="competency"> The competency. </param>
        /// <param name="facet">      The facet. </param>
        /// <param name="accuracy">   The accuracy. </param>
        /// <param name="error">      The error. </param>
        /// <param name="kappa">      The kappa. </param>
        /// <param name="mAE">        The mae. </param>
        /// <param name="rMSE">       The rmse. </param>
        /// <param name="rAE">        The rae. </param>
        /// <param name="rRSE">       The rrse. </param>
        ///
        /// ### <param name="rmSE"> The rmse. </param>
        public NaiveBayesPerformanceModel(string competency, string facet, double accuracy, double error, double kappa, double mAE, double rMSE, double rAE, double rRSE)
        {
            this.Competency = competency;
            this.Facet = facet;

            this.Accuracy = Math.Round(accuracy, 4);
            this.Error = Math.Round(error, 4);
            this.Kappa = Math.Round(kappa, 4);

            this.MAE = Math.Round(mAE, 4);
            this.RMSE = Math.Round(rMSE, 2);

            this.RAE = Math.Round(rAE, 4);      //! %
            this.RRSE = Math.Round(rRSE, 4);    //! %
        }
    }

    /// <summary>
    /// A data Model for the decision trees performance.
    /// </summary>
    public class DecisionTreesPerformanceModel : IPerformance
    {
        /// <summary>
        /// Gets or sets the competency.
        /// </summary>
        ///
        /// <value>
        /// The competency.
        /// </value>
        public string Competency { get; private set; }

        /// <summary>
        /// Gets or sets the facet.
        /// </summary>
        ///
        /// <value>
        /// The facet.
        /// </value>
        public string Facet { get; private set; }

        /// <summary>
        /// Gets or sets the accuracy.
        /// </summary>
        ///
        /// <value>
        /// The accuracy.
        /// </value>
        public double Accuracy { get; private set; }

        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        ///
        /// <value>
        /// The error.
        /// </value>
        public double Error { get; private set; }

        /// <summary>
        /// Gets or sets the kappa.
        /// </summary>
        ///
        /// <value>
        /// The kappa.
        /// </value>
        public double Kappa { get; private set; }

        /// <summary>
        /// Gets or sets the mae.
        /// </summary>
        ///
        /// <value>
        /// The mae.
        /// </value>
        public double MAE { get; private set; }

        /// <summary>
        /// Gets or sets the rmse.
        /// </summary>
        ///
        /// <value>
        /// The rmse.
        /// </value>
        public double RMSE { get; private set; }

        /// <summary>
        /// Gets or sets the rae.
        /// </summary>
        ///
        /// <value>
        /// The rae.
        /// </value>
        public double RAE { get; private set; }

        /// <summary>
        /// Gets or sets the rrse.
        /// </summary>
        ///
        /// <value>
        /// The rrse.
        /// </value>
        public double RRSE { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        ///
        /// <param name="competency"> The competency. </param>
        /// <param name="facet">      The facet. </param>
        /// <param name="accuracy">   The accuracy. </param>
        /// <param name="error">      The error. </param>
        /// <param name="kappa">      The kappa. </param>
        /// <param name="mAE">        The mae. </param>
        /// <param name="rMSE">       The rmse. </param>
        /// <param name="rAE">        The rae. </param>
        /// <param name="rRSE">       The rrse. </param>
        public DecisionTreesPerformanceModel(string competency, string facet, double accuracy, double error, double kappa, double mAE, double rMSE, double rAE, double rRSE)
        {
            this.Competency = competency;
            this.Facet = facet;

            this.Accuracy = Math.Round(accuracy, 4);
            this.Error = Math.Round(error, 4);
            this.Kappa = Math.Round(kappa, 4);

            this.MAE = Math.Round(mAE, 4);
            this.RMSE = Math.Round(rMSE, 2);

            this.RAE = Math.Round(rAE, 4);      //! %
            this.RRSE = Math.Round(rRSE, 4);    //! %
        }
    }
}
