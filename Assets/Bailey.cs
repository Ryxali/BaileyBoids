using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bailey {

    private RelationTracker tracker;
    private static readonly float RELATION_DELTA_MOD = 0.2f;

    public Bailey()
    {
        tracker = new RelationTracker();
        tracker.Initialize(Object.FindObjectsOfType<Agent>());
    }
    
    public virtual void OnInteract(Agent a, Agent b)
    {
        var relation = tracker.relations.Find(x => x.Contains(a, b));
        float agreement = 1f - Mathf.Abs(a.state.opinion - b.state.opinion);
        relation.relation += agreement * RELATION_DELTA_MOD;
    }
}
