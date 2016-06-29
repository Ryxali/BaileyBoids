using UnityEngine;
using System.Collections;

public class BaileyBoid : Bailey {

    public BaileyBoid(GameObject host) : base(host)
    {

    }

    private static readonly float RELATION_DELTA_MOD = 0.2f;
    public override void OnInteract(Agent a, Agent b)
    {
        base.OnInteract(a, b);
        float agreement = tracker.relations.Find(x => x.Contains(a, b)).relation * RELATION_DELTA_MOD;
        Boidy(a, b, agreement);
        Boidy(b, a, agreement);
    }

    private void Boidy(Agent agent, Agent affector, float agreement)
    {
        var affectorRelations = tracker.relations.FindAll(x => x.Contains(affector) && !x.Contains(agent));
        foreach(var relation in affectorRelations)
        {
            var agentRelation = tracker.relations.Find(x => x.Contains(agent) && (x.Contains(relation.a) || x.Contains(relation.b)) && !x.Contains(affector));
            agentRelation.relation = 
                Mathf.Clamp(
                    Mathf.MoveTowards(
                        agentRelation.relation, 
                        relation.relation, 
                        Mathf.Abs(relation.relation - agentRelation.relation) * agreement
                    ),
                    -1f,
                    1f
                );
        }
    }
}
