using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DigitalElectronics.Concepts;
using DigitalElectronics.Utilities;
using DigitalElectronics.ViewModels.Modules;
using DigitalElectronics.ViewModels.Modules.Annotations;

namespace DigitalElectronics.Boards;

public sealed class BusTransferBoard : INotifyPropertyChanged, IDisposable
{
    private ReadOnlyObservableCollection<bool>? _busState;

    public BusTransferBoard() : this(new EightBitRegisterViewModel(), new EightBitRegisterViewModel())
    {
    }

    public BusTransferBoard(IRegisterViewModel registerA, IRegisterViewModel registerB)
    {
        RegisterA = registerA;
        RegisterB = registerB;

        RegisterA.EnableChanged += OnRegisterEnableChanged;
        RegisterB.EnableChanged += OnRegisterEnableChanged;
    }

    public IRegisterViewModel RegisterA { get; }

    public IRegisterViewModel RegisterB { get; }

    public ReadOnlyObservableCollection<bool>? BusState
    {
        get => _busState;
        private set
        {
            if (_busState != value)
            {
                _busState = value;
                RaisePropertyChanged();
            }
        }
    }

    public void Clock()
    {
        if (RegisterA.Enable && RegisterB.Enable)
            throw new BusCollisionException("Register A and Register B should not be enabled at the same time.");

        if (RegisterB.Enable) SyncRegisterWithBus(RegisterA);
        RegisterA.Clock();
        if (RegisterA.Enable) SyncRegisterWithBus(RegisterB);
        RegisterB.Clock();
    }

    private void SyncRegisterWithBus(IRegisterViewModel register)
    {
        for (var i = 0; i < register.Data.Count; i++)
        {
            register.Data[i].Value = BusState?[i] ?? false;
        }
    }

    private void OnRegisterEnableChanged(object? sender, EventArgs e)
    {
        BusState = GetRegisterState((IRegisterViewModel)sender);
    }

    private ReadOnlyObservableCollection<bool>? GetRegisterState(IRegisterViewModel register)
    {
        return register.Enable ?
            new ReadOnlyObservableCollection<bool>(
                new ObservableCollection<bool>(register.Output!)) :
            null;
    }

    public void Dispose()
    {
        RegisterA.EnableChanged -= OnRegisterEnableChanged;
        RegisterB.EnableChanged -= OnRegisterEnableChanged;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
