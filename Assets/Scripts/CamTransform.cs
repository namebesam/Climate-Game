using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Transform camPos;

    // Update is called once per frame
    void Update()
    {
        //Updates the position of CamPos to the player's position
        transform.position = camPos.position;
    }
}
