using DigitalElectronics.Concepts;
using DigitalElectronics.Modules.ALUs;
using DigitalElectronics.Utilities;
using DigitalElectronics.ViewModels.Modules;

namespace DigitalElectronics.Boards;

public class AluBoard
{
    public AluBoard(IRegisterViewModel registerA, IRegisterViewModel registerB, IArithmeticLogicUnit alu)
    {
        RegisterA = registerA;
        RegisterB = registerB;
        ALU = alu;
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
}
