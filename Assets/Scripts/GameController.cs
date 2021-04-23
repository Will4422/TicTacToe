using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{

    public int whoTurn; // 0 = X, 1 = O
    public int turnCount; // counts the number of turns played
    public GameObject [] turnIcons; // displays whos turn it is
    public Sprite[] playerIcons; // 0 = X Icon, 1 = O icon
    public Button[] tictactoeSpaces; // playable spaces
    public int[] markedSpaces; // ids which space was marked by which player
    public Text WinnerText; // holds the text component of the winner text;
    public GameObject[] WinningLines; // holds all the different lines for showing that there is a winner
    public GameObject WinnerPanel;
    public int xPlayersScore;
    public int oPlayersScore;
    public Text xPlayersScoreText;
    public Text oPlayersScoreText;
    public Button xPlayersButton;
    public Button oPlayersButton;
    public GameObject catImage;
    public AudioSource buttonClickAudio;

    // Start is called before the first frame update
    void Start()
    {
        GameSetUp();
    }

    // initializes variables 
    void GameSetUp() {
        whoTurn = 0;
        turnCount = 0;
        turnIcons[0].SetActive(true);
        turnIcons[1].SetActive(false);
        for (int i = 0; i < tictactoeSpaces.Length; i++) {
            tictactoeSpaces[i].interactable = true;
            tictactoeSpaces[i].GetComponent<Image>().sprite = null;
        }
        for (int i = 0; i < markedSpaces.Length; i++) {
            markedSpaces[i] = -100;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Function called everytime a player makes a move to place an X or an O
    public void TicTacToeButton(int whichNumber) {
        xPlayersButton.interactable = false;
        oPlayersButton.interactable = false;
        tictactoeSpaces[whichNumber].image.sprite = playerIcons[whoTurn];
        tictactoeSpaces[whichNumber].interactable = false;

        markedSpaces[whichNumber] = whoTurn+1;
        turnCount++;
        if(turnCount > 4) {
            bool isWinner = WinnerCheck();
            if (turnCount == 9 && isWinner == false){
                Cat();
            }
        }

        if (whoTurn == 0) {
            whoTurn = 1;
            turnIcons[0].SetActive(false);
            turnIcons[1].SetActive(true);
            AIMakeMove();
        } else {
            whoTurn = 0;
            turnIcons[0].SetActive(true);
            turnIcons[1].SetActive(false);
        }
    }

    // returns true if a player has won the game, false otherwise
    bool WinnerCheck() {
        int s1 = markedSpaces[0] + markedSpaces[1] + markedSpaces[2];
        int s2 = markedSpaces[3] + markedSpaces[4] + markedSpaces[5];
        int s3 = markedSpaces[6] + markedSpaces[7] + markedSpaces[8];
        int s4 = markedSpaces[0] + markedSpaces[3] + markedSpaces[6];
        int s5 = markedSpaces[1] + markedSpaces[4] + markedSpaces[7];
        int s6 = markedSpaces[2] + markedSpaces[5] + markedSpaces[8];
        int s7 = markedSpaces[0] + markedSpaces[4] + markedSpaces[8];
        int s8 = markedSpaces[2] + markedSpaces[4] + markedSpaces[6];

        var solutions = new int[] { s1, s2, s3, s4, s5, s6, s7, s8 };

        for (int i = 0; i < solutions.Length; i++) {
            if(solutions[i] == 3*(whoTurn+1)) {
                WinnerDisplay(i);
                return true;
            }
        }
        return false;
    }

    // handles displaying the visuals of a win condition
    void WinnerDisplay(int indexIn) {
        WinnerPanel.gameObject.SetActive(true);
        if(whoTurn == 0) {
            xPlayersScore++;
            xPlayersScoreText.text = xPlayersScore.ToString();
            WinnerText.text = "Player X Wins!";
        } else if (whoTurn == 1) {
            oPlayersScore++;
            oPlayersScoreText.text = oPlayersScore.ToString();
            WinnerText.text = "Player O Wins!";
        }
        WinningLines[indexIn].SetActive(true);
    }

    // resets everything except the scores
    public void Rematch() {
        GameSetUp();
        for (int i = 0; i < WinningLines.Length; i++) {
            WinningLines[i].SetActive(false);
        }
        WinnerPanel.SetActive(false);
        xPlayersButton.interactable = true;
        oPlayersButton.interactable = true;
        catImage.SetActive(false);
    }

    // reset everything - including the scores
    public void Restart() {
        Rematch();
        xPlayersScore = 0;
        oPlayersScore = 0;
        xPlayersScoreText.text = "0";
        oPlayersScoreText.text = "0";
    }

    // clicking on the player icon allows players to switch who goes first
    public void SwitchPlayer(int whichPlayer) {
        // taking this out for now so that i can safely implement the AI

        // if (whichPlayer == 0) {
        //     whoTurn = 0;
        //     turnIcons[0].SetActive(true);
        //     turnIcons[1].SetActive(false);
        // } else if (whichPlayer == 1) {
        //     whoTurn = 1;
        //     turnIcons[0].SetActive(false);
        //     turnIcons[1].SetActive(true);
        // }
    }

    // handles a tie
    void Cat() {
        WinnerPanel.SetActive(true);
        catImage.SetActive(true);
        WinnerText.text = "CAT";
    }

    // plays click sound
    public void PlayButtonClick() {
        buttonClickAudio.Play();
    }

    // calculate next move and call TicTacToeButton(int) with the chose space to move
    void AIMakeMove() {
        int num = GenerateRandomBetween0and9();
        if (!isMarked(num)) {
            TicTacToeButton(num);
        } else {
            AIMakeMove();
        }
    }

    int GenerateRandomBetween0and9() {
        return Random.Range(0,8);
    }

    bool isMarked(int num) {
        return markedSpaces[num] != -100;
    }
}
