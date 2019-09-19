using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAntiCollision : MonoBehaviour
{
    public Collider c1;
    public Collider c2;

    private void OnCollisionEnter(Collision bla)
    {

        if (bla.gameObject.tag == "Enemy")
        {
            Physics.IgnoreCollision(bla.collider, c1);
            Physics.IgnoreCollision(bla.collider, c2);
        }

    }
}
