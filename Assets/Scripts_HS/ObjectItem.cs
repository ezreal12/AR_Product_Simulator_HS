using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 �������� ������ ������ 1���� ��� VO ����
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
