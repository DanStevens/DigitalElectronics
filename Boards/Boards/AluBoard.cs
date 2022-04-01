using DigitalElectronics.Components.Memory;
using DigitalElectronics.Concepts;
using DigitalElectronics.Modules.ALUs;
using DigitalElectronics.Utilities;
using DigitalElectronics.ViewModels.Modules;

namespace DigitalElectronics.Boards;

public sealed class AluBoard : IDisposable
{
    public AluBoard(
        IRegisterViewModel registerAVM,
        IRegister registerA,
        IRegisterViewModel registerBVM,
        IRegister registerB,
        IArithmeticLogicUnit alu)
    {
        RegisterAVM = registerAVM;
        RegisterA = registerA;
        RegisterBVM = registerBVM;
        RegisterB = registerB;
        ALU = alu;

        RegisterAVM.DataChanged += OnRegisterADataChanged;
        RegisterBVM.DataChanged += OnRegisterBDataChanged;
    }

    private void OnRegisterADataChanged(object? sender, EventArgs e)
    {
        ALU.SetInputA(RegisterA.ProbeState());
    }

    private void OnRegisterBDataChanged(object? sender, EventArgs e)
    {
        ALU.SetInputB(RegisterB.ProbeState());
    }

    public IRegisterViewModel RegisterAVM { get; }

    public IRegisterViewModel RegisterBVM { get; }

    public IRegister RegisterA { get; }

    public IRegister RegisterB { get; }

    public IArithmeticLogicUnit ALU { get; }

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
