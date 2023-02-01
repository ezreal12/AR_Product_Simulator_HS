using UnityEngine;
using System.Collections;
using TMPro;
using System;
using UnityEngine.Networking;

public class OneLevelCanvus : MonoBehaviour
{


    public TestLogCompo testLog; // �׽�Ʈ �α׿� ������Ʈ
    public GameObject imageViewerGameObject;
    private ImageViewer imageViewer; // �������� ������ �̹��� ��¿� �װ�
    private ServerFileList fileList; // Json �������� �������� ������ ������ ���� ��ϵ�
    public ObjectSpawnManager spawnManager;
    // Start is called before the first frame update
    void Start()
    {
        AddLog("Start");
        imageViewer = imageViewerGameObject.GetComponent<ImageViewer>();
    }
    void OnEnable() {
        AddLog("OnEnable");
        if(imageViewer!=null)
            imageViewer.ResetImgs();
        GetFileList();
    }
    private void SetImageFromFiles() {
        AddLog("SetImageFromFiles");
        if (fileList == null) {
            AddLog("ERR! fileList is NULL SetImageFromFiles");
            return;
        }

        string[] imgList = fileList.images;
        string[] objList = fileList.files;
        for (int i=0;i< imgList.Length;i++) {
            StartCoroutine(GetServerData(imgList[i], objList[i], imgList.Length)); // idx�� 1���� ����
        }
       
    }
    private void GetFileList() {
        AddLog("GetFileList");
        StartCoroutine(Get("http://192.168.137.1:8080/obj/file_list.json", (request) =>
        {
            if (request.result == UnityWebRequest.Result.Success && request.responseCode == 200)
            {
                string r = request.downloadHandler.text;
                Debug.Log(r);
                fileList = JsonUtility.FromJson<ServerFileList>(r);
                SetImageFromFiles();
            }
            else
            {
                string err = "[Error]:" + request.responseCode + request.error;
                Debug.Log(err);
                AddLog(err);
            }
        }));
    }
    private void AddLog(string s) {
        if (testLog != null)
        {
            testLog.AddLog("[OneLevelCanvus] : " + s);
        }
    }
    // Get ����ϰ� ����� ��ȯ�ϱ�
    IEnumerator Get(string url, Action<UnityWebRequest> callback)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        callback(request);
    }

    IEnumerator GetServerData(string imgUrl,string objUrl,int arrSize)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imgUrl);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            if (imageViewer!=null) {
                //imageViewer.AddTexture(((DownloadHandlerTexture)www.downloadHandler).texture);
                ObjectItem item = new ObjectItem(((DownloadHandlerTexture)www.downloadHandler).texture, imgUrl, objUrl);
                imageViewer.AddItem(item);
                // ��� �̹��� �ε��� �Ϸ��������
                if (arrSize == imageViewer.GetCount()) {
                    imageViewerGameObject.SetActive(true);
                    imageViewer.Show();
                }
            }
            //img.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
    }

    public void LoadModelFromWeb(string url) {
        if (spawnManager == null)
            return;

        spawnManager.GetObejctFromWeb(url);

    }


    [Serializable]
    public class ServerFileList {
        public string[] images;
        public string[] files;
    }
}
