using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Models
{
    public class OpcOtherReadModel : INotifyPropertyChanged
    {
        public Guid SpecialCodeId { get; set; }
        public string NodeId { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        private string Value { get => Math.Round(readValue, 0).ToString(); }

        private decimal ReadValue;

        public decimal readValue
        {
            get { return ReadValue; }
            set
            {
                ReadValue = value;
                NotifyPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}
