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
    using System.ComponentModel;

    /// <summary>
    /// Validation and Verification options.
    /// 
    /// <see cref="BayesNet.CorrelationAnalysisFC"/>
    /// </summary>
    public class VandVOptions
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        ///
        /// <param name="SpearmansRho"> The spearmans rho. </param>
        /// <param name="Cronbachsalpha"> The cronbachsalpha. </param>
        public VandVOptions(Double SpearmansRho, Double Cronbachsalpha)
        {
            this.SpearmansRho = SpearmansRho;
            this.Cronbachsalpha = Cronbachsalpha;
        }

        //! See https://stackoverflow.com/questions/15051298/list-of-propertygrid-attributes
        //[DisplayName(...)]
        //[Description(...)]
        //[Category(...)]
        //[TypeConverter(...)]
        //[ReadOnly(...)]
        //[Browsable(...)]
        //[DefaultValue(...)]
        //[Editor(...)]
        //[RefreshProperties(...)]
        //[NotifyParentProperty(...)]
        //[Mergable(...)]
        // 
        //Some other things are detected by patterns such as the presence of a 
        //ShouldSerialize{name} or Reset { name } method?

        /// <summary>
        /// Spearman's Rho.
        /// </summary>
        ///
        /// <value>
        /// The spearmansrho.
        /// </value>
        [Category("Validation and Verification")]
        [Description("Spearman's Rho")]
        [DisplayName("Spearman's Rho")]
        public Double SpearmansRho { get; private set; }

        /// <summary>
        /// Cronbach's alpha.
        /// </summary>
        ///
        /// <value>
        /// The cronbachsalpha.
        /// </value>
        [Category("Validation and Verification")]
        [Description("Cronbach's alpha")]
        [DisplayName("Cronbach's alpha")]
        public Double Cronbachsalpha { get; private set; }

    }
}
