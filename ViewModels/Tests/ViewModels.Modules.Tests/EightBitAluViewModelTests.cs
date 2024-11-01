using System.Collections.ObjectModel;
using DigitalElectronics.Concepts;
using DigitalElectronics.Modules.ALUs;
using DigitalElectronics.Utilities;
using FluentAssertions;
using NUnit.Framework;
using NSubstitute;

namespace DigitalElectronics.ViewModels.Modules.Tests;

public class EightBitAluViewModelTests
{
    private static readonly BitConverter BitConverter = new ();
    private static readonly BitArrayComparer BitArrayComparer = new();
    private static readonly ObservableCollection<bool> BoolCollectionFor0 = new (BitConverter.GetBits((byte)0));

    private static BitArray CreateExpectedBitArrayArg(BitArray expectedValue)
    {
        return Arg.Is<BitArray>(arg => BitArrayComparer.Compare(arg, expectedValue) == 0);
    }

    private static IArithmeticLogicUnit CreateAluMock()
    {
        var aluMock = Substitute.For<IArithmeticLogicUnit>();
        aluMock.ProbeState().Returns(new BitArray(length: 8));
        return aluMock;
    }

    [Test]
    public void InitialState()
    {
        var objUT = new EightBitAluViewModel();
        objUT.Should().NotBeNull();
        objUT.Enable.Should().Be(false);
        objUT.Subtract.Should().Be(false);
        objUT.Probe.Should().BeEquivalentTo(BoolCollectionFor0);
    }

    [Test]
    public void Enable_ShouldCallSetInputEOOnAlu_WhenSet()
    {
        var aluMock = CreateAluMock();
        var objUT = new EightBitAluViewModel(aluMock);

        objUT.Enable = true;
        aluMock.Received(1).SetInputEO(true);

        objUT.Enable = false;
        aluMock.Received(1).SetInputEO(false);
    }


    [Test]
    public void OutputE_ShouldBeNull_WhenEnableIsFalse()
    {
        var aluMock = CreateAluMock();
        var objUT = new EightBitAluViewModel(aluMock);

        objUT.Enable.Should().Be(false);
        objUT.OutputE.Should().BeNull();
    }

    [Test]
    public void OutputE_ShouldBeProbe_WhenEnableIsTrue()
    {
        var aluMock = CreateAluMock();
        var objUT = new EightBitAluViewModel(aluMock);

        objUT.Enable = true;
        objUT.OutputE.Should().BeEquivalentTo(objUT.Probe);
    }

    [Test]
    public void Probe_ShouldReturnValueEquivalentToProbeStateOnAlu()
    {
        var binary42 = new BitArray((byte)42);
        var aluMock = CreateAluMock();
        aluMock.ProbeState().Returns(binary42);
        var objUT = new EightBitAluViewModel(aluMock);
        objUT.SetInputA(binary42);
        objUT.Probe.Should().BeEquivalentTo(binary42.AsReadOnlyList<bool>());

        var binary43 = new BitArray((byte)43);
        aluMock.ProbeState().Returns(binary43);
        objUT.SetInputA(binary43);
        objUT.Probe.Should().BeEquivalentTo(binary43.AsReadOnlyList<bool>());
    }

    [Test]
    public void Subtract_ShouldCallSetInputSuOnAlu_WhenSet()
    {
        var aluMock = CreateAluMock();
        var objUT = new EightBitAluViewModel(aluMock);

        objUT.Subtract = true;
        aluMock.Received(1).SetInputSu(true);

        objUT.Subtract = false;
        aluMock.Received(1).SetInputSu(false);
    }

    [Test]
    public void SetInputA_ShouldCallSetInputAOnAlu_WhenCalled()
    {
        var aluMock = CreateAluMock();
        var objUT = new EightBitAluViewModel(aluMock);
        var bitArray = new BitArray(length: 8);
        objUT.SetInputA(bitArray);
        var expectedArg = CreateExpectedBitArrayArg(bitArray);
        aluMock.Received(1).SetInputA(expectedArg);
    }

    [Test]
    public void SetInputB_ShouldCallSetInputBOnAlu_WhenCalled()
    {
        var aluMock = CreateAluMock();
        var objUT = new EightBitAluViewModel(aluMock);
        var bitArray = new BitArray(length: 8);
        objUT.SetInputB(bitArray);
        var expectedArg = CreateExpectedBitArrayArg(bitArray);
        aluMock.Received(1).SetInputB(expectedArg);
    }

    [Test]
    public void PropertyChanged_ShouldBeRaisedForEnable_WhenEnableIsChanged()
    {
        bool raised = false;
        var aluMock = CreateAluMock();
        var objUT = new EightBitAluViewModel(aluMock);
        objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.Enable);
        objUT.Enable = true;
        raised.Should().Be(true);
    }

    [Test]
    public void PropertyChanged_ShouldBeRaisedForSubtract_WhenSubtractIsChanged()
    {
        bool raised = false;
        var aluMock = CreateAluMock();
        var objUT = new EightBitAluViewModel(aluMock);
        objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.Subtract);
        objUT.Subtract = true;
        raised.Should().Be(true);
    }

    [Test]
    public void PropertyChanged_ShouldBeRaisedForProbe_WhenSetInputAIsCalled()
    {
        bool raised = false;
        var aluMock = CreateAluMock();
        var objUT = new EightBitAluViewModel(aluMock);
        objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.Probe);
        objUT.SetInputA(new BitArray(length: 8));
        raised.Should().Be(true);
    }

    [Test]
    public void PropertyChanged_ShouldBeRaisedForProbe_WhenSetInputBIsCalled()
    {
        bool raised = false;
        var aluMock = CreateAluMock();
        var objUT = new EightBitAluViewModel(aluMock);
        objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.Probe);
        objUT.SetInputB(new BitArray(length: 8));
        raised.Should().Be(true);
    }

    [Test]
    public void PropertyChanged_ShouldBeRaisedForOutputE_WhenSetInputAIsCalledAndEnableIsTrue()
    {
        bool raised = false;
        var aluMock = CreateAluMock();
        var objUT = new EightBitAluViewModel(aluMock);
        objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.OutputE);
        objUT.Enable = true;
        objUT.SetInputA(new BitArray(length: 8));
        raised.Should().Be(true);
    }

    [Test]
    public void PropertyChanged_ShouldBeNotRaisedForOutputE_WhenSetInputAIsCalledAndEnableIsFalse()
    {
        bool raised = false;
        var aluMock = CreateAluMock();
        var objUT = new EightBitAluViewModel(aluMock);
        objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.OutputE);
        objUT.Enable = false;
        objUT.SetInputA(new BitArray(length: 8));
        raised.Should().Be(false);
    }

    [Test]
    public void PropertyChanged_ShouldBeRaisedForOutputE_WhenSetInputBIsCalledAndEnableIsTrue()
    {
        bool raised = false;
        var aluMock = CreateAluMock();
        var objUT = new EightBitAluViewModel(aluMock);
        objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.OutputE);
        objUT.Enable = true;
        objUT.SetInputB(new BitArray(length: 8));
        raised.Should().Be(true);
    }

    [Test]
    public void PropertyChanged_ShouldBeNotRaisedForOutputE_WhenSetInputBIsCalledAndEnableIsFalse()
    {
        bool raised = false;
        var aluMock = CreateAluMock();
        var objUT = new EightBitAluViewModel(aluMock);
        objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.OutputE);
        objUT.Enable = false;
        objUT.SetInputB(new BitArray(length: 8));
        raised.Should().Be(false);
    }

    [Test]
    public void PropertyChanged_ShouldBeRaisedForOutputE_WhenEnableIsChanged()
    {
        bool raised = false;
        var aluMock = CreateAluMock();
        var objUT = new EightBitAluViewModel(aluMock);
        objUT.PropertyChanged += (s, e) => raised |= e.PropertyName == nameof(objUT.OutputE);

        objUT.Enable = true;
        raised.Should().Be(true);

        raised = false;
        objUT.Enable = false;
        raised.Should().Be(true);
    }

    [Test]
    public void EnableChanged_ShouldBeRaised_WhenEnablePropertyIsChanged()
    {
        bool raised = false;
        var aluMock = CreateAluMock();
        var objUT = new EightBitAluViewModel(aluMock);
        objUT.EnableChanged += (s, e) => raised = true;

        objUT.Enable = true;
        raised.Should().Be(true);

        raised = false;
        objUT.Enable = false;
        raised.Should().Be(true);
    }
}
