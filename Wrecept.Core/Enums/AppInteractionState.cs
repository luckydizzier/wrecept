namespace Wrecept.Core.Enums;

/// <summary>
/// Finomabb alkalmazásállapotok a felhasználói interakciók követéséhez.
/// </summary>
public enum AppInteractionState
{
    /// <summary>Nincs aktív művelet.</summary>
    None,
    /// <summary>Indítási folyamat zajlik.</summary>
    Startup,
    /// <summary>A főmenü aktív.</summary>
    MainMenu,
    /// <summary>Számlák böngészése.</summary>
    BrowsingInvoices,
    /// <summary>Számlaszerkesztés folyamatban.</summary>
    EditingInvoice,
    /// <summary>Törzsadatok szerkesztése.</summary>
    EditingMasterData,
    /// <summary>Inline entitás létrehozó megnyitva.</summary>
    InlineCreatorActive,
    /// <summary>Inline megerősítő prompt látható.</summary>
    InlinePromptActive,
    /// <summary>Modális dialógus nyitva.</summary>
    DialogOpen,
    /// <summary>A program kilép.</summary>
    Exiting
}
