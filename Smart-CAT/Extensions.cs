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
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public static class Extensions
    {
        #region Methods

        /// <summary>
        /// A T extension method that gets a description.
        /// </summary>
        ///
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="value"> The value to act on. </param>
        ///
        /// <returns>
        /// The description.
        /// </returns>
        public static string GetDescription<T>(this T value)
            where T : struct
        {
            CheckIsEnum<T>(false);
            string name = Enum.GetName(typeof(T), value);
            if (name != null)
            {
                FieldInfo field = typeof(T).GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Usage: new AnEnum().GetDescriptions
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string[] GetDescriptions(this Enum e)
        {
            String[] values = Enum.GetNames(e.GetType());
            String[] Result = new String[values.Length];

            for (Int32 i = 0; i < values.Length; i++)
            {
                Result[i] = values[i];

                FieldInfo field = e.GetType().GetField(values[i]);
                if (field != null)
                {
                    DescriptionAttribute attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        Result[i] = attr.Description;
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// Executes if required delegate on a different thread, and waits for the result.
        /// </summary>
        ///
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="obj"> The object. </param>
        public delegate void InvokeIfRequiredDelegate<T>(T obj)
            where T : ISynchronizeInvoke;

        /// <summary>
        /// A T extension method that executes if required on a different thread, and waits for the
        /// result.
        /// </summary>
        ///
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="obj">    The obj to act on. </param>
        /// <param name="action"> The action. </param>
        public static void InvokeIfRequired<T>(this T obj, InvokeIfRequiredDelegate<T> action)
            where T : ISynchronizeInvoke
        {
            if (obj.InvokeRequired)
            {
                obj.Invoke(action, new object[] { obj });
            }
            else
            {
                action(obj);
            }
        }

        /// <summary>
        /// Check is enum.
        /// </summary>
        ///
        /// <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
        ///                                      illegal values. </exception>
        ///
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="withFlags"> True to with flags. </param>
        private static void CheckIsEnum<T>(bool withFlags)
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException(string.Format("Type '{0}' is not an enum", typeof(T).FullName));
            if (withFlags && !Attribute.IsDefined(typeof(T), typeof(FlagsAttribute)))
                throw new ArgumentException(string.Format("Type '{0}' doesn't have the 'Flags' attribute", typeof(T).FullName));
        }

        /// <summary>
        /// Gets current method.
        /// </summary>
        ///
        /// <returns>
        /// The current method.
        /// </returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentMethod()
        {
            return new StackTrace().GetFrame(1).GetMethod().Name;
        }

        /// <summary>
        /// An IEnumerable&lt;T&gt; extension method that join 2 string.
        /// </summary>
        ///
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="values">    The values to act on. </param>
        /// <param name="separator"> The separator. </param>
        ///
        /// <returns>
        /// A String.
        /// </returns>
        public static String Join2String<T>(this IEnumerable<T> values, String separator)
        {
            return String.Join(separator, values);
        }

        /// <summary>
        /// An IEnumerable&lt;T&gt; extension method that join 2 string.
        /// </summary>
        ///
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="values">    The values to act on. </param>
        /// <param name="separator"> The separator. </param>
        ///
        /// <returns>
        /// A String.
        /// </returns>
        public static String Join2String<T>(this IEnumerable<T> values, Char separator)
        {
            return String.Join(separator.ToString(), values);
        }

        #endregion Methods
    }
}