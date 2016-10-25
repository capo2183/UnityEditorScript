using UnityEngine;
using System.Collections;

//如果此類別有被拿來做成 Public Array 或 Public 變數
//[System.Serializable] 會叫 Unity 去對此類別做序列化
[System.Serializable]
public class BrickType
{
    public string Name;
    public Color HitColor;
}
