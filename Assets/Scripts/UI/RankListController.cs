using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankListController : MonoBehaviour
{
    [SerializeField] GameObject[] scoreElement;
    [SerializeField] GameObject you;
    [SerializeField] int listCount = 10;
    List<Dictionary<string, string>> rankList;
    Dictionary<string, string> youInfo;
    void Awake()
    {
        rankList = new List<Dictionary<string, string>>();
        youInfo = new Dictionary<string, string>();
    }

    void Start()
    {
        GetRankList();
        PaddingRankList();
    }

    void GetRankList()
    {
        // TODO: get rank list from web server
        // TEMP: gen rank list
        youInfo.Add("rank", 1.ToString());
        youInfo.Add("name", "You");
        youInfo.Add("score", GameManager.LastGameScore.ToString());
        rankList.Add(youInfo);
        for (int i = 2; i <= listCount; i++)
        {
            Dictionary<string, string> temp = new Dictionary<string, string>();
            temp.Add("rank", i.ToString());
            temp.Add("name", "None");
            temp.Add("score", "None");
            rankList.Add(temp);
        }
    }

    void PaddingRankList()
    {
        for (int i = 0; i < listCount; i++)
        {
            GameObject scoreGameObject = scoreElement[i];
            Dictionary<string, string> playerInfo = rankList[i];
            scoreGameObject.GetComponent<ScoreElementController>().Set(playerInfo["rank"], playerInfo["name"], playerInfo["score"]);
        }
        you.GetComponent<ScoreElementController>().Set(youInfo["rank"], youInfo["name"], youInfo["score"]);
    }
}
