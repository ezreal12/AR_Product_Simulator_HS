using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class ImageViewer : MonoBehaviour
{
    public RawImage[] images;

    // idx�� 1���� ����
    public int SetTextureImg(Texture t,int idx) {
        if (idx > images.Length) {
            return -1;
        }
        images[idx-1].texture = t;
        return 0;
        
    }
}
