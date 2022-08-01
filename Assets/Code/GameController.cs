using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using TMPro;


public class GameController : MonoBehaviour
{
    #region Public Variables
    public GameObject[] items;
    public float spawnTime = 3f;
    public float currentSpawnTime = 0.0f;
    public float radius = 4.5f;
    public GameObject goSlider;
    public TextMeshProUGUI pointsTXT;
    #endregion
    
    #region Private Variables
    private Slider slider;
    float nextSpawnTime = 0f;
    bool isEndless = false;
    int localPoints = 0;
    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        //One time achievement
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            //Your very first game
            Social.ReportProgress(GPGSIds.achievement_your_first_play, 100.0f, (bool success) => { });
        }

            slider = goSlider.GetComponent<Slider>();
        currentSpawnTime = spawnTime;
        if (PlayerPrefs.GetInt("Gamemode", 0) == 2)
        {
            isEndless = true;

            localPoints = 0;
            goSlider.SetActive(false);
            pointsTXT.text = "0";
        }
        else
        {
            isEndless = false;
            
            pointsTXT.text = "";
        }
    }

    // Update is called once per frame
    void Update()
    {
        float randomSmallEndless, randomLargeEndless = 0.0f;
        
        if (Time.time >= nextSpawnTime)
        {
            nextSpawnTime = Time.time + currentSpawnTime;

            if (isEndless)
            {
                if (currentSpawnTime <= 1) currentSpawnTime = 1;
                else currentSpawnTime -= Random.Range(0.05f, 0.1f);
                
            }
            else currentSpawnTime -= Random.Range(0.02f, 0.04f);
            switch (PlayerPrefs.GetInt("Gamemode", 0))
            {
                case 0:
                    slider.value = 1 - (currentSpawnTime - 1) / (spawnTime - 1);
                    //minus 1 because if the time gets to 1, the slider will be at 100% as the dificulty increases enough
                    
                    SpawnRandomItem(2.5f);
                    break;
                    
                case 1:
                    slider.value = 1 - (currentSpawnTime - 1) / (spawnTime - 1);
                    //minus 1 because if the time gets to 1, the slider will be at 100% as the dificulty increases enough
                    
                    SpawnRandomItem(1.25f);
                    SpawnRandomItem(2.25f);
                    break;
                    
                case 2:
                    randomLargeEndless = Random.Range(1.25f, 3f);
                    SpawnRandomItem(randomLargeEndless);
                    
                    randomSmallEndless = Random.Range(1f, 2f);
                    SpawnRandomItem(randomSmallEndless);

                    localPoints += (int)(randomSmallEndless * 100) + (int)(randomLargeEndless * 100);
                    pointsTXT.text = localPoints.ToString();
                    
                    if(PlayerPrefs.GetInt("PointsEndless", 0) < localPoints) 
                        PlayerPrefs.SetInt("PointsEndless", localPoints);
                    break;
                    
                default:
                    break;
            }
        }

        //Achivements

        //Is autenticated
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            //Your very first game
            Social.ReportProgress(GPGSIds.achievement_your_first_play, 100.0f, (bool success) => { });

            //The hard way
            if (PlayerPrefs.GetInt("Gamemode", 0) == 1 && PlayerPrefs.GetInt("AndroidControl", 0) == 0)
            {
                Social.ReportProgress(GPGSIds.achievement_the_real_hard_way, 100.0f, (bool success) => { });
            }

            //Endless
            if (isEndless){
                if(localPoints >= 2500)
                    Social.ReportProgress(GPGSIds.achievement_small_progress, 100.0f, (bool success) => { });
                if (localPoints >= 5000)
                    Social.ReportProgress(GPGSIds.achievement_you_have_been_practicing, 100.0f, (bool success) => { });
                if (localPoints >= 15000)
                    Social.ReportProgress(GPGSIds.achievement_youre_a_hacker, 100.0f, (bool success) => { });
            }
        }
           

        //WIN
        if (currentSpawnTime <= 1.0f && !isEndless)
        {
            int EasyWin = PlayerPrefs.GetInt("EasyVictories", 0);
            int HardWin = PlayerPrefs.GetInt("HardVictories", 0);


            switch (PlayerPrefs.GetInt("Gamemode", 0))
            {
                case 0:
                    EasyWin = PlayerPrefs.GetInt("EasyVictories", 0) + 1;
                    PlayerPrefs.SetInt("EasyVictories", EasyWin);

                    if(PlayGamesPlatform.Instance.IsAuthenticated())
                    {
                        Social.ReportScore((long)EasyWin, GPGSIds.leaderboard_easy_wins, (bool success) =>
                        {
                            Debug.Log("Success easy score? -> " + success);
                        });
                    }

                    break;
                case 1:
                    HardWin = PlayerPrefs.GetInt("HardVictories", 0) + 1;
                    PlayerPrefs.SetInt("HardVictories", HardWin);

                    if (PlayGamesPlatform.Instance.IsAuthenticated())
                    {
                        Social.ReportScore((long)HardWin, GPGSIds.leaderboard_hard_wins, (bool success) =>
                        {
                            Debug.Log("Success hard score? -> " + success);    
                        });
                    }

                    break;                
                default:
                    break;
            }

            //Achivements
            if (PlayGamesPlatform.Instance.IsAuthenticated())
            {
                //First win
                Social.ReportProgress(GPGSIds.achievement_your_first_win, 100.0f, (bool success) => { });

                //5 any gamemode
                if ((HardWin + EasyWin) >= 5)
                    Social.ReportProgress(GPGSIds.achievement_5_wins_of_any_gamemode, 100.0f, (bool success) => { });

                //10 gamemode
                if ((HardWin + EasyWin) >= 10)
                    Social.ReportProgress(GPGSIds.achievement_10_wins_of_any_gamemode, 100.0f, (bool success) => { });
            }

            /*
             * 
             * Only a test, very boring now i guess.
             * 
             * //send notifications with the addon
            var notificationParams = new Assets.SimpleAndroidNotifications.NotificationParams
            {
                Id = UnityEngine.Random.Range(0, int.MaxValue),
                Delay = System.TimeSpan.FromSeconds(1),
                Title = "Balanced Win",
                Message = "You survived! Well done!",
                Ticker = "Ticker",
                Sound = false,
                Vibrate = true,
                Light = true,
                SmallIcon = Assets.SimpleAndroidNotifications.NotificationIcon.Star,
                SmallIconColor = new Color(0.05f, 0.75f, 0.91f),
                LargeIcon = "app_icon"
            };

            Assets.SimpleAndroidNotifications.NotificationManager.SendCustom(notificationParams);*/

            Time.timeScale = 1;
            //change the scene
            SceneManager.LoadScene("FinishScene");
        }

        //change the scene to MenuScene when the escape button
        if (Input.GetButtonDown("Cancel"))
        {
            PauseAndResumeActivity();
        }

    }

    private void SpawnRandomItem(float maxScale)
    {
        
        GameObject randomObject = items[Random.Range(0, items.Length)];
        float randomAngle = Random.Range(0.0f, 360.0f);
        float randomRadius = Random.Range(1f, radius - 0.5f);
        float randomScale = Random.Range(1f, maxScale);

        Vector3 offset = randomRadius * (Quaternion.AngleAxis(randomAngle, Vector3.up) * Vector3.forward);

        Vector3 spawnPosition = transform.position + offset + transform.up * Random.Range(-5.0f, 5.0f);

        GameObject objectSpawned = GameObject.Instantiate(randomObject);
        objectSpawned.transform.position = spawnPosition;
        objectSpawned.transform.eulerAngles = new Vector3(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
        objectSpawned.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
        objectSpawned.GetComponent<Rigidbody>().mass *= randomScale;
    }

    public void PauseAndResumeActivity(){
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;

            //Stop music
            GameObject.Find("AmbientMusic").GetComponent<AudioSource>().Pause();
        }
        else
        {
            Time.timeScale = 1;

            //Resume music
            GameObject.Find("AmbientMusic").GetComponent<AudioSource>().UnPause();
        }
    }
}
