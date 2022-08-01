using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneHelixController : MonoBehaviour
{

    #region Public Variables
    public float rotationSpeed = 1f;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //make the cloud rotate around the y axis
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
