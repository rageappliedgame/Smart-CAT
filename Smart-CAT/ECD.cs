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
    using System.IO;
    using System.Linq;
    using JsonPrettyPrinterPlus;
    using Swiss;

    /// <summary>
    /// An ecd.
    /// </summary>
    public static class ECD
    {
        #region Fields

        internal const Char ListSeparator_Colon = ':';
        internal const Char ListSeparator_Comma = ',';
        internal const Char ListSeparator_Tilde = '~';
        internal const Char ListSeparator_VerticalBar = '|';

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        static ECD()
        {
            //
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Competency model.
        /// </summary>
        ///
        /// <param name="filename"> (Optional) Filename of the file. </param>
        ///
        /// <returns>
        /// A (string[],string[][])
        /// </returns>
        internal static Models LoadModelData(String filename = null)
        {
            if (File.Exists(filename))
            {
                return JsonHelper.JsonDeserialize<Models>(File.ReadAllText(filename));
            }
            else
            {
                return new Models();
            }
        }

        /// <summary>
        /// Saves a competency model.
        /// </summary>
        ///
        /// <param name="model">    The model. </param>
        /// <param name="filename"> (Optional) Filename of the file. </param>
        internal static void SaveModelData(Models model, String filename = null)
        {
            if (!String.IsNullOrEmpty(filename))
            {
                File.WriteAllText(

                    filename,
                    JsonHelper
                    .JsonSerializer(model)
                    .PrettyPrintJson());
            }
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        ///
        /// <param name="filename"> (Optional) Filename of the file. </param>
        ///
        /// <returns>
        /// A string[][][].
        /// </returns>
        [Obsolete]
        internal static string[][][] LoadItems(String filename = null)
        {
            using (IniFile ini = new IniFile(filename))
            {
                //! Stores tasks unparsed per competency.
                string[] ItemsPerCompetency = ini.ReadList("ItemModel", "Items", ListSeparator_Colon).ToArray();

                //!
                string[][] ItemsPerFacet = new string[ItemsPerCompetency.Length][];

                //! Stores the tasks structure(s) per competency.
                string[][][] Items = new string[ItemsPerCompetency.Length][][];

                for (int x = 0; x < ItemsPerCompetency.Length; x++)
                {
                    ItemsPerFacet[x] = new string[ItemsPerCompetency[x].Split(ListSeparator_VerticalBar).Select(s => s.Trim()).ToArray().Length];
                    Items[x] = new string[ItemsPerFacet[x].Length][];
                }

                for (int x = 0; x < ItemsPerCompetency.Length; x++)
                {
                    ItemsPerFacet[x] = ItemsPerCompetency[x].Split(ListSeparator_VerticalBar).Select(s => s.Trim()).ToArray();
                    for (int y = 0; y < ItemsPerFacet[x].Length; y++)
                    {
                        Items[x][y] = new string[ItemsPerFacet[x][y].Split(ListSeparator_Comma).Length];
                    }
                }

                for (int x = 0; x < ItemsPerCompetency.Length; x++)
                {
                    for (int y = 0; y < ItemsPerFacet[x].Length; y++)
                    {
                        Items[x][y] = ItemsPerFacet[x][y].Split(ListSeparator_Comma);
                    }
                }

                return Items;
            }
        }

        /// <summary>
        /// Task model.
        /// </summary>
        ///
        /// <param name="filename"> (Optional) Filename of the file. </param>
        ///
        /// <returns>
        /// A string[][][].
        /// </returns>
        [Obsolete]
        internal static string[][][] LoadTaskModel(String filename = null)
        {
            using (IniFile ini = new IniFile(filename))
            {
                //! Stores tasks unparsed per competency .
                string[] TasksPerCompetency = ini.ReadList("TaskModel", "Tasks", ListSeparator_Colon).ToArray();

                //! Stores unparsed tasks per facet .
                string[][] TasksPerFacet = new string[TasksPerCompetency.Length][];

                //! Stores the tasks structure(s) per facet of each delcared competency.
                string[][][] TaskModel = new string[TasksPerCompetency.Length][][];

                for (int x = 0; x < TasksPerCompetency.Length; x++)
                {
                    TasksPerFacet[x] = new string[TasksPerCompetency[x].Split(ListSeparator_VerticalBar).Select(s => s.Trim()).ToArray().Length];
                    TaskModel[x] = new string[TasksPerFacet[x].Length][];
                }

                for (int x = 0; x < TasksPerCompetency.Length; x++)
                {
                    TasksPerFacet[x] = TasksPerCompetency[x].Split(ListSeparator_VerticalBar).Select(s => s.Trim()).ToArray();
                    for (int y = 0; y < TasksPerFacet[x].Length; y++)
                    {
                        TaskModel[x][y] = new string[TasksPerFacet[x][y].Split(ListSeparator_Comma).Length];
                    }
                }

                for (int x = 0; x < TasksPerCompetency.Length; x++)
                {
                    for (int y = 0; y < TasksPerFacet[x].Length; y++)
                    {
                        TaskModel[x][y] = TasksPerFacet[x][y].Split(ListSeparator_Comma);
                    }
                }

                return TaskModel;
            }
        }

        #endregion Methods
    }
}