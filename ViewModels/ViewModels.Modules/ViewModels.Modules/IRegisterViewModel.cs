using System.Collections.ObjectModel;
using DigitalElectronics.Components.Memory;
using DigitalElectronics.Concepts;
using DigitalElectronics.Utilities;

namespace DigitalElectronics.ViewModels.Modules;

public interface IRegisterViewModel
{
    event EventHandler EnableChanged;
    int NumberOfBits { get; }
    bool Enable { get; set; }
    bool Load { get; set; }
    IList<Bit> Data { get; set; }
    IReadOnlyList<bool> Probe { get; }
    IReadOnlyList<bool>? Output { get; }
    void Clock();
}
