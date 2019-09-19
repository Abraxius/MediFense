using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // WICHTIG FÜR SZENENWECHSEL

public class Menu : MonoBehaviour
{
    //Szene 0 = Menü
    //Szene 1 = Spiel
    //Szene 2 = Game Over
    //Szene 3 = HighScore

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("beendet");
        Application.Quit();
    }

    //Game Over Szene
    public void HighScore()
    {
        SceneManager.LoadScene(3);
    }

    public void Hauptmenü()
    {
        SceneManager.LoadScene(0);
    }
}
