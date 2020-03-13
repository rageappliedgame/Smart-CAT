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
    using System.Linq;
    using System.Windows.Forms;

    using Controls;

    using Stateless;

    /// <summary>
    /// A state machine.
    /// </summary>
    public static class StateMachine
    {
        #region Fields

        /// <summary>
        /// Information describing the init R event.
        /// </summary>
        public const String INIT_R = "INIT_R";


        /// <summary>
        /// Information describing the configure ECD event.
        /// </summary>
        public const String CONFIGURE_ECD = "CONFIGURE_ECD";

        /// <summary>
        /// Information describing the import event.
        /// </summary>
        public const String IMPORT_DATA = "IMPORT_DATA";

        /// <summary>
        /// The step 1 completed.
        /// </summary>
        public const String STEP1_COMPLETED = "STEP1_COMPLETED";

        /// <summary>
        /// The step 2 completed.
        /// </summary>
        public const String STEP2_COMPLETED = "STEP2_COMPLETED";

        /// <summary>
        /// The step 3 completed.
        /// </summary>
        public const String STEP3_COMPLETED = "STEP3_COMPLETED";

        /// <summary>
        /// The  step 4 completed.
        /// </summary>
        public const String STEP4_COMPLETED = "STEP4_COMPLETED";

        /// <summary>
        /// The flags to signal events have been completed.
        /// </summary>
        public static Dictionary<String, Boolean> Flags = new Dictionary<String, Boolean>();

        /// <summary>
        /// The stateless StateMachine.
        /// </summary>
        public static StateMachine<States, Triggers> stateless = new StateMachine<States, Triggers>(States.StartNew);

        /// <summary>
        /// Manager for panel.
        /// </summary>
        private static PanelManager panelMgr;

        /// <summary>
        /// The cancel control.
        /// </summary>
        private static Button cancelBtn;

        /// <summary>
        /// The next control.
        /// </summary>
        private static Button nextBtn;

        /// <summary>
        /// The previous control.
        /// </summary>
        private static Button prevBtn;

        /// <summary>
        /// The busy progress bar control.
        /// </summary>
        private static ProgressBar busyBar;

        public static Dictionary<States, Worker> exitWorkers = new Dictionary<States, Worker>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Static constructor.
        /// </summary>
        static StateMachine()
        {
            stateless.OnTransitioned((transition) =>
            {
                UpdateControls();
            });
        }

        #endregion Constructors

        #region Enumerations

        /// <summary>
        /// Values that represent states.
        /// </summary>
        public enum States
        {
            [Description("Start")]
            StartNew,

            [Description("Import Data")]
            ImportData,
            [Description("Statistical Model")]
            ConfigureECD,
            [Description("ML Optimization")]
            OptimizeML,
            [Description("Validation & Verification")]
            SupportFunction,

            [Description("Finish")]
            Finish,
        }

        /// <summary>
        /// Values that represent triggers.
        /// </summary>
        public enum Triggers
        {
            /// <summary>
            /// An enum constant representing the next option.
            /// </summary>
            Next,
            /// <summary>
            /// An enum constant representing the previous option.
            /// </summary>
            Previous,
            /// <summary>
            /// An enum constant representing the cancel option.
            /// </summary>
            Cancel,
            /// <summary>
            /// An enum constant representing the new option.
            /// </summary>
            New,
        }

        #endregion Enumerations

        #region Methods

        /// <summary>
        /// Initializes the state machine.
        /// </summary>
        ///
        /// <param name="panelManager"> Manager for panel. </param>
        /// <param name="prev">         The previous control. </param>
        /// <param name="next">         The next control. </param>
        /// <param name="cancel">       The cancel control. </param>
        /// <param name="progress">     The progress control. </param>
        public static void InitStateMachine(PanelManager panelManager, Button prev, Button next, Button cancel, ProgressBar progress)
        {
            panelMgr = panelManager;

            prevBtn = prev;
            nextBtn = next;
            cancelBtn = cancel;

            busyBar = progress;

            //! StartNew -> Finish.
            //! StartNew -> ImportData.
            //
            stateless.Configure(States.StartNew)
                .Permit(Triggers.Cancel, States.Finish)
                .PermitIf(Triggers.Next, States.ImportData, () => Flags.ContainsKey(INIT_R) ? Flags[INIT_R] : false)
                  .OnEntry(() =>
                  {
                      InitControls();
                  })
                  .OnExit(() =>
                  {
                      nextBtn.Enabled = false;
                      prevBtn.Enabled = false;
                      cancelBtn.Enabled = false;

                      //! Step 1 Execution
                      exitWorkers[States.StartNew].Run();
                  });

            //! ImportData -> Finish.
            //! ImportData -> ConfigureECD.
            //
            stateless.Configure(States.ImportData)
                .Permit(Triggers.Cancel, States.Finish)
                .PermitIf(Triggers.Next, States.ConfigureECD, () => Flags.ContainsKey(IMPORT_DATA) ? Flags[IMPORT_DATA] : false)
                .OnEntry(() =>
                {
                    panelMgr.InvokeIfRequired(c => { c.SelectedIndex = (Int32)States.ImportData; });
                }).
                OnExit(() =>
                {
                    nextBtn.Enabled = false;
                    prevBtn.Enabled = false;
                    cancelBtn.Enabled = false;

                    busyBar.Show();
                    busyBar.Value = busyBar.Maximum;

                    //! Step 2 Execution
                    exitWorkers[States.ImportData].Run();
                });

            //! ConfigureECD -> Finish.
            //! ConfigureECD -> OptimizeML.
            //
            stateless.Configure(States.ConfigureECD)
                .Permit(Triggers.Cancel, States.Finish)
                .PermitIf(Triggers.Next, States.OptimizeML, () => Flags.ContainsKey(CONFIGURE_ECD) ? Flags[CONFIGURE_ECD] : false)
                .OnEntry(() =>
                {
                    panelMgr.InvokeIfRequired(c => { c.SelectedIndex = (Int32)States.ConfigureECD; });
                })
                .OnExit(() =>
                {
                    nextBtn.Enabled = false;
                    prevBtn.Enabled = false;
                    cancelBtn.Enabled = false;

                    busyBar.Show();
                    busyBar.Value = busyBar.Maximum;

                    //! Step 3 Execution
                    exitWorkers[States.ConfigureECD].Run();
                });

            //! OptimizeML -> Finish.
            //! OptimizeML -> SupportFunction.
            //
            stateless.Configure(States.OptimizeML)
                .Permit(Triggers.Cancel, States.Finish)
                .PermitIf(Triggers.Next, States.SupportFunction, () => Flags.ContainsKey(STEP3_COMPLETED) ? Flags[STEP3_COMPLETED] : false)
                .OnEntry(() =>
                {
                    panelMgr.InvokeIfRequired(c => { c.SelectedIndex = (Int32)States.OptimizeML; });
                })
                .OnExit(() =>
                {
                    nextBtn.Enabled = false;
                    prevBtn.Enabled = false;
                    cancelBtn.Enabled = false;

                    busyBar.Show();
                    busyBar.Value = busyBar.Maximum;

                    //! Step 4 Execution
                    exitWorkers[States.OptimizeML].Run();
                });

            //! SupportFunction -> Finish.
            // 
            stateless.Configure(States.SupportFunction)
                .Permit(Triggers.Cancel, States.Finish)
                .PermitIf(Triggers.Next, States.Finish, () => Flags.ContainsKey(STEP4_COMPLETED) ? Flags[STEP4_COMPLETED] : false)
                .OnEntry(() =>
                {
                    panelMgr.InvokeIfRequired(c => { c.SelectedIndex = (Int32)States.SupportFunction; });
                });

            //! Finish -> Import Data.
            // 
            stateless.Configure(States.Finish)
                .Permit(Triggers.New, States.ImportData)
                .OnEntry(() =>
                {
                    panelMgr.InvokeIfRequired(c => { c.SelectedIndex = (Int32)States.Finish; });
                    cancelBtn.InvokeIfRequired(c => { c.Enabled = true; });
                });

            InitControls();
        }

        /// <summary>
        /// Updates the controls (the Wizard Buttons).
        /// </summary>
        public static void UpdateControls()
        {
            prevBtn.InvokeIfRequired(c => { c.Enabled = stateless.PermittedTriggers.Contains(Triggers.Previous); });
            nextBtn.InvokeIfRequired(c => { c.Enabled = stateless.PermittedTriggers.Contains(Triggers.Next); });
            cancelBtn.InvokeIfRequired(c => { c.Enabled = stateless.PermittedTriggers.Contains(Triggers.Cancel); });
        }

        /// <summary>
        /// Initializes the controls.
        /// </summary>
        private static void InitControls()
        {
            //! Wizard Panel
            panelMgr.InvokeIfRequired(c => { c.SelectedIndex = 0; });

            //! Buttons
            prevBtn.InvokeIfRequired(c => { c.Enabled = stateless.PermittedTriggers.Contains(Triggers.Previous); });
            nextBtn.InvokeIfRequired(c => { c.Enabled = stateless.PermittedTriggers.Contains(Triggers.Next); });
            cancelBtn.InvokeIfRequired(c => { c.Enabled = stateless.PermittedTriggers.Contains(Triggers.Cancel); });
        }

        #endregion Methods
    }
}