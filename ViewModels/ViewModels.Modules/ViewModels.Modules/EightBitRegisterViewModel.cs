using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DigitalElectronics.Components.Memory;
using DigitalElectronics.ViewModels.Modules.Annotations;
using DigitalElectronics.Utilities;

namespace DigitalElectronics.ViewModels.Modules;

public class EightBitRegisterViewModel : INotifyPropertyChanged
{
    private const int NumberOfBits = 8;

    //public event PropertyChangedEventHandler? PropertyChanged;

    private readonly IRegister _register;
    private ObservableCollection<bool> _data;
    private bool _load;
    private bool _enable;

    public EightBitRegisterViewModel()
        : this(new Register(NumberOfBits))
    { }

    public EightBitRegisterViewModel(IRegister register)
    {
        _register = register;

        _data = new ObservableCollection<bool>(_register.ProbeState().AsEnumerable());
    }

    public bool Enable
    {
        get => _enable;
        set
        {
            if (_enable != value)
            {
                _enable = value;
                _register.SetInputE(value);
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Output));
            }
        }
    }

    public bool Load
    {
        get => _load;
        set
        {
            if (_load != value)
            {
                _load = value;
                _register.SetInputL(value);
                RaisePropertyChanged();
            }
        }
    }

    public ObservableCollection<bool> Data
    {
        get => _data;
        set
        {
            if (value is null)
                value = new ObservableCollection<bool>(new BitArray(NumberOfBits).AsEnumerable());

            if (!_data.SequenceEqual(value))
            {
                _data = value;
                _register.SetInputD(new BitArray(value.ToArray()));
                RaisePropertyChanged();
                if (Enable) RaisePropertyChanged(nameof(Output));
            }
        }
    }

    public ReadOnlyObservableCollection<bool> Probe =>
        new(new ObservableCollection<bool>(_register.ProbeState().AsEnumerable()));

    public BitArray Output => null;

    public void Clock()
    {
        if (Load && Enable)
            throw new InvalidOperationException("Load and Enable should not both be set high at the same time");

        _register.Clock();
        if (Load) RaisePropertyChanged(nameof(Probe));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
