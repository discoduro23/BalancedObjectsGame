using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class PlayerController : MonoBehaviour
{
    #region Public Variables
    public      float       speed = 10.0f;
    public      float       jumpForce = 5f;
    public      int         androidControl = 0;
    public      GameObject  upfrontJoystick = null;
    public      GameObject  backJoystick = null;

    #endregion

    #region Private Variables
    private     new Rigidbody rigidbody;
    private     Vector3     inputs = Vector3.zero;
    private     bool        canJump = true;
    private     bool        butttonJumpPressed = false;
    private     float       currentStatueTime = 0.0f;
    private     float       statueTime = 5.0f;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        androidControl = PlayerPrefs.GetInt("AndroidControl", androidControl);

        if ((Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) 
            && androidControl == 1)
        {
            upfrontJoystick.SetActive(true);
            backJoystick.SetActive(true);
        }
        else
        {
            backJoystick.SetActive(false);
            upfrontJoystick.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer ||
            Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
        {
            //get the inputs
            inputs.x = Input.GetAxis("Horizontal");
            inputs.z = Input.GetAxis("Vertical");

            butttonJumpPressed = Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire1"); //With controller
        }
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {      

            if (androidControl == 0) {

                //make the player move with android accelerometer
                inputs.x = 5.0f * Mathf.Clamp(Input.acceleration.x, -0.2f, 0.2f);
                inputs.z = 5.0f * (Mathf.Clamp(Input.acceleration.y, -0.6f, -0.2f) + 0.4f);

                //make the player jump with the fire1 axis
                butttonJumpPressed = Input.GetButtonDown("Fire1");

            }
            else if (androidControl == 1)
            {
                
                //reset at first the inputs to zero
                inputs = Vector3.zero;
                upfrontJoystick.transform.position = new Vector2(300, 250);
                backJoystick.transform.position = new Vector2(300, 250);

                //make the player move with virtual joystick
                for (int i = 0; i < Input.touchCount; i++)
                {
                    //float relativeRawPositionY = Input.touches[i].rawPosition.y / Screen.height;
                    float relativePositionX = Input.touches[i].rawPosition.x / Screen.width;

                    if (relativePositionX < 0.5)
                    {
                        Vector2 relativeDisplacement = (Input.touches[i].position - Input.touches[i].rawPosition) / Screen.height;
                        relativeDisplacement.x = Mathf.Clamp(relativeDisplacement.x, -1f, 1f);
                        relativeDisplacement.y = Mathf.Clamp(relativeDisplacement.y, -1f, 1f);

                        inputs.x = 4.0f * Mathf.Clamp(relativeDisplacement.x, -0.25f, 0.25f);
                        inputs.z = 4.0f * Mathf.Clamp(relativeDisplacement.y, -0.25f, 0.25f);

                        //move the joystic sprite
                        upfrontJoystick.transform.position = Input.touches[i].position;
                        backJoystick.transform.position = Input.touches[i].rawPosition;
                    }
                    
                    
                    if (relativePositionX > 0.5)
                    {
                        butttonJumpPressed = true;
                    }
                    else
                    {
                        butttonJumpPressed = false;
                    }
                    
                    
                } 
            }
        }
        
        
        //Jump    
        if (canJump && butttonJumpPressed)
        {
            Jump();
            canJump = false;

        }
        
    }


    void FixedUpdate()
    {
        //Update FIXEDdeltatime
        float fdt = Time.fixedDeltaTime;

        //apply movement with rigidbody
        Movement(inputs, fdt);

        //rotate the player 
        Rotate(inputs);

        //Achievements
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            //Statues
            if (inputs.x >= Mathf.Epsilon || inputs.x <= -Mathf.Epsilon || inputs.z >= Mathf.Epsilon || inputs.z <= -Mathf.Epsilon)
            {
                currentStatueTime = 0.0f;
            }
            else
            {
                currentStatueTime += Time.deltaTime;
                if (currentStatueTime > statueTime)
                {
                    currentStatueTime = 0.0f;
                    PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_lets_play_statues, 100.0f, (bool success) =>{});
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            canJump = true;
        }

        if (collision.gameObject.CompareTag("Death") || collision.gameObject.CompareTag("FallingObject"))
        {
            //Endless Win
            if (PlayGamesPlatform.Instance.IsAuthenticated())
            {
                Social.ReportScore((long)PlayerPrefs.GetInt("PointsEndless", 0), GPGSIds.leaderboard_endless_points, (bool success) =>
                {
                    Debug.Log("Success endless score? -> " + success);
                });
            }
            
            SceneManager.LoadScene("GameOverScene");
        }
        
    }

    private void Movement(Vector3 inputs, float dt) { 
        rigidbody.AddForce(dt * speed * inputs, ForceMode.Impulse);
    }
        

    private void Jump()
    {
        rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
    }

    private void Rotate(Vector3 inputs)
    {
        if (inputs.magnitude > Mathf.Epsilon)
        {
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.y = Mathf.Atan2(inputs.x, inputs.z) * Mathf.Rad2Deg;
            transform.eulerAngles = eulerAngles;
        }
        else
        {
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.y = 180;
            transform.eulerAngles = eulerAngles;
        }
    }

}
