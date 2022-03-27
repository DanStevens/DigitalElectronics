using System.Collections.ObjectModel;
using DigitalElectronics.Utilities;

namespace DigitalElectronics.ViewModels.Modules;

public interface IRegisterViewModel
{
    public event EventHandler EnableChanged;
    int NumberOfBits { get; }
    bool Enable { get; set; }
    bool Load { get; set; }
    IList<Bit> Data { get; set; }
    IReadOnlyList<bool> Probe { get; }
    IReadOnlyList<bool>? Output { get; }
    void Clock();
}
