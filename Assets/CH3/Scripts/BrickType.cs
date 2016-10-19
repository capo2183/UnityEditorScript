using UnityEngine;
using System.Collections;

//[System.Serializable] tells unity to serialize this class if 
//it's used in a public array or as a public variable in a component
[System.Serializable]
public class BrickType
{
    public string Name;
    public Color HitColor;
}
