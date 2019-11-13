using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RTS;
using UnityEngine.SceneManagement; // WICHTIG FÜR SZENENWECHSEL

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyPath enemyPath; //Muss in der Szene verknüft werden, da der Pfad in der Szene existiert
    [SerializeField] float waypointTolerance = 10f; //Abweichung zum Wegpunkt Toleranz

    Vector3 guardPosition;
    int currentWaypointIndex = 0;

    public NavMeshAgent agent;

    private EnemySpawn EnemySpawn;
    private Datenspeicherung dataStorage;
    private GameObject player;

    Animator animator;
    const float locomotionAnimationSmoothTime = .1f;

    public float beatTime;  //Die Zeit zwischen denen beim Schlagen des Enemys pause ist
    public int damage;       //Der Schaden den der Enemy verursacht

    [HideInInspector] public float countdown;
    [HideInInspector] public bool playerCollision;

    public int enemyHp;
    [HideInInspector] public int startetEnemyHP;

    [HideInInspector] public bool sterben;

    private bool SoundUsed;

    void Start()
    {
        dataStorage = GameObject.Find("Player").GetComponent<Datenspeicherung>();
        player = GameObject.Find("Player").gameObject.transform.GetChild(1).gameObject;
        EnemySpawn = GameObject.Find("EnemySpawn").GetComponent<EnemySpawn>();

        animator = GetComponentInChildren<Animator>();

        //damage = dataStorage.enemyDamage1;
        //beatTime = dataStorage.enemyBeatTime1;
        //enemyHp = dataStorage.enemy1Hp;         //Das muss noch für mehrere Enemys erweitert werden!!!
        startetEnemyHP = enemyHp;

        countdown = beatTime;

        SoundUsed = false;
    }

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
                    nearHero();
                }
                else  //Spieler nicht in der Nähe
                {
                    PfadVerfolgen();
                }
            }
            else
            {
                agent.enabled = false;    //damit er auf der stelle beim sterben stehen bleibt
            }
        } else
        {
            agent.enabled = false;
        }
    }

    private void PfadVerfolgen()
    {
        Vector3 nextPosition = guardPosition;   //guardPosition ist default

        if (enemyPath != null)
        {
            if (AtWaypoint())
            {
                CycleWaypoint();
            }
            nextPosition = GetCurrentWaypoint();
        }
        agent.SetDestination(nextPosition); //geht an die vorgesehene Position                                                         

    }

    private bool AtWaypoint()   //gibt an ob noch eine Entfernung zum nächsten Wegpunkt besteht
    {
        float distanceToWayPoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
        return distanceToWayPoint < waypointTolerance;
    }

    private void CycleWaypoint()    //Guckt nach ob es einen nächsten Wegpunkt gibt und welcher dieser ist (Index)
    {
        currentWaypointIndex = enemyPath.GetNextIndex(currentWaypointIndex);
    }

    private Vector3 GetCurrentWaypoint() //Fragt in PatrolPath.cs nach wo sich der aktuell anzusteuernde Wegpunkt befindet (Position)
    {
        return enemyPath.GetWaypoint(currentWaypointIndex);
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
                dataStorage.playerHp -= damage;
                FindObjectOfType<AudioManager>().Play("HeroHurtFX");
                countdown = 0;
            }
        } else
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
    }

    private void OnTriggerEnter(Collider collisionInfo)
    {
        if(collisionInfo.name == "EnemyZiel")
        {
            if(dataStorage.hp <= damage)
            {
                Destroy(gameObject);
                PlayerPrefs.SetInt("lastRound", dataStorage.welle);
                if (PlayerPrefs.GetInt("HighScore") <= dataStorage.welle) {
                    PlayerPrefs.SetInt("HighScore", dataStorage.welle);
                }

                SceneManager.LoadScene(2);
            } else
            {
                Destroy(gameObject);
                dataStorage.hp -= 1;
                FindObjectOfType<AudioManager>().Play("HouseDamageFX");
                EnemySpawn.EnemyListe.Remove(this.gameObject);
            }            
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

                this.gameObject.GetComponent<CapsuleCollider>().enabled = false; //Damit die anderen nicht an der Leiche hängen bleiben
                Destroy(this.gameObject.GetComponent<Rigidbody>());
            }

            animator.SetBool("death", true);

            if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Armature|Death"))   //wurde die animation abgespielt?
            {
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 2)
                {
                    dataStorage.money += dataStorage.enemyLoot;
                    animator.SetBool("death", false);
                    EnemySpawn.EnemyListe.Remove(this.gameObject);
                    Destroy(this.gameObject);
                }
            }
         }
    }
}
