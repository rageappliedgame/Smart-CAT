using System;

namespace StealthAssessmentWizard
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panelManager1 = new Controls.PanelManager();
            this.StartNew_Panel = new Controls.ManagedPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ImportData_Panel = new Controls.ManagedPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.ConfigureECD_Panel = new Controls.ManagedPanel();
            this.label13 = new System.Windows.Forms.Label();
            this.listView2 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.competencyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addCompetencyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editCompetencyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.removeCompetencyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.facetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addFacetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editFacetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.removeFacetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cancelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listView1 = new System.Windows.Forms.ListView();
            this.Facet_Column = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Variable_Column = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.OptimizeML_Panel = new Controls.ManagedPanel();
            this.label16 = new System.Windows.Forms.Label();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.MLAlgoritmSelector = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.SupportFunction_Panel = new Controls.ManagedPanel();
            this.LoadVandVExternalData = new System.Windows.Forms.Button();
            this.propertyGrid2 = new System.Windows.Forms.PropertyGrid();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.Finish_Panel = new Controls.ManagedPanel();
            this.button3 = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupedComboBox1 = new GroupedComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.progressLbl = new System.Windows.Forms.Label();
            this.busyBar = new System.Windows.Forms.ProgressBar();
            this.nextBtn = new System.Windows.Forms.Button();
            this.prevBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.helpBtn = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.errorBox = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog2 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog3 = new System.Windows.Forms.OpenFileDialog();
            this.panelManager1.SuspendLayout();
            this.StartNew_Panel.SuspendLayout();
            this.ImportData_Panel.SuspendLayout();
            this.ConfigureECD_Panel.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.OptimizeML_Panel.SuspendLayout();
            this.SupportFunction_Panel.SuspendLayout();
            this.Finish_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelManager1
            // 
            this.panelManager1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelManager1.BackColor = System.Drawing.SystemColors.Window;
            this.panelManager1.Controls.Add(this.StartNew_Panel);
            this.panelManager1.Controls.Add(this.ImportData_Panel);
            this.panelManager1.Controls.Add(this.ConfigureECD_Panel);
            this.panelManager1.Controls.Add(this.OptimizeML_Panel);
            this.panelManager1.Controls.Add(this.SupportFunction_Panel);
            this.panelManager1.Controls.Add(this.Finish_Panel);
            this.panelManager1.Location = new System.Drawing.Point(5, 8);
            this.panelManager1.Name = "panelManager1";
            this.panelManager1.SelectedIndex = 4;
            this.panelManager1.SelectedPanel = this.SupportFunction_Panel;
            this.panelManager1.Size = new System.Drawing.Size(778, 391);
            this.panelManager1.TabIndex = 6;
            this.panelManager1.SelectedIndexChanged += new System.EventHandler(this.PanelManager1_SelectedIndexChanged);
            // 
            // StartNew_Panel
            // 
            this.StartNew_Panel.Controls.Add(this.label5);
            this.StartNew_Panel.Controls.Add(this.label2);
            this.StartNew_Panel.Controls.Add(this.label8);
            this.StartNew_Panel.Controls.Add(this.label11);
            this.StartNew_Panel.Controls.Add(this.label1);
            this.StartNew_Panel.Location = new System.Drawing.Point(0, 0);
            this.StartNew_Panel.Name = "StartNew_Panel";
            this.StartNew_Panel.Size = new System.Drawing.Size(792, 391);
            this.StartNew_Panel.Text = "Start New";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.Location = new System.Drawing.Point(8, 654);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.label5.Size = new System.Drawing.Size(143, 23);
            this.label5.TabIndex = 5;
            this.label5.Text = "Using R";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(238, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Use the Next button to create a new Assessment";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label8.Location = new System.Drawing.Point(8, 677);
            this.label8.Name = "label8";
            this.label8.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.label8.Size = new System.Drawing.Size(192, 23);
            this.label8.TabIndex = 6;
            this.label8.Text = "Using weka";
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label11.Location = new System.Drawing.Point(8, 700);
            this.label11.Name = "label11";
            this.label11.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.label11.Size = new System.Drawing.Size(249, 23);
            this.label11.TabIndex = 7;
            this.label11.Text = "Using IKVM";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 37);
            this.label1.TabIndex = 0;
            this.label1.Text = "Start";
            // 
            // ImportData_Panel
            // 
            this.ImportData_Panel.Controls.Add(this.button1);
            this.ImportData_Panel.Controls.Add(this.label6);
            this.ImportData_Panel.Controls.Add(this.label7);
            this.ImportData_Panel.Location = new System.Drawing.Point(0, 0);
            this.ImportData_Panel.Name = "ImportData_Panel";
            this.ImportData_Panel.Size = new System.Drawing.Size(792, 391);
            this.ImportData_Panel.Text = "Import Data";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(11, 70);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Browse...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.ImportBtn_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 45);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(207, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Use the Browse button to select a Log File";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(4, 4);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(185, 37);
            this.label7.TabIndex = 5;
            this.label7.Text = "Import Data";
            // 
            // ConfigureECD_Panel
            // 
            this.ConfigureECD_Panel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ConfigureECD_Panel.Controls.Add(this.label13);
            this.ConfigureECD_Panel.Controls.Add(this.listView2);
            this.ConfigureECD_Panel.Controls.Add(this.listView1);
            this.ConfigureECD_Panel.Controls.Add(this.label9);
            this.ConfigureECD_Panel.Controls.Add(this.label10);
            this.ConfigureECD_Panel.Location = new System.Drawing.Point(0, 0);
            this.ConfigureECD_Panel.Name = "ConfigureECD_Panel";
            this.ConfigureECD_Panel.Size = new System.Drawing.Size(792, 391);
            this.ConfigureECD_Panel.Text = "Configure ECD";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(317, 45);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(173, 13);
            this.label13.TabIndex = 12;
            this.label13.Text = "Add uni-dimensional competencies:";
            // 
            // listView2
            // 
            this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView2.ContextMenuStrip = this.contextMenuStrip1;
            this.listView2.HideSelection = false;
            this.listView2.Location = new System.Drawing.Point(320, 64);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(338, 319);
            this.listView2.TabIndex = 11;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Competency";
            this.columnHeader1.Width = 120;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Variable";
            this.columnHeader2.Width = 120;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.competencyToolStripMenuItem,
            this.toolStripSeparator3,
            this.facetToolStripMenuItem,
            this.toolStripSeparator2,
            this.loadToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripSeparator1,
            this.cancelToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(151, 132);
            this.contextMenuStrip1.Tag = "Statistical Model";
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // competencyToolStripMenuItem
            // 
            this.competencyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addCompetencyToolStripMenuItem,
            this.editCompetencyToolStripMenuItem,
            this.toolStripSeparator4,
            this.removeCompetencyToolStripMenuItem});
            this.competencyToolStripMenuItem.Name = "competencyToolStripMenuItem";
            this.competencyToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.competencyToolStripMenuItem.Text = "Competencies";
            // 
            // addCompetencyToolStripMenuItem
            // 
            this.addCompetencyToolStripMenuItem.Name = "addCompetencyToolStripMenuItem";
            this.addCompetencyToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.addCompetencyToolStripMenuItem.Text = "Add";
            this.addCompetencyToolStripMenuItem.Click += new System.EventHandler(this.AddCompetencyMenuItem_Click);
            // 
            // editCompetencyToolStripMenuItem
            // 
            this.editCompetencyToolStripMenuItem.Name = "editCompetencyToolStripMenuItem";
            this.editCompetencyToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.editCompetencyToolStripMenuItem.Text = "Edit";
            this.editCompetencyToolStripMenuItem.Click += new System.EventHandler(this.EditCompetencyMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(114, 6);
            // 
            // removeCompetencyToolStripMenuItem
            // 
            this.removeCompetencyToolStripMenuItem.Name = "removeCompetencyToolStripMenuItem";
            this.removeCompetencyToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.removeCompetencyToolStripMenuItem.Text = "Remove";
            this.removeCompetencyToolStripMenuItem.Click += new System.EventHandler(this.RemoveCompetencyMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(147, 6);
            // 
            // facetToolStripMenuItem
            // 
            this.facetToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFacetToolStripMenuItem,
            this.editFacetToolStripMenuItem,
            this.toolStripSeparator5,
            this.removeFacetToolStripMenuItem});
            this.facetToolStripMenuItem.Name = "facetToolStripMenuItem";
            this.facetToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.facetToolStripMenuItem.Text = "Facets";
            // 
            // addFacetToolStripMenuItem
            // 
            this.addFacetToolStripMenuItem.Name = "addFacetToolStripMenuItem";
            this.addFacetToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.addFacetToolStripMenuItem.Text = "Add";
            this.addFacetToolStripMenuItem.Click += new System.EventHandler(this.AddFacetMenuItem_Click);
            // 
            // editFacetToolStripMenuItem
            // 
            this.editFacetToolStripMenuItem.Name = "editFacetToolStripMenuItem";
            this.editFacetToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.editFacetToolStripMenuItem.Text = "Edit";
            this.editFacetToolStripMenuItem.Click += new System.EventHandler(this.EditFacetMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(114, 6);
            // 
            // removeFacetToolStripMenuItem
            // 
            this.removeFacetToolStripMenuItem.Name = "removeFacetToolStripMenuItem";
            this.removeFacetToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.removeFacetToolStripMenuItem.Text = "Remove";
            this.removeFacetToolStripMenuItem.Click += new System.EventHandler(this.RemoveFacetMenuItem1_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(147, 6);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.LoadToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(147, 6);
            // 
            // cancelToolStripMenuItem
            // 
            this.cancelToolStripMenuItem.Name = "cancelToolStripMenuItem";
            this.cancelToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.cancelToolStripMenuItem.Text = "Cancel";
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Facet_Column,
            this.Variable_Column});
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(14, 62);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(300, 321);
            this.listView1.TabIndex = 10;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // Facet_Column
            // 
            this.Facet_Column.Text = "Facet";
            this.Facet_Column.Width = 120;
            // 
            // Variable_Column
            // 
            this.Variable_Column.Text = "Variable";
            this.Variable_Column.Width = 120;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 45);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(174, 13);
            this.label9.TabIndex = 9;
            this.label9.Text = "Add competencies and their facets:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(4, 4);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(249, 37);
            this.label10.TabIndex = 8;
            this.label10.Text = "Statistical Model";
            // 
            // OptimizeML_Panel
            // 
            this.OptimizeML_Panel.Controls.Add(this.label16);
            this.OptimizeML_Panel.Controls.Add(this.propertyGrid1);
            this.OptimizeML_Panel.Controls.Add(this.MLAlgoritmSelector);
            this.OptimizeML_Panel.Controls.Add(this.label12);
            this.OptimizeML_Panel.Controls.Add(this.label15);
            this.OptimizeML_Panel.Location = new System.Drawing.Point(0, 0);
            this.OptimizeML_Panel.Name = "OptimizeML_Panel";
            this.OptimizeML_Panel.Size = new System.Drawing.Size(792, 391);
            this.OptimizeML_Panel.Text = "Optimize ML";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(91, 94);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(104, 13);
            this.label16.TabIndex = 17;
            this.label16.Text = "Select ML Algorithm:";
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid1.Location = new System.Drawing.Point(91, 119);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(409, 223);
            this.propertyGrid1.TabIndex = 16;
            // 
            // MLAlgoritmSelector
            // 
            this.MLAlgoritmSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MLAlgoritmSelector.FormattingEnabled = true;
            this.MLAlgoritmSelector.Location = new System.Drawing.Point(201, 91);
            this.MLAlgoritmSelector.Name = "MLAlgoritmSelector";
            this.MLAlgoritmSelector.Size = new System.Drawing.Size(299, 21);
            this.MLAlgoritmSelector.TabIndex = 15;
            this.MLAlgoritmSelector.SelectedIndexChanged += new System.EventHandler(this.ComboBox1_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(11, 45);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(159, 13);
            this.label12.TabIndex = 14;
            this.label12.Text = "Tune Machine Learning settings";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(3, 2);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(265, 37);
            this.label15.TabIndex = 13;
            this.label15.Text = "ML Optimizations";
            // 
            // SupportFunction_Panel
            // 
            this.SupportFunction_Panel.Controls.Add(this.LoadVandVExternalData);
            this.SupportFunction_Panel.Controls.Add(this.propertyGrid2);
            this.SupportFunction_Panel.Controls.Add(this.label23);
            this.SupportFunction_Panel.Controls.Add(this.label24);
            this.SupportFunction_Panel.Location = new System.Drawing.Point(0, 0);
            this.SupportFunction_Panel.Name = "SupportFunction_Panel";
            this.SupportFunction_Panel.Size = new System.Drawing.Size(778, 391);
            this.SupportFunction_Panel.Text = "Support Function";
            // 
            // LoadVandVExternalData
            // 
            this.LoadVandVExternalData.Location = new System.Drawing.Point(11, 70);
            this.LoadVandVExternalData.Name = "LoadVandVExternalData";
            this.LoadVandVExternalData.Size = new System.Drawing.Size(75, 23);
            this.LoadVandVExternalData.TabIndex = 22;
            this.LoadVandVExternalData.Text = "Browser...";
            this.LoadVandVExternalData.UseVisualStyleBackColor = true;
            this.LoadVandVExternalData.Click += new System.EventHandler(this.LoadVandVExternalDataBtn_Click);
            // 
            // propertyGrid2
            // 
            this.propertyGrid2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid2.Location = new System.Drawing.Point(91, 119);
            this.propertyGrid2.Name = "propertyGrid2";
            this.propertyGrid2.Size = new System.Drawing.Size(2762, 1396);
            this.propertyGrid2.TabIndex = 19;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(11, 45);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(362, 13);
            this.label23.TabIndex = 21;
            this.label23.Text = "Use the Browse button to select external data for Validation and Verification";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.Location = new System.Drawing.Point(3, 2);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(390, 37);
            this.label24.TabIndex = 20;
            this.label24.Text = "Validation and Verification";
            // 
            // Finish_Panel
            // 
            this.Finish_Panel.Controls.Add(this.button3);
            this.Finish_Panel.Controls.Add(this.chart1);
            this.Finish_Panel.Controls.Add(this.groupedComboBox1);
            this.Finish_Panel.Controls.Add(this.button2);
            this.Finish_Panel.Controls.Add(this.label3);
            this.Finish_Panel.Controls.Add(this.label4);
            this.Finish_Panel.Location = new System.Drawing.Point(0, 0);
            this.Finish_Panel.Name = "Finish_Panel";
            this.Finish_Panel.Size = new System.Drawing.Size(792, 391);
            this.Finish_Panel.Text = "Finish";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(11, 99);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 20;
            this.button3.Text = "Save results";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.SaveReportBtn_Click);
            // 
            // chart1
            // 
            chartArea4.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.chart1.Legends.Add(legend4);
            this.chart1.Location = new System.Drawing.Point(92, 99);
            this.chart1.Name = "chart1";
            series4.ChartArea = "ChartArea1";
            series4.LabelToolTip = "#VAL{D0}";
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            this.chart1.Series.Add(series4);
            this.chart1.Size = new System.Drawing.Size(408, 235);
            this.chart1.TabIndex = 19;
            this.chart1.Text = "chart1";
            this.chart1.Click += new System.EventHandler(this.Chart1_Click);
            // 
            // groupedComboBox1
            // 
            this.groupedComboBox1.DataSource = null;
            this.groupedComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.groupedComboBox1.FormattingEnabled = true;
            this.groupedComboBox1.Location = new System.Drawing.Point(140, 72);
            this.groupedComboBox1.Name = "groupedComboBox1";
            this.groupedComboBox1.Size = new System.Drawing.Size(218, 21);
            this.groupedComboBox1.TabIndex = 18;
            this.groupedComboBox1.SelectedIndexChanged += new System.EventHandler(this.groupedComboBox1_SelectedIndexChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(11, 70);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 17;
            this.button2.Text = "Start new";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.NewBtn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(263, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "You can now start a new analysis or close this Wizard.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 2);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 37);
            this.label4.TabIndex = 15;
            this.label4.Text = "Finish";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.progressLbl);
            this.panel1.Controls.Add(this.busyBar);
            this.panel1.Controls.Add(this.nextBtn);
            this.panel1.Controls.Add(this.prevBtn);
            this.panel1.Controls.Add(this.cancelBtn);
            this.panel1.Controls.Add(this.helpBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 498);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(994, 63);
            this.panel1.TabIndex = 7;
            // 
            // progressLbl
            // 
            this.progressLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.progressLbl.AutoSize = true;
            this.progressLbl.Location = new System.Drawing.Point(241, 9);
            this.progressLbl.Name = "progressLbl";
            this.progressLbl.Size = new System.Drawing.Size(24, 13);
            this.progressLbl.TabIndex = 5;
            this.progressLbl.Text = "n/a";
            // 
            // busyBar
            // 
            this.busyBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.busyBar.Location = new System.Drawing.Point(241, 28);
            this.busyBar.Name = "busyBar";
            this.busyBar.Size = new System.Drawing.Size(257, 23);
            this.busyBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.busyBar.TabIndex = 4;
            this.busyBar.Visible = false;
            // 
            // nextBtn
            // 
            this.nextBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nextBtn.Enabled = false;
            this.helpProvider1.SetHelpString(this.nextBtn, "Next Button");
            this.nextBtn.Location = new System.Drawing.Point(838, 28);
            this.nextBtn.Name = "nextBtn";
            this.helpProvider1.SetShowHelp(this.nextBtn, true);
            this.nextBtn.Size = new System.Drawing.Size(75, 23);
            this.nextBtn.TabIndex = 3;
            this.nextBtn.Text = "&Next";
            this.nextBtn.UseVisualStyleBackColor = true;
            this.nextBtn.Click += new System.EventHandler(this.NextBtn_Click);
            // 
            // prevBtn
            // 
            this.prevBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.prevBtn.Enabled = false;
            this.helpProvider1.SetHelpString(this.prevBtn, "Back button");
            this.prevBtn.Location = new System.Drawing.Point(757, 28);
            this.prevBtn.Name = "prevBtn";
            this.helpProvider1.SetShowHelp(this.prevBtn, true);
            this.prevBtn.Size = new System.Drawing.Size(75, 23);
            this.prevBtn.TabIndex = 2;
            this.prevBtn.Text = "&Back";
            this.prevBtn.UseVisualStyleBackColor = true;
            this.prevBtn.Click += new System.EventHandler(this.PrevBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBtn.Location = new System.Drawing.Point(919, 28);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 1;
            this.cancelBtn.Text = "&Finish";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // helpBtn
            // 
            this.helpBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.helpProvider1.SetHelpString(this.helpBtn, "Test Help");
            this.helpBtn.Location = new System.Drawing.Point(12, 28);
            this.helpBtn.Name = "helpBtn";
            this.helpProvider1.SetShowHelp(this.helpBtn, true);
            this.helpBtn.Size = new System.Drawing.Size(75, 23);
            this.helpBtn.TabIndex = 0;
            this.helpBtn.Text = "&Help";
            this.helpBtn.UseVisualStyleBackColor = true;
            this.helpBtn.Click += new System.EventHandler(this.helpBtn_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.textBox1);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(5);
            this.splitContainer1.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.SplitContainer1_Panel1_Paint);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel2.Controls.Add(this.errorBox);
            this.splitContainer1.Panel2.Controls.Add(this.panelManager1);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(5);
            this.splitContainer1.Size = new System.Drawing.Size(994, 498);
            this.splitContainer1.SplitterDistance = 240;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 8;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(5, 366);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(230, 127);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "Credits:\r\n\r\nGeorgiadis, K.\r\nvan der Vegt, W.\r\n\r\nWebsite:\r\nhttps://github.com/rage" +
    "appliedgame/Smart-CAT";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // errorBox
            // 
            this.errorBox.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.errorBox.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.errorBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.errorBox.FullRowSelect = true;
            this.errorBox.HideSelection = false;
            this.errorBox.Location = new System.Drawing.Point(5, 405);
            this.errorBox.Name = "errorBox";
            this.errorBox.Size = new System.Drawing.Size(743, 88);
            this.errorBox.SmallImageList = this.imageList1;
            this.errorBox.TabIndex = 7;
            this.errorBox.UseCompatibleStateImageBehavior = false;
            this.errorBox.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "";
            this.columnHeader3.Width = 20;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Message";
            this.columnHeader4.Width = 320;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "information.png");
            this.imageList1.Images.SetKeyName(1, "warning.png");
            this.imageList1.Images.SetKeyName(2, "stop.png");
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileName = "*.xlsx";
            this.saveFileDialog1.Filter = "Excel Sheet files|*.xlsx|All files|*.*";
            this.saveFileDialog1.Tag = "Excel Report";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "*.xlsx";
            this.openFileDialog1.FileName = "*.xlsx";
            this.openFileDialog1.Filter = "Excel Sheet files|*.xlsx|Comma Separated Values files|*.csv|Log files|*.log|All f" +
    "iles|*.*";
            this.openFileDialog1.Tag = "Excel Raw Input";
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "*.json";
            this.openFileDialog2.Filter = "JSON files|*.json";
            this.openFileDialog2.InitialDirectory = "JSON Model";
            // 
            // saveFileDialog2
            // 
            this.saveFileDialog2.FileName = "*.json";
            this.saveFileDialog2.Filter = "JSON files|*.json";
            this.saveFileDialog2.Tag = "JSON Model";
            // 
            // openFileDialog3
            // 
            this.openFileDialog3.DefaultExt = "*.xlsx";
            this.openFileDialog3.FileName = "*.xlsx";
            this.openFileDialog3.Filter = "Excel Sheet files|*.xlsx|Comma Separated Values files|*.csv|Log files|*.log|All f" +
    "iles|*.*";
            this.openFileDialog3.Tag = "Validation and Verification";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 561);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 14, 0);
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panelManager1.ResumeLayout(false);
            this.StartNew_Panel.ResumeLayout(false);
            this.StartNew_Panel.PerformLayout();
            this.ImportData_Panel.ResumeLayout(false);
            this.ImportData_Panel.PerformLayout();
            this.ConfigureECD_Panel.ResumeLayout(false);
            this.ConfigureECD_Panel.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.OptimizeML_Panel.ResumeLayout(false);
            this.OptimizeML_Panel.PerformLayout();
            this.SupportFunction_Panel.ResumeLayout(false);
            this.SupportFunction_Panel.PerformLayout();
            this.Finish_Panel.ResumeLayout(false);
            this.Finish_Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Controls.PanelManager panelManager1;
        private Controls.ManagedPanel StartNew_Panel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button helpBtn;
        private System.Windows.Forms.Button nextBtn;
        private System.Windows.Forms.Button prevBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Controls.ManagedPanel ImportData_Panel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private Controls.ManagedPanel ConfigureECD_Panel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private Controls.ManagedPanel OptimizeML_Panel;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Controls.ManagedPanel Finish_Panel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private Controls.ManagedPanel SupportFunction_Panel;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button button1;
        internal System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader Facet_Column;
        private System.Windows.Forms.ColumnHeader Variable_Column;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem facetToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem cancelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem competencyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addCompetencyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editCompetencyToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem removeCompetencyToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem addFacetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editFacetToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem removeFacetToolStripMenuItem;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.ComboBox MLAlgoritmSelector;
        private System.Windows.Forms.PropertyGrid propertyGrid2;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Button button2;
        private GroupedComboBox groupedComboBox1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ProgressBar busyBar;
        private System.Windows.Forms.Label progressLbl;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.HelpProvider helpProvider1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ListView errorBox;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.SaveFileDialog saveFileDialog2;
        private System.Windows.Forms.Button LoadVandVExternalData;
        internal System.Windows.Forms.OpenFileDialog openFileDialog3;
    }
}

