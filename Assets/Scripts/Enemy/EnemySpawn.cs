using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject Enemy;
    public GameObject Enemy2;
    public GameObject EnemyBoss;
    public GameObject EnemyBoss2;
    private Vector3 standort;
    public int spawnAnzahl;
    public int enemyAnzahl;
    public float countdown;
    private float spawnZeit;
    public float golemSpawnZeit;
    public float skelettSpawnZeit;

    //Wellensystem
    public float wellenCountdown;
    private float wellenCountdownP;
    public int proWelleSteigerung;
    private bool wellenStart;
    public List<GameObject> EnemyListe = new List<GameObject>();

    private Datenspeicherung dataStorage;

    //Spawn soll hinten Spawnen
    private bool bossSpawn = true;
    // Start is called before the first frame update
    void Start()
    {
        dataStorage = GameObject.Find("Player").GetComponent<Datenspeicherung>();
        standort = this.transform.position;
        spawnZeit = skelettSpawnZeit;
        countdown = spawnZeit;
        wellenStart = true;
        wellenCountdownP = wellenCountdown;
    }

    private void FixedUpdate()
    {
        if (dataStorage.pauseButton == false)
        {
            //Lässt die Enemys nacheinander Spawnen und nicht alle aufeinmal
            if (wellenStart == true)    //eine welle
            {
                countdown -= Time.deltaTime;
                if (enemyAnzahl < spawnAnzahl)  //ist die gespawnte Anzahl noch kleiner als die Soll Anzahl?
                {
                    if (countdown < 0.0f)
                    {
                        //Einheiten Spawn der Wellen
                        if (dataStorage.welle < 3)  //Bis Welle 2 -> SKelette
                        {
                            Debug.Log("Szenario 1");
                            EnemyListe.Add(Instantiate(Enemy, standort, Quaternion.identity));
                            countdown = spawnZeit;
                            enemyAnzahl += 1;
                        }
                        else if (dataStorage.welle == 3)    //Genau Welle 3 -> Golems
                        {
                            Debug.Log("Szenario 2");
                            spawnZeit = golemSpawnZeit;
                            EnemyListe.Add(Instantiate(Enemy2, standort, Quaternion.identity));
                            countdown = spawnZeit;
                            enemyAnzahl += 1;
                        }
                        else if (dataStorage.welle > 3 && dataStorage.welle % 3 == 0 || dataStorage.welle % 5 == 0)   //Über Welle 5 und alle 3 Runden -> Skelette und Golems
                        {                              
                            Debug.Log("Szenario 3");
                            if (enemyAnzahl < (int)(spawnAnzahl * 0.8f))
                            {
                                spawnZeit = skelettSpawnZeit;
                                EnemyListe.Add(Instantiate(Enemy, standort, Quaternion.identity));
                                countdown = spawnZeit;
                                enemyAnzahl += 1;
                            } else
                            {
                                if (dataStorage.welle % 10 == 0 && bossSpawn == true)    //Abfrage ob ein Boss gespawnt werden soll -> nach 10 Runden
                                {
                                    EnemyListe.Add(Instantiate(EnemyBoss, standort, Quaternion.identity));
                                    countdown = spawnZeit;
                                    bossSpawn = false;
                                } else
                                {
                                    spawnZeit = golemSpawnZeit;
                                    EnemyListe.Add(Instantiate(Enemy2, standort, Quaternion.identity));
                                    countdown = spawnZeit;
                                    enemyAnzahl += 1;
                                }
                            }
                        }
                        else if (dataStorage.welle > 5 && dataStorage.welle % 4 == 0)   //Über Welle 5 und alle 4 Runden -> nur Golems
                        {
                            Debug.Log("Szenario 4");
                            spawnZeit = golemSpawnZeit;
                            EnemyListe.Add(Instantiate(Enemy2, standort, Quaternion.identity));
                            countdown = spawnZeit;
                            enemyAnzahl += 1;
                        }
                        else if (dataStorage.welle > 3) //Nur Skelette
                        {
                            Debug.Log("Szenario 5");
                            spawnZeit = skelettSpawnZeit;
                            EnemyListe.Add(Instantiate(Enemy, standort, Quaternion.identity));
                            countdown = spawnZeit;
                            enemyAnzahl += 1;
                        }

                    }
                }
                else
                {
                    bossSpawn = true;
                    wellenStart = false;
                    enemyAnzahl = 0;
                }
            }
            else //countdown bis die nächste welle beginnt
            {
                if (EnemyListe.Count < 1)
                {
                    wellenCountdown -= Time.deltaTime;
                    if (wellenCountdown < 0.0f)
                    {
                        dataStorage.welle += 1;

                        //Hier wird der Schwierigkeitsgrad der Enemys pro Welleneinheit erhöt, siehe Datenspeicherung
                        if (dataStorage.welle % 5 == 0)
                        {
                            dataStorage.enemy1Hp += dataStorage.schwierigkeitsgrad;

                            //Enemy 2 Werte
                            dataStorage.enemy2Hp += dataStorage.schwierigkeitsgrad;
                        }
                        if (dataStorage.welle % 10 == 0)    //Boss Werte nur alle 10 Runden erhöhe
                        {
                            //Boss Werte
                            dataStorage.enemyBoss1Hp += dataStorage.schwierigkeitsgrad * 10;
                            dataStorage.enemyBossDamage += dataStorage.schwierigkeitsgrad * 2;
                            dataStorage.enemyBoss2Hp += dataStorage.schwierigkeitsgrad * 10;
                            //Enemy Werte
                            dataStorage.enemyDamage1 += dataStorage.schwierigkeitsgrad;
                            dataStorage.enemyDamage2 += dataStorage.schwierigkeitsgrad;
                        }

                        //Hier wird die Steigerung der Wellen festgelegt! Wieviele, und wieviele maximal!
                        if (spawnAnzahl < 50)   //max 50 stk
                            spawnAnzahl = spawnAnzahl + (dataStorage.welle * proWelleSteigerung);
                        wellenStart = true;
                        wellenCountdown = wellenCountdownP;
                    }
                }              
            }
        }
    }
}
