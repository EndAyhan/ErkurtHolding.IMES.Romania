using ErkurtHolding.IMES.Entity.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.GridModels
{
    public class InterruptionGridModel : INotifyPropertyChanged
    {
        private bool _changeData;

        public bool changeData
        {
            get { return _changeData; }
            set
            {
                if (value)
                {
                    NotifyPropertyChanged("interruptionCauseGridModels");
                    value = false;
                }
                _changeData = value;
            }
        }

        private ObservableCollection<vw_InterruptionCauseGridModel> _interruptionCauseGridModels;

        public ObservableCollection<vw_InterruptionCauseGridModel> interruptionCauseGridModels
        {
            get
            {
                if (_interruptionCauseGridModels == null)
                    _interruptionCauseGridModels = new ObservableCollection<vw_InterruptionCauseGridModel>();

                return _interruptionCauseGridModels;
            }
            set
            {
                _interruptionCauseGridModels = value;
                NotifyPropertyChanged("interruptionCauseGridModels");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
