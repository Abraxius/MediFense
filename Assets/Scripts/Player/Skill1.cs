using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill1 : MonoBehaviour
{
    private float timer;
    private Datenspeicherung dataStorage;

    // Start is called before the first frame update
    void Start()
    {
        dataStorage = GameObject.Find("Player").GetComponent<Datenspeicherung>();
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 1.4 && timer < 2.2)
        {
            transform.GetComponent<SphereCollider>().enabled = true;
        }
        if (timer > 2.2 && timer < 4.2)
        {
            transform.GetComponent<SphereCollider>().enabled = false;
        }
        if (timer > 4.2)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            bool tmp = false;
            bool tmp2 = false;
            try
            {
                collision.gameObject.GetComponent<Enemy>().enemyHp -= dataStorage.skill1Damage;
            }
            catch
            {
                tmp = true;
            }

            if (tmp)
            {
                try
                {
                    collision.gameObject.GetComponent<Enemy_Golem>().enemyHp -= dataStorage.skill1Damage;
                } catch
                {
                    tmp2 = true;
                }
            }

            if (tmp2)
            {
                collision.gameObject.GetComponent<EnemyBoss>().enemyHp -= dataStorage.skill1Damage;
            }

        }
    }
}
