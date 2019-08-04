using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WinFileSearch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MyTraceListener _traceListener = new MyTraceListener();
        private static TraceSource _traceSource = new TraceSource("WinFileSearch", SourceLevels.All);
        const int ErrorNumber = 123;


        public MainData MainData { get; set; } = new MainData();

        public MainWindow()
        {
            DataContext = MainData;

            InitializeComponent();

            _traceListener.EventHandler += OnTraceEvent;
            _traceListener.Filter = new EventTypeFilter(SourceLevels.All);
            Trace.Listeners.Add(_traceListener);
            _traceSource.Listeners.Add(_traceListener);

            _traceSource.TraceEvent(TraceEventType.Critical, ErrorNumber, $"Critical");
            _traceSource.TraceEvent(TraceEventType.Error, ErrorNumber, $"Error");
            _traceSource.TraceEvent(TraceEventType.Warning, ErrorNumber, $"Warning");
            _traceSource.TraceEvent(TraceEventType.Information, ErrorNumber, $"Information");
            _traceSource.TraceEvent(TraceEventType.Verbose, ErrorNumber, $"Verbose");
            Trace.WriteLine($"Test Trace");
            Trace.WriteLine($"Log Trace");
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.Save();
            base.OnClosing(e);
        }

        private void OnTraceEvent(object sender, TraceEventArgs e)
        {
            if(MainData.Traces != null)
                MainData.Traces.Add(new TraceInfo { TimeStamp = DateTime.Now, TraceEventType = e.TraceEventType, Message = e.Message });
        }

        /// <summary>
        /// WinForms has AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest and AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
        /// Too bad there is no WPF equivalent
        /// </summary>
        private void ComboSearchDirectory_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                var old = ComboSearchDirectory.Text;
                if (old.EndsWith("\\"))
                {
                    MainData.ComboDirectoryItems = Directory.GetDirectories(ComboSearchDirectory.Text);
                    ComboSearchDirectory.Text = old;
                    var cmbTextBox = (TextBox)ComboSearchDirectory.Template.FindName("PART_EditableTextBox", ComboSearchDirectory);
                    cmbTextBox.CaretIndex = old.Length;
                }
            }
            catch (Exception exc)
            {
                _traceSource.TraceData(TraceEventType.Error, ErrorNumber, exc);
            }
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            SaveFileNameSearchToSettings();
            SaveSearchTextToSettings();
        }

        private void SaveFileNameSearchToSettings()
        {
            var newText = ComboBoxFileName.Text.Trim();
            if (!string.IsNullOrWhiteSpace(newText))
            {   // arrange previous search FileName by putting the latest at the top
                List<string> newFileNames;
                if (Properties.Settings.Default.ComboBoxFileNames == null)
                    newFileNames = new List<string>();
                else
                    newFileNames = new List<string>(Properties.Settings.Default.ComboBoxFileNames);
                newFileNames.RemoveAll(x => x.Equals(newText, StringComparison.CurrentCultureIgnoreCase));
                newFileNames.Insert(0, newText);
                Properties.Settings.Default.ComboBoxFileNames = newFileNames.Take(10).ToArray();
            }
        }

        private void SaveSearchTextToSettings()
        {
            var newText = ComboBoxSearchText.Text.Trim();
            if (!string.IsNullOrWhiteSpace(newText))
            {   // arrange previous search FileName by putting the latest at the top
                List<string> newSearchText;
                if (Properties.Settings.Default.ComboBoxSearchText == null)
                    newSearchText = new List<string>();
                else
                    newSearchText = new List<string>(Properties.Settings.Default.ComboBoxSearchText);
                newSearchText.RemoveAll(x => x.Equals(newText, StringComparison.CurrentCultureIgnoreCase));
                newSearchText.Insert(0, newText);
                Properties.Settings.Default.ComboBoxSearchText = newSearchText.Take(10).ToArray();
            }
        }
    }
}
