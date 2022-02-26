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
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using RDotNet;

    using Swiss;

    public static class Utils
    {
        /// <summary>
        /// The engine.
        /// </summary>
        public static REngine engine;

        //https://www.freestatistics.org/cran
        const String CRAN = "https://cran.r-project.org";


        #region Methods

        /// <summary>
        /// Makes a path relative.
        /// </summary>
        ///
        /// <param name="filename"> Filename of the file. </param>
        ///
        /// <returns>
        /// A string.
        /// </returns>
        public static string MakePathRelative(String filename)
        {

            return filename
                            .Replace(Path.GetDirectoryName(IniFile.AppData), "%AppData%")
                            .Replace(IniFile.AppDir, ".");
        }

        /// <summary>
        /// Initializes the rengine.
        /// </summary>
        ///
        /// <param name="label">   The label. </param>
        /// <param name="busyBar"> The busy bar control. </param>
        ///
        /// <returns>
        /// True if it succeeds, false if it fails.
        /// </returns>
        public static bool InitRengine(Label label, ProgressBar busyBar)
        {
            //! There seems to be a mismatch between the name of the MyDocuments special folder on BV5/6 (Dutch/Englis).

            //! See https://stackoverflow.com/questions/606483/what-is-the-meaning-of-these-windows-environment-variables-homedrive-homepath
            String mydocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            String mydocfolder = mydocuments.Split(new Char[] { Path.DirectorySeparatorChar }).Last();

            Logger.Info($"MyDocuments: '{mydocuments}'.");

            String display = GetDisplayName(Environment.SpecialFolder.MyDocuments);

            if (mydocfolder != display)
            {
                Logger.Warn($"MyDocuments Name/DisplayName Miscmatch: '{mydocfolder}' / '{display}'.");
            }

            String homedrive = Environment.GetEnvironmentVariable("HOMEDRIVE");
            String homepath = Environment.GetEnvironmentVariable("HOMEPATH");
            String homeshare = Environment.GetEnvironmentVariable("HOMESHARE");

            Logger.Info($"HOMEDRIVE: '{homedrive}'.");
            Logger.Info($"HOMEPATH: '{homepath}'.");
            Logger.Info($"HOMESHARE: '{homeshare}'.");

            //! Fix for OUNL BV5/6 (GetFolderPath is Dutch where GetDisplayName returns English).
            // 
            if (!Directory.Exists(mydocuments) || !String.IsNullOrEmpty(homeshare))
            // || !mydocuments.EndsWith(display)
            {
                Logger.Info($"MyDocuments issues, using alternative method");

                if (!String.IsNullOrEmpty(homedrive))
                {
                    mydocuments = Path.Combine(
                        homedrive + homepath, mydocfolder);
                }
                else if (!String.IsNullOrEmpty(homeshare))
                {
                    mydocuments = Path.Combine(homeshare + homepath, mydocfolder);
                }

                Logger.Info($"MyDocuments: '{mydocuments}'.");
            }

            label.InvokeIfRequired(o =>
            {
                label.Text = "Installing R packages";
            });

            busyBar.InvokeIfRequired(o =>
            {
                busyBar.Show();
                busyBar.Value = busyBar.Maximum;
            });

            try
            {
                //! Check R Engine version.
                //
                REngine.SetEnvironmentVariables();

                //! There are several options to initialize the engine, but by default the following suffice:
                engine = REngine.GetInstance();

                Logger.Info($"Using R v{engine.DllVersion}.");

                //! R uses only major.minor for custom library folders.
                // 
                String ver = engine.DllVersion;
                if (ver.Count(p => p == '.') == 2)
                {
                    ver = ver.Substring(0, ver.LastIndexOf('.'));
                }

                String rpath = $"{mydocuments.Replace("\\", "/")}/R/win-library/{ver}";

                Logger.Info($"R Install Path: '{rpath}'.");

                //! veg 15-07-2019
                //
                //! NOTE: We need an R version that allows Metrics 1.2 or higher.
                // DllVersion returns v3.6.1 (not 3.6 we need for the private library path)
                // Debug.WriteLine($"R.dll v{engine.DllVersion}");

                //! veg 15-07-2019
                // NOTE: We also need Metrics to be installed.
                // 
                // install.packages('Metrics')
                // unloadNamespace('Metrics')
                // library('Metrics')
                // install.packages('Metrics','R_LIBS=%HOMEDRIVE/%HOMEPATH%/Documents/R/win-library/3.6')
                //
                // In My Documents .Renviron file with location library like:
                // R_LIBS=%HOMEDRIVE%/%HOMEPATH%/Documents/R/win-library/3.6

                //---R

                try
                {
                    Logger.Info($"Checking presence of R 'Metrics' package.");

                    engine.Evaluate($"library('Metrics', lib.loc = '{rpath}')");
                }
                catch (EvaluationException)
                {
                    Logger.Info($"Downloading and Installing R 'Metrics' package.");
                    Logger.Info($"Using '{CRAN}' mirror.");
                    engine.Evaluate($"install.packages('Metrics', repos='{CRAN}', lib='{rpath}')");

                    Logger.Info($"Loading 'Metrics' package into R.");
                    engine.Evaluate($"library('Metrics', lib.loc = '{rpath}')");
                }

                //---R
                Logger.Info($"Checking R 'Metrics' package version.");
                {
                    CharacterVector e3 = engine.Evaluate("utils::packageVersion('Metrics')").AsCharacter();

                    String MetricsVersion = e3[0].Trim(new char[] { 'c', '(', ')' }).Replace(',', '.');
                    Logger.Info($"Detected Metrics v{MetricsVersion}.");

                    //! Check Metrics Version to be 0.1.4 to check if the R version is recent enough.
                    // 
                    Double mul = 1;
                    Double chk = 0;
                    foreach (String part in MetricsVersion.Split('.'))
                    {
                        chk += mul * Int32.Parse(part);
                        mul /= 10;
                    }

                    Logger.Info($"Detected R v{ver}.");

                    if (chk < 0.14)
                    {
                        Logger.Error($"R Version to low to load Metrics v0.1.4 or higher. Please upgrade R.");

                        return false;
                    }
                }

                Logger.Info($"Finding download source for 1.6.9 version of R psych package and its dependencies.");
/*
                //! Psych dependends on mnormt.
                // 

                //---R
                try
                {
                    Logger.Info($"Checking presence of R 'mnormt' package.");

                    engine.Evaluate($"library('mnormt', lib.loc = '{rpath}')");
                }
                catch (EvaluationException)
                {
                    Logger.Info($"Downloading and Installing R 'mnormt' package.");

                    engine.Evaluate($"install.packages('mnormt', repos='{CRAN}', lib='{rpath}')");
                }
                
                //---R

                //https://cran.r-project.org/package=%{packname}&version=%{version}#/%{packname}_%{version}.tar.gz
                // 

                try
                {
                    Logger.Info($"Checking presence of R 'psych' package.");

                    engine.Evaluate($"library('psych', lib.loc = '{rpath}')");
                }
                catch (EvaluationException)
                {
                    Logger.Info($"Downloading and Installing R 'psych' package.");

                    //! Fails because downloaded file is a tar.gz (but is saved without extension).
                    // 
                    //engine.Evaluate($"install.packages('{CRAN}/package=psych&version=1.6.9', repos=NULL, type='source', lib='{rpath}')");
                    // 
                    engine.Evaluate($"install.packages('{CRAN}/src/contrib/Archive/psych/psych_1.6.9.tar.gz', repos=NULL, type='source', lib='{rpath}')");

                    Logger.Info("Loading 'psych' package into R.");
                    engine.Evaluate($"library('psych', lib.loc = '{rpath}')");
                }



                //---R
*/

                try
                {
                    Logger.Info($"Checking presence of R 'psych' package.");

                    engine.Evaluate($"library('psych', lib.loc = '{rpath}')");
                }
                catch (EvaluationException)
                {
                    Logger.Info($"Downloading and Installing R 'psych' package.");
                    Logger.Info($"Using '{CRAN}' mirror.");
                    engine.Evaluate($"install.packages('psych', repos='{CRAN}', lib='{rpath}')");

                    Logger.Info($"Loading 'psych' package into R.");
                    engine.Evaluate($"library('psych', lib.loc = '{rpath}')");
                }

                return true;
            }
            catch (Exception e)
            {
                Logger.Error($"{e.GetType().Name} - {e.Message}.");

                MessageBox.Show(
                    $"Error initializing Rengine:\r\n\r\n{e.Message}",
                    Application.ProductName,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }
            finally
            {
                label.InvokeIfRequired(o =>
                {
                    label.Text = String.Empty;
                });

                busyBar.InvokeIfRequired(o =>
                {
                    busyBar.Hide();
                    busyBar.Value = busyBar.Minimum;
                });
            }
        }

        /// <summary>
        /// Gets display name.
        /// </summary>
        ///
        /// <param name="specialFolder"> Pathname of the special folder. </param>
        ///
        /// <returns>
        /// The display name.
        /// </returns>
        public static string GetDisplayName(Environment.SpecialFolder specialFolder)
        {
            IntPtr pidl = IntPtr.Zero;
            try
            {
                HResult hr = SHGetFolderLocation(IntPtr.Zero, (int)specialFolder, IntPtr.Zero, 0, out pidl);
                if (hr.IsFailure)
                    return null;

                if (0 != SHGetFileInfo(
                            pidl,
                            FILE_ATTRIBUTE_NORMAL,
                            out SHFILEINFO shfi,
                            (uint)Marshal.SizeOf(typeof(SHFILEINFO)),
                            SHGFI_PIDL | SHGFI_DISPLAYNAME))
                {
                    return shfi.szDisplayName;
                }
                return null;
            }
            finally
            {
                if (pidl != IntPtr.Zero)
                    ILFree(pidl);
            }
        }

        /// <summary>
        /// Gets display name.
        /// </summary>
        ///
        /// <param name="path"> Full pathname of the file. </param>
        ///
        /// <returns>
        /// The display name.
        /// </returns>
        public static string GetDisplayName(string path)
        {
            if (0 != SHGetFileInfo(
                        path,
                        FILE_ATTRIBUTE_NORMAL,
                        out SHFILEINFO shfi,
                        (uint)Marshal.SizeOf(typeof(SHFILEINFO)),
                        SHGFI_DISPLAYNAME))
            {
                return shfi.szDisplayName;
            }
            return null;
        }

        /// <summary>
        /// The file attribute normal.
        /// </summary>
        private const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;

        /// <summary>
        /// get display name.
        /// </summary>
        private const uint SHGFI_DISPLAYNAME = 0x000000200;

        /// <summary>
        /// pszPath is a pidl.
        /// </summary>
        private const uint SHGFI_PIDL = 0x000000008;

        /// <summary>
        /// Sh get file information.
        /// </summary>
        ///
        /// <param name="pszPath">          Full pathname of the file. </param>
        /// <param name="dwFileAttributes"> The file attributes. </param>
        /// <param name="psfi">             [out] The psfi. </param>
        /// <param name="cbFileInfo">       Information describing the file. </param>
        /// <param name="flags">            The flags. </param>
        ///
        /// <returns>
        /// An int.
        /// </returns>
        [DllImport("shell32", CharSet = CharSet.Unicode)]
        private static extern int SHGetFileInfo(String pszPath, uint dwFileAttributes, out SHFILEINFO psfi, uint cbFileInfo, uint flags);

        /// <summary>
        /// Sh get file information.
        /// </summary>
        ///
        /// <param name="pidl">             [out] The PIDL. </param>
        /// <param name="dwFileAttributes"> The file attributes. </param>
        /// <param name="psfi">             [out] The psfi. </param>
        /// <param name="cbFileInfo">       Information describing the file. </param>
        /// <param name="flags">            The flags. </param>
        ///
        /// <returns>
        /// An int.
        /// </returns>
        [DllImport("shell32", CharSet = CharSet.Unicode)]
        private static extern int SHGetFileInfo(IntPtr pidl, uint dwFileAttributes, out SHFILEINFO psfi, uint cbFileInfo, uint flags);

        /// <summary>
        /// Sh get folder location.
        /// </summary>
        ///
        /// <param name="hwnd">       The hwnd. </param>
        /// <param name="nFolder">    Pathname of the folder. </param>
        /// <param name="token">      The token. </param>
        /// <param name="dwReserved"> The reserved. </param>
        /// <param name="pidl">       [out] The PIDL. </param>
        ///
        /// <returns>
        /// A hResult.
        /// </returns>
        [DllImport("shell32", CharSet = CharSet.Unicode)]
        private static extern HResult SHGetFolderLocation(IntPtr hwnd, int nFolder, IntPtr token, int dwReserved, out IntPtr pidl);

        /// <summary>
        /// Il free.
        /// </summary>
        ///
        /// <param name="pidl"> The PIDL. </param>
        [DllImport("shell32", CharSet = CharSet.Unicode)]
        private static extern void ILFree(IntPtr pidl);

        /// <summary>
        /// A shfileinfo.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        /// <summary>
        /// A hresult.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct HResult
        {
            private int _value;

            /// <summary>
            /// Gets the value.
            /// </summary>
            ///
            /// <value>
            /// The value.
            /// </value>
            public int Value
            {
                get { return _value; }
            }

            /// <summary>
            /// Gets the exception.
            /// </summary>
            ///
            /// <value>
            /// The exception.
            /// </value>
            public Exception Exception
            {
                get { return Marshal.GetExceptionForHR(_value); }
            }

            /// <summary>
            /// Gets a value indicating whether this object is success.
            /// </summary>
            ///
            /// <value>
            /// True if this object is success, false if not.
            /// </value>
            public bool IsSuccess
            {
                get { return _value >= 0; }
            }

            /// <summary>
            /// Gets a value indicating whether this object is failure.
            /// </summary>
            ///
            /// <value>
            /// True if this object is failure, false if not.
            /// </value>
            public bool IsFailure
            {
                get { return _value < 0; }
            }
        }

        #endregion
    }
}
