using InvoiceApp.Core.Enums;
using InvoiceApp.MAUI.Services;
using Microsoft.Maui.Input;
using Xunit;

namespace InvoiceApp.Tests;

public class KeyboardManagerTests
{
    private static KeyEventArgs CreateArgs(Key key)
        => new(key, false);

    private class TrueHandler : IKeyboardHandler
    {
        public bool Called;
        public bool HandleKey(KeyEventArgs e) { Called = true; return true; }
    }

    private class FalseHandler : IKeyboardHandler
    {
        public bool Called;
        public bool HandleKey(KeyEventArgs e) { Called = true; return false; }
    }

    [StaFact]
    public void Process_ReturnsHandlerResult_ForCurrentState()
    {
        var state = new AppStateService("x") { InteractionState = AppInteractionState.MainMenu };
        var manager = new KeyboardManager(state);
        var handler = new TrueHandler();
        manager.Register(AppInteractionState.MainMenu, handler);

        var result = manager.Process(CreateArgs(Key.Enter));

        Assert.True(result);
        Assert.True(handler.Called);
    }

    [StaFact]
    public void Process_ReturnsFalse_WhenNoHandler()
    {
        var state = new AppStateService("x") { InteractionState = AppInteractionState.None };
        var manager = new KeyboardManager(state);

        var result = manager.Process(CreateArgs(Key.Enter));

        Assert.False(result);
    }

    [StaFact]
    public void Process_SwitchesHandlersWithState()
    {
        var state = new AppStateService("x") { InteractionState = AppInteractionState.MainMenu };
        var manager = new KeyboardManager(state);
        var trueHandler = new TrueHandler();
        var falseHandler = new FalseHandler();
        manager.Register(AppInteractionState.MainMenu, trueHandler);
        manager.Register(AppInteractionState.EditingInvoice, falseHandler);

        var result1 = manager.Process(CreateArgs(Key.A));
        state.InteractionState = AppInteractionState.EditingInvoice;
        var result2 = manager.Process(CreateArgs(Key.B));

        Assert.True(result1);
        Assert.True(trueHandler.Called);
        Assert.True(falseHandler.Called);
        Assert.False(result2);
    }
}
