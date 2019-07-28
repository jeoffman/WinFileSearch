using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace WinFileSearch
{
    public class MyTraceListener : TraceListener, INotifyPropertyChanged
    {
        private readonly StringBuilder _builder;

        public EventHandler<TraceEventArgs> EventHandler { get; set; }

        public MyTraceListener()
        {
            _builder = new StringBuilder();
        }

        public string Trace => _builder.ToString();

        public override void Write(string message)
        {
            _builder.Append(message);
            OnPropertyChanged(new PropertyChangedEventArgs("Trace"));
        }

        public override void WriteLine(string message)
        {
            _builder.AppendLine(message);
            OnPropertyChanged(new PropertyChangedEventArgs("Trace"));
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            EventHandler(this, new TraceEventArgs { TraceEventType = eventType, Message = message });

            //TraceEvent(eventCache, source, eventType, id, message);
            _builder.Append($"<Italic>{message}</Italic>");
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            _builder.Append(string.Format(format, args));
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

    }

    public class TraceEventArgs : EventArgs
    {
        public TraceEventType TraceEventType { get; set; }
        public string Message { get; set; }
    }
}
