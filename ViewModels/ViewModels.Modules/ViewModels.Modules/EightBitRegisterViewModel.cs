using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DigitalElectronics.Components.Memory;
using DigitalElectronics.Concepts;
using DigitalElectronics.ViewModels.Modules.Annotations;
using DigitalElectronics.Utilities;

namespace DigitalElectronics.ViewModels.Modules;

public class EightBitRegisterViewModel : RegisterViewModel
{
    public EightBitRegisterViewModel()
        : base(8)
    {
    }

    public EightBitRegisterViewModel(IReadWriteRegister register)
        : base(register)
    {
    }
}
