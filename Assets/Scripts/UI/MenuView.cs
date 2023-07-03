using TMPro;

public class MenuView : View<MenuView>
{
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Result;
    public TextMeshProUGUI Hint;

    public void OnPlayButton()
    {
        if (GameManager.CurrentPhase == GamePhase.MainMenu)
            GameManager.ChangePhase(GamePhase.Game);

        if (GameManager.CurrentPhase == GamePhase.End)
            GameManager.Instance.RestartGame();
    }

    protected override void OnGamePhaseChanged(GamePhase gamePhase)
    {
        base.OnGamePhaseChanged(gamePhase);

        switch (gamePhase)
        {
            case GamePhase.MainMenu:
                SetStartViewText();
                Transition(true);
                break;

            case GamePhase.Game:
                FadeOutView();
                break;

            case GamePhase.End:
                SetEndViewText();
                Transition(true);
                break;
        }
    }

    void SetStartViewText()
    {
        Title.text = "I Made\nA Game\nWith Bees\nIn It";
        Result.text = "";
        Hint.text = "Tap Anywhere To Start";
    }

    void SetEndViewText()
    {
        Title.text = "Game Over\nHoney!";
        
        Result.text = "You Squashed \n" +
                      GameManager.Instance.GetPlayerScore() +
                      "\nBees";
        
        Hint.text = "Tap Anywhere To Restart";
    }

    private void FadeOutView()
    {
        if (Visible)
            Transition(false);
    }
}