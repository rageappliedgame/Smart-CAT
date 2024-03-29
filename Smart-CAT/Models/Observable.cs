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
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
 

    /// <summary>
    /// An observable.
    /// </summary>
    ///
    /// <typeparam name="T"> Generic type parameter. </typeparam>
    [DataContract]
    public class Observable<T>
    {
        #region Fields

        /// <summary>
        /// The items.
        /// </summary>
        [IgnoreDataMember]
        public T[] Items = Array.Empty<T>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        ///
        /// <param name="size"> The size. </param>
        /// <param name="name"> The name. </param>
        public Observable(int size, string name)
        {
            this.ObservableName = name;
            Items = new T[size];
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the length.
        /// </summary>
        ///
        /// <value>
        /// The length.
        /// </value>
        public int Length => Items.Length;

        /// <summary>
        /// Gets or sets the name of the observable.
        /// </summary>
        ///
        /// <value>
        /// The name of the observable.
        /// </value>
        [DataMember]
        public String ObservableName { get; private set; }

        #endregion Properties

        #region Indexers

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
            get { return Items[i]; }
            set { Items[i] = value; }
        }

        #endregion Indexers

        #region Methods

        /// <summary>
        /// Convert this object into a string representation.
        /// </summary>
        ///
        /// <returns>
        /// A string that represents this object.
        /// </returns>
        public override string ToString()
        {
            return JsonHelper.JsonSerializer(this);
        }

        #endregion Methods
    }

    /// <summary>
    /// The Observables.
    /// </summary>
    [DataContract]
    public class Observables : IList<Observable<String>>
    {
        #region Fields

        /// <summary>
        /// The items.
        /// </summary>
        [DataMember]
        public List<Observable<String>> Items = new List<Observable<String>>();

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the number of elements contained in the
        /// <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        ///
        /// <value>
        /// The number of elements contained in the
        /// <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </value>
        public int Count
        {
            get { return Items.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" />
        /// is read-only.
        /// </summary>
        ///
        /// <value>
        /// <see langword="true" /> if the <see cref="T:System.Collections.Generic.ICollection`1" /> is
        /// read-only; otherwise, <see langword="false" />.
        /// </value>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the names.
        /// </summary>
        ///
        /// <value>
        /// The names.
        /// </value>
        public IEnumerable<String> Names => this.Select(p => p.ObservableName);

        #endregion Properties

        #region Indexers

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        ///
        /// <exception cref="T:System.ArgumentOutOfRangeException"> . </exception>
        /// <exception cref="T:System.NotSupportedException">       The property is set and the
        ///                                                         <see cref="T:System.Collections.Generic.IList`1" />
        ///                                                         is read-only. </exception>
        ///
        /// <param name="i"> The zero-based index of the element to get or set. </param>
        ///
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        public Observable<String> this[int i]
        {
            get { return Items[i]; }
            set { Items[i] = value; }
        }

        #endregion Indexers

        #region Methods

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        ///
        /// <exception cref="T:System.NotSupportedException"> The
        ///                                                   <see cref="T:System.Collections.Generic.ICollection`1" />
        ///                                                   is read-only. </exception>
        ///
        /// <param name="item"> The object to add to the
        ///                     <see cref="T:System.Collections.Generic.ICollection`1" />. </param>
        public void Add(Observable<String> item)
        {
            Items.Add(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        ///
        /// <exception cref="T:System.NotSupportedException"> The
        ///                                                   <see cref="T:System.Collections.Generic.ICollection`1" />
        ///                                                   is read-only. </exception>
        public void Clear()
        {
            Items.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a
        /// specific value.
        /// </summary>
        ///
        /// <param name="item"> The object to locate in the
        ///                     <see cref="T:System.Collections.Generic.ICollection`1" />. </param>
        ///
        /// <returns>
        /// <see langword="true" /> if <paramref name="item" /> is found in the
        /// <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise,
        /// <see langword="false" />.
        /// </returns>
        public bool Contains(Observable<String> item)
        {
            return Items.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an
        /// <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// 
        /// </summary>
        ///
        /// <exception cref="T:System.ArgumentNullException">       . </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"> . </exception>
        /// <exception cref="T:System.ArgumentException">           The number of elements in the source
        ///                                                         <see cref="T:System.Collections.Generic.ICollection`1" />
        ///                                                         is greater than the available space
        ///                                                         from <paramref name="arrayIndex" />
        ///                                                         to the end of the destination
        ///                                                         <paramref name="array" />. </exception>
        ///
        /// <param name="array">      The one-dimensional <see cref="T:System.Array" /> that is the
        ///                           destination of the elements copied from
        ///                           <see cref="T:System.Collections.Generic.ICollection`1" />. The
        ///                           <see cref="T:System.Array" /> must have zero-based indexing. </param>
        /// <param name="arrayIndex"> The zero-based index in <paramref name="array" /> at which copying
        ///                           begins. </param>
        public void CopyTo(Observable<String>[] array, int arrayIndex)
        {
            Items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        ///
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Observable<String>> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        ///
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through
        /// the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Items).GetEnumerator();
        }

        /// <summary>
        /// Determines the index of a specific item in the
        /// <see cref="T:System.Collections.Generic.IList`1" />.
        /// </summary>
        ///
        /// <param name="item"> The object to locate in the
        ///                     <see cref="T:System.Collections.Generic.IList`1" />. </param>
        ///
        /// <returns>
        /// The index of <paramref name="item" /> if found in the list; otherwise, -1.
        /// </returns>
        public int IndexOf(Observable<String> item)
        {
            return Items.IndexOf(item);
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1" /> at the specified
        /// index.
        /// </summary>
        ///
        /// <exception cref="T:System.ArgumentOutOfRangeException"> . </exception>
        /// <exception cref="T:System.NotSupportedException">       The
        ///                                                         <see cref="T:System.Collections.Generic.IList`1" />
        ///                                                         is read-only. </exception>
        ///
        /// <param name="index"> The zero-based index at which <paramref name="item" /> should be
        ///                      inserted. </param>
        /// <param name="item">  The object to insert into the
        ///                      <see cref="T:System.Collections.Generic.IList`1" />. </param>
        public void Insert(int index, Observable<String> item)
        {
            Items.Insert(index, item);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the
        /// <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        ///
        /// <exception cref="T:System.NotSupportedException"> The
        ///                                                   <see cref="T:System.Collections.Generic.ICollection`1" />
        ///                                                   is read-only. </exception>
        ///
        /// <param name="item"> The object to remove from the
        ///                     <see cref="T:System.Collections.Generic.ICollection`1" />. </param>
        ///
        /// <returns>
        /// <see langword="true" /> if <paramref name="item" /> was successfully removed from the
        /// <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise,
        /// <see langword="false" />. This method also returns <see langword="false" /> if
        /// <paramref name="item" /> is not found in the original
        /// <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        public bool Remove(Observable<String> item)
        {
            return Items.Remove(item);
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1" /> item at the specified index.
        /// </summary>
        ///
        /// <exception cref="T:System.ArgumentOutOfRangeException"> . </exception>
        /// <exception cref="T:System.NotSupportedException">       The
        ///                                                         <see cref="T:System.Collections.Generic.IList`1" />
        ///                                                         is read-only. </exception>
        ///
        /// <param name="index"> The zero-based index of the item to remove. </param>
        public void RemoveAt(int index)
        {
            Items.RemoveAt(index);
        }

        #endregion Methods
    }
}