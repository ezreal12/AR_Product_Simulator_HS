using System.Collections;

using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class ObjectSpawnManager : MonoBehaviour
{
    public AnchorCreator zeroLevelMenu;
    public TestLogCompo testLog; // 테스트 로그용 컴포넌트


    private void AddLog(string s)
    {
       if (testLog != null)
        {
            testLog.AddLog("[ObjectRespawnManager] : " + s);
        }
    }
    //IEnumerator GetServerData(string imgUrl, string objUrl, int arrSize)
    public void GetObejctFromWeb(string src)
    {
        //AddLog("GetObejctFromWeb : " + src);
        //StartCoroutine(LoadFromWebProcess("http://192.168.137.1:8080/obj/prefabs/lowpoly_tree_sample", "lowpoly_tree_sample"));
        string[] s = src.Split('/');
        string assetName = s[s.Length-1];
        StartCoroutine(LoadFromWebProcess(src, assetName));
        // 주의!!! : AssetBunble의 Name은 소문자만 들어가는 개 그지같은 문제가 있다.
    }

    private IEnumerator LoadFromWebProcess(string url, string name)
    {
        Debug.Log("LoadFromWebProcess name : "+name);
        AddLog("LoadFromWebProcess name : " + name);
        var webRequest = UnityWebRequestAssetBundle.GetAssetBundle(url);
        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(webRequest.error);
            Debug.Log("LoadFromWebProcess error");
            AddLog("LoadFromWebProcess error");
        }
        else
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(webRequest);
            if (bundle == null)
            {
                Debug.LogError("LoadFromWebProcess !!! bundle is NULL !!");
                AddLog("LoadFromWebProcess !!! bundle is NULL !!");
                yield break;
            }

            GameObject prefab = bundle.LoadAsset<GameObject>(name);

            if (prefab == null)
            {
                Debug.LogError("LoadFromWebProcess !!! prefab is NULL !!");
                AddLog("LoadFromWebProcess !!! prefab is NULL !!");
                bundle.Unload(true);
                yield break;
            }
            //var t = bundle.LoadAllAssets<Mesh>()[0];
            GameObject inst = Instantiate(prefab);

        // https://m.blog.naver.com/sisi0616/221420297632
        //  False : 번들내에서 에셋의 압축파일 데이터는 언로드 되지만, 이 번들로부터 이미 로드된 실제 객체들은 그대로 둔다. 또한 이 번들로부터 추가적으로 불러올 수 없다.
        // True: 번들로부터 로드된 모든 객체들이 같이 제거된다. 만약 씬 내에 이 에셋들을 참조하는 객체는 해당 참조가 누락된다.
            bundle.Unload(false);
            AddLog("LoadFromWebProcess Complete");
            Debug.Log("LoadFromWebProcess Complete");
            if (zeroLevelMenu != null)
                zeroLevelMenu.ShowObject(inst);
        }
    }


}
