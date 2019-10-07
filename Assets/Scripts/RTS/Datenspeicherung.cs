using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Datenspeicherung : MonoBehaviour
{
    public int hp;  //HaupthausHP

    public string SPIELERWERTE = "-------------------------------------------------";
    //Spielerwerte----------------------------------------------------
    public int playerHp;
    public int playerHpStart;
    public int playerHpMax;
    public int respawnTimePlayer;
    private float countdownRespawn;
    public bool playerDied;
    public int damagePlayer;
    public int startDamagePlayer;
    public float beatTimePlayer;
    public float aoe;   //noch ohne Grund, kommt durch die Upgrades dann zu stande
    public string ENEMY = "-------------------------------------------------";
    //Enemy-----------------------------------------------------------
    //Enemy 1 Werte
    public int enemy1Hp;
    public float enemyBeatTime1;  
    public int enemyDamage1;

    //Boss Werte
    public int enemyBoss1Hp;
    public float enemyBossBeatTime;
    public int enemyBossDamage;
    public int enemyBoss2Hp;
    public int enemyLoot;
    public int enemyBossLoot;

    //Enemy 2 Werte
    public int enemy2Hp;
    public float enemyBeatTime2;
    public int enemyDamage2;

    //Wellen Werte
    public string RATHAUS = "-------------------------------------------------";
    //Rathaus----------------------------------------------------------
    //Rathaus Ausbau Kosten
    //public int ersteAusbaustufe;
    public int zweiteAusbaustufe;
    public int dritteAusbaustufe;

    //Rathaus Bonus Werte
    public float rathausBonusProduktion;
    public string MINE = "-------------------------------------------------";
    //-----------------------------------------------------------------

    //Mine-------------------------------------------------------------
    public float abbauRate = 0.1f;

    public string ALLGEMEINEWERTE = "-------------------------------------------------";
    //Allgemein Werte--------------------------------------------------
    public int welle = 0;       
    public float money = 100f;       //Gold
    public int gen = 0;         //Erze
    public int schwierigkeitsgrad = 1;      //um wieviel sollen die Einheiten stärker werden?
    public int schwierigkeitsgradZahler = 1; //wie oft sollen die Einheiten verstärkt werden? alle 2 Runden, alle 3?
 
    public Vector3 spawnPoint;

    //Skill
    public int skill1Damage;

    public string TURMWERTE = "-------------------------------------------------";
    //Turwerte---------------------------------------------------------
    //Turm 1
    public int towerCosts1;
    public float towerAttackSpeed1;
    public float towerRange1;
    public int towerDamage1;
    public int startTowerDamage1;
    public int tower1Bauzeit;
    //Turm 2
    public int towerCosts2;
    public float towerAttackSpeed2;
    public float towerRange2;
    public int towerDamage2;
    public int startTowerDamage2;
    public int tower2Bauzeit;

    public string UPGRADEKOSTEN = "-------------------------------------------------";
    //Upgradekosten------------------------------------------------------
    public int skillstufe1;
    public int skillstufe2;
    public int skillstufe3;
    // neue 
    public int spielerHpPlusKosten = 1;
    public int spielerDamagePlusKosten = 1;
    public int towerDamagePlusKosten = 1;

    public int spielerDamageX2Kosten = 5;
    public int spielerHpX2Kosten = 5;
    public int towerDamageX2Kosten = 5;

    public int fähigkeitKostenAoE = 2;
    public int fähigkeitKostenHeal = 2;
    public int fähigkeitKostenPort = 2;
    public string SKILLBONUSE = "-------------------------------------------------";
    //Skillbonuse   //alle noch ohne Funktion, nur schonmal als Platzhalter für die kommenden funktionierenden Bonuse
    public float angriffsBonus;
    public float hpBonus;
    // Angriff 2x
    // Turm Angriff 2x
    //AoE
    //Heal
    //Port

    public float turmSchadenBonus;

    public string ANDERES = "-------------------------------------------------";
    //Pause Button
    public bool pauseButton;
    public Text wiederbelebungsCountdown;
    public GameObject spielerTodPanel;
    // Start is called before the first frame update
    void Start()
    {
        countdownRespawn = respawnTimePlayer;

        gen = PlayerPrefs.GetInt("Rubine");    

        welle = 0;
        playerHpMax = playerHpStart + PlayerPrefs.GetInt("spielerHpPlusSave") * PlayerPrefs.GetInt("spielerHpX2");

        playerHp = playerHpMax;

        //pause
        pauseButton = false;
    }

    private void Update()
    {
        //Spieler wiederbelebung
        if (playerDied == true)
        {
            spielerTodPanel.SetActive(true);
            countdownRespawn = countdownRespawn - 1 * Time.deltaTime;
            int tmp = (int)countdownRespawn;
            wiederbelebungsCountdown.text = tmp.ToString();
            if (0 > countdownRespawn)
            {
                spielerTodPanel.SetActive(false);
                playerHp = playerHpMax;
                this.gameObject.transform.GetChild(1).gameObject.SetActive(true);
                countdownRespawn = respawnTimePlayer;
                wiederbelebungsCountdown.text = respawnTimePlayer.ToString();
                playerDied = false;
            }
        }

        PlayerPrefs.SetInt("Rubine", gen);

        //Upgrades
        damagePlayer = startDamagePlayer + PlayerPrefs.GetInt("spielerDamagePlusSave") * PlayerPrefs.GetInt("spielerDamageX2");
        playerHpMax = playerHpStart + PlayerPrefs.GetInt("spielerHpPlusSave") * PlayerPrefs.GetInt("spielerHpX2");
        towerDamage2 = startTowerDamage2 + PlayerPrefs.GetInt("towerDamagePlusSave") * PlayerPrefs.GetInt("towerDamageX2");
        towerDamage1 = startTowerDamage1 + PlayerPrefs.GetInt("towerDamagePlusSave") * PlayerPrefs.GetInt("towerDamageX2");


        //Automatische Geldproduktion
        money = money + rathausBonusProduktion; 

        // Geld geben für Test per m drücken
        if (Input.GetKey(KeyCode.M))
            money += 10;

        if (Input.GetKey(KeyCode.N))
            gen += 1;
    }
}
