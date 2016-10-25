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

    void OnDrawGizmosSelected()
    {
        float x = Radius * Mathf.Sin(PreviewTime.Time);
        Vector3 position = CenterPos + new Vector2(x, 0.0f);

        Gizmos.color = new Color(1f, 1f, 0f, 1f);
        Gizmos.DrawWireCube(position, new Vector3(1.0f, 0.2f, 1.0f));

        Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
        Gizmos.DrawCube(position, new Vector3(1.0f, 0.2f, 1.0f));
    }
}
