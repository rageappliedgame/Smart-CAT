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
namespace Swiss
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Reflection;
    using System.Windows.Forms;

    /// <summary>
    /// Collection of enum items.
    /// </summary>
    public class EnumItemCollection : ICollection, IEnumerable, IList
    {
        #region Fields

        /// <summary>
        /// Type of the enum.
        /// </summary>
        Type enumType;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        ///
        /// <param name="enumType"> Type of the enum. </param>
        public EnumItemCollection(Type enumType)
        {
            if (enumType.IsEnum)
                this.enumType = enumType;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection" />.
        /// </summary>
        ///
        /// <value>
        /// The number of elements contained in the <see cref="T:System.Collections.ICollection" />.
        /// </value>
        public int Count
        {
            get
            {
                return enumType.GetFields().Length - 1;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.IList" /> has a fixed
        /// size.
        /// </summary>
        ///
        /// <value>
        /// <see langword="true" /> if the <see cref="T:System.Collections.IList" /> has a fixed size;
        /// otherwise, <see langword="false" />.
        /// </value>
        public bool IsFixedSize
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.IList" /> is read-only.
        /// </summary>
        ///
        /// <value>
        /// <see langword="true" /> if the <see cref="T:System.Collections.IList" /> is read-only;
        /// otherwise, <see langword="false" />.
        /// </value>
        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" />
        /// is synchronized (thread safe).
        /// </summary>
        ///
        /// <value>
        /// <see langword="true" /> if access to the <see cref="T:System.Collections.ICollection" /> is
        /// synchronized (thread safe); otherwise, <see langword="false" />.
        /// </value>
        public bool IsSynchronized
        {
            get
            {
                // TODO:  Add EnumItemCollection.IsSynchronized getter implementation
                return false;
            }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the
        /// <see cref="T:System.Collections.ICollection" />.
        /// </summary>
        ///
        /// <value>
        /// An object that can be used to synchronize access to the
        /// <see cref="T:System.Collections.ICollection" />.
        /// </value>
        public object SyncRoot
        {
            get
            {
                // TODO:  Add EnumItemCollection.SyncRoot getter implementation
                return null;
            }
        }

        #endregion Properties

        #region Indexers

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        ///
        /// <exception cref="T:System.ArgumentOutOfRangeException"> . </exception>
        /// <exception cref="T:System.NotSupportedException">       The property is set and the
        ///                                                         <see cref="T:System.Collections.IList" />
        ///                                                         is read-only. </exception>
        ///
        /// <param name="index"> The zero-based index of the element to get or set. </param>
        ///
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        public object this[int index]
        {
            get
            {
                //! The next statement returns one extra element (RtFieldInfo) int between the MtFieldInfo's
                //! that is not always located at index 0 (hence the old + 1 that failed sometimes).
                //! If the extra RtFieldInfo was not element 1 the code crashed.
                //FieldInfo[] fields = this.enumType.GetFields();

                String[] Names = this.enumType.GetEnumNames();
                Array Values = this.enumType.GetEnumValues();

                EnumPair Result = new EnumPair(
                    Names[index],
                    Values.GetValue(index),
                    GetEnumDescription(this.enumType.GetField(Names[index])));

                //return new EnumPair(
                //fields[index + 1].Name,
                //(Int32)fields[index + 1].GetValue(this),
                //GetEnumDescription(fields[index + 1]));

                return Result;
            }
            set
            {
                // TODO:  Add EnumItemCollection.this setter implementation
            }
        }

        #endregion Indexers

        #region Methods

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.IList" />.
        /// </summary>
        ///
        /// <exception cref="T:System.NotSupportedException"> The <see cref="T:System.Collections.IList" />
        ///                                                   is read-only.-or- The
        ///                                                   <see cref="T:System.Collections.IList" />
        ///                                                   has a fixed size. </exception>
        ///
        /// <param name="value"> The object to add to the <see cref="T:System.Collections.IList" />. </param>
        ///
        /// <returns>
        /// The position into which the new element was inserted, or -1 to indicate that the item was not
        /// inserted into the collection.
        /// </returns>
        public int Add(object value)
        {
            // TODO:  Add EnumItemCollection.Add implementation
            return 0;
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.IList" />.
        /// </summary>
        ///
        /// <exception cref="T:System.NotSupportedException"> The <see cref="T:System.Collections.IList" />
        ///                                                   is read-only. </exception>
        public void Clear()
        {
            // TODO:  Add EnumItemCollection.Clear implementation
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.IList" /> contains a specific value.
        /// </summary>
        ///
        /// <param name="value"> The object to locate in the <see cref="T:System.Collections.IList" />. </param>
        ///
        /// <returns>
        /// <see langword="true" /> if the <see cref="T:System.Object" /> is found in the
        /// <see cref="T:System.Collections.IList" />; otherwise, <see langword="false" />.
        /// </returns>
        public bool Contains(object value)
        {
            // TODO:  Add EnumItemCollection.Contains implementation
            return false;
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an
        /// <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        ///
        /// <exception cref="T:System.ArgumentNullException">       . </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"> . </exception>
        /// <exception cref="T:System.ArgumentException">           . </exception>
        ///
        /// <param name="array"> The one-dimensional <see cref="T:System.Array" /> that is the destination
        ///                      of the elements copied from
        ///                      <see cref="T:System.Collections.ICollection" />. The
        ///                      <see cref="T:System.Array" /> must have zero-based indexing. </param>
        /// <param name="index"> The zero-based index in <paramref name="array" /> at which copying
        ///                      begins. </param>
        public void CopyTo(Array array, int index)
        {
            // TODO:  Add EnumItemCollection.CopyTo implementation
        }

        /// <summary>
        /// Description from name.
        /// </summary>
        ///
        /// <param name="name"> The name. </param>
        ///
        /// <returns>
        /// A String.
        /// </returns>
        public String DescriptionFromName(string name)
        {
            FieldInfo[] fields = this.enumType.GetFields();

            foreach (FieldInfo field in fields)
            {
                if (name == field.Name)
                {
                    return GetEnumDescription(field);
                }
            }
            return String.Empty;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        ///
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through
        /// the collection.
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            return new EnumItemEnumerator(enumType.GetFields());
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.IList" />.
        /// </summary>
        ///
        /// <param name="value"> The object to locate in the <see cref="T:System.Collections.IList" />. </param>
        ///
        /// <returns>
        /// The index of <paramref name="value" /> if found in the list; otherwise, -1.
        /// </returns>
        public int IndexOf(object value)
        {
            // TODO:  Add EnumItemCollection.IndexOf implementation
            return 0;
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.IList" /> at the specified index.
        /// </summary>
        ///
        /// <exception cref="T:System.ArgumentOutOfRangeException"> . </exception>
        /// <exception cref="T:System.NotSupportedException">       The
        ///                                                         <see cref="T:System.Collections.IList" />
        ///                                                         is read-only.-or- The
        ///                                                         <see cref="T:System.Collections.IList" />
        ///                                                         has a fixed size. </exception>
        /// <exception cref="T:System.NullReferenceException">      . </exception>
        ///
        /// <param name="index"> The zero-based index at which <paramref name="value" /> should be
        ///                      inserted. </param>
        /// <param name="value"> The object to insert into the <see cref="T:System.Collections.IList" />. </param>
        public void Insert(int index, object value)
        {
            // TODO:  Add EnumItemCollection.Insert implementation
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the
        /// <see cref="T:System.Collections.IList" />.
        /// </summary>
        ///
        /// <exception cref="T:System.NotSupportedException"> The <see cref="T:System.Collections.IList" />
        ///                                                   is read-only.-or- The
        ///                                                   <see cref="T:System.Collections.IList" />
        ///                                                   has a fixed size. </exception>
        ///
        /// <param name="value"> The object to remove from the <see cref="T:System.Collections.IList" />. </param>
        public void Remove(object value)
        {
            // TODO:  Add EnumItemCollection.Remove implementation
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.IList" /> item at the specified index.
        /// </summary>
        ///
        /// <exception cref="T:System.ArgumentOutOfRangeException"> . </exception>
        /// <exception cref="T:System.NotSupportedException">       The
        ///                                                         <see cref="T:System.Collections.IList" />
        ///                                                         is read-only.-or- The
        ///                                                         <see cref="T:System.Collections.IList" />
        ///                                                         has a fixed size. </exception>
        ///
        /// <param name="index"> The zero-based index of the item to remove. </param>
        public void RemoveAt(int index)
        {
            // TODO:  Add EnumItemCollection.RemoveAt implementation
        }

        /// <summary>
        /// Value from name.
        /// </summary>
        ///
        /// <param name="name"> The name. </param>
        ///
        /// <returns>
        /// An Int32.
        /// </returns>
        public Int32 ValueFromName(string name)
        {
            FieldInfo[] fields = this.enumType.GetFields();

            foreach (FieldInfo field in fields)
                if (name == field.Name)
                    return (Int32)field.GetValue(null);

            return -1;
        }

        /// <summary>
        /// Gets enum description.
        /// </summary>
        ///
        /// <param name="fi"> The fi. </param>
        ///
        /// <returns>
        /// The enum description.
        /// </returns>
        internal static String GetEnumDescription(FieldInfo fi)
        {
            DescriptionAttribute attr = Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute)) as DescriptionAttribute;

            return attr != null ? attr.Description : String.Empty;
        }

        #endregion Methods

        #region Nested Types

        /// <summary>
        /// An enum item enumerator.
        /// </summary>
        public class EnumItemEnumerator : IEnumerator
        {
            #region Fields

            /// <summary>
            /// Information describing the field.
            /// </summary>
            private FieldInfo[] fieldInfo;

            /// <summary>
            /// Zero-based index of the.
            /// </summary>
            private int index;

            #endregion Fields

            #region Constructors

            /// <summary>
            /// Constructor.
            /// </summary>
            ///
            /// <param name="fieldInfo"> Information describing the field. </param>
            public EnumItemEnumerator(FieldInfo[] fieldInfo)
            {
                this.fieldInfo = fieldInfo;
                this.index = 0;
            }

            #endregion Constructors

            #region Properties

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            ///
            /// <value>
            /// The element in the collection at the current position of the enumerator.
            /// </value>
            public object Current
            {
                get
                {
                    return new EnumPair(
                this.fieldInfo[this.index].Name,
                (Int32)fieldInfo[index].GetValue(null),
                GetEnumDescription(this.fieldInfo[index]));
                }
            }

            #endregion Properties

            #region Methods

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            ///
            /// <exception cref="T:System.InvalidOperationException"> The collection was modified after the
            ///                                                       enumerator was created. </exception>
            ///
            /// <returns>
            /// <see langword="true" /> if the enumerator was successfully advanced to the next element;
            /// <see langword="false" /> if the enumerator has passed the end of the collection.
            /// </returns>
            public bool MoveNext()
            {
                if (this.index < this.fieldInfo.Length - 1)
                {
                    this.index++;
                    return true;
                }
                else
                    return false;
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the
            /// collection.
            /// </summary>
            ///
            /// <exception cref="T:System.InvalidOperationException"> The collection was modified after the
            ///                                                       enumerator was created. </exception>
            public void Reset()
            {
                //Note: It's an index and not the enum value.
                this.index = 0;
            }

            #endregion Methods
        }

        #endregion Nested Types
    }

    /// <summary>
    /// See http://www.codeproject.com/KB/cs/enumdatasource.aspx.
    /// </summary>
    public class EnumPair
    {
        #region Fields

        /// <summary>
        /// The description.
        /// </summary>
        public String Description;

        /// <summary>
        /// The name.
        /// </summary>
        public string Name;

        /// <summary>
        /// The value.
        /// </summary>
        public Object Value;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        ///
        /// <param name="Name">        The name. </param>
        /// <param name="Value">       The value. </param>
        /// <param name="Description"> (Optional) The description. </param>
        public EnumPair(string Name, Object Value, String Description = "")
        {
            this.Name = Name;
            this.Value = Value;
            this.Description = Description;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Utility Method to get the SelectedItem Value of a ComboBox.
        /// </summary>
        ///
        /// <typeparam name="T"> The EnumPair's Value Type. </typeparam>
        /// <param name="cb">     The Combobox. </param>
        /// <param name="defval"> The Default Value. </param>
        ///
        /// <returns>
        /// The SelectedItem Value or the Default Value.
        /// </returns>
        public static T EnumPairGetter<T>(ComboBox cb, T defval)
        {
            if (cb.SelectedItem != null)
            {
                return (T)((EnumPair)cb.SelectedItem).Value;
            }
            else
            {
                return defval;
            }
        }

        /// <summary>
        /// Utility Method to set the SelectedItem Value of a ComboBox.
        /// </summary>
        ///
        /// <typeparam name="T"> The EnumPair's Value Type. </typeparam>
        /// <param name="cb">    The Combobox. </param>
        /// <param name="value"> The Value. </param>
        public static void EnumPairSetter<T>(ComboBox cb, T value)
        {
            foreach (EnumPair ep in cb.Items)
            {
                if (((T)(ep.Value)).ToString().Equals(value.ToString()))
                {
                    cb.SelectedItem = ep;
                    break;
                }
            }
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
            if (String.IsNullOrEmpty(Description))
            {
                return Name;
            }
            else
            {
                return Description;
            }
        }

        #endregion Methods
    }
}