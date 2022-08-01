using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class MenuCanvasController : MonoBehaviour
{
    public GameObject androidButton;
    public TextMeshProUGUI androidText;
    public TextMeshProUGUI victories;
    public TextMeshProUGUI gamemodeText;
    public TextMeshProUGUI GPStatusText;
    public GameObject leaderbButton;
    public GameObject achievementButton;
        
    // Start is called before the first frame update
    public void Start()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated()) GPStatusText.text = "Play Status Enabled";
        else GPStatusText.text = "Play Status Disabled";

        if (PlayerPrefs.GetInt("AndroidControl") == 0)
        {
            androidText.text = "Accelerometer\nMode";
        }
        else if (PlayerPrefs.GetInt("AndroidControl") == 1)
        {
            androidText.text = "Joystick\nMode";
        }
        else
        {
            androidText.text = "????\nError";
        }

        switch (PlayerPrefs.GetInt("Gamemode", 0))
        {
            case 0:
                victories.text = "You have achieved " + PlayerPrefs.GetInt("EasyVictories", 0) + " easy victories.";
                gamemodeText.text = "Easy";
                break;
            case 1:
                victories.text = "You have achieved " + PlayerPrefs.GetInt("HardVictories", 0) + " hard victories.";
                gamemodeText.text = "Hard";
                break;
            case 2:
                victories.text = "You have scored " + PlayerPrefs.GetInt("PointsEndless",0 ) + " endless points";
                gamemodeText.text = "Endless";
                break;
            default:
                victories.text = "Oops, an error getting your victories!";
                gamemodeText.text = "Error";
                break;
        }

        if (Application.platform != RuntimePlatform.Android)
        {
            androidButton.SetActive(false);
            leaderbButton.SetActive(false);
            achievementButton.SetActive(false);
            GPStatusText.text = "";
        }
        else
        {
            androidButton.SetActive(true);
            leaderbButton.SetActive(true);
            achievementButton.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeAndroidMode()
    {
        if (PlayerPrefs.GetInt("AndroidControl") == 1)
        {
            androidText.text = "Accelerometer\nMode";
            PlayerPrefs.SetInt("AndroidControl", 0);
        }
        else if (PlayerPrefs.GetInt("AndroidControl") == 0)
        {
            androidText.text = "Joystick\nMode";
            PlayerPrefs.SetInt("AndroidControl", 1);
        }
        
    }

    public void ChangeGamemode()
    {
        switch (PlayerPrefs.GetInt("Gamemode", 0))
        {
            case 0:
                PlayerPrefs.SetInt("Gamemode", 1);
                victories.text = "You have achieved " + PlayerPrefs.GetInt("HardVictories", 0) + " hard victories.";
                gamemodeText.text = "Hard";
                break;
            case 1:
                PlayerPrefs.SetInt("Gamemode", 2);
                victories.text = "You have scored " + PlayerPrefs.GetInt("PointsEndless", 0) + " endless points";
                gamemodeText.text = "Endless";
                break;
            case 2:

                PlayerPrefs.SetInt("Gamemode", 0);
                victories.text = "You have achieved " + PlayerPrefs.GetInt("EasyVictories", 0) + " easy victories.";
                gamemodeText.text = "Easy";
                break;
            
            default:
                PlayerPrefs.SetInt("Gamemode", 0);
                victories.text = "You have achieved " + PlayerPrefs.GetInt("EasyVictories", 0) + " easy victories.";
                gamemodeText.text = "Easy";
                break;
        }                
    }

    public void AccessLeaderboard()
    {
        if(PlayGamesPlatform.Instance.IsAuthenticated())
        {
            if(PlayerPrefs.GetInt("Gamemode", 0) == 0)
            {
                // show leaderboard UI easy
                PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_easy_wins);
            }
            else if(PlayerPrefs.GetInt("Gamemode", 0) == 1)
            {
                // show leaderboard UI hard
                PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_hard_wins);
            }
            else if (PlayerPrefs.GetInt("Gamemode", 0) == 2)
            {
                // show leaderboard UI endless
                PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_endless_points);
            }
        }
    }

    public void AccessAchievement()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            // show achievements UI
            PlayGamesPlatform.Instance.ShowAchievementsUI();
        }
    }
}
