using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // WICHTIG FÜR SZENENWECHSEL

public class UI : MonoBehaviour
{
    public Text hpText;
    public Text welleText;
    public Text moneyText;
    public Text erzeText;
    public Text playerHpText;
    private Datenspeicherung dataStorage;

    public GameObject SkillTree;
    public GameObject normalesCanvas;

    public GameObject gamePanel;
    public GameObject pausePanel;

    public GameObject acceptMessage;
    // Start is called before the first frame update
    void Start()
    {
        dataStorage = GameObject.Find("Player").GetComponent<Datenspeicherung>();

        //hpText.text = "HP: " + dataStorage.hp.ToString();
        //welleText.text = "Welle: " + dataStorage.welle.ToString();
        //playerHpText.text = "Spieler HP: " + ((int)dataStorage.playerHp).ToString();
        //moneyText.text = "Money: " + ((int)dataStorage.money).ToString();
        //erzeText.text = "Erze: " + ((int)dataStorage.gen).ToString();

        hpText.text = "Haus\n" + dataStorage.hp.ToString() + " LP";
        welleText.text = "Welle:\n" + dataStorage.welle.ToString();
        playerHpText.text = "Spieler\n" + ((int)dataStorage.playerHp).ToString()+" LP";
        moneyText.text = ((int)dataStorage.money).ToString();
        erzeText.text = ((int)dataStorage.gen).ToString();
    }

    // Update is called once per frame
    void Update()
    {

        //hpText.text = "HP: " + dataStorage.hp.ToString();
        //welleText.text = "Welle:\n" + dataStorage.welle.ToString();
        //playerHpText.text = "Spieler HP: " + ((int)dataStorage.playerHp).ToString();
        //moneyText.text = "Money: " + ((int)dataStorage.money).ToString();
        //erzeText.text = "Erze: " + ((int)dataStorage.gen).ToString();

        hpText.text = "Haus\n" + dataStorage.hp.ToString() + " LP";
        welleText.text = "Welle:\n" + dataStorage.welle.ToString();
        if (dataStorage.playerHp < 0)
        {
            dataStorage.playerHp = 0;
        }
        playerHpText.text = "Spieler\n" + ((int)dataStorage.playerHp).ToString() + " LP";
        moneyText.text = ((int)dataStorage.money).ToString();
        erzeText.text = ((int)dataStorage.gen).ToString();

        //Pause
        if (dataStorage.pauseButton == true)
        {
            gamePanel.SetActive(false);
            pausePanel.SetActive(true);
        } else
        {
            gamePanel.SetActive(true);
            pausePanel.SetActive(false);
        }
    }

    public void BackToMainMenu()
    {
        acceptMessage.SetActive(true);
    }

    public void BackToMainMenuAccept()
    {
        SceneManager.LoadScene(0);
    }

    public void BackToMainMenuDelete()
    {
        acceptMessage.SetActive(false);
    }
}
