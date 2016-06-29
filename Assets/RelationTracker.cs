using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RelationTracker {

    public List<AgentRelation> relations;

	public class AgentRelation
    {
        public Agent a { get; set; }
        public Agent b { get; set; }
        public float relation { get; set; }
        public bool Contains(Agent agent)
        {
            return a == agent || b == agent;
        }
        public bool Contains(Agent agent0, Agent agent1)
        {
            return Contains(agent0) && Contains(agent1);
        }
    }

    public void Initialize(Agent[] agents)
    {
        relations = new List<AgentRelation>();
        for(int i = 0; i < agents.Length-1; i++)
        {
            for (int j = i+1; j < agents.Length; j++)
            {
                relations.Add(new AgentRelation
                {
                    a = agents[i],
                    b = agents[j],
                    relation = Random.Range(-1f, 1f)
                });
            }
        }
    }
}
