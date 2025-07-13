using InvoiceApp.Core.Enums;
using InvoiceApp.MAUI.Services;
using Microsoft.Maui.Input;

namespace Microsoft.Maui.Input;

public enum Keys { Enter, A, B }

public class KeyEventArgs(Keys key, bool isRepeat) : EventArgs
{
    public Keys Key { get; } = key;
    public bool IsRepeat { get; } = isRepeat;
}

using Xunit;

namespace Wrecept.Tests;

public class KeyboardManagerTests
{
    private static KeyEventArgs CreateArgs(Keys key)
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

        var result = manager.Process(CreateArgs(Keys.Enter));

        Assert.True(result);
        Assert.True(handler.Called);
    }

    [StaFact]
    public void Process_ReturnsFalse_WhenNoHandler()
    {
        var state = new AppStateService("x") { InteractionState = AppInteractionState.None };
        var manager = new KeyboardManager(state);

        var result = manager.Process(CreateArgs(Keys.Enter));

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

        var result1 = manager.Process(CreateArgs(Keys.A));
        state.InteractionState = AppInteractionState.EditingInvoice;
        var result2 = manager.Process(CreateArgs(Keys.B));

        Assert.True(result1);
        Assert.True(trueHandler.Called);
        Assert.True(falseHandler.Called);
        Assert.False(result2);
    }
}
