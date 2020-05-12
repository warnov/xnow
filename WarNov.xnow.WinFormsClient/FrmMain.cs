using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Windows.Forms;

namespace WarNov.xnow.WinFormsClient
{
    public partial class FrmMain : Form
    {
        static List<Executable> customExes;
        //Inside xNowDirPath we can find all of our custom shortcuts plus the autocomplete.txt file for using
        static readonly string xNowDirPath = Environment.GetEnvironmentVariable("XNOWDIRPATH");
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
            var autoCompleteTextPath = Path.Combine(xNowDirPath, "autocomplete.txt");
            var autoCompleteLines = File.ReadAllLines(autoCompleteTextPath);
            source.AddRange(autoCompleteLines);
            TxtCommand.AutoCompleteCustomSource = source;
            TxtCommand.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private static void ReadCustomExes()
        {
            customExes = new List<Executable>();

            foreach (var filePath in Directory.GetFiles(xNowDirPath))
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
            var command = TxtCommand.Text.ToLower();
            HideForm();
            CommandType cmdType = GetCommandType(command, out string modifiedCommand);
            switch (cmdType)
            {
                case CommandType.xnow:
                    switch (modifiedCommand)
                    {
                        //Exit app
                        case "exit":
                        case "bye":
                            NtfMain.Visible = false;
                            Application.DoEvents();
                            Environment.Exit(1);
                            break;
                    }
                    break;
                case CommandType.cmd:
                    break;
                case CommandType.shell:
                    switch (modifiedCommand)
                    {
                        case "controlpanel":
                            ExecuteShell("controlPanelFolder");
                            break;
                        case "apps":
                            ExecuteShell("appsFolder");
                            break;
                    }
                    break;
                case CommandType.lnk:
                    ExecuteCommand(modifiedCommand);
                    break;
                case CommandType.internetSearch:
                    ExecuteInternetSearch(modifiedCommand);
                    break;
                case CommandType.restCall:
                    break;
                default:
                    break;
            }
        }       

        private CommandType GetCommandType(string command, out string modifiedCommand)
        {
            modifiedCommand = command;
            if (command.EndsWith("?"))
                return CommandType.internetSearch;
            else
            {
                modifiedCommand = command;
                if (command.EndsWith("?"))
                    return CommandType.internetSearch;
                else
                {
                    switch (command)
                    {
                        //Exit app
                        case "exit":
                        case "bye":
                            return CommandType.xnow;
                        //Shell Commands
                        case "controlpanel":
                        case "apps":
                            return CommandType.shell;
                        default:
                            var lnkPath = LnkPath(command);
                            if (!String.IsNullOrEmpty(lnkPath))
                            {
                                modifiedCommand = lnkPath;
                                return CommandType.lnk;
                            }
                            else
                            {
                                return CommandType.cmd;
                            }
                    }
                }
            }
        }

        //If the command is a lnk returns its path. Otherwise returns null
        private string LnkPath(string command)
        {
            var currentExecutable = new Executable()
            {
                Name = command
            };
            if (currentExecutable.InList(customExes))
            {
                return currentExecutable.FilePath;
            }
            else return null;
        }

        private void ExecuteInternetSearch(string command)
        {
            var searchString = HttpUtility.UrlEncode(command.Remove(command.Length - 1));
            var finalUrl = $"https://www.google.com/search?q={searchString}";
            var internetSearchCommand = @$"""C:\Users\warnov\OneDrive\wscript\UNDERBEAST\web.lnk"" {finalUrl}";
            ExecuteCommand(internetSearchCommand);
        }

        private void ExecuteShell(string command)
        {
            var shellCommand = $"start shell:{command}";
            ExecuteCommand(shellCommand);
        }

        void ExecuteCommand(string command)
        {
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
        }   
        #endregion
    }
}