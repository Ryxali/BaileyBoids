using UnityEngine;
using System.Collections;

public class BaileyFramework : MonoBehaviour {
    public bool changeAgentColor = false;
    public Bailey bailey;
    private GameObject lineRendererContainer;
	// Use this for initialization
	void Start () {
        lineRendererContainer = new GameObject(name + " : LR Container");
        bailey = new Bailey(lineRendererContainer);
	}

    void OnAgentInteract(AgentPair agents)
    {
        bailey.OnInteract(agents.a, agents.b);
        
        
    }
    
    void LateUpdate()
    {
        bailey.UpdateLineRenderers();
        foreach (var rend in lineRendererContainer.GetComponentsInChildren<LineRenderer>())
        {
            rend.enabled = changeAgentColor;
        }
    }


}
