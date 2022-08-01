using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    bool canStart = false;
    public static PlayGamesPlatform platform;
    // Start is called before the first frame update

    void Start()
    {
        PlayerPrefs.SetInt("GPlayStatus", 0);

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);

        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;

        // Activate the Google Play Games platform
        platform =  PlayGamesPlatform.Activate();

        //Login
        Social.Active.localUser.Authenticate(success =>
        {
            if (success)
            {
                Debug.Log("Login success");
                canStart = true;

            }
        }
        );
    }

    private void Update()
    {
        if (canStart == true)
        {
            if (PlayGamesPlatform.Instance.IsAuthenticated())
            {
                int HardWin = PlayerPrefs.GetInt("HardVictories", 0);
                int EasyWin = PlayerPrefs.GetInt("EasyVictories", 0);

                Social.ReportScore(EasyWin, GPGSIds.leaderboard_easy_wins, (bool success) =>
                {
                    Debug.Log("Success easy? -> " + success);
                });

                Social.ReportScore(HardWin, GPGSIds.leaderboard_hard_wins, (bool success) =>
                {
                    Debug.Log("Success hard? -> " + success);
                });
            }
            SceneManager.LoadScene("MenuScene");
        }

        if (Time.time >= 5) canStart = true;
    }

    
}
