using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class ImageViewer : MonoBehaviour
{
    public RawImage[] images;
    private List<Texture> imgTextureList;
    private int nowIndex = 0;
    public ImageViewer() {
        imgTextureList = new List<Texture>();
    }

    public void AddTexture(Texture t) {
        imgTextureList.Add(t);
    }
    public void ResetImgs() {
        imgTextureList.Clear();
    }
    public void Show()
    {
        nowIndex = 0;
        DrawImage();
    }

    public int GetCount() {
        return imgTextureList.Count;
    }

    private void DrawImage() {
        /*
           øﬁ¬ ∫Œ≈Õ 0 1 2 / 0 1 2 3 4 
         */
        for (int i=0; i< images.Length; i++) {
            //  nowIndex + i;
            int ti = nowIndex + i;
            if (ti >= imgTextureList.Count)
            {
                ti = 0;
            }
            images[i].texture = imgTextureList[ti];
        }
    }

    public void Next() {
        if (nowIndex+ images.Length >= imgTextureList.Count) {
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

}
