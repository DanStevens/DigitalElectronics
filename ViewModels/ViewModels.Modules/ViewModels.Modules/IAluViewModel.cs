using System.Collections.ObjectModel;
using System.ComponentModel;
using DigitalElectronics.Concepts;

namespace DigitalElectronics.ViewModels.Modules;

public interface IAluViewModel : INotifyPropertyChanged
{
    bool Enable { get; set; }
    ReadOnlyObservableCollection<bool>? OutputE { get; }
    bool Subtract { get; set; }
    ReadOnlyObservableCollection<bool> Probe { get; }
    void SetInputA(BitArray value);
    void SetInputB(BitArray value);
}
