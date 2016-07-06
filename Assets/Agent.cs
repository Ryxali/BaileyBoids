using UnityEngine;
using System.Collections;
public class AgentPair
{
    public Agent a { get; set; }
    public Agent b { get; set; }
}

public class Agent : MonoBehaviour {

    private static float WALK_BOUNDS = 4f;
    private static float WALK_SPEED = 1f;
    private static int idCounter = 0;
    public AgentState state { get; private set; }

    public int id { get; private set; }
	// Use this for initialization
	void Start () {
        id = idCounter++;
        state = new AgentState(Random.Range(-1f, 1f));
        GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(Color.black, Color.blue, (state.opinion + 1f) / 2f);
        AgentProximityDetector.inst.Add(this);
        StartCoroutine(BehaviourCycle());
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    private IEnumerator BehaviourCycle()
    {
        while(true)
        {
            yield return StartCoroutine(WalkToPoint(new Vector3
            {
                x = Random.Range(-WALK_BOUNDS, WALK_BOUNDS),
                y = Random.Range(-WALK_BOUNDS, WALK_BOUNDS)
            }));
            yield return new WaitForSeconds(Random.Range(0.3f, 1f));
        }
        
    }


    private IEnumerator WalkToPoint(Vector3 point)
    {
        while(Vector3.Distance(transform.position, point) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, point, Time.deltaTime * WALK_SPEED);
            yield return null;
        }
    }
   

    public void InteractWith(Agent other)
    {
        Debug.Log(name + " talks to " + other.name);
        SendMessageUpwards("OnAgentInteract", new AgentPair
        {
            a = this,
            b = other
        });
    }
}
