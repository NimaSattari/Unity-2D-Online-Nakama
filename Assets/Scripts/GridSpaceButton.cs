using UnityEngine;
using UnityEngine.UI;

public class GridSpaceButton : MonoBehaviour
{

    public Button button;
    public Text buttonText;
    public int buttonNumber;

    private TicTacController gameController;
    [SerializeField] TicTacNakamaConnection connection;

    public void SetGameControllerReference(TicTacController controller)
    {
        gameController = controller;
    }

    public void SetSpace()
    {
        if(connection.player == gameController.GetPlayerSide())
        {
            buttonText.text = gameController.GetPlayerSide();
            button.interactable = false;
            gameController.EndTurn(buttonNumber);
            connection.Ping(buttonNumber);
        }
    }
    public void NakamaSetSpace()
    {
        buttonText.text = gameController.GetPlayerSide();
        button.interactable = false;
        gameController.EndTurn(buttonNumber);
    }
}