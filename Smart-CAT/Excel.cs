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

    using OfficeOpenXml;

    using Swiss;

    /// <summary>
    /// An excel.
    /// </summary>
    public static class Excel
    {
        internal const string GSATScratchPad = "Smart-CAT";

        /// <summary>
        /// Returns true if the file exists and is locked for R/W access.
        /// </summary>
        ///
        /// <param name="filename"> The File to be checked. </param>
        ///
        /// <returns>
        /// Returns true if the file exists and is locked for R/W acces.
        /// </returns>
        public static Boolean IsFileLocked(String filename)
        {
            try
            {
                if (File.Exists(filename))
                {
                    using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);

                return true;
            }
        }

        /// <summary>
        /// Use Epplus to read a xlsx file int a Tuple.
        /// </summary>
        ///
        /// <param name="filename"> Filename of the file. </param>
        ///
        /// <returns>
        /// A Tuple&lt;string[],string[][]&gt;
        /// </returns>
        internal static Tuple<string[], string[][]> EPPlus(string filename)
        {
            //! Variables for storing the data.
            string[] AllObservables = new string[] { };
            string[][] AllData = new string[][] { };

            FileInfo fileInfo = new FileInfo(filename);

            if (fileInfo.Exists)
            {
                using (ExcelPackage p = new ExcelPackage(fileInfo))
                {


                    //! Save the size of the worksheet.
                    // 
                    if (!p.Workbook.Worksheets.Any(q => q.Name.Equals(GSATScratchPad)))
                    {
                        using (ExcelWorksheet ws = p.Workbook.Worksheets[0])
                        {
                            Logger.Info($"Spreadsheet Raw Data Size {ws.Dimension.Rows - 1} x {ws.Dimension.Columns}.");

                            using (IniFile ini = new IniFile(Path.ChangeExtension(filename, ".ini")))
                            {
                                ini.WriteInteger("RawData", "Rows", ws.Dimension.Rows);
                                ini.WriteInteger("RawData", "Colums", ws.Dimension.Columns);
                                ini.UpdateFile();
                            }

                            ExcelWorksheet worksheet = p.Workbook.Worksheets.Copy(ws.Name, GSATScratchPad);

                            p.Save();
                        }
                    }

                    using (ExcelWorksheet ws = p.Workbook.Worksheets[GSATScratchPad])
                    {
                        AllObservables = new String[ws.Dimension.Columns];

                        for (Int32 i = 0; i < ws.Dimension.Columns; i++)
                        {
                            AllObservables[i] = ws.Cells[1, i + 1].Value.ToString();
                        }

                        AllData = new String[ws.Dimension.Columns][];

                        for (Int32 c = 0; c < ws.Dimension.Columns; c++)
                        {
                            AllData[c] = new String[ws.Dimension.Rows - 1];
                            for (Int32 r = 0; r < ws.Dimension.Rows - 1; r++)
                            {
                                AllData[c][r] = ws.Cells[r + 2, c + 1].Value.ToString();
                            }
                        }
                    }
                }
            }

            return new Tuple<string[], string[][]>(AllObservables, AllData);
        }

        /// <summary>
        /// Add Labels for Competencies in the Excel file.
        /// </summary>
        ///
        /// <param name="CompetencyModel"> The competency model. </param>
        /// <param name="CheckLabels">     The check labels. </param>
        /// <param name="LabelledData">    Information describing the labeled data. </param>
        /// <param name="filename">        Filename of the file. </param>
        internal static void AddLabelsforCompetencies(Tuple<string[], string[][]> CompetencyModel, Tuple<bool[][], bool[][][]> CheckLabels, Tuple<int[][], int[][][]> LabelledData, string filename)
        {
            //! Browse for labeled data for each declared facet and decide which ML algorithm to apply accordingly.
            // 
            FileInfo fileInfo = new FileInfo(filename);

            if (fileInfo.Exists)
            {
                using (ExcelPackage p = new ExcelPackage(fileInfo))
                {
                    using (ExcelWorksheet ws = p.Workbook.Worksheets[GSATScratchPad])
                    {
                        for (int x = 0; x < CompetencyModel.Item1.Length; x++)
                        {
                            if (!CheckLabels.Item1[x][0])
                            {
                                Int32 i = ws.Dimension.Columns;
                                Int32 length = LabelledData.Item1[x].Length;

                                Logger.Info($"Adding {length} Competency Labels in Column {i}.");

                                ws.Cells[1, i + 1].Value = CompetencyModel.Item1[x].ToString();
                                ws.Cells[1, i + 1].Style.Font.Bold = true;

                                for (int y = 0; y < length; y++)
                                {
                                    ws.Cells[2 + y, i + 1].Value = LabelledData.Item1[x][y];
                                }
                            }
                        }

                        p.Save();
                    }
                }
            }
        }

        /// <summary>
        /// Add Labels for Facets in the Excel file.
        /// </summary>
        ///
        /// <param name="CompetencyModel"> The competency model. </param>
        /// <param name="CheckLabels">     The check labels. </param>
        /// <param name="LabelledData">    Information describing the labeled data. </param>
        /// <param name="filename">        Filename of the file. </param>
        internal static void AddLabelsforFacets(Tuple<string[], string[][]> CompetencyModel, Tuple<bool[][], bool[][][]> CheckLabels, Tuple<int[][], int[][][]> LabelledData, string filename)
        {
            //! Browse for labeled data for each declared facet and decide which ML algorithm to apply accordingly.
            // 
            FileInfo fileInfo = new FileInfo(filename);

            if (fileInfo.Exists)
            {
                using (ExcelPackage p = new ExcelPackage(fileInfo))
                {
                    using (ExcelWorksheet ws = p.Workbook.Worksheets[GSATScratchPad])
                    {

                        for (int x = 0; x < CompetencyModel.Item1.Length; x++)
                        {
                            for (int y = 0; y < CompetencyModel.Item2[x].Length; y++)
                            {
                                if (!CheckLabels.Item2[x][y][0])
                                {
                                    Int32 i = ws.Dimension.Columns;
                                    Int32 length = LabelledData.Item1[x].Length;

                                    Logger.Info($"Adding {length} Facet Labels in Column {i}.");

                                    ws.Cells[1, i + 1].Value = CompetencyModel.Item2[x][y].ToString();
                                    ws.Cells[1, i + 1].Style.Font.Bold = true;

                                    for (int w = 0; w < length; w++)
                                    {
                                        ws.Cells[2 + w, i + 1].Value = LabelledData.Item2[x][y][w];
                                    }
                                }
                            }
                        }

                        p.Save();
                    }
                }
            }
        }

        internal static void AddLabelsforUniCompetencies(string[] UniCompetencyModel, bool[][] UniCheckLabels, int[][] UniLabelledData, string filename)
        {
            //! Browse for labeled data for each declared facet and decide which ML algorithm to apply accordingly.
            // 
            FileInfo fileInfo = new FileInfo(filename);

            if (fileInfo.Exists)
            {
                using (ExcelPackage p = new ExcelPackage(fileInfo))
                {
                    using (ExcelWorksheet ws = p.Workbook.Worksheets[GSATScratchPad])
                    {
                        for (int x = 0; x < UniCompetencyModel.Length; x++)
                        {
                            if (!UniCheckLabels[x][0])
                            {
                                Int32 i = ws.Dimension.Columns;
                                Int32 length = UniLabelledData[x].Length;

                                Logger.Info($"Adding {length} Competency Labels in Column {i}.");

                                ws.Cells[1, i + 1].Value = UniCompetencyModel[x].ToString();
                                ws.Cells[1, i + 1].Style.Font.Bold = true;

                                for (int y = 0; y < length; y++)
                                {
                                    ws.Cells[2 + y, i + 1].Value = UniLabelledData[x][y];
                                }
                            }
                        }

                        p.Save();
                    }
                }
            }
        }
    }
}
