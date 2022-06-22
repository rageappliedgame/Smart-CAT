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
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Windows.Forms;
    using System.Windows.Forms.DataVisualization.Charting;

    using OfficeOpenXml;
    using OfficeOpenXml.Table;

    using Swiss;

    public partial class MainForm : Form
    {
        #region Fields

        private readonly EnumItemCollection AlgorithmItems = new EnumItemCollection(typeof(MLAlgorithms));

#pragma warning disable IDE0044 // Add readonly modifier
        private Font SelectedFont = null;
#pragma warning restore IDE0044 // Add readonly modifier

        //! What to do with sheets with the same name? Ask/Add a suffix?
        // 
        //  [Smart-CAT Projects]
        //     [sheet name]
        //         sheet
        //         json
        //             [timestamp]
        //                  copy sheet
        //                  copy json
        //                  [data]

        /// <summary>
        /// (Immutable) my projects.
        /// </summary>
        private readonly String MyProjects = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Smart-CAT Projects");
        private String MyProject = String.Empty;
        private String MyData = String.Empty;
        private String MyInput = String.Empty;

        private readonly BackgroundWorker Step1BackgroundWorker = new BackgroundWorker();
        private readonly BackgroundWorker Step2BackgroundWorker = new BackgroundWorker();
        private readonly BackgroundWorker Step3BackgroundWorker = new BackgroundWorker();
        private readonly BackgroundWorker Step4BackgroundWorker = new BackgroundWorker();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the StealthAssessmentWizard.Form1 class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            SelectedFont = new Font(Font, FontStyle.Bold);

            StateMachine.InitStateMachine(
                panelManager1,
                prevBtn, nextBtn, cancelBtn,
                busyBar);

            //! StartNew
            Step1BackgroundWorker.DoWork += new DoWorkEventHandler(Step1_DoWork);
            Step1BackgroundWorker.RunWorkerCompleted += Step1_RunWorkerCompleted;
            Step1BackgroundWorker.ProgressChanged += Step1_ProgressChanged;
            Step1BackgroundWorker.WorkerSupportsCancellation = true;
            Step1BackgroundWorker.WorkerReportsProgress = true;

            StateMachine.exitWorkers.Add(StateMachine.States.StartNew, new Worker()
            {
                worker = Step1BackgroundWorker,
                argument = null,
            });

            Step2BackgroundWorker.DoWork += new DoWorkEventHandler(Step2_DoWork);
            Step2BackgroundWorker.RunWorkerCompleted += Step2_RunWorkerCompleted;
            Step2BackgroundWorker.ProgressChanged += Step2_ProgressChanged;
            Step2BackgroundWorker.WorkerSupportsCancellation = true;
            Step2BackgroundWorker.WorkerReportsProgress = true;

            StateMachine.exitWorkers.Add(StateMachine.States.ImportData, new Worker()
            {
                worker = Step2BackgroundWorker,
                argument = null,
            });

            Step3BackgroundWorker.DoWork += new DoWorkEventHandler(Step3_DoWork);
            Step3BackgroundWorker.RunWorkerCompleted += Step3_RunWorkerCompleted;
            Step3BackgroundWorker.ProgressChanged += Step3_ProgressChanged;
            Step3BackgroundWorker.WorkerSupportsCancellation = true;
            Step3BackgroundWorker.WorkerReportsProgress = true;

            StateMachine.exitWorkers.Add(StateMachine.States.ConfigureECD, new Worker()
            {
                worker = Step3BackgroundWorker,
                argument = null,
            });

            Step4BackgroundWorker.DoWork += new DoWorkEventHandler(Step4_DoWork);
            Step4BackgroundWorker.RunWorkerCompleted += Step4_RunWorkerCompleted;
            Step4BackgroundWorker.ProgressChanged += Step4_ProgressChanged;
            Step4BackgroundWorker.WorkerSupportsCancellation = true;
            Step4BackgroundWorker.WorkerReportsProgress = true;

            StateMachine.exitWorkers.Add(StateMachine.States.OptimizeML, new Worker()
            {
                worker = Step4BackgroundWorker,
                argument = null,
            });

            SetHelpStrings();

            Directory.CreateDirectory(MyProjects);

            openFileDialog1.InitialDirectory = MyProjects;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Loads the ECD.
        /// </summary>
        private static void LoadDefaultECD()
        {
            Debug.WriteLine("Loading Default ECD configuration...");
            {
                Debug.Indent();

                //! Load the Model.
                Debug.Write("Loading Default Observables, Competency and Evidence Models... ");

                using (Models models = ECD.LoadModelData())
                {
                    //! Skip observables, loaded (with data) from Excel.
                    Data.unicompetencies = models.unicompetencies;
                    Data.competencies = models.competencies;
                }

                Debug.WriteLine("Completed.");

                Debug.Unindent();
            }
            Debug.WriteLine("Loading of Default ECD configuration completed.\r\n");
        }

        /// <summary>
        /// Event handler. Called by addCompetencyMenuItem for click events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        private void AddCompetencyMenuItem_Click(object sender, EventArgs e)
        {
            ToolStrip ts = ((ToolStripItem)sender).OwnerItem.Owner;
            ListView lv = (ListView)((ContextMenuStrip)ts).SourceControl;

            Boolean UniDimensional = (lv == listView2);

            IEnumerable<String> items = Data.observables.Names;

            if (lv != null)
            {
                using (InputSelectDialog id = new InputSelectDialog("Add Competency", "Enter the name of a Competency", String.Empty, items))
                {
                    id.SelectorEnabled = UniDimensional;

                    id.Text = UniDimensional
                        ? "Add uni-dimensional model"
                        : "Add multi-dimensional model";

                    if (id.ShowDialog() == DialogResult.OK)
                    {
                        switch (UniDimensional)
                        {
                            //! Checked.
                            //
                            case true:
                                {
                                    ListViewItem lvi = lv.Items.Add(id.Input, id.Input, 0);
                                    lvi.SubItems.Add(String.Join(",", id.CheckedItems.Select(p => p.ToString())));
                                    lvi.ForeColor = id.CheckedItems.Count() == 0
                                        ? Color.Red
                                        : listView1.ForeColor;
                                    lvi.Group = lv.Groups[0];
                                }
                                break;
                            //! Checked.
                            //
                            case false:
                                {
                                    foreach (ListViewGroup lvg in lv.Groups)
                                    {
                                        if (lvg.Name.Equals(id.Input, StringComparison.OrdinalIgnoreCase))
                                        {
                                            //! Compentency already exists.
                                            return;
                                        }
                                    }

                                    //! Add Compentency Group and a dummy Facet so Compentency Group shows.
                                    String fid = $"Facet #{lv.Items.Count + 1}";
                                    ListViewItem lvi = lv.Items.Add(fid, fid, 0);
                                    lvi.SubItems.Add(String.Empty);
                                    lvi.ForeColor = Color.Red;
                                    lvi.Group = lv.Groups.Add(id.Input, id.Input);
                                }
                                break;
                        }
                    }
                }
            }

            StateMachine.Flags[StateMachine.CONFIGURE_ECD] = ValidateCompetencies();

            StateMachine.UpdateControls();
        }

        /// <summary>
        /// Event handler. Called by addToolStripMenuItem1 for click events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        private void AddFacetMenuItem_Click(object sender, EventArgs e)
        {
            ToolStrip ts = ((ToolStripItem)sender).OwnerItem.Owner;
            ListView lv = (ListView)((ContextMenuStrip)ts).SourceControl;

            Boolean UniDimensional = (lv == listView2);

            if (lv != null && lv.SelectedItems.Count == 1 && lv.SelectedItems[0].Group != null)
            {
                String group = lv.SelectedItems[0].Group.Name;

                IEnumerable<String> items = Data.observables.Names;

                switch (UniDimensional)
                {
                    //! Checked.
                    //
                    case true:
                        {
                            //! Should not occur for listView2.
                        }
                        break;

                    //! Checked.
                    //
                    case false:
                        {
                            using (InputSelectDialog id = new InputSelectDialog("Add Facet", "Enter the name of a Facet", String.Empty, items))
                            {
                                id.SelectorEnabled = true;

                                if (id.ShowDialog() == DialogResult.OK)
                                {
                                    String fid = id.Input;

                                    //! Add new Facet to Competency Group.
                                    ListViewItem lvi = lv.Items.Add(fid, fid, 0);
                                    lvi.SubItems.Add(String.Join(",", id.CheckedItems.Select(p => p.ToString())));
                                    lvi.Group = listView1.Groups[group];
                                }
                            }
                        }
                        break;
                }

            }

            StateMachine.Flags[StateMachine.CONFIGURE_ECD] = ValidateCompetencies();

            StateMachine.UpdateControls();
        }

        /// <summary>
        /// Event handler. Called by BayesNet for update progress events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        private void BayesNet_UpdateProgress(object sender, EventArgs e)
        {
            busyBar.Refresh();
        }

        /// <summary>
        /// Event handler. Called by Button3 for click events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        private void SaveReportBtn_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = $"Results_{Excel.Stamp}.xlsx";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (Excel.IsFileLocked(saveFileDialog1.FileName))
                {
                    MessageBox.Show(String.Format("Could not create file:\r\n\r\n{0}", Path.GetFileName(saveFileDialog1.FileName)), IniFile.AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (File.Exists(saveFileDialog1.FileName))
                    {
                        File.Delete(saveFileDialog1.FileName);
                    }

                    MLAlgorithms alg = (MLAlgorithms)((EnumPair)MLAlgoritmSelector.SelectedItem).Value;

                    String suffix = String.Empty;
                    Double percentsplit = 0;

                    switch (alg)
                    {
                        case MLAlgorithms.NaiveBayes:
                            suffix = "_BayesAccord";
                            percentsplit = (Data.MLOptions as NaiveBayesAlgorithm).PercentSplit;
                            break;
                        case MLAlgorithms.DecisionTrees:
                            suffix = "_DecisionTreesAccord";
                            percentsplit = (Data.MLOptions as DecisionTreesAlgorithm).PercentSplit;
                            break;
                    }

                    String filenameTS = Excel.WorkingCopyFileName(MyInput);

                    //! Load Scratch Spreadsheet.
                    // 
                    using (ExcelPackage ep = new ExcelPackage(new FileInfo(filenameTS)))
                    {
                        Debug.WriteLine($"WorkSheets: {ep.Workbook.Worksheets.Count}");

                        //! Dirty Fix for worksheet count being zero sometimes.
                        while (ep.Workbook.Worksheets.Count == 0)
                        {
                            Thread.Sleep(10);
                            Application.DoEvents();
                        }

                        //! Compensate for 1 header row.
                        // 
                        int rawSize = ep.Workbook.Worksheets[Excel.ScratchPadName].Dimension.Rows - 1;  //0
                        int trainSize = (int)Math.Round((double)rawSize * percentsplit / 100);
                        int testSize = rawSize - trainSize;

                        //---R

                        //! Save Results Spreadsheet.
                        // 
                        using (ExcelPackage package = new ExcelPackage(new FileInfo(saveFileDialog1.FileName)))
                        {
                            for (Int32 i = 0; i < Data.competencies.Count; i++)
                            {
                                Int32 rofs = 1;
                                Int32 cofs = 1;

                                //! 1) Multi-Dimensional.
                                //
                                String Competency = Data.competencies[i].CompetencyName;

                                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(Competency);

                                ExcelTableCollection tblcollection = worksheet.Tables;

                                PropertyInfo[] props = Data.MLOptions.GetType().GetProperties();

                                Logger.Info($"Building Report Sheet for Competency: {Competency}.");

                                //! Setup.
                                //
                                using (ExcelRange Rng = worksheet.Cells[rofs, cofs, rofs + 1 + props.Length, cofs + 1])
                                {
                                    ExcelTable table = tblcollection.Add(Rng, $"tbl_setup_{Competency}");
                                    table.ShowHeader = true;
                                    table.ShowFilter = false;
                                    table.TableStyle = TableStyles.Light20;

                                    worksheet.Cells[rofs, cofs + 0].Value = "Setting";
                                    worksheet.Cells[rofs, cofs + 1].Value = "Value";
                                    rofs++;

                                    worksheet.Cells[rofs, cofs + 0].Value = "Model Type";
                                    worksheet.Cells[rofs, cofs + 1].Value = "Multi-Dimensional Model";
                                    rofs++;

                                    foreach (PropertyInfo prop in props)
                                    {
                                        worksheet.Cells[rofs, cofs + 0].Value = prop.Name;

                                        object val = prop.GetValue(Data.MLOptions);
                                        worksheet.Cells[rofs, cofs + 1].Value = (Double.TryParse(val.ToString(), out Double dbl)
                                            ? dbl
                                            : val);

                                        rofs++;
                                    }

                                    for (Int32 j = 0; j < 2; j++)
                                    {
                                        worksheet.Column(cofs + j).AutoFit();
                                    }
                                }

                                rofs = 1;
                                cofs += 3;

                                //! Model.
                                //
                                Int32 cnt = Data.competencies.ToStatisticalSubmodel()[i].Sum(p => p.Length) + 1;

                                //! Observables.
                                // 
                                List<String> obs = new List<string>();

                                using (ExcelRange Rng = worksheet.Cells[rofs, cofs, rofs + cnt + Data.competencies[i].Count, cofs + 2])
                                {
                                    ExcelTable table1 = tblcollection.Add(Rng, $"tbl_model_{Competency}");
                                    table1.ShowHeader = true;
                                    table1.ShowFilter = false;
                                    table1.TableStyle = TableStyles.Light20;

                                    worksheet.Cells[rofs + 0, cofs + 0].Value = "Competency";
                                    worksheet.Cells[rofs + 0, cofs + 1].Value = "Facets";
                                    worksheet.Cells[rofs + 0, cofs + 2].Value = "Observables";

                                    worksheet.Cells[rofs + 1, cofs + 0].Value = Competency;
                                    rofs++;

                                    for (Int32 f = 0; f < Data.competencies[i].Count; f++)
                                    {
                                        worksheet.Cells[rofs + 1, cofs + 1].Value = Data.competencies[i][f].FacetName;

                                        rofs++;

                                        foreach (String o in Data.competencies[i][f].Names)
                                        {
                                            obs.Add(o);

                                            worksheet.Cells[rofs + 1, cofs + 2].Value = o;

                                            rofs++;
                                        }
                                    }

                                    for (Int32 j = 0; j < 3; j++)
                                    {
                                        worksheet.Column(cofs + j).AutoFit();
                                    }
                                }

                                rofs = 1;
                                cofs += 4;

                                //! Accuracy.
                                //
                                for (Int32 f = 0; f < Data.competencies[i].Count(); f++)
                                {

                                }

                                using (ExcelRange Rng = worksheet.Cells[rofs, cofs, rofs + cnt + Data.competencies[i].Count, cofs + 6])
                                {
                                    ExcelTable table1 = tblcollection.Add(Rng, $"tbl_accuracy_{Competency}");
                                    table1.ShowHeader = true;
                                    table1.ShowFilter = false;
                                    table1.TableStyle = TableStyles.Light20;

                                    worksheet.Cells[rofs + 0, cofs + 0].Value = "Accuracy(%)";
                                    worksheet.Cells[rofs + 0, cofs + 1].Value = "Error";
                                    worksheet.Cells[rofs + 0, cofs + 2].Value = "Kappa";
                                    worksheet.Cells[rofs + 0, cofs + 3].Value = "MAE";
                                    worksheet.Cells[rofs + 0, cofs + 4].Value = "RMSE";
                                    worksheet.Cells[rofs + 0, cofs + 5].Value = "RAE(%)";
                                    worksheet.Cells[rofs + 0, cofs + 6].Value = "RRSE(%)";

                                    using (IniFile ini = new IniFile(Path.Combine(MyData, Competency + $"{suffix}.ini")))
                                    {
                                        worksheet.Cells[rofs + 1, cofs + 0].Value = ini.ReadDouble("Performance", "Accuracy", 0) * 100;
                                        worksheet.Cells[rofs + 1, cofs + 1].Value = ini.ReadDouble("Performance", "Error", 0);
                                        worksheet.Cells[rofs + 1, cofs + 2].Value = ini.ReadDouble("Performance", "Kappa", 0);
                                        worksheet.Cells[rofs + 1, cofs + 3].Value = ini.ReadDouble("Performance", "MAE", 0);
                                        worksheet.Cells[rofs + 1, cofs + 4].Value = ini.ReadDouble("Performance", "RMSE", 0);
                                        worksheet.Cells[rofs + 1, cofs + 5].Value = ini.ReadDouble("Performance", "RAE(%)", 0);
                                        worksheet.Cells[rofs + 1, cofs + 6].Value = ini.ReadDouble("Performance", "RRSE(%)", 0);

                                        rofs++;
                                    }

                                    for (Int32 f = 0; f < Data.competencies[i].Count(); f++)
                                    {
                                        String Facet = Data.competencies[i][f].FacetName;

                                        using (IniFile ini = new IniFile(Path.Combine(MyData, Competency, Facet + $"{suffix}.ini")))
                                        {
                                            worksheet.Cells[rofs + 1, cofs + 0].Value = ini.ReadDouble("Performance", "Accuracy", 0) * 100;
                                            worksheet.Cells[rofs + 1, cofs + 1].Value = ini.ReadDouble("Performance", "Error", 0);
                                            worksheet.Cells[rofs + 1, cofs + 2].Value = ini.ReadDouble("Performance", "Kappa", 0);
                                            worksheet.Cells[rofs + 1, cofs + 3].Value = ini.ReadDouble("Performance", "MAE", 0);
                                            worksheet.Cells[rofs + 1, cofs + 4].Value = ini.ReadDouble("Performance", "RMSE", 0);
                                            worksheet.Cells[rofs + 1, cofs + 5].Value = ini.ReadDouble("Performance", "RAE(%)", 0);
                                            worksheet.Cells[rofs + 1, cofs + 6].Value = ini.ReadDouble("Performance", "RRSE(%)", 0);

                                            rofs++;

                                            foreach (String o in Data.competencies.ToStatisticalSubmodel()[i][f])
                                            {
                                                rofs++;
                                            }
                                        }
                                    }

                                    for (Int32 j = 0; j < 7; j++)
                                    {
                                        worksheet.Column(cofs + j).AutoFit();
                                    }
                                }

                                rofs = 1;
                                cofs += 8;

                                //! V&V
                                // 
                                using (ExcelRange Rng = worksheet.Cells[rofs, cofs, rofs + cnt + Data.competencies[i].Count, cofs + 1])
                                {
                                    ExcelTable table1 = tblcollection.Add(Rng, $"tbl_VandV_{Competency}");
                                    table1.ShowHeader = true;
                                    table1.ShowFilter = false;
                                    table1.TableStyle = TableStyles.Light20;

                                    //! Header
                                    worksheet.Cells[rofs, cofs + 0].Value = "Cronbach";
                                    worksheet.Cells[rofs, cofs + 1].Value = "Spearman's Rho";

                                    rofs = 1;
                                    for (Int32 k = 0; k < Data.cronbachAlphaMulti.Item1.Length; k++)
                                    {
                                        if (!String.IsNullOrEmpty(Data.cronbachAlphaMulti.Item1[k])
                                            && Data.cronbachAlphaMulti.Item1[k].Equals(Competency))
                                        {
                                            rofs++;

                                            worksheet.Cells[rofs, cofs + 0].Value = Data.cronbachAlphaMulti.Item2[k];

                                            rofs++;

                                            for (Int32 l = 0; l < Data.cronbachAlphaMulti.Item3[k].Length; l++)
                                            {
                                                worksheet.Cells[rofs, cofs + 0].Value = Data.cronbachAlphaMulti.Item4[k][l];
                                                rofs += Data.competencies.ToStatisticalSubmodel()[k][l].Length + 1;
                                            }
                                        }
                                    }

                                    rofs = 1;
                                    for (Int32 k = 0; k < Data.spearmansMulti.Item1.Length; k++)
                                    {
                                        if (!String.IsNullOrEmpty(Data.spearmansMulti.Item1[k])
                                            && Data.spearmansMulti.Item1[k].Equals(Competency))
                                        {
                                            rofs++;

                                            for (Int32 l = 0; l < Data.spearmansMulti.Item2[k].Length; l++)
                                            {
                                                worksheet.Cells[rofs, cofs + 1].Value = Data.spearmansMulti.Item2[k][l];
                                                rofs++;
                                            }
                                        }
                                    }

                                    for (Int32 j = 0; j < 2; j++)
                                    {
                                        worksheet.Column(cofs + j).AutoFit();
                                    }
                                }

                                rofs = 1;
                                cofs += 3;

                                //! Raw Data
                                // 
                                using (ExcelRange Rng = worksheet.Cells[rofs, cofs, rofs + testSize, cofs + obs.Count])
                                {
                                    ExcelTable table1 = tblcollection.Add(Rng, $"tbl_raw_{Competency}");
                                    table1.ShowHeader = true;
                                    table1.ShowFilter = false;
                                    table1.TableStyle = TableStyles.Light20;

                                    //! Header
                                    worksheet.Cells[rofs, cofs].Value = "Classification";
                                    for (Int32 o = 0; o < obs.Count; o++)
                                    {
                                        worksheet.Cells[rofs, cofs + o + 1].Value = obs[o];
                                    }

                                    rofs++;

                                    using (IniFile ini = new IniFile(Path.Combine(MyData, Competency + $"{suffix}.ini")))
                                    {
                                        for (Int32 r = 0; r < testSize; r++)
                                        {
                                            //! CLassification.
                                            // 
                                            worksheet.Cells[rofs + r, cofs].Value = ini.ReadInteger($"Instance_{r + 1}", "Classified as", 0);

                                            //! Raw Data.
                                            Int32 tmp = 1;
                                            for (Int32 c = 1; c < ep.Workbook.Worksheets[Excel.ScratchPadName].Dimension.Columns + 1; c++)
                                            {
                                                if (obs.Contains(ep.Workbook.Worksheets[Excel.ScratchPadName].Cells[1, c].Value.ToString()))
                                                {
                                                    //! Copy some data into save results.
                                                    worksheet.Cells[rofs + r, cofs + tmp].Value = ep.Workbook.Worksheets[Excel.ScratchPadName].Cells[r + trainSize + 2, c].Value;
                                                    tmp += 1;
                                                }
                                            }
                                        }

                                        for (Int32 j = 0; j < obs.Count + 2; j++)
                                        {
                                            worksheet.Column(cofs + j).AutoFit();
                                        }
                                    }
                                }

                                package.Save();
                            }

                            //---R

                            for (Int32 i = 0; i < Data.unicompetencies.Count; i++)
                            {
                                Int32 rofs = 1;
                                Int32 cofs = 1;

                                //! 2. Uni-Dimensional.
                                //
                                String Competency = Data.unicompetencies[i].CompetencyName;

                                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(Competency);

                                ExcelTableCollection tblcollection = worksheet.Tables;

                                Logger.Info($"Building Report Sheet for Uni-Dimensional Competency: {Competency}.");

                                PropertyInfo[] props = Data.MLOptions.GetType().GetProperties();

                                //! Setup.
                                //
                                using (ExcelRange Rng = worksheet.Cells[rofs, cofs, rofs + 1 + props.Length, cofs + 1])
                                {
                                    ExcelTable table = tblcollection.Add(Rng, $"tbl_setup_{Competency}");
                                    table.ShowHeader = true;
                                    table.ShowFilter = false;
                                    table.TableStyle = TableStyles.Light20;

                                    worksheet.Cells[rofs, cofs + 0].Value = "Setting";
                                    worksheet.Cells[rofs, cofs + 1].Value = "Value";
                                    rofs++;

                                    worksheet.Cells[rofs, cofs + 0].Value = "Model Type";
                                    worksheet.Cells[rofs, cofs + 1].Value = "Uni-Dimensional Model";
                                    rofs++;

                                    foreach (PropertyInfo prop in props)
                                    {
                                        worksheet.Cells[rofs, cofs + 0].Value = prop.Name;

                                        object val = prop.GetValue(Data.MLOptions);
                                        worksheet.Cells[rofs, cofs + 1].Value = (Double.TryParse(val.ToString(), out Double dbl)
                                            ? dbl
                                            : val);
                                        rofs++;
                                    }

                                    for (Int32 j = 0; j < 3; j++)
                                    {
                                        worksheet.Column(cofs + j).AutoFit();
                                    }
                                }

                                rofs = 1;
                                cofs += 3;

                                //! Cache Observables.
                                // 
                                List<String> obs = new List<string>();

                                //! Model.
                                //
                                Int32 cnt = Data.unicompetencies[i].Length;
                                using (ExcelRange Rng = worksheet.Cells[rofs, cofs, rofs + cnt + 1, cofs + 1])
                                {
                                    ExcelTable table = tblcollection.Add(Rng, $"tbl_model_{Competency}");
                                    table.ShowHeader = true;
                                    table.ShowFilter = false;
                                    table.TableStyle = TableStyles.Light20;

                                    worksheet.Cells[rofs + 0, cofs + 0].Value = "Competency";
                                    worksheet.Cells[rofs + 0, cofs + 1].Value = "Observables";

                                    worksheet.Cells[rofs + 1, cofs + 0].Value = Data.unicompetencies[i].CompetencyName;

                                    rofs++;

                                    foreach (String o in Data.unicompetencies[i].Names)
                                    {
                                        obs.Add(o);

                                        worksheet.Cells[rofs + 1, cofs + 1].Value = o;
                                        rofs++;
                                    }

                                    for (Int32 j = 0; j < 3; j++)
                                    {
                                        worksheet.Column(cofs + j).AutoFit();
                                    }
                                }

                                rofs = 1;
                                cofs += 3;

                                //! Accuracy.
                                //
                                using (ExcelRange Rng = worksheet.Cells[rofs, cofs, rofs + cnt + 1, cofs + 6])
                                {
                                    ExcelTable table = tblcollection.Add(Rng, $"tbl_performance_{Competency}");
                                    table.ShowHeader = true;
                                    table.ShowFilter = false;
                                    table.TableStyle = TableStyles.Light20;

                                    worksheet.Cells[rofs + 0, cofs + 0].Value = "Accuracy(%)";
                                    worksheet.Cells[rofs + 0, cofs + 1].Value = "Error";
                                    worksheet.Cells[rofs + 0, cofs + 2].Value = "Kappa";
                                    worksheet.Cells[rofs + 0, cofs + 3].Value = "MAE";
                                    worksheet.Cells[rofs + 0, cofs + 4].Value = "RMSE";
                                    worksheet.Cells[rofs + 0, cofs + 5].Value = "RAE(%)";
                                    worksheet.Cells[rofs + 0, cofs + 6].Value = "RRSE(%)";

                                    using (IniFile ini = new IniFile(Path.Combine(MyData, Competency + $"{suffix}.ini")))
                                    {
                                        worksheet.Cells[rofs + 1, cofs + 0].Value = ini.ReadDouble("Performance", "Accuracy", 0) * 100;
                                        worksheet.Cells[rofs + 1, cofs + 1].Value = ini.ReadDouble("Performance", "Error", 0);
                                        worksheet.Cells[rofs + 1, cofs + 2].Value = ini.ReadDouble("Performance", "Kappa", 0);
                                        worksheet.Cells[rofs + 1, cofs + 3].Value = ini.ReadDouble("Performance", "MAE", 0);
                                        worksheet.Cells[rofs + 1, cofs + 4].Value = ini.ReadDouble("Performance", "RMSE", 0);
                                        worksheet.Cells[rofs + 1, cofs + 5].Value = ini.ReadDouble("Performance", "RAE(%)", 0);
                                        worksheet.Cells[rofs + 1, cofs + 6].Value = ini.ReadDouble("Performance", "RRSE(%)", 0);

                                        rofs++;
                                    }

                                    for (Int32 j = 0; j < 7; j++)
                                    {
                                        worksheet.Column(cofs + j).AutoFit();
                                    }
                                }

                                rofs = 1;
                                cofs += 8;

                                //! V&V
                                // 
                                using (ExcelRange Rng = worksheet.Cells[rofs, cofs, rofs + cnt + 1, cofs + 1])
                                {
                                    //! Header
                                    ExcelTable table1 = tblcollection.Add(Rng, $"tbl_VandV_{Competency}");
                                    table1.ShowHeader = true;
                                    table1.ShowFilter = false;
                                    table1.TableStyle = TableStyles.Light20;

                                    //! Header
                                    worksheet.Cells[rofs, cofs + 0].Value = "Cronbach";
                                    worksheet.Cells[rofs, cofs + 1].Value = "Spearman's Rho";

                                    rofs = 1;
                                    for (Int32 k = 0; k < Data.cronbachAlphaUni.Item1.Length; k++)
                                    {
                                        if (!String.IsNullOrEmpty(Data.cronbachAlphaUni.Item1[k])
                                            && Data.cronbachAlphaUni.Item1[k].Equals(Competency))
                                        {
                                            rofs++;

                                            worksheet.Cells[rofs, cofs + 0].Value = Data.cronbachAlphaUni.Item2[k];
                                        }
                                    }

                                    rofs = 1;
                                    for (Int32 k = 0; k < Data.spearmansUni.Item1.Length; k++)
                                    {
                                        if (!String.IsNullOrEmpty(Data.spearmansUni.Item1[k])
                                            && Data.spearmansUni.Item1[k].Equals(Competency))
                                        {
                                            rofs++;

                                            for (Int32 l = 0; l < Data.spearmansUni.Item2[k].Length; l++)
                                            {
                                                worksheet.Cells[rofs, cofs + 1].Value = Data.spearmansUni.Item2[k][l];
                                                rofs++;
                                            }
                                        }
                                    }

                                    for (Int32 j = 0; j < 2; j++)
                                    {
                                        worksheet.Column(cofs + j).AutoFit();
                                    }

                                    for (Int32 j = 0; j < 3; j++)
                                    {
                                        worksheet.Column(cofs + j).AutoFit();
                                    }
                                }

                                rofs = 1;
                                cofs += 3;

                                //! Raw Data
                                // 
                                using (ExcelRange Rng = worksheet.Cells[rofs, cofs, rofs + testSize, cofs + obs.Count])
                                {
                                    ExcelTable table1 = tblcollection.Add(Rng, $"tbl_raw_{Competency}");
                                    table1.ShowHeader = true;
                                    table1.ShowFilter = false;
                                    table1.TableStyle = TableStyles.Light20;

                                    //! Header
                                    worksheet.Cells[rofs, cofs].Value = "Classification";
                                    for (Int32 o = 0; o < obs.Count; o++)
                                    {
                                        worksheet.Cells[rofs, cofs + o + 1].Value = obs[o];
                                    }

                                    rofs++;

                                    using (IniFile ini = new IniFile(Path.Combine(MyData, Competency + $"{suffix}.ini")))
                                    {
                                        for (Int32 r = 0; r < testSize; r++)
                                        {
                                            //! CLassification.
                                            // 
                                            worksheet.Cells[rofs + r, cofs].Value = ini.ReadInteger($"Instance_{r + 1}", "Classified as", 0);

                                            //! Raw Data.
                                            Int32 tmp = 1;
                                            for (Int32 c = 1; c < ep.Workbook.Worksheets[Excel.ScratchPadName].Dimension.Columns + 1; c++)
                                            {
                                                if (obs.Contains(ep.Workbook.Worksheets[Excel.ScratchPadName].Cells[1, c].Value.ToString()))
                                                {
                                                    worksheet.Cells[rofs + r, cofs + tmp].Value = ep.Workbook.Worksheets[Excel.ScratchPadName].Cells[r + trainSize + 2, c].Value;
                                                    tmp += 1;
                                                }
                                            }
                                        }
                                    }

                                    for (Int32 j = 0; j < obs.Count + 2; j++)
                                    {
                                        worksheet.Column(cofs + j).AutoFit();
                                    }
                                }

                                package.Save();
                            }

                            //---R
                        }
                    }

                    if (File.Exists(saveFileDialog1.FileName))
                    {
                        try
                        {
                            Process.Start(saveFileDialog1.FileName);
                        }
                        catch (Win32Exception e1)
                        {
                            MessageBox.Show(e1.Message);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Event handler. Called by cancelBtn_Click for 1 events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        private void CancelBtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                $"Are you sure you want to cancel {Application.ProductName}?",
                Application.ProductName,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
                //StateMachine.stateless.Fire(StateMachine.Triggers.Cancel);
            }
        }

        /// <summary>
        /// Event handler. Called by Chart1 for click events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        private void Chart1_Click(object sender, EventArgs e)
        {
            // Dummy to enable mouse message processing and tooltips.
        }

        /// <summary>
        /// Event handler. Called by comboBox1 for selected index changed events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MLAlgoritmSelector.Visible)
            {
                propertyGrid1.SelectedObject = null;

                MLAlgorithms alg = (MLAlgorithms)((EnumPair)MLAlgoritmSelector.SelectedItem).Value;

                switch (alg)
                {
                    case MLAlgorithms.NaiveBayes:
                        Data.MLOptions = new NaiveBayesAlgorithm();
                        break;
                    case MLAlgorithms.DecisionTrees:
                        Data.MLOptions = new DecisionTreesAlgorithm();
                        break;
                }

                propertyGrid1.SelectedObject = Data.MLOptions;
            }
        }

        /// <summary>
        /// Event handler. Called by contextMenuStrip1 for opening events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Cancel event information. </param>
        private void ContextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ListView lv = (ListView)((ContextMenuStrip)sender).SourceControl;

            Boolean selected = (lv != null && lv.SelectedItems.Count == 1 && lv.SelectedItems[0].Group != null);

            addCompetencyToolStripMenuItem.Enabled = true;
            editCompetencyToolStripMenuItem.Enabled = true;
            removeCompetencyToolStripMenuItem.Enabled = true;

            toolStripSeparator3.Visible = lv == listView1;
            facetToolStripMenuItem.Visible = lv == listView1;

            addFacetToolStripMenuItem.Enabled = selected && lv == listView1;
            editFacetToolStripMenuItem.Enabled = selected && lv == listView1;
            removeFacetToolStripMenuItem.Enabled = selected && lv == listView1;
        }

        /// <summary>
        /// Data 2 visualization.
        /// </summary>
        private void Data2Visualization()
        {
            listView1.InvokeIfRequired(o =>
            {
                listView1.Items.Clear();
                listView1.Groups.Clear();

                //! Visualize the Competencies/Facets/Observables in ListView1.
                //
                for (Int32 c = 0; c < Data.competencies.Count; c++)
                {
                    //! 1) Multi-Dimensional.
                    //
                    Int32 gndx = listView1.Groups.IndexOf(
                                    listView1.Groups.Add(Data.competencies[c].CompetencyName, Data.competencies[c].CompetencyName));

                    for (Int32 f = 0; f < Data.competencies[c].Count(); f++)
                    {
                        ListViewItem lvi = listView1.Items.Add(Data.competencies[c][f].FacetName, Data.competencies[c][f].FacetName, 0);
                        lvi.Group = listView1.Groups[gndx];

                        String[] vars = Data.competencies[c][f].Names.ToArray();
                        lvi.ForeColor = vars.Length == 0
                            ? Color.Red
                            : listView1.ForeColor;
                        lvi.SubItems.Add(String.Join(",", vars));
                    }
                }

                listView1.Refresh();
            });

            listView2.InvokeIfRequired(o =>
            {
                listView2.Items.Clear();
                listView2.Groups.Clear();

                Int32 cndx = listView2.Groups.IndexOf(listView2.Groups.Add("Competencies", "Competencies"));

                //! Visualize the Competencies/Observables in ListView2.
                //
                for (Int32 c = 0; c < Data.unicompetencies.Count; c++)
                {
                    //! Uni-Dimensional.
                    //
                    ListViewItem lvi = listView2.Items.Add(Data.unicompetencies[c].CompetencyName, Data.unicompetencies[c].CompetencyName, 0);
                    lvi.Group = listView2.Groups[cndx];

                    //String[] vars = Data.uni UniEvidenceModel[c];

                    lvi.SubItems.Add(String.Join(",", Data.unicompetencies[c].Names));
                }

                listView2.Refresh();
            });
        }

        /// <summary>
        /// Event handler. Called by editToolStripMenuItem for click events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        private void EditCompetencyMenuItem_Click(object sender, EventArgs e)
        {
            ToolStrip ts = ((ToolStripItem)sender).OwnerItem.Owner;
            ListView lv = (ListView)((ContextMenuStrip)ts).SourceControl;

            Boolean UniDimensional = (lv == listView2);

            if (lv != null && lv.SelectedItems.Count == 1 && lv.SelectedItems[0].Group != null)
            {
                switch (UniDimensional)
                {
                    //! Checked.
                    //
                    case true:
                        {
                            String sub = lv.SelectedItems[0].SubItems.Count == 1
                                ? String.Empty
                                : lv.SelectedItems[0].SubItems[1]?.Text;

                            IEnumerable<String> items = Data.observables.Names;

                            IEnumerable<String> chekeditems = lv.SelectedItems[0].SubItems[1].Text.Split(',');

                            using (InputSelectDialog id = new InputSelectDialog("Edit Competency", "Enter the name of a Competency", lv.SelectedItems[0].Text, items, chekeditems))
                            {
                                id.SelectorEnabled = true;

                                if (id.ShowDialog() == DialogResult.OK)
                                {
                                    //! Update observables.
                                    lv.SelectedItems[0].Text = id.Input;
                                    lv.SelectedItems[0].SubItems[1].Text = String.Join(",", id.CheckedItems.Select(p => p.ToString()));
                                }
                            }
                        }
                        break;

                    //! Checked.
                    //
                    case false:
                        {
                            String oldgroup = lv.SelectedItems[0].Group.Name;

                            String sub = lv.SelectedItems[0].SubItems.Count == 1
                                ? String.Empty
                                : lv.SelectedItems[0].SubItems[1]?.Text;

                            IEnumerable<String> items = Data.observables.Names;

                            using (InputSelectDialog id = new InputSelectDialog("Edit Competency", "Enter the name of a Competency", lv.SelectedItems[0].Group.Header, items, Array.Empty<string>()))
                            {
                                id.SelectorEnabled = false;

                                if (id.ShowDialog() == DialogResult.OK)
                                {
                                    //! Add facet.
                                    lv.Groups[oldgroup].Header = id.Input;
                                    lv.Groups[oldgroup].Name = id.Input;
                                }
                            }
                        }
                        break;
                }

            }

            StateMachine.Flags[StateMachine.CONFIGURE_ECD] = ValidateCompetencies();

            StateMachine.UpdateControls();
        }

        /// <summary>
        /// Event handler. Called by editToolStripMenuItem1 for click events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        private void EditFacetMenuItem_Click(object sender, EventArgs e)
        {
            ToolStrip ts = ((ToolStripItem)sender).OwnerItem.Owner;
            ListView lv = (ListView)((ContextMenuStrip)ts).SourceControl;

            Boolean UniDimensional = (lv == listView2);

            if (lv != null && lv.SelectedItems.Count == 1 && lv.SelectedItems[0].Group != null)
            {
                switch (UniDimensional)
                {
                    //! Checked.
                    //
                    case true:
                        {
                            //! Should not occur for listView2.
                        }
                        break;

                    //! Checked.
                    //
                    case false:
                        {
                            Int32 ndx = lv.SelectedItems[0].Index;

                            IEnumerable<String> items = Data.observables.Names;
                            String sub = lv.SelectedItems[0].SubItems.Count == 1
                                ? String.Empty
                                : lv.SelectedItems[0].SubItems[1]?.Text;
                            IEnumerable<String> chekeditems = String.IsNullOrEmpty(sub)
                                ? Array.Empty<string>()
                                : lv.SelectedItems[0].SubItems[1].Text.Split(',');

                            using (InputSelectDialog id = new InputSelectDialog("Edit Facet", "Enter the new name of a Facet", lv.SelectedItems[0].Text, items, chekeditems))
                            {
                                id.SelectorEnabled = true;

                                if (id.ShowDialog() == DialogResult.OK)
                                {
                                    String fid = id.Input;

                                    //! Add new Facet to Competency Group.
                                    ListViewItem lvi = lv.Items[ndx];

                                    lvi.Text = id.Input;
                                    lvi.Name = id.Input;
                                    lvi.ForeColor = id.CheckedItems.Count() == 0
                                           ? Color.Red
                                           : listView1.ForeColor;
                                    lvi.SubItems[1].Text = String.Join(",", id.CheckedItems.Select(p => p.ToString()));
                                }
                            }
                        }
                        break;
                }
            }

            StateMachine.Flags[StateMachine.CONFIGURE_ECD] = ValidateCompetencies();

            StateMachine.UpdateControls();
        }

        /// <summary>
        /// Event handler. Called by groupedComboBox1 for selected index changed events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        private void GroupedComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (groupedComboBox1.SelectedItem != null)
            {
                //TODO Always expecting multidimensional.
                String Competency = (groupedComboBox1.SelectedItem as GCI).Group;
                String Facet = (groupedComboBox1.SelectedItem as GCI).Value;
                Boolean IsCompetency = (groupedComboBox1.SelectedItem as GCI).IsCompetency;

                //Debug.WriteLine($"{Competency} - {Facet}");

                chart1.Series.Clear();
                chart1.ChartAreas.Clear();

                chart1.Series.Add("Default");
                chart1.ChartAreas.Add("Default");

                chart1.Series["Default"].ChartType = SeriesChartType.Column;
                chart1.Series["Default"]["PointWidth"] = "0.9";

                for (Int32 i = 0; i <= 2; i++)
                {
                    chart1.Series["Default"].Points.AddXY(i, 0);
                    chart1.ChartAreas["Default"].AxisX.CustomLabels.Add(-0.4 + i, i + 0.4, i.ToString()).GridTicks = GridTickTypes.TickMark;
                }

                using (chart1.DoBatch())
                {
                    MLAlgorithms alg = (MLAlgorithms)((EnumPair)MLAlgoritmSelector.SelectedItem).Value;

                    String suffix = String.Empty;

                    switch (alg)
                    {
                        case MLAlgorithms.NaiveBayes:
                            suffix = "_BayesAccord";
                            break;
                        case MLAlgorithms.DecisionTrees:
                            suffix = "_DecisionTreesAccord";
                            break;
                    }

                    if (IsCompetency)
                    {
                        //! Note: The GCI.Facet member contains the Competency Name.
                        // 
                        using (IniFile ini = new IniFile(Path.Combine(MyData, Facet + $"{suffix}.ini")))
                        {
                            for (Int32 i = 0; i < 3; i++)
                            {
                                chart1.Series["Default"].Points[i].YValues[0] = ini.ReadInteger("Summary", $"Classified as {i}", 0);
                            }
                        }
                    }
                    else
                    {
                        using (IniFile ini = new IniFile(Path.Combine(MyData, Competency, Facet + $"{suffix}.ini")))
                        {
                            for (Int32 i = 0; i < 3; i++)
                            {
                                chart1.Series["Default"].Points[i].YValues[0] = ini.ReadInteger("Summary", $"Classified as {i}", 0);
                            }
                        }
                    }
                    //! Show Tooltips on legend and bars.
                    //
                    chart1.Series["Default"].LegendToolTip = "#TOTAL{D0}";
                    chart1.Series[0].ToolTip = "#VAL{D0}";
                }

                chart1.Invalidate();
            }
        }

        /// <summary>
        /// Event handler. Called by helpBtn for click events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        private void HelpBtn_Click(object sender, EventArgs e)
        {
            //! See https://docs.microsoft.com/en-us/dotnet/framework/winforms/controls/helpprovider-component-overview-windows-forms
            //helpProvider1.

            //! See https://stackoverflow.com/questions/22300244/open-a-chm-file-to-a-specific-topic
            foreach (StateMachine.States step in Enum.GetValues(typeof(StateMachine.States)))
            {
                if (StateMachine.stateless.IsInState(step))
                {
                    Help.ShowHelp(this, "Smart-CAT.chm", HelpNavigator.Topic, $"/Topics/Panel{((int)step) + 1}.htm");
                }
            }

            // mk:@MSITStore:Smart-CAT.chm::/Topics/Panel1.htm
            // 
            //PopupNotifier popupNotifier = new PopupNotifier();

            //popupNotifier.ShowCloseButton = true;
            //popupNotifier.ShowOptionsButton = true;
            //popupNotifier.ShowGrip = true;
            //popupNotifier.ContentPadding = new Padding(10);
            //popupNotifier.TitleText = "Smart-CAT Help";
            //popupNotifier.ContentText = LorumnIpsum;
            //popupNotifier.IsRightToLeft = false;
            //popupNotifier.Popup();

            // This loads a chm file
            // this.helpProvider1.HelpNamespace = "C:\\YourFile.chm";

            //! See https://stackoverflow.com/questions/16463599/popup-window-in-winform-c-sharp

            //! Notification Icon (bottom right of the screen).
            //
            //this.notifyIcon1.BalloonTipText = LorumnIpsum;
            //this.notifyIcon1.BalloonTipTitle = "Lorem Ipsum";
            ////this.notifyIcon1.Icon = new Icon("icon.ico");
            //this.notifyIcon1.Visible = true;
            //this.notifyIcon1.ShowBalloonTip(10000);

            //! Tooltip, very temporary.
            //
            //ToolTip toolTip1 = new ToolTip();

            //// Set up the delays for the ToolTip.
            //toolTip1.AutoPopDelay = 5000;
            //toolTip1.InitialDelay = 1000;
            //toolTip1.ReshowDelay = 500;

            //// Force the ToolTip text to be displayed whether or not the form is active.
            //toolTip1.ShowAlways = true;

            //// Set up the ToolTip text for the Button.
            ////! No Word Wrapping.
            //toolTip1.SetToolTip(helpBtn, LorumnIpsum);
        }

        /// <summary>
        /// Event handler. Called by ImportBtn for click events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        private void ImportBtn_Click(object sender, EventArgs e)
        {
            //! ExceptionDlg Test Code.
            //Int32 a = 0; a = a / a;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StateMachine.Flags[StateMachine.IMPORT_DATA] = false;

                if (Excel.IsFileLocked(openFileDialog1.FileName))
                {
                    MessageBox.Show(String.Format("Could not open locked file:\r\n\r\n{0}", Path.GetFileName(openFileDialog1.FileName)), IniFile.AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    //  [Smart-CAT Projects]
                    //     [sheet name]
                    //         sheet
                    //         json
                    //             [timestamp]
                    //                  copy sheet
                    //                  copy json
                    //                  [data]

                    // Create Timestamp for copies.
                    Excel.Stamp = $"{DateTime.Now:yyyyMMdd-HHmmss}";

                    StateMachine.Flags[StateMachine.IMPORT_DATA] = true;

                    // Create project folder.
                    MyProject = Path.Combine(MyProjects, Path.GetFileNameWithoutExtension(openFileDialog1.FileName));
                    if (!Directory.Exists(MyProject))
                    {
                        Directory.CreateDirectory(MyProject);
                        Logger.Info($"Created Project Folder <My Document>{MyProject.Replace(MyProjects, String.Empty)}.");
                    }

                    MyInput = Path.Combine(MyProject, Excel.Stamp, Path.GetFileName(openFileDialog1.FileName));

                    // Create data folder in project folder if not present already.
                    MyData = Path.Combine(MyProject, Excel.Stamp, "data");
                    if (!Directory.Exists(MyData))
                    {
                        Directory.CreateDirectory(MyData);
                        Logger.Info($"Created Run Folder <My Document>{MyProject.Replace(MyProjects, String.Empty)}\\{Excel.Stamp}.");
                    }

                    // Clear data folder if present.
                    foreach (String file in Directory.EnumerateFiles(MyData, "*", SearchOption.AllDirectories))
                    {
                        File.Delete(file);
                    }
                    foreach (String folder in Directory.EnumerateDirectories(MyData, "*", SearchOption.AllDirectories))
                    {
                        Directory.Delete(folder);
                    }

                    if (!File.Exists(MyInput))
                    {
                        Logger.Info("Copying Input File to Project Folder.");

                        File.Copy(openFileDialog1.FileName, MyInput);
                    }
                    else
                    {
#warning TODO - OVERWRITE QUESTION?
                        Logger.Warn("Input File already in the Project Folder, using Project one.");
                    }

                    //! Set Initial Directories to match the current project.
                    // 
                    saveFileDialog1.InitialDirectory = MyProject;
                    saveFileDialog2.InitialDirectory = MyProject;
                    openFileDialog2.InitialDirectory = MyProject;
                    openFileDialog3.InitialDirectory = MyProject;

                    Text = $"{Application.ProductName} - {Path.GetFileName(MyInput)} - [{Excel.Stamp}]";

                    StateMachine.exitWorkers[StateMachine.States.ImportData].argument = MyInput;
                    StateMachine.exitWorkers[StateMachine.States.ConfigureECD].argument = MyInput;
                    StateMachine.exitWorkers[StateMachine.States.OptimizeML].argument = MyInput;

                    //! Clear Model after loading a new input file.
                    // 
                    Data.competencies = new Competencies();
                    Data.unicompetencies = new UniCompetencies();
                    //Data.StatisticalSubmodel = Array.Empty<string[][]>();
                }
            }

            StateMachine.UpdateControls();
        }

        /// <summary>
        /// Event handler. Called by LoadToolStripMenuItem for click events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        private void LoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                Data.LoadECD(openFileDialog2.FileName);

                Data2Visualization();

                StateMachine.Flags[StateMachine.CONFIGURE_ECD] = ValidateCompetencies();

                StateMachine.UpdateControls();
            }
        }

        /// <summary>
        /// Event handler. Called by MainForm for form closing events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Form closing event information. </param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //ConsoleDialog.Visible = false;
        }

        /// <summary>
        /// Event handler. Called by MainForm for load events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            ExceptionDlg.InitializeExceptionHandlers(true);

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            Logger.Init(errorBox);

            panelManager1.Width = errorBox.Width;
            errorBox.Columns[1].Width = errorBox.Width - errorBox.Columns[0].Width - 32;

            propertyGrid1.Size = new Size(409, 223);
            propertyGrid2.Size = new Size(409, 223);
            propertyGrid2.Hide();

            //Logger.Info("Information");
            //Logger.Warn("Warning");
            //Logger.Error("Error");

            //! Add Console
            //ConsoleDialog.Show();

            //! Fix new VS2017 behavior (redirecting all to the output window instead of the Console(Dialog)).
            //ConsoleDialog.ReclaimStdOutHandle();

            //Debug.Listeners.Add(consoleListener);

            //ConsoleDialog.OnChar += new ConsoleDialog.OnCharDelegate(OnChar);
            //ConsoleDialog.Caption = this.Text + " - Interactive Console";

            //ConsoleDialog.Escape = false;
            //ConsoleDialog.Cursor = true;
            //ConsoleDialog.CursorSize = 5;

            //ConsoleDialog.ForegroundColor = ConsoleColor.Yellow;

            Text = Application.ProductName;

            //! Check R Version and Install & Load packages needed.
            // 
            StateMachine.Flags[StateMachine.INIT_R] = Utils.InitRengine(progressLbl, busyBar);
            StateMachine.UpdateControls();

            //! veg 15-07-2019
            //
            //! NOTE: We need an R version that allows Metrics 1.2 or higher.
            //
            label5.Text = $"Using R v{Utils.engine.DllVersion}";
            //label8.Text = $"Using weka v{new weka.core.Version()}";
            //label11.Text = $"Using IKVM v{FileVersionInfo.GetVersionInfo("IKVM.Runtime.dll").ProductVersion}";

            MLAlgoritmSelector.DataSource = AlgorithmItems;

            //openFileDialog1.InitialDirectory = Path.Combine(IniFile.AppDir, "ConfigFiles");
            //openFileDialog1.FileName = "GameLogs.xlsx";

            Logger.Info($"Loading default ECD from '{Utils.MakePathRelative(Path.Combine(IniFile.AppData, IniFile.AppIni))}'.");
        }

        /// <summary>
        /// Event handler. Called by NewBtn for click events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        private void NewBtn_Click(object sender, EventArgs e)
        {
            StateMachine.Flags.Clear();
            StateMachine.Flags.Add(StateMachine.INIT_R, true);

            StateMachine.stateless.Fire(StateMachine.Triggers.New);
        }

        /// <summary>
        /// Event handler. Called by nextBtn for click events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        private void NextBtn_Click(object sender, EventArgs e)
        {
            StateMachine.stateless.Fire(StateMachine.Triggers.Next);
        }

        /// <summary>
        /// Raises the key press event.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information to send to registered event handlers. </param>
        private void OnChar(object sender, KeyPressEventArgs e)
        {
            //ConsoleDialog.Write(e.KeyChar);
        }

        /// <summary>
        /// Event handler. Called by panelManager1 for selected index changed events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        private void PanelManager1_SelectedIndexChanged(object sender, EventArgs e)
        {
            splitContainer1.Panel1.Invalidate();

            switch (panelManager1.ManagedPanels.IndexOf(panelManager1.SelectedPanel))
            {
                //! Panel 2
                case (Int32)StateMachine.States.ImportData:
                    break;

                //! Panel 3
                case (Int32)StateMachine.States.ConfigureECD:
                    break;

                //! Panel 4
                case (Int32)StateMachine.States.OptimizeML:
                    ComboBox1_SelectedIndexChanged(MLAlgoritmSelector, EventArgs.Empty);
                    break;

                //! Panel 5
                case (Int32)StateMachine.States.SupportFunction:
#warning TODO Calculate and Store Rho and Alpha.
                    propertyGrid2.SelectedObject = new VandVOptions(Double.NaN, Double.NaN);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Event handler. Called by prevBtn for click events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        private void PrevBtn_Click(object sender, EventArgs e)
        {
            StateMachine.stateless.Fire(StateMachine.Triggers.Previous);
        }

        /// <summary>
        /// Event handler. Called by removeCompetencyMenuItem for click events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        private void RemoveCompetencyMenuItem_Click(object sender, EventArgs e)
        {
            ToolStrip ts = ((ToolStripItem)sender).OwnerItem.Owner;
            ListView lv = (ListView)((ContextMenuStrip)ts).SourceControl;

            Boolean UniDimensional = (lv == listView2);

            //! Should not occur for lsitView2.
            //
            if (lv != null && lv.SelectedItems.Count == 1 && lv.SelectedItems[0].Group != null)
            {
                switch (UniDimensional)
                {
                    //! Checked
                    //
                    case true:
                        {
                            String competency = lv.SelectedItems[0].Text;
                            if (MessageBox.Show(
                              $"Remove: {competency}?",
                              "Remove Compentency",
                              MessageBoxButtons.OKCancel,
                              MessageBoxIcon.Question) == DialogResult.OK)
                            {
                                //! Remove Competency.
                                lv.Items.Remove(lv.SelectedItems[0]);
                            }
                        }
                        break;

                    //! Checked
                    //
                    case false:
                        {
                            String group = lv.SelectedItems[0].Group.Name;
                            Int32 ndx = lv.Groups.IndexOf(lv.SelectedItems[0].Group);

                            if (MessageBox.Show(
                                    $"Remove: {group}?",
                                    "Remove Compentency and it's Facets",
                                    MessageBoxButtons.OKCancel,
                                    MessageBoxIcon.Question) == DialogResult.OK)
                            {
                                //! Remove Competency Group Facets.
                                while (lv.Groups[ndx].Items.Count != 0)
                                {
                                    lv.Items.Remove(lv.Groups[ndx].Items[0]);
                                }

                                //! Remove Competency Group.
                                lv.Groups.RemoveAt(ndx);
                            }
                        }
                        break;
                }
            }

            StateMachine.Flags[StateMachine.CONFIGURE_ECD] = ValidateCompetencies();

            StateMachine.UpdateControls();
        }

        /// <summary>
        /// Event handler. Called by removeToolStripMenuItem1 for click events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        private void RemoveFacetMenuItem1_Click(object sender, EventArgs e)
        {
            ToolStrip ts = ((ToolStripItem)sender).OwnerItem.Owner;
            ListView lv = (ListView)((ContextMenuStrip)ts).SourceControl;

            Boolean UniDimensional = (lv == listView2);

            if (lv != null && lv.SelectedItems.Count == 1 && lv.SelectedItems[0].Group != null)
            {
                Int32 ndx = lv.SelectedItems[0].Index;
                //String group = lv.SelectedItems[0].Group.Name;
                Int32 gndx = lv.Groups.IndexOf(lv.SelectedItems[0].Group);

                switch (UniDimensional)
                {
                    //! Checked
                    //
                    case true:
                        {
                            //! Should not occur for listView2.
                        }
                        break;

                    //! Checked
                    //
                    case false:
                        {
                            if (MessageBox.Show(
                                    $"Remove: {lv.SelectedItems[0].Text}?",
                                    "Remove Facet",
                                    MessageBoxButtons.OKCancel,
                                    MessageBoxIcon.Question) == DialogResult.OK)
                            {
                                //! Remove Facet Item.
                                lv.Items.RemoveAt(ndx);

                                //! Remove empty Competency Group.
                                if (lv.Groups[gndx].Items.Count == 0)
                                {
                                    lv.Groups.RemoveAt(gndx);
                                }
                            }
                        }
                        break;
                }
            }

            StateMachine.Flags[StateMachine.CONFIGURE_ECD] = ValidateCompetencies();

            StateMachine.UpdateControls();
        }

        /// <summary>
        /// Event handler. Called by SaveToolStripMenuItem for click events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                Visualization2Data();

                Data.SaveECD(saveFileDialog2.FileName);
            }
        }

        /// <summary>
        /// Sets help strings.
        /// </summary>
        private void SetHelpStrings()
        {
            helpProvider1.HelpNamespace = Path.Combine(IniFile.AppDir, "Smart-CAT.chm");
            //helpProvider1.SetHelpString(nextBtn, LorumnIpsum);
            helpProvider1.SetHelpNavigator(helpBtn, HelpNavigator.TableOfContents);
        }

        /// <summary>
        /// Event handler. Called by SplitContainer1_Panel1 for paint events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Paint event information. </param>
        private void SplitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {
            SizeF X = e.Graphics.MeasureString("X", Font);
            float Hspacing = X.Width;
            float Vspacing = X.Height;

            foreach (StateMachine.States step in Enum.GetValues(typeof(StateMachine.States)))
            {
                Int32 ndx = (Int32)step + 1;
                e.Graphics.DrawString(
                    $"{(Int32)ndx}) {step.GetDescription()}",
                    StateMachine.stateless.IsInState(step)
                    ? SelectedFont
                    : Font,
                    new SolidBrush(Color.Black),
                    new PointF(2 * Hspacing, 2 * ndx * Vspacing));
            }
        }

        /// <summary>
        /// Event handler. Called by Step1BackgroundWorker for do work events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Do work event information. </param>
        private void Step1_DoWork(object sender, DoWorkEventArgs e)
        {
            StateMachine.Flags[StateMachine.STEP1_COMPLETED] = false;

            //ConsoleDialog.HighVideo();
            //Debug.WriteLine($"[{Extensions.GetCurrentMethod()}]");
            //ConsoleDialog.NormVideo();

            //! Load the DEFAULT ECD configuration
            LoadDefaultECD();

            Data.DumpStatisticalSubModels();
        }

        /// <summary>
        /// Event handler. Called by Step1 for progress changed events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Progress changed event information. </param>
        private void Step1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            switch (e.ProgressPercentage)
            {
                default:
                    progressLbl.InvokeIfRequired(o =>
                    {
                        progressLbl.Text = String.Empty;
                    });
                    break;
            }
        }

        /// <summary>
        /// Event handler. Called by Step1BackgroundWorker for run worker completed events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Run worker completed event information. </param>
        private void Step1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (busyBar.Visible)
            {
                busyBar.Hide();
            }

            StateMachine.Flags[StateMachine.STEP1_COMPLETED] = true;

            StateMachine.UpdateControls();
        }

        /// <summary>
        /// Event handler. Called by Step2BackgroundWorker for do work events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Do work event information. </param>
        private void Step2_DoWork(object sender, DoWorkEventArgs e)
        {
            Logger.Info("Step2");

            StateMachine.Flags[StateMachine.STEP2_COMPLETED] = false;

            String filename = e.Argument.ToString();

            (sender as BackgroundWorker).ReportProgress(1);

            //! Disabked for now.
            //Data.LoadDataAsDefault(Path.ChangeExtension(filename, ".ini"));

            //! Load all the available game logs.
            // 
            Logger.Info($"Loading Game Logs from: '{Utils.MakePathRelative(filename)}'.");

            Debug.Write("Loading all game logs... ");
            Data.observables = BayesNet.LoadAllData(filename);
            Debug.WriteLine("Completed.\r\n");

            //Data.DumpObservables();

            //! Load only the instances for the declared statistical sub-models.
            Debug.Write("Loading instances for the declared statistical submodels... ");
            Data.Inst = BayesNet.LoadInstances(Data.observables, Data.competencies.ToTuple(), Data.competencies.ToStatisticalSubmodel());
            Data.InstUni = BayesNet.LoadInstancesUni(Data.observables, Data.unicompetencies.ToTuple(), Data.unicompetencies.ToUniEvidenceModel());
            Debug.WriteLine("Completed.\r\n");

            //! Visualize ECD Configuration using two ListViews.
            //
            (sender as BackgroundWorker).ReportProgress(2);

            Data2Visualization();

            StateMachine.Flags[StateMachine.CONFIGURE_ECD] = ValidateCompetencies();

            StateMachine.UpdateControls();

            (sender as BackgroundWorker).ReportProgress(3);

            //! Normalization of the instances for the declared statistical submodels.
            Debug.Write("Normalisation of instances... ");
            Data.Instances = BayesNet.Normalisation(Data.Inst.facets);
            Data.InstancesUni = BayesNet.NormalisationUni(Data.InstUni.Item1);
            Debug.WriteLine("Completed.\r\n");

            (sender as BackgroundWorker).ReportProgress(4);

            //! Check for labeled data.
            Debug.Write("Checking for labeled data to decide ML approach... ");
            Data.CheckLabels = BayesNet.CheckLabelling(Data.observables, Data.competencies.ToTuple());
            Data.CheckLabelsUni = BayesNet.CheckLabellingUni(Data.observables, Data.unicompetencies.ToTuple());
            Debug.WriteLine("Completed.\r\n");

            (sender as BackgroundWorker).ReportProgress(5);

            //! Retrieve labeled data.
            Debug.Write("Retrieving labeled data...");
            Data.LabelledData = BayesNet.GetLabelledData(Data.competencies.ToTuple(), Data.observables);
            Data.LabelledDataUni = BayesNet.GetLabelledDataUni(Data.unicompetencies.ToTuple(), Data.observables);
            Debug.WriteLine("Completed.\r\n");
        }

        /// <summary>
        /// Event handler. Called by Step2 for progress changed events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Progress changed event information. </param>
        private void Step2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            switch (e.ProgressPercentage)
            {
                case 1:
                    progressLbl.InvokeIfRequired(o =>
                    {
                        progressLbl.Text = "Loading data";
                    });
                    break;
                case 2:
                    progressLbl.InvokeIfRequired(o =>
                    {
                        progressLbl.Text = "Visualizing statistical submodel";
                    });
                    break;
                case 3:
                    progressLbl.InvokeIfRequired(o =>
                    {
                        progressLbl.Text = "Normalizing of instances";
                    });
                    break;
                case 4:
                    progressLbl.InvokeIfRequired(o =>
                    {
                        progressLbl.Text = "Check for labeled data";
                    });
                    break;
                case 5:
                    progressLbl.InvokeIfRequired(o =>
                    {
                        progressLbl.Text = "Load labeled data";
                    });
                    break;
            }
        }

        /// <summary>
        /// Event handler. Called by Step2BackgroundWorker for run worker completed events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Run worker completed event information. </param>
        private void Step2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (busyBar.Visible)
            {
                busyBar.Hide();
            }
            progressLbl.Text = String.Empty;

            StateMachine.Flags[StateMachine.STEP2_COMPLETED] = true;

            StateMachine.UpdateControls();
        }

        /// <summary>
        /// Event handler. Called by Step3BackgroundWorker for do work events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Do work event information. </param>
        private void Step3_DoWork(object sender, DoWorkEventArgs e)
        {
            Logger.Info("Step3");

            StateMachine.Flags[StateMachine.STEP3_COMPLETED] = false;

            String filename = e.Argument.ToString();
            String filenameTS = Excel.WorkingCopyFileName(filename);

            (sender as BackgroundWorker).ReportProgress(1);

            Debug.Write("Loading edited statistical submodels... ");

            Visualization2Data();

            (sender as BackgroundWorker).ReportProgress(2);

            Data.DumpStatisticalSubModels();

            Data.SaveModelToExcel(filenameTS);

#warning Competenses & Facets from model are not saved.

            Data.SaveECD(Path.ChangeExtension(filename, ".json"));

            //! Needs to move to after the ML Options.
            //
            Data.CheckLabels = BayesNet.CheckLabelling(Data.observables, Data.competencies.ToTuple());

            Data.CheckLabelsUni = BayesNet.CheckLabellingUni(Data.observables, Data.unicompetencies.ToTuple());

            if (!BayesNet.AllLabelled(Data.CheckLabels))
            {
                (sender as BackgroundWorker).ReportProgress(3);

                {
                    //! Redo part of Step1 as StatisticalSubmodel may have changed.
                    Debug.Write("Loading instances for the declared statistical submodels... ");
                    Data.Inst = BayesNet.LoadInstances(Data.observables, Data.competencies.ToTuple(), Data.competencies.ToStatisticalSubmodel());
                    Data.InstUni = BayesNet.LoadInstancesUni(Data.observables, Data.unicompetencies.ToTuple(), Data.unicompetencies.ToUniEvidenceModel());
                    Debug.WriteLine("Completed.\r\n");

                    //! Normalization of the instances for the declared statistical submodels.
                    Debug.Write("Normalisation of instances... ");
                    Data.Instances = BayesNet.Normalisation(Data.Inst.facets);
                    Data.InstancesUni = BayesNet.NormalisationUni(Data.InstUni.Item1);
                    Debug.WriteLine("Completed.\r\n");
                }

                {
                    //! Retrieve labeled data.
                    Debug.Write("Retrieving labeled data...");
                    Data.LabelledData = BayesNet.GetLabelledData(Data.competencies.ToTuple(), Data.observables);
                    Data.LabelledDataUni = BayesNet.GetLabelledDataUni(Data.unicompetencies.ToTuple(), Data.observables);
                    Debug.WriteLine("Completed.\r\n");

                    //! Generate .arff files for facets.
                    Debug.WriteLine("Generating .arff files for the declared facets...");
                    BayesNet.GenerateArffFilesForFacets(MyData, Data.competencies.ToTuple(), Data.competencies.ToStatisticalSubmodel(), Data.Instances, Data.CheckLabels, Data.LabelledData);
                    Debug.WriteLine("Generation of .arff files completed.\r\n");

                    //! Select ML algorithms for the declared facets.
                    //
                    Debug.WriteLine("Select ML algoritms for te declared facets....");
                    Data.LabelledOutputF = BayesNet.SelectLabelsforFacets(MyData, Data.competencies.ToTuple(), Data.LabelledData);
                    Debug.WriteLine("Selection completed.\r\n");

                    //! Generate .arff files for competencies.
                    Debug.WriteLine("Generating .arff files for the declared competencies...");
                    BayesNet.GenerateArffFilesForCompetencies(MyData, Data.competencies.ToTuple(), Data.competencies.ToStatisticalSubmodel(), Data.CheckLabels, Data.LabelledData);
                    BayesNet.GenerateArffFilesForUniCompetencies(MyData, Data.unicompetencies.ToTuple(), Data.unicompetencies.ToUniEvidenceModel(), Data.InstancesUni, Data.CheckLabelsUni, Data.LabelledDataUni);
                    Debug.WriteLine("Generation of .arff files completed.\r\n");


                    //! Select ML algorithms for the declared competencies.
                    //
                    Debug.WriteLine("Select ML algoritms for te declared competencies....");
                    Data.LabelledOutputC = BayesNet.SelectLabelsforCompetencies(MyData, Data.competencies.ToTuple(), Data.LabelledData);
                    Data.UniLabelledOutputC = BayesNet.SelectLabelsforUniCompetencies(MyData, Data.unicompetencies.ToTuple(), Data.LabelledDataUni);
                    Debug.WriteLine("Selection completed.\r\n");
                }

                {
                    Data.OutputLabels = (competencies: Data.LabelledOutputC.output, facets: Data.LabelledOutputF.output);
                }

                (sender as BackgroundWorker).ReportProgress(4);

                //! Write Labels on Excel file.
                {
                    Debug.WriteLine("Adding labels for the declared facets to Excel file...");
                    Excel.AddLabelsforFacets(Data.competencies.ToTuple(), Data.CheckLabels, Data.OutputLabels, filenameTS);
                    Debug.WriteLine("Adding labels for the declared competencies to Excel file...");
                    Excel.AddLabelsforCompetencies(Data.competencies.ToTuple(), Data.CheckLabels, Data.OutputLabels, filenameTS);
                    Debug.WriteLine("Adding labels for the declared uni-competencies to Excel file...");
                    Excel.AddLabelsforUniCompetencies(Data.unicompetencies.ToTuple(), Data.CheckLabelsUni, Data.UniLabelledOutputC.Item3, filenameTS);
                    Debug.WriteLine("Adding labels completed.\r\n");

                    Data.observables = BayesNet.LoadAllData(filename);

                    Data.CheckLabels = BayesNet.CheckLabelling(Data.observables, Data.competencies.ToTuple());
                    Data.CheckLabelsUni = BayesNet.CheckLabellingUni(Data.observables, Data.unicompetencies.ToTuple());
                }
            }

            Debug.WriteLine("Data saved as default.\r\n");
        }

        /// <summary>
        /// Event handler. Called by Step3 for progress changed events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Progress changed event information. </param>
        private void Step3_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            switch (e.ProgressPercentage)
            {
                case 1:
                    progressLbl.InvokeIfRequired(o =>
                    {
                        progressLbl.Text = "Loading edited statistical submodels";
                    });
                    break;
                case 2:
                    progressLbl.InvokeIfRequired(o =>
                    {
                        progressLbl.Text = "Checking labels";
                    });
                    break;
                case 3:
                    progressLbl.InvokeIfRequired(o =>
                    {
                        progressLbl.Text = "Preparing for adding missing labels";
                    });
                    break;
                case 4:
                    progressLbl.InvokeIfRequired(o =>
                    {
                        progressLbl.Text = "Adding missing labels";
                    });
                    break;
                default:
                    progressLbl.InvokeIfRequired(o =>
                    {
                        progressLbl.Text = String.Empty;
                    });
                    break;

            }
        }

        /// <summary>
        /// Event handler. Called by Step3BackgroundWorker for run worker completed events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Run worker completed event information. </param>
        private void Step3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (busyBar.Visible)
            {
                busyBar.Hide();
            }
            progressLbl.Text = String.Empty;

            StateMachine.Flags[StateMachine.STEP3_COMPLETED] = true;

            StateMachine.UpdateControls();
        }

        /// <summary>
        /// Event handler. Called by Step4BackgroundWorker for do work events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Do work event information. </param>
        private void Step4_DoWork(object sender, DoWorkEventArgs e)
        {
            Logger.Info("Step4");

            StateMachine.Flags[StateMachine.STEP4_COMPLETED] = false;

            String filename = e.Argument.ToString();

            {
                //! Redo part of Step1 as StatisticalSubmodel may have changed.

                Debug.Write("Loading instances for the declared statistical submodels...");
                Data.Inst = BayesNet.LoadInstances(Data.observables, Data.competencies.ToTuple(), Data.competencies.ToStatisticalSubmodel());
                Debug.WriteLine("Completed.\r\n");

                //! Normalization of the instances for the declared statistical submodels.
                Debug.Write("Normalisation of instances... ");
                Data.Instances = BayesNet.Normalisation(Data.Inst.facets);
                Debug.WriteLine("Completed.\r\n");

                //! Check for labeled data.
                Debug.Write("Checking for labeled data to decide ML approach... ");
                Data.CheckLabels = BayesNet.CheckLabelling(Data.observables, Data.competencies.ToTuple());
                Debug.WriteLine("Completed.\r\n");

                //! Retrieve labeled data.
                Debug.Write("Retrieving labeled data...");
                Data.LabelledData = BayesNet.GetLabelledData(Data.competencies.ToTuple(), Data.observables);
                Debug.WriteLine("Completed.\r\n");
            }

            Logger.Info("Assessing Reliability.");

            //Run reliability analysis for multi-dimensional competencies.
            Data.cronbachAlphaMulti = BayesNet.ReliabilityAnalysisMulti(Data.competencies.ToTuple(), Data.competencies.ToStatisticalSubmodel(), Data.Inst.observables);

            //Run reliability analysis for uni-dimensional competencies.
            Data.cronbachAlphaUni = BayesNet.ReliabilityAnalysisUni(Data.unicompetencies.ToTuple(), Data.InstUni.Item2, Data.unicompetencies.ToUniEvidenceModel());

            //! Generate .arff files for facets.
            Debug.WriteLine("Generating .arff files for the declared facets...");
            BayesNet.GenerateArffFilesForFacets(MyData, Data.competencies.ToTuple(), Data.competencies.ToStatisticalSubmodel(), Data.Instances, Data.CheckLabels, Data.LabelledData);
            Debug.WriteLine("Generation of .arff files completed.\r\n");

            //! Select ML algorithms for the declared facets.
            Debug.WriteLine("Select ML algoritms for te declared facets....");
            Data.LabelledOutputF = BayesNet.SelectMLforFacets(MyData, Data.competencies.ToTuple(), Data.LabelledData);
            Debug.WriteLine("Selection completed.\r\n");

            //! Generate .arff files for competencies.
            Debug.WriteLine("Generating .arff files for the declared competencies...");
            BayesNet.GenerateArffFilesForCompetencies(MyData, Data.competencies.ToTuple(), Data.competencies.ToStatisticalSubmodel(), Data.CheckLabels, Data.LabelledData);
            Debug.WriteLine("Generation of .arff files completed.\r\n");

            //! Select ML algorithms for the declared competencies.
            Debug.WriteLine("Select ML algoritms for te declared competencies....");
            Data.LabelledOutputC = BayesNet.SelectMLforCompetencies(MyData, Data.competencies.ToTuple(), Data.LabelledData);
            Debug.WriteLine("Selection completed.\r\n");

            Data.OutputLabels = (competencies: Data.LabelledOutputC.output, facets: Data.LabelledOutputF.output);

            {
                //! Redo part of Step1 as StatisticalSubmodel may have changed.

                Debug.Write("Loading instances for the declared statistical submodels...");
                Data.InstUni = BayesNet.LoadInstancesUni(Data.observables, Data.unicompetencies.ToTuple(), Data.unicompetencies.ToUniEvidenceModel());
                Debug.WriteLine("Completed.\r\n");

                //! Normalization of the instances for the declared statistical submodels.
                Debug.Write("Normalisation of instances... ");
                Data.InstancesUni = BayesNet.NormalisationUni(Data.InstUni.Item1);
                Debug.WriteLine("Completed.\r\n");

                //! Check for labeled data.
                Debug.Write("Checking for labeled data to decide ML approach... ");
                Data.CheckLabelsUni = BayesNet.CheckLabellingUni(Data.observables, Data.unicompetencies.ToTuple());
                Debug.WriteLine("Completed.\r\n");

                //! Retrieve labeled data.
                Debug.Write("Retrieving labeled data...");
                Data.LabelledDataUni = BayesNet.GetLabelledDataUni(Data.unicompetencies.ToTuple(), Data.observables);
                Debug.WriteLine("Completed.\r\n");
            }

            //! Generate .arff files for competencies.
            Debug.WriteLine("Generating .arff files for the declared competencies...");
            BayesNet.GenerateArffFilesForUniCompetencies(MyData, Data.unicompetencies.ToTuple(), Data.unicompetencies.ToUniEvidenceModel(), Data.InstancesUni, Data.CheckLabelsUni, Data.LabelledDataUni);
            Debug.WriteLine("Generation of .arff files completed.\r\n");

            //! Select ML algorithms for the declared competencies.
            Debug.WriteLine("Select ML algoritms for te declared competencies....");
            Data.UniLabelledOutputC = BayesNet.SelectMLforUniCompetencies(MyData, Data.unicompetencies.ToTuple(), Data.LabelledDataUni);
            Debug.WriteLine("Selection completed.\r\n");

            Data.OutputLabels = (competencies: Data.LabelledOutputC.output, facets: Data.LabelledOutputF.output);

            groupedComboBox1.InvokeIfRequired(o =>
            {
                groupedComboBox1.DataSource = null;

                List<GCI> items = new List<GCI>();
                for (int x = 0; x < Data.competencies.Count; x++)
                {
                    for (int y = 0; y < Data.competencies[x].Count(); y++)
                    {
                        items.Add(new GCI()
                        {
                            Group = Data.competencies[x].CompetencyName,
                            Value = Data.competencies[x][y].FacetName,
                            Display = Data.competencies[x][y].FacetName,
                            IsCompetency = false,
                        });
                    }
                }

                for (int x = 0; x < Data.competencies.Count; x++)
                {
                    items.Add(new GCI()
                    {
                        Group = "Competencies",
                        Value = Data.competencies[x].CompetencyName,
                        Display = Data.competencies[x].CompetencyName,
                        IsCompetency = true,
                    });
                }

                for (int x = 0; x < Data.unicompetencies.Count; x++)
                {
                    items.Add(new GCI()
                    {
                        Group = "Competencies",
                        Value = Data.unicompetencies[x].CompetencyName,
                        Display = Data.unicompetencies[x].CompetencyName,
                        IsCompetency = true,
                    });
                }
                //TODO Always expecting multidimensional!!.
                groupedComboBox1.ValueMember = "Value";
                groupedComboBox1.DisplayMember = "Display";
                groupedComboBox1.GroupMember = "Group";
                groupedComboBox1.DataSource = new BindingSource(items, String.Empty);
            });
        }

        /// <summary>
        /// Event handler. Called by Step4 for progress changed events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Progress changed event information. </param>
        private void Step4_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            switch (e.ProgressPercentage)
            {
                default:
                    progressLbl.InvokeIfRequired(o =>
                    {
                        progressLbl.Text = String.Empty;
                    });
                    break;

            }
        }

        /// <summary>
        /// Event handler. Called by Step4BackgroundWorker for run worker completed events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Run worker completed event information. </param>
        private void Step4_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (busyBar.Visible)
            {
                busyBar.Hide();
            }
            progressLbl.Text = String.Empty;

            StateMachine.Flags[StateMachine.STEP4_COMPLETED] = true;

            StateMachine.UpdateControls();
        }

        /// <summary>
        /// Validates the competencies.
        /// </summary>
        ///
        /// <returns>
        /// True if it succeeds, false if it fails.
        /// </returns>
        private bool ValidateCompetencies()
        {
            return (listView1.Items.Count != 0 && listView1.Groups.Count != 0) || (listView2.Items.Count != 0);
        }

        /// <summary>
        /// Visualization 2 data.
        /// </summary>
        private void Visualization2Data()
        {
            //! Load Observable Mapper ListView into Data.CompetencyModel/EvidenceModel.
            //
            //string[] Competencies = new String[listView1.Groups.Count/* + listView2.Groups[gndx].Items.Count*/];
            //string[][] Facets = new string[Competencies.Length][];

            Data.competencies = new Competencies();

            //! Retrieve the Competencies/Facets/Observables from ListView1.
            //
            //! 1) Multi-Dimensional.
            //
            for (Int32 g = 0; g < listView1.Groups.Count; g++)
            {
                ListViewGroup lvg = listView1.Groups[g];

                //Data.competencies.Add(new Competency(lvg.Items.Count, lvg.Name));
                //Facets[g] = new String[lvg.Items.Count];

                Competency competency = new Competency(lvg.Name);
                for (Int32 i = 0; i < lvg.Items.Count; i++)
                {
                    String[] observables = lvg.Items[i].SubItems[1].Text.Split(',').ToArray();

                    Facet facet = new Facet(observables.Length, lvg.Items[i].Text);
                    for (Int32 j = 0; j < observables.Length; j++)
                    {
                        facet[j] = observables[j];
                    }
                    competency.Add(facet);
                }

                Data.competencies.Add(competency);
            }

            //! Retrieve the Competencies/Observables from ListView2.
            //
            //! 2) Uni-Dimensional.
            Data.unicompetencies = new UniCompetencies();
            for (Int32 i = 0; i < listView2.Groups[0].Items.Count; i++)
            {
                ListViewItem lvi = listView2.Groups[0].Items[i];

                String[] observables = lvi.SubItems[1].Text.Split(',').ToArray();

                UniCompetency unicompetency = new UniCompetency(observables.Length, lvi.Text);

                for (Int32 j = 0; j < observables.Length; j++)
                {
                    unicompetency[j] = observables[j];
                }

                Data.unicompetencies.Add(unicompetency);
            }

            Debug.WriteLine("Completed.\r\n");
        }

        /// <summary>
        /// Event handler. Called by LoadVandVExternalDataBtn for click events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        private void LoadVandVExternalDataBtn_Click(object sender, EventArgs e)
        {
            if (openFileDialog3.ShowDialog() == DialogResult.OK)
            {
#warning Insert V&V Code here.

                if (Excel.IsFileLocked(openFileDialog3.FileName))
                {
                    MessageBox.Show(String.Format("Could not open locked file:\r\n\r\n{0}", Path.GetFileName(openFileDialog3.FileName)), IniFile.AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    String filename = openFileDialog3.FileName;

                    //! Load all the available external data.
                    // 
                    Logger.Info($"Loading External Data from: '{Utils.MakePathRelative(filename)}'.");

                    Debug.Write("Loading external data... ");
                    Data.observables = BayesNet.LoadAllData(filename);
                    Debug.WriteLine("Completed.\r\n");

                    //Data.DumpObservables();

                    Debug.WriteLine("ExternalData: {0}", Data.ExternalData);

                    //Run correlation analysis for multi-dimensional competencies
                    Data.spearmansMulti = BayesNet.CorrelationAnalysisMulti(Data.competencies.ToTuple().competencies, Data.LabelledOutputC.output, Data.ExternalData);

                    //Run correlation analysis for uni-dimensional competencies
                    Data.spearmansUni = BayesNet.CorrelationAnalysisUni(Data.unicompetencies.ToTuple(), Data.UniLabelledOutputC.Item3, Data.ExternalData);
                }
            }

            StateMachine.UpdateControls();
        }

        #endregion Methods
    }
}