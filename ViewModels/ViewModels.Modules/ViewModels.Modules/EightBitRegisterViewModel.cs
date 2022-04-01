using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DigitalElectronics.Components.Memory;
using DigitalElectronics.Concepts;
using DigitalElectronics.ViewModels.Modules.Annotations;
using DigitalElectronics.Utilities;
using BitArray = DigitalElectronics.Concepts.BitArray;

namespace DigitalElectronics.ViewModels.Modules;

public sealed class EightBitRegisterViewModel : INotifyPropertyChanged, IRegisterViewModel
{
    private const int _NumberOfBits = 8;

    //public event PropertyChangedEventHandler? PropertyChanged;

    private readonly IRegister _register;
    private ObservableCollection<Bit> _data;
    private ObservableCollection<bool> _output;
    private bool _load;
    private bool _enable;

    public EightBitRegisterViewModel()
        : this(new Register(_NumberOfBits))
    { }

    public EightBitRegisterViewModel(IRegister register)
    {
        _register = register;

        var bits = _register.ProbeState().ToArray<bool>();
        _data = new ObservableCollection<Bit>(bits.Select(b => new Bit(b)));
        _data.CollectionChanged += OnDataBitChanged;
        _output = new ObservableCollection<bool>(bits);
    }

    private void OnDataBitChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        Sync();
        RaisePropertyChanged(nameof(Data));
        RaisePropertyChanged(nameof(Probe));
    }

    public int NumberOfBits => _NumberOfBits;

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
                EnableChanged?.Invoke(this, EventArgs.Empty);
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

    public ObservableCollection<Bit> Data
    {
        get => _data;
        set
        {
            if (value is null)
                value = new ObservableCollection<Bit>(new BitArray(_NumberOfBits));

            if (!_data.SequenceEqual(value))
            {
                _data = value;
                Sync();
                RaisePropertyChanged();
                if (Enable) RaisePropertyChanged(nameof(Output));
            }
        }
    }

    private void Sync()
    {
        var bits = _data.Select(_ => _.Value).ToArray();
        _register.SetInputD(new BitArray(bits));
        _output = new ObservableCollection<bool>(bits);
    }

    public ReadOnlyObservableCollection<bool> Probe =>
        new(new ObservableCollection<bool>(_register.ProbeState()));

    public ReadOnlyObservableCollection<bool>? Output => Enable ? new ReadOnlyObservableCollection<bool>(_output) : null;

    IList<Bit> IRegisterViewModel.Data
    {
        get => Data;
        set => Data = new ObservableCollection<Bit>(value);
    }

    IReadOnlyList<bool> IRegisterViewModel.Probe => Probe;

    IReadOnlyList<bool>? IRegisterViewModel.Output => Output;

    public void Clock()
    {
        if (Load && Enable)
            throw new InvalidOperationException("Load and Enable should not both be set high at the same time");

        Sync();
        _register.Clock();
        if (Load) RaisePropertyChanged(nameof(Probe));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public event EventHandler EnableChanged;

    public event EventHandler DataChanged;
}
