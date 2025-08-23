using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.Models
{
    /// <summary>
    /// Represents an additional OPC read point (node) defined on the panel.
    /// Notifies UI when <see cref="readValue"/> changes.
    /// </summary>
    [Serializable]
    public class OpcOtherReadModel : INotifyPropertyChanged
    {
        /// <summary>Special code id mapping this node's semantics (e.g., PokaYoke, RunMode).</summary>
        public Guid SpecialCodeId { get; set; }

        /// <summary>OPC UA NodeId string to read from (e.g., "ns=2;s=Machine/Counter").</summary>
        public string NodeId { get; set; }

        /// <summary>Human‑readable description.</summary>
        public string Description { get; set; }

        /// <summary>Optional location/area hint.</summary>
        public string Location { get; set; }

        // Backing field for readValue
        private decimal _readValue;

        /// <summary>
        /// Last read decimal value from OPC. Setting this raises <see cref="PropertyChanged"/>.
        /// </summary>
        public decimal readValue
        {
            get { return _readValue; }
            set
            {
                if (_readValue != value)
                {
                    _readValue = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(DisplayValue));
                }
            }
        }

        /// <summary>
        /// Rounded string representation (0 decimals) for quick display in grids/labels.
        /// </summary>
        public string DisplayValue
        {
            get { return Math.Round(readValue, 0).ToString(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Raises <see cref="PropertyChanged"/> for data binding.</summary>
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
