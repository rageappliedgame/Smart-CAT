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
    using System.Diagnostics;

    using Swiss;

    internal class ConsoleListener : TraceListener
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Swiss.DebugForm.NextGridTraceListener class.
        /// </summary>
        ///
        /// <param name="target"> Target for the. </param>
        public ConsoleListener()
        {
            //ConsoleDialog.ForegroundColor = ConsoleColor.White;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// When overridden in a derived class, writes the specified message to the listener you create
        /// in the derived class.
        /// </summary>
        ///
        /// <param name="message"> A message to write. </param>
        public override void Write(string message)
        {
            //ConsoleDialog.Write(message);
        }

        /// <summary>
        /// When overridden in a derived class, writes a message to the listener you create in the
        /// derived class, followed by a line terminator.
        /// </summary>
        ///
        /// <param name="message"> A message to write. </param>
        public override void WriteLine(string message)
        {
            //ConsoleDialog.Write(message + Environment.NewLine);
        }

        #endregion Methods
    }
}
