using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DigitalElectronics.Concepts;
using DigitalElectronics.Modules.Memory;
using DigitalElectronics.Utilities;
using DigitalElectronics.ViewModels.Modules.Annotations;

namespace DigitalElectronics.ViewModels.Modules;

public class SixteenByteRAMViewModel : INotifyPropertyChanged
{
    private readonly IRAM _ram;
    private ObservableCollection<Bit> _data;
    private ObservableCollection<Bit> _address;
    private ObservableCollection<BitArray> _probe;
    private ObservableCollection<bool> _output;
    private bool _load;
    private bool _enable;

    public SixteenByteRAMViewModel()
        : this(new SixteenByteRAM())
    { }

    public SixteenByteRAMViewModel(IRAM ram)
    {
        _ram = ram ?? throw new ArgumentNullException(nameof(ram));

        _data = CreateBitObservableCollection(_ram.WordSize, true);

        var initialAddress = new BitArray(new[] { _ram.Capacity - 1 }).Trim();
        _ram.SetInputA(initialAddress);
        _address = CreateBitObservableCollection(initialAddress.Count);

        _probe = new ObservableCollection<BitArray>(_ram.ProbeState());
        _output = new ObservableCollection<bool>(_ram.ProbeState(initialAddress));
    }

    private static ObservableCollection<Bit> CreateBitObservableCollection(int length, bool bitState = false)
    {
        var bits = Enumerable.Range(0, length).Select(b => new Bit(bitState));
        return new ObservableCollection<Bit>(bits);
    }

    public bool Enable
    {
        get => _enable;
        set
        {
            if (_enable != value)
            {
                _enable = value;
                _ram.SetInputE(value);
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
                _ram.SetInputL(value);
                RaisePropertyChanged();
            }
        }
    }

    public ObservableCollection<Bit> Data
    {
        get => _data;
        set
        {
            if (!_data.SequenceEqual(value))
            {
                _data = value;
                _ram.SetInputD(_data.ToBitArray());
                RaisePropertyChanged();
                if (Enable) RaisePropertyChanged(nameof(Output));
            }
        }
    }

    public ObservableCollection<Bit> Address
    {
        get => _address;
        set
        {
            if (!_address.SequenceEqual(value))
            {
                _address = value;
                _ram.SetInputA(_address.ToBitArray().Trim(4));
                RaisePropertyChanged();
            }
        }
    }

    public ReadOnlyObservableCollection<bool>? Output => Enable ? new(_output) : null;

    public ReadOnlyObservableCollection<BitArray> ProbeAll => new(_probe);

    public void Clock()
    {
        if (Load && Enable)
            throw new InvalidOperationException("Load and Enable should not both be set high at the same time");

        _ram.Clock();
        if (Load) RaisePropertyChanged(nameof(ProbeAll));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
