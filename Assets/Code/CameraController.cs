using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform player = null;
    public float minorVisibleDistance = 3.0f;

    private float minorYpossible = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        minorYpossible = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        //Check if the player is in the minor visible distance
        if (player.position.y > minorYpossible - minorVisibleDistance)
        {
            //Move the camera to the player
            transform.position = new Vector3(transform.position.x, player.position.y + minorVisibleDistance, transform.position.z);
        }
        else
        {
            //If the player is not in the minor visible distance, move the camera to the minor visible distance
            transform.position = new Vector3(transform.position.x, minorYpossible, transform.position.z);
        }
    }
}
