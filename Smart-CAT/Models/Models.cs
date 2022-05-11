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
    using System.Linq;

    /// <summary>
    /// A models.
    /// </summary>
    public class Models : IDisposable
    {
        #region Fields

        /// <summary>
        /// The competencies.
        /// </summary>
        public Competencies competencies;

        /// <summary>
        /// The observables.
        /// </summary>
        public IEnumerable<String> observables;

        /// <summary>
        /// The unicompetencies.
        /// </summary>
        public UniCompetencies unicompetencies;

        /// <summary>
        /// True to disposed value.
        /// </summary>
        private bool disposedValue;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Models()
        {
            observables = new List<string>();
            competencies = new Competencies();
            unicompetencies = new UniCompetencies();
        }

        #endregion Constructors

        #region Methods

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Models()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
        /// resources.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        ///
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return JsonHelper.JsonSerializer(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the Models and optionally releases the managed
        /// resources.
        /// </summary>
        ///
        /// <param name="disposing"> True to release both managed and unmanaged resources; false to
        ///                          release only unmanaged resources. </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        #endregion Methods
    }
}