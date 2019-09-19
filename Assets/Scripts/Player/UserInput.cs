using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class UserInput : MonoBehaviour
{
    bool scrollTaste = false;
    float scrollXpos = 0;
    float scrollYpos = 0;


    //Pausemenü
    private Datenspeicherung dataStorage;

    private bool kameraFreeze;
    public SphereCollider col1;
    public SphereCollider col2;

    private void Start()
    {
        dataStorage = GameObject.Find("Player").GetComponent<Datenspeicherung>();
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            kameraFreeze = true;
            Destroy(Camera.main.GetComponent<Rigidbody>());
        }

        if (Input.GetKeyUp("space"))
        {
            kameraFreeze = false;
            Rigidbody gameObjectsRigidBody = Camera.main.gameObject.AddComponent<Rigidbody>();
            gameObjectsRigidBody.useGravity = false;
            gameObjectsRigidBody.freezeRotation = true;
        }

        if (kameraFreeze == false)
        {
            col1.enabled = true;
            col2.enabled = true;

            if (Input.GetMouseButtonDown(2))    //Legt Die Koordinaten fest beim Klicken des Mausrades um zu wissen wohin er sich bewegen soll
            {
                scrollTaste = true;
                scrollXpos = Input.mousePosition.x;
                scrollYpos = Input.mousePosition.y;
            }

            if (!Input.GetMouseButton(2))   //Resetet Mausrad Koordinaten
            {
                scrollTaste = false;
                scrollXpos = 0;
                scrollYpos = 0;
            }

            MoveCamera();
            RotateCamera();
        } else
        {
            col1.enabled = false;
            col2.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))   //Pause menü
        {
            Debug.Log("Es ist Pause");
            dataStorage.pauseButton = !dataStorage.pauseButton;
        }
    }

    private void MoveCamera()
    {
        float xpos = Input.mousePosition.x;
        float ypos = Input.mousePosition.y;

        Vector3 movement = new Vector3(0, 0, 0);

        //Rand Bewegungen--------------------------------------------------------------------------------------------------------------------------
        //horizontale Kamera Bewegung
        if (xpos >= 0 && xpos < RessourceManager.ScrollWidth || Input.GetKey("a") || xpos < scrollXpos && scrollXpos != 0)  //Wenn Mauszeiger nach links bewegt wird
        {
            movement.x -= RessourceManager.ScrollSpeed;
        }
        else if (xpos <= Screen.width && xpos > Screen.width - RessourceManager.ScrollWidth || Input.GetKey("d") || xpos > scrollXpos && scrollYpos != 0) //Wenn nach rechts bewegt wird
        {
            movement.x += RessourceManager.ScrollSpeed;
        }

        //vertikale Kamera Bewegung
        if (ypos >= 0 && ypos < RessourceManager.ScrollWidth || Input.GetKey("s") || ypos < scrollYpos && scrollYpos != 0)  //Wenn Mauszeiger nach hinten bewegt wird
        {
            movement.z -= RessourceManager.ScrollSpeed;
        }
        else if (ypos <= Screen.height && ypos > Screen.height - RessourceManager.ScrollWidth || Input.GetKey("w") || ypos > scrollYpos && scrollYpos != 0) //Wenn nach vorne bewegt wird
        {
            movement.z += RessourceManager.ScrollSpeed;
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------

        //Kontrolliert ob es sich in Blickrichtung bewegt und ignoriert die Neigung der Kamera
        movement = Camera.main.transform.TransformDirection(movement);
        movement.y = 0;

        //hoch und runter scrollen
        movement.y -= RessourceManager.ScrollSpeed * Input.GetAxis("Mouse ScrollWheel");

        //Hier wird der zuvor erhaltene Input angewendet
        Vector3 origin = Camera.main.transform.position;
        Vector3 destination = origin;

        destination.x += movement.x;
        destination.y += movement.y;
        destination.z += movement.z;

        //Kontrolliert ob die Kamera im min und max Bereich ist
        //Höhe
        if (destination.y > RessourceManager.MaxCameraHeight)
        {
            destination.y = RessourceManager.MaxCameraHeight;
        }
        else if (destination.y < RessourceManager.MinCameraHeight)
        {
            destination.y = RessourceManager.MinCameraHeight;

        }
        //Breite
        if (destination.z > RessourceManager.MaxCameraWidth)
        {
            destination.z = RessourceManager.MaxCameraWidth;
        }
        else if (destination.z < RessourceManager.MinCameraWidth)
        {
            destination.z = RessourceManager.MinCameraWidth;
        }

        //Länge
        if (destination.x > RessourceManager.MaxCameraLength)
        {
            destination.x = RessourceManager.MaxCameraLength;
        }
        else if (destination.x < RessourceManager.MinCameraLength)
        {
            destination.x = RessourceManager.MinCameraLength;
        }

        //wenn ein angewendeter Unterschied festgestellt wird, wird die Kameraposition angepasst
        if (destination != origin)
        {
            Camera.main.transform.position = Vector3.MoveTowards(origin, destination, Time.deltaTime * RessourceManager.ScrollSpeed);
        }
    }

    private void RotateCamera()
    {
        Vector3 origin = Camera.main.transform.eulerAngles;
        Vector3 destination = origin;

        //Rotieren beim halten der rechten Mausrades
        if (Input.GetMouseButton(1))
        {
            destination.x -= Input.GetAxis("Mouse Y") * RessourceManager.RotateAmount;
            destination.y += Input.GetAxis("Mouse X") * RessourceManager.RotateAmount;
        }

        //wenn eine änderung festgestellt wird, wird diese auf die Kamerarotation angepasst
        if (destination != origin)
        {
            Camera.main.transform.eulerAngles = Vector3.MoveTowards(origin, destination, Time.deltaTime * RessourceManager.RotateSpeed);
        }
    }
}
