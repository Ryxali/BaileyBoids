using UnityEngine;
using System.Collections;


public class BaileyBoidFramework : MonoBehaviour {
    public bool changeAgentColor = false;
    public BaileyBoid baileyBoid;
    private GameObject lineRendererContainer;
    // Use this for initialization
    void Start () {
        
	}
	
    public void Initialize(RelationTracker tracker)
    {
        lineRendererContainer = new GameObject(name + " : LR Container");
        baileyBoid = new BaileyBoid(lineRendererContainer);
        baileyBoid.tracker.Initialize(tracker, lineRendererContainer);
    }
   

	void OnAgentInteract(AgentPair agents)
    {
        baileyBoid.OnInteract(agents.a, agents.b);
        
    }

    void FixedUpdate()
    {
        baileyBoid.FixedUpdate();
    }

    void LateUpdate()
    {
        baileyBoid.UpdateLineRenderers();
        foreach (var rend in lineRendererContainer.GetComponentsInChildren<LineRenderer>())
        {
            rend.enabled = changeAgentColor;
        }
    }
}
