﻿/*
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
    /// A Grouped Combobox Item.
    /// </summary>
    public class GCI
    {
        /// <summary>
        /// Gets or sets the group.
        /// </summary>
        ///
        /// <value>
        /// The group.
        /// </value>
        public String Group
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        ///
        /// <value>
        /// The value.
        /// </value>
        public String Value
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the display.
        /// </summary>
        ///
        /// <value>
        /// The display.
        /// </value>
        public String Display
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the tool tip.
        /// </summary>
        ///
        /// <value>
        /// The tool tip.
        /// </value>
        public String ToolTip
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether it's a competency.
        /// </summary>
        ///
        /// <value>
        /// True if a competency, false if not.
        /// </value>
        public Boolean IsCompetency
        {
            get;
            set;
        }
    }
}
