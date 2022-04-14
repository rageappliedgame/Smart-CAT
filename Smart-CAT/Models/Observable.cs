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
    using System.Threading.Tasks;

    /// <summary>
    /// The Observables.
    /// </summary>
    ///
    /// <typeparam name="T"> Generic type parameter. </typeparam>
    public class Observables<T> : List<Observable<T>>
    {
#warning Why is this a String and not a Double? Converted in BayesNet.LoadInstances()

        public IEnumerable<String> Names => this.Select(p => p.ObservableName);
    }

    public class Observable<T>
    {
        private T[] arr = Array.Empty<T>();

        public String ObservableName { get; private set; }

        /// <summary>
        /// Indexer to get or set items within this collection using array index syntax.
        /// </summary>
        ///
        /// <param name="i"> Zero-based index of the entry to access. </param>
        ///
        /// <returns>
        /// The indexed item.
        /// </returns>
        public T this[int i]
        {
            get { return arr[i]; }
            set { arr[i] = value; }
        }

        /// <summary>
        /// Gets the length.
        /// </summary>
        ///
        /// <value>
        /// The length.
        /// </value>
        public int Length => arr.Length;

        /// <summary>
        /// Constructor.
        /// </summary>
        ///
        /// <param name="size"> The size. </param>
        public Observable(int size, string name)
        {
            this.ObservableName = name;
            arr = new T[size];
        }
    }
}
