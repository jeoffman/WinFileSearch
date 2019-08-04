using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace WinFileSearch
{
    [Notify]
    public class MainData : INotifyPropertyChanged
    {
        public ObservableCollection<TraceInfo> Traces { get { return traces; } set { SetProperty(ref traces, value, tracesPropertyChangedEventArgs); } }
        public string[] ComboDirectoryItems { get { return comboDirectoryItems; } set { SetProperty(ref comboDirectoryItems, value, comboDirectoryItemsPropertyChangedEventArgs); } }


        public MainData()
        {
        }

        #region NotifyPropertyChangedGenerator

        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<TraceInfo> traces = new ObservableCollection<TraceInfo>();
        private static readonly PropertyChangedEventArgs tracesPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(Traces));
        private string[] comboDirectoryItems;
        private static readonly PropertyChangedEventArgs comboDirectoryItemsPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(ComboDirectoryItems));

        private void SetProperty<T>(ref T field, T value, PropertyChangedEventArgs ev)
        {
            if (!System.Collections.Generic.EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                PropertyChanged?.Invoke(this, ev);
            }
        }

        #endregion
    }

    public class TraceInfo
    {
        public DateTime TimeStamp { get; set; }
        public TraceEventType? TraceEventType { get; set; }
        public string Message { get; set; }
    }
}
