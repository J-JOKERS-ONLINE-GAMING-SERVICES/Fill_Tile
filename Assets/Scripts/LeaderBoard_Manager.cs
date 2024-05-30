using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class LeaderBoard_Manager : MonoBehaviour
{
    public static LeaderBoard_Manager instance;

    public List<string> Name = new List<string>();
    public List<int> Score = new List<int>();

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Login();
    }

    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            //CustomId = "ABCD",
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    void OnSuccess(LoginResult result)
    {
//        Debug.Log("Successful login/account create!");
        GetLeaderBoard();
    }

    void OnError(PlayFabError error)
    {
      //  Debug.Log("Error while logging in/creating account!");
        Debug.Log(error.GenerateErrorReport());
    }

    void RegisterName()
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = PlayerPrefs.GetString("Name"),
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdated, OnError);
     }

    void OnDisplayNameUpdated(UpdateUserTitleDisplayNameResult result)
    {
//        Debug.Log("Successfull name Changed");

    }
    public void SaveScores(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> {
                new StatisticUpdate {
                    StatisticName = "NumiTile_Scores", Value = score
                }
            }
          };
        RegisterName();
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
       // Debug.Log("Successfull leaderboard sent");
        Invoke("GetLeaderBoard", 3);
    }

    public void GetLeaderBoard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "NumiTile_Scores",
            StartPosition = 0,
            MaxResultsCount = 100
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardLoaded, OnError);
    }

    void OnLeaderboardLoaded(GetLeaderboardResult result)
    {
        Name.Clear();
        Score.Clear();
        foreach (var item in result.Leaderboard)
        {
//            Debug.Log(item.Position + " " + item.DisplayName + " " + item.StatValue);
            Name.Add(item.DisplayName);
            Score.Add(item.StatValue);

            for (int i = 0; i < Score.Count; i++)
            {
                if (item.StatValue > Score[i])
                {
                    int tempScore = Score[i];
                    string tempName = Name[i];

                    Score[i] = item.StatValue;
                    Name[i] = item.DisplayName;

                    Name[Name.Count - 1] = tempName;
                    Score[Score.Count - 1] = tempScore;
                }
            }
        }
    }

   
}
