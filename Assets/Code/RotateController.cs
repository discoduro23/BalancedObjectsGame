using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateController : MonoBehaviour
{
    #region Public Variables
    public float rotationSpeed = 1f;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        //make the rotation in axis y random at start
        transform.Rotate(0, Random.Range(0, 360), 0);
    }

    // Update is called once per frame
    void Update()
    {
        //make the cloud rotate around the y axis
        transform.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime);
    }
}
