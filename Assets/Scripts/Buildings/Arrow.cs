using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Arrow : MonoBehaviour
{
    // Adjust the speed for the application.
    public float speed = 1.0f;

    // The target (cylinder) position.
    public GameObject target;
    private GameObject tmpTarget;

    private Datenspeicherung dataStorage;

    public bool fireball;
    public GameObject fireballHit = null;

    //public List<GameObject> trails;

    // Start is called before the first frame update
    void Start()
    {
        dataStorage = GameObject.Find("Player").GetComponent<Datenspeicherung>();
        tmpTarget = target;
    }

    // Update is called once per frame
    void Update()
    {
        // Move our position a step closer to the target.
        float step = speed * Time.deltaTime; // calculate distance to move

        if (tmpTarget == null || tmpTarget.GetComponent<NavMeshAgent>().enabled == false)  //Sicherheitsabfrage, falls der Enemy stirbt und ein Pfeil noch fliegt
        {
            Destroy(this.gameObject);
        } else
        {
            transform.position = Vector3.MoveTowards(transform.position, tmpTarget.transform.position, step);

            // Check if the position of the cube and sphere are approximately equal.
            if (Vector3.Distance(transform.position, tmpTarget.transform.position) < 0.001f)
            {
                // Swap the position of the cylinder.
                tmpTarget.transform.position *= -1.0f;
            }
        }

    }

    private void OnTriggerEnter(Collider collisionInfo)
    {
        if (collisionInfo.tag == "Enemy")
        {
            bool tmp = false;
            bool tmp2 = false;

            if (fireball) //Feuerball
            {
                Instantiate(fireballHit, this.transform.position, Quaternion.identity);

                try
                {
                    tmpTarget.GetComponent<Enemy>().enemyHp -= dataStorage.towerDamage2;
                }
                catch
                {
                    tmp = true;
                }

                if (tmp == true)
                {
                    try
                    {
                        tmpTarget.GetComponent<Enemy_Golem>().enemyHp -= dataStorage.towerDamage2;
                    }
                    catch
                    {
                        tmp2 = true;
                    }
                }

                if (tmp == true && tmp2 == true)
                {
                    tmpTarget.GetComponent<EnemyBoss>().enemyHp -= dataStorage.towerDamage2;
                }
            } else //Arrow Pfeil
            {
                try
                {
                    tmpTarget.GetComponent<Enemy>().enemyHp -= dataStorage.towerDamage1;
                }
                catch
                {
                    tmp = true;
                }

                if (tmp == true)
                {
                    try
                    {
                        tmpTarget.GetComponent<Enemy_Golem>().enemyHp -= dataStorage.towerDamage1;
                    }
                    catch
                    {
                        tmp2 = true;
                    }
                }

                if (tmp == true && tmp2 == true)
                {
                    tmpTarget.GetComponent<EnemyBoss>().enemyHp -= dataStorage.towerDamage1;
                }
            }

            FindObjectOfType<AudioManager>().Play("MonsterHurtFX");
            Destroy(this.gameObject);            
        }
    }
}
