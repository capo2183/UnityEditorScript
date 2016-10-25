using UnityEngine;
using System.Collections;

[System.Serializable]
public class LayerData
{
    public LayerData(GameObject _go, string _layerName, int _layerOrder)
    {
        go = _go;
        LayerName = _layerName;
        LayerOrder = _layerOrder;
        AddSpaceLayer = 1;
    }

    public GameObject go;
    public string LayerName;
    public int LayerOrder;
    public int AddSpaceLayer;
    public int tempLayerOrderBeforeChange;
}
