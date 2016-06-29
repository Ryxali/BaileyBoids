using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bailey {

    protected RelationTracker tracker;
    private static readonly float AGREEMENT_DELTA_MOD = 0.2f;

    public Bailey(GameObject host)
    {
        tracker = new RelationTracker();
        tracker.Initialize(Object.FindObjectsOfType<Agent>(), host);
    }
    
    public virtual void OnInteract(Agent a, Agent b)
    {
        var relation = tracker.relations.Find(x => x.Contains(a, b));
        float agreement = 1f - Mathf.Abs(a.state.opinion - b.state.opinion);
        relation.relation += agreement * AGREEMENT_DELTA_MOD;
    }

    public void UpdateLineRenderers()
    {
        foreach(var relation in tracker.relations)
        {
            relation.renderer.SetPositions(new Vector3[]
            {
                relation.a.transform.position,
                relation.b.transform.position
            });
            Color col = Color.Lerp(Color.red, Color.green, (relation.relation + 1f) / 2f);
            relation.renderer.SetColors(col, col);
            
        }
    }
}
