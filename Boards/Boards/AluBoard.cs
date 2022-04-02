using DigitalElectronics.Components.Memory;
using DigitalElectronics.Concepts;
using DigitalElectronics.Modules.ALUs;
using DigitalElectronics.Utilities;
using DigitalElectronics.ViewModels.Modules;

namespace DigitalElectronics.Boards;

public sealed class AluBoard : IDisposable
{
    private readonly IRegister _registerA;
    private readonly IRegister _registerB;
    private readonly IArithmeticLogicUnit _alu;

    public AluBoard(
        IRegisterViewModel registerAVM,
        IRegister registerA,
        IRegisterViewModel registerBVM,
        IRegister registerB,
        IArithmeticLogicUnit alu)
    {
        _alu = alu;
        _registerA = registerA;
        _registerB = registerB;
        RegisterAVM = registerAVM;
        RegisterBVM = registerBVM;

        RegisterAVM.DataChanged += OnRegisterADataChanged;
        RegisterBVM.DataChanged += OnRegisterBDataChanged;
    }

    private void OnRegisterADataChanged(object? sender, EventArgs e)
    {
        _alu.SetInputA(_registerA.ProbeState());
    }

    private void OnRegisterBDataChanged(object? sender, EventArgs e)
    {
        _alu.SetInputB(_registerB.ProbeState());
    }

    public IRegisterViewModel RegisterAVM { get; }

    public IRegisterViewModel RegisterBVM { get; }

    public void Clock()
    {
        if (RegisterAVM.Enable && RegisterBVM.Enable)
            throw new BusCollisionException("Register A and Register B should not be enabled at the same time.");
        
        RegisterAVM.Clock();
        RegisterBVM.Clock();
    }

    public void Dispose()
    {
        RegisterAVM.DataChanged -= OnRegisterADataChanged;
        RegisterBVM.DataChanged -= OnRegisterBDataChanged;
    }
}
