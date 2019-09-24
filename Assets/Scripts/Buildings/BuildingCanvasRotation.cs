using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCanvasRotation : MonoBehaviour
{
    // Sorgt dafür das die Schrift immer in Richtung der Kamera zeigt
    void Update()
    {
        CanvasRotate();
    }

    private void CanvasRotate()
    {
        var temp = Camera.main.transform.rotation;
        this.transform.rotation = temp;
    }
}
