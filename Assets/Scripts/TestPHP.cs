using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class TestPHP : MonoBehaviour {

	// Use this for initialization
	//IEnumerator Start () {
 //       WWWForm form = new WWWForm();

 //       form.AddField("name", "test");
 //       form.AddField("email", "test@test.com");
 //       form.AddField("card", 250);

 //       using (var w = UnityWebRequest.Post("http://theminjoo.einvention.kr/rank/tmjranking.php", form))
 //       {
 //           yield return w.SendWebRequest();
 //           if (w.isNetworkError || w.isHttpError)
 //           {
 //               print(w.error);
 //           }
 //           else
 //           {
 //               print("Finished Uploading Screenshot");
 //           }
 //       }

 //   }

    IEnumerator Start()
    {
        WWWForm form = new WWWForm();

        //form.AddField("name", "test");
        //form.AddField("email", "test@test.com");
        //form.AddField("card", 250);
                
        // Create a download object
        var download = UnityWebRequest.Post("theminjoo.einvention.kr/rank/tmjgetrank.php", form);

        // Wait until the download is done
        yield return download.SendWebRequest();

        if (download.isNetworkError || download.isHttpError)
        {
            print("Error downloading: " + download.error);
        }
        else
        {
            // show the highscores
            Debug.Log(download.downloadHandler.text);
            Dictionary<string, string> dic = download.GetResponseHeaders();

            foreach (string s in dic.Values)
            {
                Debug.Log(s);
            }
        }

    }

    // Update is called once per frame
    void Update () {
		
	}
}
