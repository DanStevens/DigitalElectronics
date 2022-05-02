using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DigitalElectronics.Concepts;
using DigitalElectronics.Modules.Memory;
using DigitalElectronics.Utilities;
using DigitalElectronics.ViewModels.Modules.Annotations;
using DigitalElectronics.ViewModels.Utilities;

namespace DigitalElectronics.ViewModels.Modules;

public class SixteenByteRAMViewModel : INotifyPropertyChanged
{
    private const int AddressLength = 4;
    private readonly IRAM _ram;
    private ObservableCollection<Bit> _data;
    private FullyObservableCollection<Bit>? _address;
    private ObservableCollection<BitArray> _probe;
    private ObservableCollection<bool>? _output;
    private bool _load;
    private bool _enable;

    public SixteenByteRAMViewModel()
        : this(new SixteenByteDARAM())
    { }

    public SixteenByteRAMViewModel(int initialAddress = 0)
        : this(new SixteenByteDARAM(), initialAddress)
    { }

    public SixteenByteRAMViewModel(IRAM ram, int initialAddress = 0)
    {
        _ram = ram ?? throw new ArgumentNullException(nameof(ram));
        initialAddress = Math.Clamp(initialAddress, 0, ram.Capacity - 1);

        _data = new ObservableCollection<Bit>(CreateBits(_ram.WordSize, true));

        var initialAddressBitArray = new BitArray(new [] { initialAddress }).Trim(AddressLength);
        Address = new FullyObservableCollection<Bit>(CreateBits(initialAddressBitArray));

        _probe = new ObservableCollection<BitArray>(_ram.ProbeState());
        _output = _ram.Output != null ? new ObservableCollection<bool>(_ram.Output) : null;
    }

    private static IEnumerable<Bit> CreateBits(BitArray bitArray)
    {
        return bitArray.Select(b => new Bit(b));
    }
    private static IEnumerable<Bit> CreateBits(int length, bool bitState)
    {
        return Enumerable.Range(0, length).Select(_ => new Bit(bitState));
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

                // Assertion may fail when using mock IRAM
                //System.Diagnostics.Debug.Assert(value ? _ram.Output != null : _ram.Output == null,
                //    $"{nameof(_ram)}.{nameof(_ram.Output)} was {(value ? "not null" : "null")} " +
                //     "when not as expected.");
                
                SyncOutput();
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
                _ram.SetInputLD(value);
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

    public FullyObservableCollection<Bit> Address
    {
        get => _address!;
        set
        {
            if (_address?.SequenceEqual(value) != true)
            {
                var newAddress = value.ToBitArray().Trim(AddressLength);
                SyncAddress(newAddress);
                if (_address != null) _address.ItemPropertyChanged -= OnAddressBitChanged;
                _address = new FullyObservableCollection<Bit>(CreateBits(newAddress));
                _address.ItemPropertyChanged += OnAddressBitChanged;
                RaisePropertyChanged();
            }
        }
    }

    private void OnAddressBitChanged(object? sender, ItemPropertyChangedEventArgs e) =>
        SyncAddress(_address.ToBitArray().Trim(AddressLength));

    public ReadOnlyObservableCollection<bool>? Output => _output != null ? new ReadOnlyObservableCollection<bool>(_output) : null;

    public ReadOnlyObservableCollection<BitArray> ProbeAll => new(_probe);

    public void Clock()
    {
        if (Load && Enable)
            throw new InvalidOperationException("Load and Enable should not both be set high at the same time");

        _ram.SetInputD(_data.ToBitArray());
        SyncOutput();
        _ram.Clock();

        if (Load)
        {
            _probe = new ObservableCollection<BitArray>(_ram.ProbeState());
            RaisePropertyChanged(nameof(ProbeAll));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void SyncAddress(BitArray address)
    {

        ((IDedicatedAddrInput)_ram).SetInputA(address);
        SyncOutput();
    }

    private void SyncOutput()
    {
        _output = _ram.Output != null ? new ObservableCollection<bool>(_ram.Output) : null;
        RaisePropertyChanged(nameof(Output));
    }
}
