using System;
using System.Diagnostics;

namespace WinFileSearch
{
    public class MyTraceListener : TraceListener
    {
        public EventHandler<TraceEventArgs> EventHandler { get; set; }

        public MyTraceListener()
        {
        }

        public override void Write(string message)
        {
            if (EventHandler != null)
                EventHandler(this, new TraceEventArgs { TraceEventType = null, Message = message });
        }

        public override void WriteLine(string message)
        {
            if (EventHandler != null)
                EventHandler(this, new TraceEventArgs { TraceEventType = null, Message = message });
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            if(EventHandler != null)
                EventHandler(this, new TraceEventArgs { TraceEventType = eventType, Message = message });
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            if (EventHandler != null)
                EventHandler(this, new TraceEventArgs { TraceEventType = eventType, Message = String.Format(format, args) });
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if(EventHandler != null)
                EventHandler(this, new TraceEventArgs { TraceEventType = eventType, Message = data.ToString() });
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            if(EventHandler != null)
                EventHandler(this, new TraceEventArgs { TraceEventType = eventType, Message = data.ToString() });
        }
    }

    public class TraceEventArgs : EventArgs
    {
        public TraceEventType? TraceEventType { get; set; }
        public string Message { get; set; }
    }
}
