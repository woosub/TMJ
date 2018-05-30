using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Ranking : MonoBehaviour {

    List<string> dataList = new List<string>();

    [SerializeField]
    Transform tr;

    [SerializeField]
    GameObject regi;

    [SerializeField]
    InputField ifName;
    
    [SerializeField]
    GameObject warning;

    [SerializeField]
    GameObject gameLayer;

    [SerializeField]
    GameObject backPanel;

    bool warningFlag = false;

    public bool isRegiRank = false;
    
    // Use this for initialization
    void Start ()
    {
    }
	
	// Update is called once per frame
	void Update () {
	}

    public void ViewRank(bool flag)
    {
        gameLayer.SetActive(!flag);
        backPanel.SetActive(flag);
        regi.SetActive(false);

        if (flag)
        {
            StartCoroutine(GetInfo());
        }
        else
        {
            while (tr.childCount > 0)
            {
                DestroyImmediate(tr.GetChild(0).gameObject);
            }
        }

        tr.parent.parent.gameObject.SetActive(flag);
    }

    public void ViewRegistRank()
    {
        if (warningFlag)
            return;

        if (isRegiRank)
        {
            warningFlag = true;
            StartCoroutine(warning_("이미 등록되어 있습니다."));
            return;
        }

        backPanel.SetActive(true);
        gameLayer.SetActive(false);
        regi.SetActive(true);
    }

    public void BackRegistRank()
    {
        backPanel.SetActive(false);
        gameLayer.SetActive(true);
        regi.SetActive(false);
    }

    public void RegistRank()
    {
        if (warningFlag)
            return;

        if (ifName.text == "")
        {
            warningFlag = true;
            StartCoroutine(warning_("닉네임을 입력해주세요."));
            return;
        }

        StartCoroutine(RegistInfo());
    }

    IEnumerator warning_(string msg)
    {
        warning.GetComponentInChildren<Text>().text = msg;
        warning.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        warning.SetActive(false);

        warningFlag = false;
    }

    public void BackRanking()
    {
        ViewRank(false);
    }

    public void ViewRanking()
    {
        ViewRank(true);
    }

    IEnumerator RegistInfo()
    {
        WWWForm form = new WWWForm();

        form.AddField("name", ifName.text);
        form.AddField("email", "");
        form.AddField("card", GetComponent<StageMgr>().cardGauge);

        using (var w = UnityWebRequest.Post("http://run.theminjoo.kr/rank/tmjranking.php", form))
        {
            yield return w.SendWebRequest();
            if (w.isNetworkError || w.isHttpError)
            {
                print(w.error);
            }
            else
            {
                print("Finished Uploading");
            }
        }

        isRegiRank = true;

        ViewRanking();
    }

    IEnumerator GetInfo()
    {
        WWWForm form = new WWWForm();

        //form.AddField("name", "test");
        //form.AddField("email", "test@test.com");
        //form.AddField("card", 250);

        // Create a download object
        var download = UnityWebRequest.Post("http://run.theminjoo.kr/rank/tmjgetrank.php", form);

        // Wait until the download is done
        yield return download.SendWebRequest();

        if (download.isNetworkError || download.isHttpError)
        {
            print("Error downloading: " + download.error);
        }
        else
        {
            dataList.Clear();

            string str = download.downloadHandler.text.Trim();
            string[] temp = str.Split(new string[] { "\n" }, System.StringSplitOptions.None);

            GameObject rankCard = Resources.Load<GameObject>("RankingCard");

            Debug.Log(temp.Length);

            int cnt = 1;

            for (int i = 2; i < temp.Length; i++)
            {
                Debug.Log(temp[i]);
                GameObject card = Instantiate(rankCard);

                card.transform.SetParent(tr);

                card.transform.localScale = Vector3.one;
                card.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, -31f + (i-2) * -125f);

                string[] temp2 = temp[i].Split(new string[] { ":" }, System.StringSplitOptions.None);

                card.GetComponentInChildren<Text>().text = (cnt++).ToString() + "위 - " + temp2[0] + " / 카드 - " + temp2[2] + "개";
                //Debug.Log(temp[i]);
            }

        }

    }
}
