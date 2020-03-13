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
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    internal static class Logger
    {
        private static ListView lv = null;

        /// <summary>
        /// Initializes this object.
        /// </summary>
        ///
        /// <param name="listView"> The list view control. </param>
        internal static void Init(ListView listView)
        {
            lv = listView;
        }

        /// <summary>
        /// Logs.
        /// </summary>
        ///
        /// <param name="severity"> The severity. </param>
        /// <param name="message">  The message. </param>
        private static void Log(Severity severity, String message)
        {
            lv?.InvokeIfRequired(c =>
            {
                ListViewItem lvi = c.Items
                 .Add(new ListViewItem(new String[] { String.Empty, message }));
                lvi.ImageIndex = (Int32)severity;
                lvi.EnsureVisible();
                //c.Invalidate();
                //c.Refresh();

                //c.Parent.Invalidate();
                //c.Parent.Refresh();
            });

            Thread.Sleep(5);
            Application.DoEvents();
        }

        /// <summary>
        /// Infos.
        /// </summary>
        ///
        /// <param name="message"> The message. </param>
        internal static void Info(String message)
        {
            Log(Severity.Info, message);
        }

        /// <summary>
        /// Warns.
        /// </summary>
        ///
        /// <param name="message"> The message. </param>
        internal static void Warn(String message)
        {
            Log(Severity.Warn, message);
        }

        /// <summary>
        /// Errors.
        /// </summary>
        ///
        /// <param name="message"> The message. </param>
        internal static void Error(String message)
        {
            Log(Severity.Error, message);
        }

        /// <summary>
        /// Values that represent the severity.
        /// </summary>
        public enum Severity : Int32
        {
            /// <summary>
            /// An enum constant representing the Information option.
            /// </summary>
            Info = 0,
            /// <summary>
            /// An enum constant representing the Warning option.
            /// </summary>
            Warn = 1,
            /// <summary>
            /// An enum constant representing the error option.
            /// </summary>
            Error = 2,
        }

    }
}
