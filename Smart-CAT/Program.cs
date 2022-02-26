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
    using System.Diagnostics;
    using System.Reflection;
    using System.Windows.Forms;
    using OfficeOpenXml;
    using Swiss;

    static class Program
    {
        #region Methods

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //! https://docs.microsoft.com/en-us/dotnet/api/system.appdomain.assemblyload?view=netframework-4.8
            //AppDomain currentDomain = AppDomain.CurrentDomain;
            //currentDomain.AssemblyLoad += new AssemblyLoadEventHandler(OnAssemblyLoadEventHandler);
            //currentDomain.UnhandledException += new UnhandledExceptionEventHandler(OnUnhandledExceptionEventHandler);

            ExceptionDlg.InitializeExceptionMode(UnhandledExceptionMode.Automatic);

            ExceptionDlg.ShownWmiKeys.AddRange(
                new Wmi.WmiKeys[] {
                    Wmi.WmiKeys.Win32_ComputerSystem,
                    Wmi.WmiKeys.Win32_Processor,
                    Wmi.WmiKeys.Win32_PhysicalMemory });

            ExceptionDlg.PreloadedWmiKeys.AddRange(
                new Wmi.WmiKeys[] {
                    Wmi.WmiKeys.Win32_ComputerSystem ,
                    Wmi.WmiKeys.Win32_Processor ,
                    Wmi.WmiKeys.Win32_PhysicalMemory });

            ExceptionDlg.AppInfo.Add("AppVersion", IniFile.AppVersion);
            ExceptionDlg.AppInfo.Add("AppData", IniFile.AppData);
            ExceptionDlg.AppInfo.Add("AppDir", IniFile.AppDir);
            ExceptionDlg.AppInfo.Add("AppDoc", IniFile.AppDoc);
            ExceptionDlg.AppInfo.Add("AppCommonData", IniFile.AppCommonData);
            ExceptionDlg.AppInfo.Add("AppExe", IniFile.AppExe);
            ExceptionDlg.AppInfo.Add("AppIni", IniFile.AppIni);

            //! If you are a commercial business and have
            //! purchased commercial licenses use the static property
            //! LicenseContext of the ExcelPackage class :
            // ExcelPackage.LicenseContext = LicenseContext.Commercial;

            //! If you use EPPlus in a noncommercial context
            //! according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());

            //PrintLoadedAssemblies(currentDomain);
        }

        /// <summary>
        /// Raises the unhandled exception event.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="args">   Event information to send to registered event handlers. </param>
        private static void OnUnhandledExceptionEventHandler(object sender, UnhandledExceptionEventArgs args)
        {
            //IKVM throws a lot java.lang.ClassNotFoundException/ClassNotFoundException (see https://sourceforge.net/p/ikvm/wiki/ClassLoader/)
            Exception e = (Exception)args.ExceptionObject;

            Debug.WriteLine($"OnUnhandledExceptionEventHandler caught : {e.Message}");
            Debug.WriteLine($"Runtime terminating: {args.IsTerminating}");
        }

        /// <summary>
        /// Print loaded assemblies.
        /// </summary>
        ///
        /// <param name="domain"> The domain. </param>
#pragma warning disable IDE0051 // Remove unused private members
        private static void PrintLoadedAssemblies(AppDomain domain)
#pragma warning restore IDE0051 // Remove unused private members
        {
            Debug.WriteLine("LOADED ASSEMBLIES:");
            Debug.Indent();
            foreach (Assembly a in domain.GetAssemblies())
            {
                Debug.WriteLine($"{a.FullName}");
            }
            Debug.Unindent();
        }

        /// <summary>
        /// Raises the assembly load event.
        /// </summary>
        ///
        /// <param name="sender"> Source of the event. </param>
        /// <param name="args">   Event information to send to registered event handlers. </param>
        private static void OnAssemblyLoadEventHandler(object sender, AssemblyLoadEventArgs args)
        {
            Debug.WriteLine($"ASSEMBLY LOADED: {args.LoadedAssembly.FullName}");
        }

        #endregion Methods
    }
}