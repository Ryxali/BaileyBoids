using UnityEngine;
using System.Collections;


public class BaileyBoidFramework : MonoBehaviour {
    public bool changeAgentColor = false;
    private BaileyBoid baileyBoid;
    private GameObject lineRendererContainer;
    // Use this for initialization
    void Start () {
        lineRendererContainer = new GameObject(name + " : LR Container");
        baileyBoid = new BaileyBoid(lineRendererContainer);
	}
	
   

	void OnAgentInteract(AgentPair agents)
    {
        baileyBoid.OnInteract(agents.a, agents.b);
        
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
