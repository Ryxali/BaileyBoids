using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bailey {

    public RelationTracker tracker;
    private static readonly float AGREEMENT_DELTA_MOD = 0.2f;

    public Bailey(GameObject host)
    {
        tracker = new RelationTracker();
        tracker.Initialize(Object.FindObjectsOfType<Agent>(), host);
    }

    public virtual void OnInteract(Agent a, Agent b)
    {
        ModifyRelation( tracker.relations.Find(x => x.a == a && x.b == b));
        ModifyRelation(tracker.relations.Find(x => x.a == b && x.b == a));
    }

    private void ModifyRelation(RelationTracker.AgentRelation relation) {
        float agreement = 1f - Mathf.Abs(relation.a.state.opinion - relation.b.state.opinion);
        relation.relation += agreement * AGREEMENT_DELTA_MOD;
        relation.relation = Mathf.Clamp(relation.relation, -1f, 1f);
    }

    public void UpdateLineRenderers()
    {
        foreach(var relation in tracker.relations)
        {
            Vector3 dirF = (relation.b.transform.position - relation.a.transform.position).normalized;
            Vector3 dirR = Vector3.Cross(dirF, Vector3.forward);
            relation.renderer.SetPositions(new Vector3[]
            {
                relation.a.transform.position + dirR * 0.03f,
                relation.b.transform.position + dirR * 0.03f
            });
            Color col = Color.Lerp(Color.red, Color.green, (relation.relation + 1f) / 2f);
            relation.renderer.SetColors(col, col);
            
        }
    }
}
