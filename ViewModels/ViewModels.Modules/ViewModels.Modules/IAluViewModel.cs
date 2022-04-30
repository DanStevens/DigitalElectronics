using System.Collections.ObjectModel;
using System.ComponentModel;
using DigitalElectronics.Concepts;

namespace DigitalElectronics.ViewModels.Modules;

public interface IAluViewModel : INotifyPropertyChanged
{
    event EventHandler? EnableChanged;
    int WordSize { get; }
    bool Enable { get; set; }
    bool Subtract { get; set; }
    IReadOnlyList<bool> Probe { get; }
    IReadOnlyList<bool>? OutputE { get; }
    void SetInputA(IEnumerable<bool> value);
    void SetInputB(IEnumerable<bool> value);
}
