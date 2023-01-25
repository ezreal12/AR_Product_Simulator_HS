using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 서버에서 가져온 데이터 1개를 담는 VO 역할
 */
public class ObjectItem 
{
    public Texture texture;
    public string ImgSrc;
    public string ObjSrc;

    public ObjectItem(Texture texture, string ImgSrc, string ObjSrc) {
        this.texture = texture;
        this.ImgSrc = ImgSrc;
        this.ObjSrc = ObjSrc;
    }
}
