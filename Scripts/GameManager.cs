using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{

    private bool gameHasEnded = false;
    public GameObject playerDead;
    public GameObject playerWon;

    private void Start()
    {
        playerDead.SetActive(false);
        playerWon.SetActive(false);
    }

    //When Player is Dead
    public void EndGame(int type)
    {
        if (!gameHasEnded)
        {
            gameHasEnded = true;
            if (type == 0)
            {
                PlayerWon();
            }
            else if (type == 1)
            {
                PlayerDead();
            }
        }
    }
    //Shows PlayerDead Screen
    private void PlayerDead()
    {
        playerDead.SetActive(true);
    }
    //Shows PlayerWon Screen
    private void PlayerWon()
    {
        playerWon.SetActive(true);
    }
    //Reloads Level/Scene
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
