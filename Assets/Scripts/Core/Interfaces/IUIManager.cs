namespace Core.Interfaces
{
    public interface IUIManager
    {
        void ShowMainMenu();
        void HideMainMenu();
        void ShowHud();
        void HideHud();
        void ShowPauseMenu();
        void HidePauseMenu();
        void ShowGameOver();
        void HideGameOver();
    }
}
