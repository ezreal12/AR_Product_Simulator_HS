using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TestLogCompo : MonoBehaviour
{
    public TextMeshProUGUI DebugMesh;
    public int maxLogCnt = 6; // 최대 로그 제한 , 로그는 6줄까지만 출력
    public void AddLog(string log)
    {
        log = log.Replace("\n"," ");
        if (DebugMesh != null)
        {
            string oldText = DebugMesh.text;
            if (oldText.Length < 1)
            {
                DebugMesh.SetText(log);
            }
            else
            {
                string[] oldTestSp = oldText.Split("\n");
                string s = "";
                for (int i = 0; i < oldTestSp.Length; i++)
                {
                    if (i == 0 && (oldTestSp.Length > maxLogCnt))
                    {
                        continue;
                    }
                    s += oldTestSp[i] + "\n";
                }
                s += log;
                DebugMesh.SetText(s);
            }

        }
    }
}
