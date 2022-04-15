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
        internal const string ModelScratchPad = "Model";

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
        internal static Observables EPPlus(string filename)
        {
            //! Variables for storing the data.
            Observables observables = new Observables();

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
                        for (Int32 c = 0; c < ws.Dimension.Columns; c++)
                        {
                            observables.Add(new Observable<String>(ws.Dimension.Rows - 1, ws.Cells[1, c + 1].Value.ToString()));

                            for (Int32 r = 0; r < ws.Dimension.Rows - 1; r++)
                            {
                                observables[c][r] = ws.Cells[r + 2, c + 1].Value.ToString();
                            }
                        }
                    }
                }
            }

            return observables;
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

        /// <summary>
        /// Adds a labelsfor uni competencies.
        /// </summary>
        ///
        /// <param name="UniCompetencyModel"> The uni competency model. </param>
        /// <param name="UniCheckLabels">     The uni check labels. </param>
        /// <param name="UniLabelledData">    Information describing the uni labelled. </param>
        /// <param name="filename">           Filename of the file. </param>
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


        internal static void AddModel(string filename)
        {
            //! Browse for labeled data for each declared facet and decide which ML algorithm to apply accordingly.
            // 
            FileInfo fileInfo = new FileInfo(filename);

            if (fileInfo.Exists)
            {
                using (ExcelPackage p = new ExcelPackage(fileInfo))
                {
                    //! Create an empty model worksheet.
                    // 
                    if (p.Workbook.Worksheets.Any(q => q.Name.Equals(ModelScratchPad)))
                    {
                        p.Workbook.Worksheets.Delete(ModelScratchPad);
                    }
                    p.Workbook.Worksheets.Add(ModelScratchPad);

                    int row = 1;
                    int col = 1;

                    using (ExcelWorksheet ws = p.Workbook.Worksheets[ModelScratchPad])
                    {
                        ws.Cells[row, col].Style.Font.Bold = true;
                        ws.Cells[row++, col++].Value = "Observables";

                        foreach (String observable in Data.observables.Names)
                        {
                            ws.Cells[row, col++].Value = observable;
                            if ((col - 2) % 8 == 0)
                            {
                                col = 2;
                                row++;
                            }
                        }
                        row += 2;
                        col = 1;

                        ws.Cells[row, col].Style.Font.Bold = true;
                        ws.Cells[row, col].Value = "Statistical Submodel";
                        row += 1;
                        col++;

                        //! 1) CompetencyModel.
                        //string[] Competencies = Data.competencies.Names;
                        //string[][] Facets = Data.competencies.Item2;

                        for (int x = 0; x < Data.competencies.Count; x++)
                        {
                            ws.Cells[row++, col++].Value = Data.competencies[x].CompetencyName;

                            for (int y = 0; y < Data.competencies[x].Count(); y++)
                            {
                                ws.Cells[row++, col++].Value = Data.competencies[x][y].FacetName;

                                for (int z = 0; z < Data.competencies[x][y].Length; z++)
                                {
                                    ws.Cells[row++, col].Value = Data.competencies[x][y][z];
                                }
                                col = 3;
                            }
                            col = 2;
                        }

                        p.Save();
                    }
                }
            }
        }
    }
}
