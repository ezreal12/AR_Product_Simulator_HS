using UnityEngine;
using System.Collections;
using TMPro;
using System;
using UnityEngine.Networking;

public class OneLevelCanvus : MonoBehaviour
{
    private TextMeshPro TestLog;
    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "TestLog") {
                TestLog = transform.GetChild(i).GetComponent<TextMeshPro>();
                break;
            }
        }


    }
    void OnEnable() {
        StartCoroutine(Get("http://localhost:8080/obj/file_list.json", (request) =>
        {
            if (request.result == UnityWebRequest.Result.Success && request.responseCode == 200)
            {
                Debug.Log(request.downloadHandler.text);
                if (TestLog != null)
                {
                    TestLog.SetText(request.downloadHandler.text);
                }
            }
            else
            {
                string err = "[Error]:" + request.responseCode + request.error;
                Debug.Log(err);
                if (TestLog != null)
                {
                    TestLog.SetText(err);
                }
            }
        }));
    }
    IEnumerator Get(string url, Action<UnityWebRequest> callback)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        callback(request);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
