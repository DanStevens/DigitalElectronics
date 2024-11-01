using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DigitalElectronics.Concepts;
using DigitalElectronics.ViewModels.Modules;
using DigitalElectronics.ViewModels.Modules.Annotations;

namespace DigitalElectronics.Boards;

public sealed class AluBoard : INotifyPropertyChanged, IDisposable
{
    private ReadOnlyObservableCollection<bool>? _busState;

    public AluBoard()
        : this(new EightBitAluViewModel(), new EightBitRegisterViewModel(), new EightBitRegisterViewModel())
    {
    }

    public AluBoard(
        IAluViewModel aluVM,
        IRegisterViewModel registerAVM,
        IRegisterViewModel registerBVM)
    {
        ALU = aluVM;
        RegisterA = registerAVM;
        RegisterB = registerBVM;

        ALU.EnableChanged += OnAluEnableChanged;
        RegisterA.EnableChanged += OnRegisterEnableChanged;
        RegisterB.EnableChanged += OnRegisterEnableChanged;

        ALU.SetInputA(RegisterA.Probe);
        ALU.SetInputB(RegisterB.Probe);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public IAluViewModel ALU { get; }

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
        CheckForBusContention();

        if (RegisterB.Enable) SyncRegisterWithBus(RegisterA);
        if (RegisterA.Enable) SyncRegisterWithBus(RegisterB);
        if (ALU.Enable) SyncRegisterWithBus(RegisterA);
        if (ALU.Enable) SyncRegisterWithBus(RegisterB);

        RegisterA.Clock();
        RegisterB.Clock();

        ALU.SetInputA(RegisterA.Probe);
        ALU.SetInputB(RegisterB.Probe);
        if (ALU.Enable) SyncBusWithAlu();
    }

    public void Dispose()
    {
        RegisterA.EnableChanged -= OnRegisterEnableChanged;
        RegisterB.EnableChanged -= OnRegisterEnableChanged;
    }

    private void OnRegisterEnableChanged(object? sender, EventArgs e)
    {
        var register = (IRegisterViewModel)sender!;
        try
        {
            CheckForBusContention();
            //if (register.Enable) BusState = GetRegisterState(register);
            BusState = GetRegisterState(register);
        }
        catch (BusContentionException)
        {
            // Undo the enabling of this register
            // Set register.Enable to false without retriggering this method
            register.EnableChanged -= OnRegisterEnableChanged;
            register.Enable = false;
            register.EnableChanged += OnRegisterEnableChanged;
        }
    }

    private void OnAluEnableChanged(object? sender, EventArgs e)
    {
        try
        {
            CheckForBusContention();
            SyncBusWithAlu();
        }
        catch (BusContentionException)
        {
            // Undo the enabling of the ALU
            // Set ALU.Enable to false without retriggering this method
            ALU.EnableChanged -= OnAluEnableChanged;
            ALU.Enable = false;
            ALU.EnableChanged += OnAluEnableChanged;
        }
    }


    private static ReadOnlyObservableCollection<bool>? GetRegisterState(IRegisterViewModel register)
    {
        return register.Enable ?
            new ReadOnlyObservableCollection<bool>(
                new ObservableCollection<bool>(register.Output!)) :
            null;
    }

    private void CheckForBusContention()
    {
        // If more than one component is enabled, it's a bus contention
        if (new[] { RegisterA.Enable, RegisterB.Enable, ALU.Enable }.Count(b => b) > 1)
            throw new BusContentionException();
    }

    private void SyncRegisterWithBus(IRegisterViewModel register)
    {
        if (register.Load)
        {
            for (var i = 0; i < register.Data.Count; i++)
            {
                register.Data[i] = BusState?[i] ?? false;
            }
        }
    }

    private void SyncBusWithAlu()
    {
        BusState = ALU.Enable
            ? new ReadOnlyObservableCollection<bool>(
                new ObservableCollection<bool>(ALU.OutputE!))
            : null;
    }

    [NotifyPropertyChangedInvocator]
    private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
