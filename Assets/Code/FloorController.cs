using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    #region public variables
    public float stability = 0.3f;
    public float speed = 2.0f;
    #endregion

    #region Private Variables
    private new Rigidbody rigidbody;
    #endregion

    //Start is called before the first frame update
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Floor hover-torque movement (Copied from a unty forum, but almost understanded. Not erased because it's neccesary)
        Vector3 predictedUp = Quaternion.AngleAxis(
            rigidbody.angularVelocity.magnitude * Mathf.Rad2Deg * stability / speed,
            rigidbody.angularVelocity
        ) * transform.forward; 
        Vector3 torqueVector = Vector3.Cross(predictedUp, Vector3.up); 
        rigidbody.AddTorque(torqueVector * speed * speed * Time.deltaTime);
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("FallingObject"))
        {
            collision.gameObject.tag = "FallingObjectDeactivated";
        }
    }
}
