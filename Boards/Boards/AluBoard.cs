using DigitalElectronics.Concepts;
using DigitalElectronics.Modules.ALUs;
using DigitalElectronics.Utilities;
using DigitalElectronics.ViewModels.Modules;

namespace DigitalElectronics.Boards;

public sealed class AluBoard : IDisposable
{
    public AluBoard(IRegisterViewModel registerA, IRegisterViewModel registerB, IArithmeticLogicUnit alu)
    {
        RegisterA = registerA;
        RegisterB = registerB;
        ALU = alu;

        RegisterA.DataChanged += OnRegisterADataChanged;
        RegisterB.DataChanged += OnRegisterBDataChanged;
    }

    private void OnRegisterADataChanged(object? sender, EventArgs e)
    {
        ALU.SetInputA(RegisterA.Register.ProbeState());
    }

    private void OnRegisterBDataChanged(object? sender, EventArgs e)
    {
        ALU.SetInputB(RegisterB.Register.ProbeState());
    }


    public IRegisterViewModel RegisterA { get; }

    public IRegisterViewModel RegisterB { get; }

    public IArithmeticLogicUnit ALU { get; }

    public void Clock()
    {
        if (RegisterA.Enable && RegisterB.Enable)
            throw new BusCollisionException("Register A and Register B should not be enabled at the same time.");
        
        RegisterA.Clock();
        RegisterB.Clock();
    }

    public void Dispose()
    {
        RegisterA.DataChanged -= OnRegisterADataChanged;
        RegisterB.DataChanged -= OnRegisterBDataChanged;
    }
}
