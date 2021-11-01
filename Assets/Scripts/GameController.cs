using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable] public class Player
{
    public Image panel;
    public Text text;
    public Button button;
}

[System.Serializable] public class PlayerColor
{
    public Color panelColor;
    public Color textColor;
} 

public class GameController : MonoBehaviour
{
    public GameObject startInfo;
    public Player PlayerX;
    public Player PlayerO;
    public PlayerColor activePlayerColor;
    public PlayerColor inactivePlayerColor;
    public Text[] buttonList;
    private string playerSide;
    public GameObject gameOverPanel;
    public Text gameOverText;
    public GameObject restartBtn;
    private int moveCount = 0;

    void Awake()
    {
        gameOverPanel.SetActive(false);
        restartBtn.SetActive(false);
        SetGameControllerReferenceOnButtons();
    }

    void SetPlayerButtons(bool toggle)
    {
        PlayerX.button.interactable = toggle;
        PlayerO.button.interactable = toggle;
    }

    void SetGameControllerReferenceOnButtons()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }

    public string GetPlayerSide()
    {
        return playerSide;
    }

    public void SetStartingSide(string startingSide)
    {
        playerSide = startingSide;
        if (playerSide == "X")
        {
            SetPlayerColors(PlayerX, PlayerO);
        }
        else
        {
            SetPlayerColors(PlayerO, PlayerX);
        }

        StartGame();
    }

    void StartGame()
    {
        startInfo.SetActive(false);
        SetBoardInteractable(true);
        SetPlayerButtons(false);
    }

    public bool HorizontalWinCondition()
    {
        if (!string.IsNullOrWhiteSpace(buttonList[0].text) 
            && buttonList[0].text == buttonList[1].text 
            && buttonList[0].text == buttonList[2].text) return true;

        if (!string.IsNullOrWhiteSpace(buttonList[3].text) 
            && buttonList[3].text == buttonList[4].text 
            && buttonList[3].text == buttonList[5].text) return true;

        if (!string.IsNullOrWhiteSpace(buttonList[6].text) 
            && buttonList[6].text == buttonList[7].text 
            && buttonList[6].text == buttonList[8].text) return true;

        return false;
    }

    public bool VerticalWinCondition()
    {
        if (!string.IsNullOrWhiteSpace(buttonList[0].text) 
            && buttonList[0].text == buttonList[3].text 
            && buttonList[0].text == buttonList[6].text) return true;

        if (!string.IsNullOrWhiteSpace(buttonList[1].text) 
            && buttonList[1].text == buttonList[4].text 
            && buttonList[1].text == buttonList[7].text) return true;

        if (!string.IsNullOrWhiteSpace(buttonList[2].text) 
            && buttonList[2].text == buttonList[5].text 
            && buttonList[2].text == buttonList[8].text) return true;

        return false;
    }

    public bool DiagonalWinCondition()
    {
        if (!string.IsNullOrWhiteSpace(buttonList[0].text) 
            && buttonList[0].text == buttonList[4].text 
            && buttonList[0].text == buttonList[8].text) return true;

        if (!string.IsNullOrWhiteSpace(buttonList[2].text) 
            && buttonList[2].text == buttonList[4].text 
            && buttonList[2].text == buttonList[6].text) return true;

        return false;
    }

    public void EndTurn()
    {
        moveCount++;

        if (HorizontalWinCondition() || VerticalWinCondition() || DiagonalWinCondition())
        {
            GameOver();
            return;
        }

        if(moveCount >= 9)
        {
            SetGameOverText("It is a Draw!");
            SetPlayerColorsInactive();
        } else
        {
            ChangeSides();
        }
    }

    void ChangeSides()
    {
        playerSide = (playerSide == "X") ? "O" : "X";

        if (playerSide == "X") SetPlayerColors(PlayerX, PlayerO);

        if (playerSide == "O") SetPlayerColors(PlayerO, PlayerX);
    }

    void GameOver()
    {
        SetBoardInteractable(false);
        SetGameOverText(playerSide + " Wins!");
        restartBtn.SetActive(true);
    }

    void SetGameOverText(string value)
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = value;
    }

    public void RestartGame()
    {
        moveCount = 0;
        startInfo.SetActive(true);
        gameOverPanel.SetActive(false);
        restartBtn.SetActive(false);
        ClearTiles();
        SetPlayerButtons(true);
        SetPlayerColorsInactive();
    }

    void ClearTiles()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].text = "";
        }
    }
    void SetBoardInteractable(bool toggle)
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = toggle;
        }
    }

    void SetPlayerColorsInactive() { 
        PlayerX.panel.color = inactivePlayerColor.panelColor; 
        PlayerX.text.color = inactivePlayerColor.textColor; 
        PlayerO.panel.color = inactivePlayerColor.panelColor; 
        PlayerO.text.color = inactivePlayerColor.textColor; 
    }

    void SetPlayerColors(Player newPlayer, Player oldPlayer)
    {
        newPlayer.panel.color = activePlayerColor.panelColor;
        newPlayer.text.color = activePlayerColor.textColor;
        oldPlayer.panel.color = inactivePlayerColor.panelColor;
        oldPlayer.text.color = inactivePlayerColor.textColor;
    }

}