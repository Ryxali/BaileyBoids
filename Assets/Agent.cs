using UnityEngine;
using System.Collections;
public class AgentPair
{
    public Agent a { get; set; }
    public Agent b { get; set; }
}

public class Agent : MonoBehaviour {

    private static float WALK_BOUNDS = 4f;
    private static float WALK_SPEED = 5f;
    private static int idCounter = 0;
    public AgentState state { get; private set; }

    public float agentState {  get { return state.opinion; } set { state.opinion = value; } }

    public int id { get; private set; }

    public Agent ()
    {
        id = idCounter++;
        
    }

	// Use this for initialization
	void Start () {
        state = new AgentState(Random.Range(-1f, 1f));
        name += " (" + ((decimal)state.opinion).ToString("F") + ")"; 
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
            //transform.rotation = Quaternion.LookRotation()
            transform.up = (point - transform.position).normalized;
            yield return null;
        }
    }
   

    public void InteractWith(Agent other)
    {
        //Debug.Log(name + " talks to " + other.name);
        SendMessageUpwards("OnAgentInteract", new AgentPair
        {
            a = this,
            b = other
        });
    }
}
