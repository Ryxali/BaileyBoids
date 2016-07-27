using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
    public int nAgents;
    public Sprite sprite;
    public float size;
	// Use this for initialization
	void Start () {
        for (int i = 0; i < nAgents; i++)
        {
            var obj = new GameObject("Agent " + i);
            obj.AddComponent<Agent>();
            obj.transform.parent = transform;

            var spriteObj = new GameObject("Sprite");
            spriteObj.AddComponent<SpriteRenderer>().sprite = sprite;
            spriteObj.transform.parent = obj.transform;
            spriteObj.transform.localPosition = Vector3.zero;
            spriteObj.transform.localRotation = Quaternion.identity;

            obj.transform.position = new Vector3
            {
                x = Random.Range(-size, size),
                y = Random.Range(-size, size)
            };
            obj.transform.localRotation = Quaternion.identity;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
