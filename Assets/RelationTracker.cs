using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RelationTracker {

    public List<AgentRelation> relations;

    public float minRelation = 0.3f;

	public class AgentRelation
    {
        public Agent a { get; set; }
        public Agent b { get; set; }
        public float relation { get; set; }
        public LineRenderer renderer { get; set; }
        public bool Contains(Agent agent)
        {
            return a == agent || b == agent;
        }
        public bool Contains(Agent agent0, Agent agent1)
        {
            return Contains(agent0) && Contains(agent1);
        }
    }

    public void Initialize(Agent[] agents, GameObject host)
    {
        relations = new List<AgentRelation>();
        for(int i = 0; i < agents.Length; i++)
        {
            for (int j = 0; j < agents.Length; j++)
            {
                if (i == j) continue;
                GameObject go = new GameObject();
                go.transform.parent = host.transform;
                var rend = go.AddComponent<LineRenderer>();
                rend.SetWidth(0.05f, 0.05f);
                rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                rend.receiveShadows = false;
                rend.material = new Material(Shader.Find("Sprites/Default"));
                relations.Add(new AgentRelation
                {
                    a = agents[i],
                    b = agents[j],
                    relation = Random.Range(minRelation, 1f),
                    renderer = rend
                });
            }
        }
    }
}
