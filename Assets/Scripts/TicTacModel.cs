public class TicTacModel
{
    public string playerSide;
    public int moveCount;
    public string[] placeList;

    public string SetStartingSide(string startingSide)
    {
        placeList = new string[9];
        playerSide = startingSide;
        if (playerSide == "X")
        {
            return "X";
        }
        else
        {
            return "O";
        }
    }
    public string GetPlayerSide()
    {
        return playerSide;
    }
    public string EndTurn(int placeNumber)
    {
        moveCount++;
        placeList[placeNumber] = playerSide;
        if (placeList[0] == playerSide && placeList[1] == playerSide && placeList[2] == playerSide)
        {
            return playerSide;
        }
        else if (placeList[3] == playerSide && placeList[4] == playerSide && placeList[5] == playerSide)
        {
            return playerSide;
        }
        else if (placeList[6] == playerSide && placeList[7] == playerSide && placeList[8] == playerSide)
        {
            return playerSide;
        }
        else if (placeList[0] == playerSide && placeList[3] == playerSide && placeList[6] == playerSide)
        {
            return playerSide;
        }
        else if (placeList[1] == playerSide && placeList[4] == playerSide && placeList[7] == playerSide)
        {
            return playerSide;
        }
        else if (placeList[2] == playerSide && placeList[5] == playerSide && placeList[8] == playerSide)
        {
            return playerSide;
        }
        else if (placeList[0] == playerSide && placeList[4] == playerSide && placeList[8] == playerSide)
        {
            return playerSide;
        }
        else if (placeList[2] == playerSide && placeList[4] == playerSide && placeList[6] == playerSide)
        {
            return playerSide;
        }
        else if (moveCount >= 9)
        {
            return "draw";
        }
        else
        {
            return "change";
        }
    }

    public string ChangeSides()
    {
        if (playerSide == "X")
        {
            playerSide = "O";
            return playerSide;
        }
        else
        {
            playerSide = "X";
            return playerSide;
        }
    }
    public void RestartGame()
    {
        moveCount = 0;

        for (int i = 0; i < placeList.Length; i++)
        {
            placeList[i] = "";
        }
    }
}
