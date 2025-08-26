using ErkurtHolding.IMES.Entity.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ErkurtHolding.IMES.Romania.OperatorPanel.GridModels
{
    /// <summary>
    /// View model for listing and refreshing interruption causes in a grid.
    /// </summary>
    public class InterruptionGridModel : INotifyPropertyChanged
    {
        private bool _changeData;
        private ObservableCollection<vw_InterruptionCauseGridModel> _interruptionCauseGridModels;

        /// <summary>
        /// A “trigger” flag: setting this to <c>true</c> will raise
        /// <see cref="PropertyChanged"/> for <see cref="interruptionCauseGridModels"/>
        /// and then automatically reset this flag back to <c>false</c>.
        /// Consumers can bind to this if they need to react to refresh events.
        /// </summary>
        /// <remarks>
        /// This property is not intended to be a persistent state.
        /// It is used to signal that the bound grid should refresh/re-evaluate the collection.
        /// </remarks>
        public bool changeData
        {
            get { return _changeData; }
            set
            {
                if (value == _changeData) return;

                // Update backing field and notify for changeData itself
                _changeData = value;
                OnPropertyChanged();

                if (value)
                {
                    // Notify that the collection changed (even if reference is same),
                    // useful for grid refresh scenarios.
                    OnPropertyChanged(nameof(interruptionCauseGridModels));

                    // Auto-reset trigger to false and notify again
                    _changeData = false;
                    OnPropertyChanged(nameof(changeData));
                }
            }
        }

        /// <summary>
        /// The collection displayed in the grid.
        /// Lazily initialized to a non-null collection to simplify binding.
        /// </summary>
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
                if (!ReferenceEquals(_interruptionCauseGridModels, value))
                {
                    _interruptionCauseGridModels = value ?? new ObservableCollection<vw_InterruptionCauseGridModel>();
                    OnPropertyChanged();
                }
            }
        }

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises <see cref="PropertyChanged"/> for the given property.
        /// </summary>
        /// <param name="name">The property name. Automatically supplied by the compiler.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string name = null)
        {
            var handler = PropertyChanged; // thread-safe capture
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Explicitly triggers a refresh notification for <see cref="interruptionCauseGridModels"/>.
        /// Equivalent to setting <see cref="changeData"/> to <c>true</c>.
        /// </summary>
        public void RefreshInterruptionGrid()
        {
            // This avoids toggling changeData if you prefer an imperative call
            OnPropertyChanged(nameof(interruptionCauseGridModels));
        }
    }
}
