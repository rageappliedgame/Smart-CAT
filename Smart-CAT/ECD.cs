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
    using System.Linq;

    using Swiss;

    /// <summary>
    /// An ecd.
    /// </summary>
    public static class ECD
    {
        #region Fields

        internal static readonly Char ListSeparator_Colon = ':';
        internal static readonly Char ListSeparator_Comma = ',';
        internal static readonly Char ListSeparator_Tilde = '~';
        internal static readonly Char ListSeparator_VerticalBar = '|';

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        static ECD()
        {
            /*
            //! Generate Default Data...
            //
            //! Saved as %APPDATA%\Smart-CAT\Smart-CAT.ini.
            //
            using (IniFile ini = new IniFile(null))
            {
                //! Example:
                // 
                //[CompetencyModel]
                //Competencies=Competency1, Competency2
                //Facets=Facet1C1,Facet2C1,Facet3C1: Facet1C2,Facet2C2

                //[EvidenceModel]
                //Statistical Submodel=Obs1C1,Obs2C1,Obs3C1|Obs4C1|Obs5C1: Obs1C2,Obs2C2,Obs3C2|Obs4C2

                //[ObservableModel]
                //Observables=Obs1C1, Obs2C1, Obs3C1, Obs4C1, Obs5C1, Obs6C1, Obs7C1, Obs1C2, Obs2C2, Obs3C2, Obs4C2

                //[UniCompetencyModel]
                //UniCompetencies=UniCompetency0, UniCompetency1, UniCompetency2

                //[UniEvidenceModel]
                //UniStatistical Submodel = Obs1C1, Obs2C1, Obs3C1: Obs4C1: Obs5C1

                //! 1) CompetencyModel.
                //
                if (!ini.SectionExists("CompetencyModel"))
                {
                    //! Competencies.
                    //
                    ini.WriteList("CompetencyModel", "Competencies",
                        new List<String> { "Competency1", "Competency2" },
                        ListSeparator_Comma);

                    //! Facets.
                    //
                    ini.WriteList("CompetencyModel", "Facets",
                         new List<String>  {
                            String.Join(ListSeparator_Comma.ToString(),
                            new List<String> { "Facet1C1", "Facet2C1", "Facet3C1" }),
                                 String.Join(ListSeparator_Comma.ToString(),
                            new List<String> { "Facet1C2", "Facet2C2" }) },
                         ListSeparator_Colon);
                }

                //! 2) Statistical Submodel.
                //
                if (!ini.SectionExists("EvidenceModel"))
                {
                    ini.WriteList("EvidenceModel", "Statistical Submodel",
                            new List<String>
                            {
                                String.Join(ListSeparator_VerticalBar.ToString(),
                                    new List<String>
                                    {
                                        String.Join(ListSeparator_Comma.ToString(),new List<String>{"Obs1C1","Obs2C1","Obs3C1" }),
                                        String.Join(ListSeparator_Comma.ToString(),new List<String>{"Obs4C1"}),
                                        String.Join(ListSeparator_Comma.ToString(),new List<String>{"Obs5C1"}),
                                    }),
                                String.Join(ListSeparator_VerticalBar.ToString(),
                                    new List<String>
                                    {
                                        String.Join(ListSeparator_Comma.ToString(),new List<String>{"Obs1C2","Obs2C2","Obs3C2" }),
                                        String.Join(ListSeparator_Comma.ToString(),new List<String>{"Obs4C2"}),
                                    }),
                            }, ListSeparator_Colon);
                }

                //! 3) ObservablesModel.
                // 
                if (!ini.SectionExists("ObservableModel"))
                {
                    ini.WriteList("ObservableModel", "Observables",
                        new List<String> { "Obs1C1", "Obs2C1", "Obs3C1", "Obs4C1", "Obs5C1", "Obs6C1", "Obs7C1", "Obs1C2", "Obs2C2", "Obs3C2", "Obs4C2" },
                        ListSeparator_Comma);
                }

                //! 4) UniCompetencies.
                //
                if (!ini.SectionExists("UniCompetencyModel"))
                {
                    ini.WriteList("UniCompetencyModel", "UniCompetencies",
                    new List<String> { "UniCompetency0", "UniCompetency1", "UniCompetency2" },
                    ListSeparator_Comma);
                }

                //! 5) UniStatistical Submodel.
                //
                if (!ini.SectionExists("UniEvidenceModel"))
                {
                    ini.WriteList("UniEvidenceModel", "UniStatistical Submodel",
                         new List<String>
                         {
                            String.Join(ListSeparator_Comma.ToString(),
                            new List<String> { "Obs1C1","Obs2C1","Obs3C1"  }),
                            String.Join(ListSeparator_Comma.ToString(),
                            new List<String> { "Obs4C1" }),
                            String.Join(ListSeparator_Comma.ToString(),
                            new List<String> { "Obs5C1" })
                         }, ListSeparator_Colon);

                }

                if (ini.Dirty)
                {
                    ini.UpdateFile();
                }
            }
            */
            //! Nothing yet.
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
        /// A Tuple&lt;string[],string[][]&gt;
        /// </returns>
        internal static Tuple<string[], string[][]> LoadCompetencyModel(String filename = null)
        {
            using (IniFile ini = new IniFile(filename))
            {
                //! Stores competencies.
                string[] Competencies = ini.ReadList("CompetencyModel", "Competencies", ListSeparator_Comma).ToArray();

                //! Stores facets unparsed per competency.
                string[] FacetsPerCompetency = ini.ReadList("CompetencyModel", "Facets", ListSeparator_Colon).ToArray();

                //! Stores the structure of the influence diagram(s).
                string[][] Facets = new string[Competencies.Length][];

                for (int x = 0; x < Competencies.Length; x++)
                {
                    try
                    {
                        Facets[x] = new string[FacetsPerCompetency[x].Split(ListSeparator_Comma).Select(s => s.Trim()).ToArray().Length];

                        for (int y = 0; y < FacetsPerCompetency[x].Split(ListSeparator_Comma).Select(s => s.Trim()).ToArray().Length; y++)
                        {
                            string[] facets = FacetsPerCompetency[x].Split(ListSeparator_Comma).Select(s => s.Trim()).ToArray();     //Stores facets parsed for a competency.
                            Facets[x][y] = facets[y];
                        }
                    }

                    catch (IndexOutOfRangeException)
                    {
                        Facets[x] = Array.Empty<string>();
                    }

                }

                return new Tuple<string[], string[][]>(Competencies, Facets);
            }
        }

        /// <summary>
        /// Saves a competency model.
        /// </summary>
        ///
        /// <param name="model">    The model. </param>
        /// <param name="filename"> (Optional) Filename of the file. </param>
        internal static void SaveCompetencyModel(Tuple<string[], string[][]> model, String filename = null)
        {
            using (IniFile ini = new IniFile(filename))
            {
                string[] Competencies = model.Item1;
                string[][] Facets = model.Item2;

                //! Stores competencies.

                ini.EraseSection("CompetencyModel");

                ini.WriteList("CompetencyModel", "Competencies", Competencies.ToList(), ListSeparator_Comma);

                //! Stores facets unparsed per competency.

                string[] tmp = new string[Competencies.Length];

                for (int x = 0; x < Competencies.Length; x++)
                {
                    tmp[x] = Facets[x].Join2String(ListSeparator_Comma);
                }

                ini.WriteList("CompetencyModel", "Facets", tmp.ToList(), ListSeparator_Colon);

                ini.UpdateFile();
            }
        }

        /// <summary>
        /// Evidence model.
        /// </summary>
        ///
        /// <param name="filename"> (Optional) Filename of the file. </param>
        ///
        /// <returns>
        /// A Tuple&lt;string[][][][],string[][][]&gt;
        /// </returns>
        internal static Tuple<string[][][][], string[][][]> LoadEvidenceModel(String filename = null)
        {
            using (IniFile ini = new IniFile(filename))
            {
                ////! Evidence Rules

                ////! Stores evidence rules unparsed per competency.
                //string[] EvidenceRulesPerCompetency = ini.ReadList("EvidenceModel", "Evidence Rules", ListSeparator_Colon).ToArray();

                ////! Stores evidence rules unparsed per facet.
                //string[][] EvidenceRulesPerFacet = new string[EvidenceRulesPerCompetency.Length][];

                ////! Stores evidence rules unparsed per task.
                //string[][][] EvidenceRulesPerTask = new string[EvidenceRulesPerCompetency.Length][][];

                ////! Stores the Evidence Rules parsed for each observables per task.
                //string[][][][] EvidenceRules = new string[EvidenceRulesPerCompetency.Length][][][];

                //for (int x = 0; x < EvidenceRulesPerCompetency.Length; x++)
                //{
                //    EvidenceRulesPerFacet[x] = new string[EvidenceRulesPerCompetency[x].Split(ListSeparator_VerticalBar).Select(s => s.Trim()).ToArray().Length];
                //    EvidenceRulesPerTask[x] = new string[EvidenceRulesPerFacet[x].Length][];
                //    EvidenceRules[x] = new string[EvidenceRulesPerFacet[x].Length][][];
                //}

                //for (int x = 0; x < EvidenceRulesPerCompetency.Length; x++)
                //{
                //    EvidenceRulesPerFacet[x] = EvidenceRulesPerCompetency[x].Split(ListSeparator_VerticalBar).Select(s => s.Trim()).ToArray();
                //    for (int y = 0; y < EvidenceRulesPerFacet[x].Length; y++)
                //    {
                //        EvidenceRulesPerTask[x][y] = new string[EvidenceRulesPerFacet[x][y].Split(ListSeparator_Tilde).Length];
                //        EvidenceRules[x][y] = new string[EvidenceRulesPerTask[x][y].Length][];
                //    }
                //}

                //for (int x = 0; x < EvidenceRulesPerCompetency.Length; x++)
                //{
                //    for (int y = 0; y < EvidenceRulesPerFacet[x].Length; y++)
                //    {
                //        EvidenceRulesPerTask[x][y] = EvidenceRulesPerFacet[x][y].Split(ListSeparator_Tilde);
                //        for (int i = 0; i < EvidenceRulesPerTask[x][y].Length; i++)
                //        {
                //            EvidenceRules[x][y][i] = new string[EvidenceRulesPerTask[x][y].Length];
                //        }
                //    }
                //}

                //for (int x = 0; x < EvidenceRulesPerCompetency.Length; x++)
                //{
                //    for (int y = 0; y < EvidenceRulesPerFacet[x].Length; y++)
                //    {
                //        for (int i = 0; i < EvidenceRulesPerTask[x][y].Length; i++)
                //        {
                //            EvidenceRules[x][y][i] = EvidenceRulesPerTask[x][y][i].Split(ListSeparator_Comma);
                //        }
                //    }
                //}

                //! Statistical Submodel

                //<add key="Statistical Submodel" value="Obs1C1,Obs2C1,Obs3C1|Obs4C1|Obs5C1,Obs6C1,Obs7C1 : Obs1C2,Obs2C2,Obs3C2|Obs4C2" />

                //! Stores evidence rules unparsed per competency.
                string[] StatisticalSubmodelPerCompetency = ini.ReadList("EvidenceModel", "Statistical Submodel", ListSeparator_Colon).ToArray();

                //! Stores evidence rules unparsed per facet.
                string[][] StatisticalSubmodelPerFacet = new string[StatisticalSubmodelPerCompetency.Length][];

                //! Stores the Statistical Submodel parsed for each observables per facet.
                string[][][] StatisticalSubmodel = new string[StatisticalSubmodelPerCompetency.Length][][];

                for (int x = 0; x < StatisticalSubmodelPerCompetency.Length; x++)
                {
                    StatisticalSubmodelPerFacet[x] = new string[StatisticalSubmodelPerCompetency[x].Split(ListSeparator_VerticalBar).Select(s => s.Trim()).ToArray().Length];
                    StatisticalSubmodel[x] = new string[StatisticalSubmodelPerFacet[x].Length][];
                }

                for (int x = 0; x < StatisticalSubmodelPerCompetency.Length; x++)
                {
                    StatisticalSubmodelPerFacet[x] = StatisticalSubmodelPerCompetency[x].Split(ListSeparator_VerticalBar).Select(s => s.Trim()).ToArray();
                    for (int y = 0; y < StatisticalSubmodelPerFacet[x].Length; y++)
                    {
                        StatisticalSubmodel[x][y] = new string[StatisticalSubmodelPerFacet[x][y].Split(ListSeparator_Comma).Length];
                    }
                }

                for (int x = 0; x < StatisticalSubmodelPerCompetency.Length; x++)
                {
                    for (int y = 0; y < StatisticalSubmodelPerFacet[x].Length; y++)
                    {
                        StatisticalSubmodel[x][y] = StatisticalSubmodelPerFacet[x][y].Split(ListSeparator_Comma);
                    }
                }

                return new Tuple<string[][][][], string[][][]>(Array.Empty<string[][][]>(), StatisticalSubmodel);
            }
        }

        /// <summary>
        /// Saves an evidence model.
        /// </summary>
        ///
        /// <param name="model">    The model. </param>
        /// <param name="filename"> (Optional) Filename of the file. </param>
        internal static void SaveEvidenceModel(string[][][] model, String filename = null)
        {
            using (IniFile ini = new IniFile(filename))
            {
                //! Statistical Submodel

                ini.EraseSection("EvidenceModel");

                string[] tmp = new string[model.Length];
                for (Int32 x = 0; x < model.Length; x++)
                {
                    string[] subtmp = new string[model[x].Length];

                    for (Int32 y = 0; y < model[x].Length; y++)
                    {
                        subtmp[y] = model[x][y].Join2String(ListSeparator_Comma);
                    }

                    tmp[x] = subtmp.Join2String(ListSeparator_VerticalBar);
                }

                ini.WriteList("EvidenceModel", "Statistical Submodel", tmp.ToList(), ListSeparator_Colon);

                ini.UpdateFile();
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
        /// Gets the observables.
        /// </summary>
        ///
        /// <param name="filename"> (Optional) Filename of the file. </param>
        ///
        /// <returns>
        /// A string[].
        /// </returns>
        internal static string[] LoadObservables(String filename = null)
        {
            using (IniFile ini = new IniFile(filename))
            {
                //! Load observables

                //! Stores observables unparsed per competency.
                string[] Observables = ini.ReadList("ObservableModel", "Observables", ListSeparator_Comma).ToArray();

                return Observables;
            }
        }
        internal static void SaveObservables(String[] Observables, String filename = null)
        {
            using (IniFile ini = new IniFile(filename))
            {
                //! Save observables
                ini.EraseSection("ObservableModel");

                ini.WriteList("ObservableModel", "Observables", Observables.ToList(), ListSeparator_Comma);

                ini.UpdateFile();
            }
        }

        internal static void SaveObservables(Observables<String> observables, String filename = null)
        {
            using (IniFile ini = new IniFile(filename))
            {
                //! Save observables
                ini.EraseSection("ObservableModel");

                ini.WriteList("ObservableModel", "Observables", observables.Names.ToList(), ListSeparator_Comma);

                ini.UpdateFile();
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

        /// <summary>
        /// Loads uni competency model.
        /// </summary>
        ///
        /// <param name="filename"> (Optional) Filename of the file. </param>
        ///
        /// <returns>
        /// An array of string.
        /// </returns>
        internal static string[] LoadUniCompetencyModel(String filename = null)
        {
            using (IniFile ini = new IniFile(filename))
            {
                //! Stores uni-dimensional competencies.
                string[] UniCompetencies = ini.ReadList("UniCompetencyModel", "UniCompetencies", ListSeparator_Comma).ToArray();

                return UniCompetencies;
            }
        }

        /// <summary>
        /// Saves an uni competency model.
        /// </summary>
        ///
        /// <param name="UniCompetencies"> The uni competencies. </param>
        /// <param name="filename">        (Optional) Filename of the file. </param>
        internal static void SaveUniCompetencyModel(string[] UniCompetencies, String filename = null)
        {
            using (IniFile ini = new IniFile(filename))
            {
                //! Stores uni-dimensional competencies.
                ini.EraseSection("UniCompetencyModel");

                ini.WriteList("UniCompetencyModel", "UniCompetencies", UniCompetencies.ToList(), ListSeparator_Comma);

                ini.UpdateFile();
            }
        }

        /// <summary>
        /// Loads uni evidence model.
        /// </summary>
        ///
        /// <param name="filename"> (Optional) Filename of the file. </param>
        ///
        /// <returns>
        /// An array of string[].
        /// </returns>
        internal static string[][] LoadUniEvidenceModel(String filename = null)
        {
            using (IniFile ini = new IniFile(filename))
            {

                //! UniStatistical Submodel

                //<add key="UniStatistical Submodel" value="Obs1C1,Obs2C1,Obs3C1:Obs4C1:Obs5C1" />

                //! Stores evidence rules unparsed per competency.
                string[] UniStatisticalSubmodelPerCompetency = ini.ReadList("UniEvidenceModel", "UniStatistical Submodel", ListSeparator_Colon).ToArray();

                //! Stores the Statistical Submodel parsed for each observables per competecy.
                string[][] UniStatisticalSubmodel = new string[UniStatisticalSubmodelPerCompetency.Length][];

                for (int x = 0; x < UniStatisticalSubmodelPerCompetency.Length; x++)
                {
                    UniStatisticalSubmodel[x] = new string[UniStatisticalSubmodelPerCompetency[x].Split(ListSeparator_Comma).Select(s => s.Trim()).ToArray().Length];

                    string[] observables = UniStatisticalSubmodelPerCompetency[x].Split(ListSeparator_Comma).Select(s => s.Trim()).ToArray();     //Stores observables parsed for a competency.

                    for (int y = 0; y < observables.Length; y++)
                    {
                        UniStatisticalSubmodel[x][y] = observables[y];
                    }
                }

                return UniStatisticalSubmodel;
            }
        }

        /// <summary>
        /// Saves an uni evidence model.
        /// </summary>
        ///
        /// <param name="UniStatisticalSubmodel"> The uni statistical submodel. </param>
        /// <param name="filename">               (Optional) Filename of the file. </param>
        internal static void SaveUniEvidenceModel(string[][] UniStatisticalSubmodel, String filename = null)
        {
            using (IniFile ini = new IniFile(filename))
            {
                //! UniStatistical Submodel
                string[] UniStatisticalSubmodelPerCompetency = new string[UniStatisticalSubmodel.Length];

                for (int x = 0; x < UniStatisticalSubmodelPerCompetency.Length; x++)
                {
                    UniStatisticalSubmodelPerCompetency[x] = String.Join(ListSeparator_Comma.ToString(), UniStatisticalSubmodel[x]);
                }

                ini.EraseSection("UniEvidenceModel");

                ini.WriteList("UniEvidenceModel", "UniStatistical Submodel", UniStatisticalSubmodelPerCompetency.ToList(), ListSeparator_Colon);

                ini.UpdateFile();
            }
        }
        #endregion Methods
    }
}