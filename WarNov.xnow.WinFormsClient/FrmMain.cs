using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WarNov.xnow.WinFormsClient
{
    public partial class FrmMain : Form
    {
        static readonly string ws2path = Environment.GetEnvironmentVariable("WS2PATH");
        static readonly string computerName = Environment.GetEnvironmentVariable("COMPUTERNAME");
        static List<Executable> customExes;
        private IKeyboardMouseEvents m_GlobalHook;


        #region Initializing Routines
        public FrmMain()
        {
            InitializeComponent();
            RefreshCommands();
            Subscribe();
        }

        private void RefreshCommands()
        {
            ReadCustomExes();
            LoadAutoCompleteText();
        }

        private void LoadAutoCompleteText()
        {

            var source = new AutoCompleteStringCollection();

            var currentDir = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            var autoCompleteTextPath = Path.Combine(currentDir, "autocomplete.txt");
            var autoCompleteLines = File.ReadAllLines(autoCompleteTextPath);
            source.AddRange(autoCompleteLines);
            TxtCommand.AutoCompleteCustomSource = source;
            TxtCommand.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private static void ReadCustomExes()
        {
            customExes = new List<Executable>();
            if (Directory.Exists(ws2path))
            {
                var customExesDirPath = Path.Combine(ws2path, computerName);
                foreach (var filePath in Directory.GetFiles(customExesDirPath))
                {
                    FileInfo file = new FileInfo(filePath);
                    customExes.Add(new Executable()
                    {
                        Name = GetNameWithoutExtension(file.Name),
                        Extension = file.Extension,
                        FilePath = file.FullName
                    });
                }
            }
        }

        private static string GetNameWithoutExtension(string name)
        {
            const string DOT = ".";
            if (name.Contains(DOT))
            {
                var idx = name.LastIndexOf(".");
                var ret = name.Remove(idx);
                return ret;
            }
            else return name;
        }

        public void Subscribe()
        {
            // Note: for the application hook, use the Hook.AppEvents() instead
            m_GlobalHook = Hook.GlobalEvents();
            m_GlobalHook.KeyUp += GlobalHook_KeyUp;
        }
        #endregion

        #region UI Event Handlers      

        private void TxtCommand_MouseClick(object sender, MouseEventArgs e)
        {
            TxtCommand.SelectAll();
        }

        private void TxtCommand_Enter(object sender, EventArgs e)
        {
            TxtCommand.SelectAll();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            TxtCommand.Focus();
        }

        private void FrmMain_Deactivate(object sender, EventArgs e)
        {
            HideForm();
        }

        private void NtfMain_Click(object sender, EventArgs e)
        {
            ShowForm();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            NtfMain.Visible = false;
            Application.DoEvents();
        }
        #endregion

        #region UI Tasks
        private void HideApplication()
        {
            HideForm();
        }

        private void HideForm()
        {
            Hide();
            if (NtfMain == null)
                NtfMain.Visible = true;
        }

        private void ShowForm()
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            NtfMain.Visible = false;
            TxtCommand.Focus();
            TxtCommand.SelectAll();
        }
        #endregion

        #region Command Processing
        private void GlobalHook_KeyUp(object sender, KeyEventArgs e)
        {
            //Keyboard handling when app is opened
            if (Visible)
            {
                if (e.KeyCode == Keys.Enter)
                    ProcessCommand();
                else if (e.KeyCode == Keys.Escape)
                    HideApplication();
                else if (e.KeyCode == Keys.F5)
                    RefreshCommands();
            }
            //form hidden
            else
            {
                if (e.KeyCode == Keys.Multiply && e.Shift && e.Control)
                {
                    ShowForm();
                    Activate();
                }
            }
        }

        private void ProcessCommand()
        {
            HideForm();
            ExecuteCommand(TxtCommand.Text);
        }

        void ExecuteCommand(string command)
        {
            var currentExecutable = new Executable()
            {
                Name = command
            };

            if (currentExecutable.InList(customExes))
            {
                command = currentExecutable.FilePath;
            }

            var processInfo = new ProcessStartInfo("cmd.exe", "/c " + command)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            var process = Process.Start(processInfo);

            process.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
                Debug.WriteLine("output>>" + e.Data);
            process.BeginOutputReadLine();

            process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
                Debug.WriteLine("error>>" + e.Data);
            process.BeginErrorReadLine();

            process.WaitForExit();

            Debug.WriteLine("ExitCode: {0}", process.ExitCode);
            process.Close();
        }
        #endregion


    }
}
