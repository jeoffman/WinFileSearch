using System;
using System.Diagnostics;
using System.Windows;

namespace WinFileSearch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MyTraceListener _traceListener = new MyTraceListener();
        private static TraceSource _traceSource = new TraceSource("WinFileSearch", SourceLevels.All);


        public MainData MainData { get; set; } = new MainData();

        public MainWindow()
        {
            DataContext = MainData;

            InitializeComponent();

            _traceListener.EventHandler += OnTraceEvent;
            _traceListener.Filter = new EventTypeFilter(SourceLevels.All);
            Trace.Listeners.Add(_traceListener);
            _traceSource.Listeners.Add(_traceListener);

            _traceSource.TraceEvent(TraceEventType.Critical, 123, $"Critical");
            _traceSource.TraceEvent(TraceEventType.Error, 123, $"Error");
            _traceSource.TraceEvent(TraceEventType.Warning, 123, $"Warning");
            _traceSource.TraceEvent(TraceEventType.Information, 123, $"Information");
            _traceSource.TraceEvent(TraceEventType.Verbose, 123, $"Verbose");
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
            MainData.Traces.Add(new TraceInfo { TimeStamp = DateTime.Now, TraceEventType = e.TraceEventType, Message = e.Message });
        }


    }
}
