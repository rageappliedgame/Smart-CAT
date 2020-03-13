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
    using System.ComponentModel;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    public partial class InputSelectDialog : Form, IDisposable
    {
        #region Fields

        internal Int32 hintHeight = 13;
        internal ToolTip toolTip = new ToolTip();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public InputSelectDialog()
        {
            InitializeComponent();

            SelectorEnabled = true;
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Text"></param>
        public InputSelectDialog(String Text)
            : this()
        {
            this.Text = Text;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="Prompt"></param>
        public InputSelectDialog(String Text, String Prompt)
            : this(Text)
        {
            this.Prompt = Prompt;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        ///
        /// <param name="Text">   . </param>
        /// <param name="Prompt"> . </param>
        /// <param name="Input">  . </param>
        public InputSelectDialog(String Text, String Prompt, String Input)
            : this(Text, Prompt)
        {
            this.InputBox.Text = Input;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        ///
        /// <param name="Text">   . </param>
        /// <param name="Prompt"> . </param>
        /// <param name="Input">  . </param>
        /// <param name="Items">  The items. </param>
        public InputSelectDialog(String Text, String Prompt, String Input, IEnumerable<Object> Items)
            : this(Text, Prompt, Input)
        {
            ItemsBox.Items.AddRange(Items.ToArray());
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        ///
        /// <param name="Text">         . </param>
        /// <param name="Prompt">       . </param>
        /// <param name="Input">        . </param>
        /// <param name="Items">        The items. </param>
        /// <param name="CheckedItems"> The checked items. </param>
        public InputSelectDialog(String Text, String Prompt, String Input, IEnumerable<Object> Items, IEnumerable<Object> CheckedItems)
            : this(Text, Prompt, Input, Items)
        {
            for (Int32 i = 0; i < ItemsBox.Items.Count; i++)
            {
                ItemsBox.SetItemChecked(i, false);
            }

            foreach (Object item in CheckedItems)
            {
                Int32 ndx = ItemsBox.Items.IndexOf(item);
                ItemsBox.SetItemChecked(ndx, true);
            }
        }
        #endregion Constructors

        #region Delegates

        /// <summary>
        /// Event fired when the Input is changed.
        /// </summary>
        /// <param name="sender">The InputDialog</param>
        /// <param name="e">The EventArgs of the TextBox</param>
        public delegate void InputChangedHandler(object sender, EventArgs e);

        #endregion Delegates

        #region Events

        /// <summary>
        /// Event fired when the Input is changed.
        /// </summary>
        [Category("Behavior"),
        Description("Event fired when the Input is changed"),
        ]
        public event InputChangedHandler InputChanged;

        #endregion Events

        #region Properties

        /// <summary>
        /// Help Text.
        /// </summary>
        public String Help { get; set; }

        /// <summary>
        /// Number of lines to be shown in the HintMessage. This property expands/shrinks the dialog.
        /// </summary>
        [DefaultValue(1)]
        public Int32 HintLines
        {
            get
            {
                return HintMessage.Height / hintHeight;
            }
            set
            {
                this.Height = (value - HintLines) * hintHeight;
                HintMessage.Height = value * hintHeight;
            }
        }

        /// <summary>
        /// Input.
        /// </summary>
        public String Input
        {
            get
            {
                return InputBox.Text;
            }
            set
            {
                InputBox.Text = value;
            }
        }

        /// <summary>
        /// Prompt.
        /// </summary>
        public String Prompt
        {
            get
            {
                return PromptLbl.Text;
            }

            set
            {
                PromptLbl.Text = value;
            }
        }

        /// <summary>
        /// Gets the checked items.
        /// </summary>
        ///
        /// <value>
        /// The checked items.
        /// </value>
        public IEnumerable<Object> CheckedItems
        {
            get
            {
                return ItemsBox.CheckedItems.Cast<Object>().AsEnumerable();
            }
        }

        /// <summary>
        /// Gets or sets the checked indices.
        /// </summary>
        ///
        /// <value>
        /// The checked indices.
        /// </value>
        public IEnumerable<Object> CheckedIndices
        {
            /// <summary>
            /// An enum constant representing the get option.
            /// </summary>
            get
            {
                return ItemsBox.CheckedIndices.Cast<Object>();
            }
            set
            {
                foreach (Object item in value)
                {
                    ItemsBox.SetItemChecked(ItemsBox.Items.IndexOf(item), true);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the selector is enabled.
        /// </summary>
        ///
        /// <value>
        /// True if selector enabled, false if not.
        /// </value>
        public Boolean SelectorEnabled
        {
            get
            {
                return ItemsBox.Enabled;
            }
            set
            {
                ItemsBox.Enabled = value;
                ItemsBox.Visible = value;
                SelectLbl.Visible = value;
                ClientSize = new Size(ClientSize.Width,
                    value
                    ? 205 : 102);
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Dispose.
        /// </summary>
        void IDisposable.Dispose()
        {
            Dispose();
            //
            //GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Event handler. Called by InputSelect for shown events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        private void InputSelect_Shown(object sender, EventArgs e)
        {
            InputBox.Focus();

            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 250;
            this.toolTip.ReshowDelay = 50;
            this.toolTip.ShowAlways = true;

            toolTip.SetToolTip(InputBox, Help);
            toolTip.SetToolTip(linkLabel1, Help);
        }

        /// <summary>
        /// Event handler. Called by InputBox for key press events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Key press event information. </param>
        private void InputBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (Char)27:
                    CancelBtn.PerformClick();
                    break;

                case (Char)13:
                    if (Input.Length != 0)
                    {
                        OKBtn.PerformClick();
                    }
                    break;
            }
        }

        /// <summary>
        /// Event handler. Called by InputBox for text changed events.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="e">      Event information. </param>
        private void InputBox_TextChanged(object sender, EventArgs e)
        {
            InputChanged?.Invoke(this, e);
        }

        #endregion Methods
    }
}