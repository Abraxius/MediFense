using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RTS;
using UnityEngine.SceneManagement; // WICHTIG FÜR SZENENWECHSEL

public class EnemyBoss : MonoBehaviour
{

    public NavMeshAgent agent;
    public GameObject ziel;
    private Vector3 zielVector;
    public Collider col;

    private EnemySpawn EnemySpawn;
    private Datenspeicherung dataStorage;
    private GameObject player;

    Animator animator;
    NavMeshAgent navAgent;
    const float locomotionAnimationSmoothTime = .1f;

    public float beatTime;  //Die Zeit zwischen denen beim Schlagen des Enemys pause ist
    public int damage;       //Der Schaden den der Enemy verursacht

    public float countdown;
    public bool playerCollision;

    public int enemyHp;
    public int startetEnemyHP;

    public bool sterben;
    public int tmpID;
    public bool onTheRoad;
    public BoxCollider colTrigger;
    public Vector3 tmpLocation;

    private bool SoundUsed;

    public GameObject brucke;
    // Start is called before the first frame update
    void Start()
    {
        tmpID = agent.agentTypeID;  //speichert die ID des Agents ab um Sie in der Update zurück zu ändern
        onTheRoad = true;

        //ziel = GameObject.Find("EnemyZiel");
        zielVector = ziel.transform.position;
        dataStorage = GameObject.Find("Player").GetComponent<Datenspeicherung>();
        player = GameObject.Find("Player").gameObject.transform.GetChild(1).gameObject;
        EnemySpawn = GameObject.Find("EnemySpawn").GetComponent<EnemySpawn>();

        animator = GetComponentInChildren<Animator>();

        damage = dataStorage.enemyBossDamage;
        beatTime = dataStorage.enemyBossBeatTime;
        enemyHp = dataStorage.enemyBoss1Hp;         //Das muss noch für mehrere Enemys erweitert werden!!!
        startetEnemyHP = dataStorage.enemyBoss1Hp;

        countdown = beatTime;

        SoundUsed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (dataStorage.pauseButton == false)
        {
            died(); //Sterben Abfrage

            if (dataStorage.playerDied == true) //extra Todes Abfrage des Spielers, damit die Enemys aufhören die Luft zu hauen
            {
                animator.SetBool("attack", false);
            }

            if (sterben == false)
            {
                agent.enabled = true;
                //get the current speed on the NaveMesh
                float speedPercent = agent.velocity.magnitude / agent.speed;
                //set the Animation blend to the Speedvalue and smooth the Transition
                animator.SetFloat("speedPercent", speedPercent, locomotionAnimationSmoothTime, Time.deltaTime);

                if (Vector3.Distance(this.transform.position, player.transform.position) < 5 && dataStorage.playerDied == false)  //Spieler in der Nähe
                {
                    agent.agentTypeID = 0;  //0 = Humanoid
                    if (onTheRoad == true)
                    {
                        tmpLocation = this.transform.position;
                    }

                    nearHero();
                }
                else  //Spieler nicht in der Nähe
                {
                    if (onTheRoad == false)
                    {
                        colTrigger.size = new Vector3(0.5f, 0.5f, 0.5f);
                        agent.SetDestination(tmpLocation);
                    }
                    else
                    {
                        agent.SetDestination(zielVector);
                        agent.agentTypeID = tmpID;  //alte ID*/
                    }

                }
            }
            else
            {
                agent.enabled = false;   //damit er auf der stelle beim sterben stehen bleibt
            }
        }
        else
        {
            agent.enabled = false;
        }
    }

    void nearHero()
    {
        agent.SetDestination(player.transform.position);    //muss durch das selbe wie in update ersetzt werden nur in schneller? damit sie rennen?
        countdown = countdown + 1 * Time.deltaTime;
        if (playerCollision)
        {
            animator.SetBool("attack", true);
            if (countdown > beatTime)
            {
                //Debug.Log("Stirb Held");           
                dataStorage.playerHp -= damage;
                FindObjectOfType<AudioManager>().Play("HeroHurtFX");
                //player.transform.position += Vector3.up;    //an dieser Stelle die Schadens animation des Helden einfügen
                countdown = 0;
            }
        }
        else
        {
            animator.SetBool("attack", false);
        }
    }

    private void OnTriggerStay(Collider collisionInfo)
    {
        if (collisionInfo.name == "character")
        {
            playerCollision = true;
        }
    }

    private void OnTriggerExit(Collider collisionInfo)
    {
        if (collisionInfo.name == "character")
        {
            playerCollision = false;
        }

        if (collisionInfo.tag == "Enemy Path")
        {
            if (Vector3.Distance(this.transform.position, brucke.transform.position) > 25)
            {
                onTheRoad = false;
            }
        }
    }

    private void OnTriggerEnter(Collider collisionInfo)
    {
        if (collisionInfo.name == "EnemyZiel")
        {
            if (dataStorage.hp <= damage)
            {
                Destroy(col.gameObject);
                SceneManager.LoadScene(2);
            }
            else
            {
                Destroy(col.gameObject);
                dataStorage.hp -= 5;
                FindObjectOfType<AudioManager>().Play("HouseDamageFX");
                EnemySpawn.EnemyListe.Remove(this.gameObject);
                Debug.Log("Aua");
            }
        }

        if (collisionInfo.tag == "Enemy Path")
        {
            colTrigger.size = new Vector3(0.3f, 0.5f, 0.3f);
            onTheRoad = true;
        }
    }

    private void died()
    {
        if (enemyHp < 1)
        {
            sterben = true;

            if (SoundUsed == false)
            {
                FindObjectOfType<AudioManager>().Play("MonsterDyingFX");
                SoundUsed = true;

                this.gameObject.GetComponent<BoxCollider>().enabled = false; //Damit die anderen nicht an der Leiche hängen bleiben
                Destroy(this.gameObject.GetComponent<Rigidbody>());
            }

            animator.SetBool("death", true);

            if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Armature|Death"))   //wurde die animation abgespielt?
            {
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 2)
                {
                    dataStorage.gen += dataStorage.enemyBossLoot;
                    animator.SetBool("death", false);
                    EnemySpawn.EnemyListe.Remove(this.gameObject);
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
