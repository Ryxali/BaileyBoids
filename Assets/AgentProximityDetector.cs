using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgentProximityDetector : MonoBehaviour {

    private static AgentProximityDetector _inst;
    public static AgentProximityDetector inst { get { if (_inst == null) Debug.LogError("No instance of singleton exists"); return _inst; } }
    private List<Agent> agents;
    private List<AgentDetection> collisions = new List<AgentDetection>();

    private class AgentDetection
    {
        public Agent a { get; set; }
        public Agent b { get; set; }

        public bool Contains(Agent agent)
        {
            return a == agent || b == agent;
        }
    }

    void Start()
    {
        if (_inst != null) Debug.LogWarning("Multiple instances of singleton exists!");
        _inst = this;
        agents = new List<Agent>();
    }


    void Update()
    {
        var coll = new List<AgentDetection>();
        foreach(var agent in agents)
        {
            var colliding = agents.FindAll(x => x != agent && Vector3.Distance(agent.transform.position, x.transform.position) < 5f);
            var newCollisions = colliding.FindAll(x => !coll.Exists(z => z.Contains(x) && z.Contains(agent)));
            foreach (var val in newCollisions)
            {
                coll.Add(new AgentDetection
                {
                    a = agent,
                    b = val
                });
            }
        }
        var enters = coll.FindAll(x => !collisions.Exists(z => z.Contains(x.a) && z.Contains(x.b)));
        foreach(var v in enters)
        {
            v.a.InteractWith(v.b);
        }
        collisions = coll;
    }


    public void Add(Agent agent)
    {
        agents.Add( agent);
    }

}
