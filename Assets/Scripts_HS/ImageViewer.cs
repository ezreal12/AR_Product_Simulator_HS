using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class ImageViewer : MonoBehaviour
{
    public RawImage[] images;
    private List<ObjectItem> itemList;
    private int nowIndex = 0; // nowIndex�� 0���� RawImageUI�� ���� -1������ 
    public TestLogCompo testLog; // �׽�Ʈ �α׿� ������Ʈ
    public OneLevelCanvus parentMenu;

    public ImageViewer() {
        itemList = new List<ObjectItem>();
    }

    public void AddItem(ObjectItem o) {
        itemList.Add(o);
    }
    public void ResetImgs() {
        itemList.Clear();
    }
    public void Show()
    {
        nowIndex = 0;
        DrawImage();
    }

    public int GetCount() {
        return itemList.Count;
    }

    private void DrawImage() {
        /*
           ���ʺ��� 0 1 2 
         */
        for (int i=0; i< images.Length; i++) {
            //  nowIndex + i;
            int ti = nowIndex + i;
            if (ti >= itemList.Count)
            {
                ti = 0;
            }
            //images[i].texture = imgTextureList[ti];
            images[i].texture = itemList[ti].texture;
        }
    }

    public void Next() {
        if (nowIndex+ images.Length >= itemList.Count) {
            return;
        }
        nowIndex++;
        DrawImage();
    }

    public void Re()
    {
        if (nowIndex <= 0) {
            return;
        }
        nowIndex--;
        DrawImage();
    }

    // idx�� 1���� 3���� 3�� 
    public void OnClickImg(int idx) {
        string objUrl = "";
        switch (idx) {
            case 1: objUrl = itemList[nowIndex].ObjSrc; break;
            case 2: objUrl = itemList[nowIndex + 1].ObjSrc; break;
            case 3: objUrl = itemList[nowIndex + 2].ObjSrc; break;
            default: break;
        }
        if (parentMenu != null)
            parentMenu.LoadModelFromWeb(objUrl);

    }

}
