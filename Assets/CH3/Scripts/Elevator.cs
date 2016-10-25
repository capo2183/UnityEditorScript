using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour {

    public float Radius;
    public Vector2 CenterPos;

    private float _time;

	// Use this for initialization
	void Start () {
        _time = 0.0f;

    }
	
	// Update is called once per frame
	void Update () {
        _time += Time.deltaTime;

        float x = Radius * Mathf.Sin(_time);
        this.gameObject.transform.position = CenterPos + new Vector2(x, 0.0f);
    }
}
