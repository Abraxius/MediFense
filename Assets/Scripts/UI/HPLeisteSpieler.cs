using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPLeisteSpieler : MonoBehaviour
{
    private Image healthBar;
    private Datenspeicherung dataStorage;

    // Start is called before the first frame update
    void Start()
    {
        healthBar = transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>();
        dataStorage = GameObject.Find("Player").GetComponent<Datenspeicherung>();
    }

    // Update is called once per frame
    void Update()
    {
        HPAnzeige();
        CanvasRotate();
    }

    //Sorgt dafür dass die Schrift immer in Richtung Spieler zeigt
    private void CanvasRotate()
    {
        var temp = Camera.main.transform.rotation;
        this.transform.rotation = temp;
    }

    //Aktualisiert die HP Leiste
    private void HPAnzeige()
    {
        healthBar.fillAmount = (float)dataStorage.playerHp / (float)dataStorage.playerHpMax;
    }
}
