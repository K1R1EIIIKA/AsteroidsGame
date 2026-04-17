using Core.Interfaces;
using UI.Views;

namespace UI
{
    public class UIManager : IUIManager
    {
        private readonly MainMenuView _mainMenu;
        private readonly HudView _hud;
        private readonly PauseMenuView _pauseMenu;
        private readonly GameOverView _gameOver;

        public UIManager(
            MainMenuView mainMenu,
            HudView hud,
            PauseMenuView pauseMenu,
            GameOverView gameOver)
        {
            _mainMenu = mainMenu;
            _hud = hud;
            _pauseMenu = pauseMenu;
            _gameOver = gameOver;
        }

        public void ShowMainMenu() => _mainMenu.Show();
        public void HideMainMenu() => _mainMenu.Hide();

        public void ShowHud() => _hud.Show();
        public void HideHud() => _hud.Hide();

        public void ShowPauseMenu() => _pauseMenu.Show();
        public void HidePauseMenu() => _pauseMenu.Hide();

        public void ShowGameOver() => _gameOver.Show();
        public void HideGameOver() => _gameOver.Hide();
    }
}
