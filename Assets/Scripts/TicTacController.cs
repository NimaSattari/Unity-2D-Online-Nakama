using UnityEngine;

public class TicTacController : MonoBehaviour
{
    [SerializeField] TicTacView view;
    TicTacModel model;
    [SerializeField] TicTacNakamaConnection connection;

    private void Awake()
    {
        model = new TicTacModel();
    }

    public void SetStartingSide(string startingSide)
    {
        model.SetStartingSide(startingSide);
        view.SetStartingSide(startingSide);
    }

    public void SetGameControllerReferenceOnButtons()
    {
        for (int i = 0; i < view.buttonList.Length; i++)
        {
            view.buttonList[i].SetGameControllerReference(this);
        }
    }
    public string GetPlayerSide()
    {
        return model.GetPlayerSide();
    }
    public void EndTurn(int buttonNumber)
    {
        string situationAfterTurn = model.EndTurn(buttonNumber);
        if (situationAfterTurn == "X" || situationAfterTurn == "O")
        {
            view.GameOver(situationAfterTurn);
        }
        else if(situationAfterTurn == "draw")
        {
            view.GameOver("draw");
        }
        else
        {
            model.ChangeSides();
            view.ChangeSides(model.GetPlayerSide());
        }
    }

    public void RestartGame()
    {
        model.RestartGame();
        view.RestartGame();
        SetStartingSide("X");
        connection.Ping(10);
    }
    public void NakamaRestartGame()
    {
        model.RestartGame();
        view.RestartGame();
        SetStartingSide("X");
    }
}
