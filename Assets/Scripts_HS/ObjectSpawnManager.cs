using System.Collections;

using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class ObjectSpawnManager : MonoBehaviour
{
    public AnchorCreator zeroLevelMenu;
    public TestLogCompo testLog; // �׽�Ʈ �α׿� ������Ʈ


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
        // ����!!! : AssetBunble�� Name�� �ҹ��ڸ� ���� �� �������� ������ �ִ�.
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
        //  False : ���鳻���� ������ �������� �����ʹ� ��ε� ������, �� ����κ��� �̹� �ε�� ���� ��ü���� �״�� �д�. ���� �� ����κ��� �߰������� �ҷ��� �� ����.
        // True: ����κ��� �ε�� ��� ��ü���� ���� ���ŵȴ�. ���� �� ���� �� ���µ��� �����ϴ� ��ü�� �ش� ������ �����ȴ�.
            bundle.Unload(false);
            AddLog("LoadFromWebProcess Complete");
            Debug.Log("LoadFromWebProcess Complete");
            if (zeroLevelMenu != null)
                zeroLevelMenu.ShowObject(inst);
        }
    }


}
