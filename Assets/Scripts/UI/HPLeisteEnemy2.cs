using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPLeisteEnemy2 : MonoBehaviour
{
    private Image healthBar;
    private Datenspeicherung dataStorage;
    private Enemy enemyScript;

    // Start is called before the first frame update
    void Start()
    {
        enemyScript = transform.parent.parent.gameObject.GetComponent<Enemy>();
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
        healthBar.fillAmount = (float)enemyScript.enemyHp / (float)enemyScript.startetEnemyHP;
    }
}
