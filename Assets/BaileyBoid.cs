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
        //float agreement = tracker.relations.Find(x => x.Contains(a, b)).relation * RELATION_DELTA_MOD;
        //Boidy(a, b, agreement);
        //Boidy(b, a, agreement);
    }

    public void FixedUpdate()
    {
        var relations = tracker.relations;
        foreach(var relation in relations)
        {
            var agent = relation.a;
            var agentRelations = tracker.relations.FindAll(x => x.a != agent && x.b == relation.b);
            //Debug.Log("----");
            //Debug.Log("Agent " + agent.name + " -> " + relation.b);
            // Calculate average
            float average = 0f;
            float likedAverage = 0f;
            float nLiked = 0;
            float dislikedAverage = 0f;
            float nDisliked = 0;
            foreach(var rel in agentRelations)
            {
                //Debug.Log("Avg: " + rel.a.name + " -> " + rel.b.name);
                average += rel.relation;
                var relVal = tracker.relations.Find(y => y.a == agent && y.b == rel.a).relation;
                if (relVal > 0)
                {
                    //Debug.Log("\tConsidered Positive");
                    
                    likedAverage +=  rel.relation;
                    nLiked++;
                } else if( relVal < 0)
                {
                    //Debug.Log("\tConsidered Negative");
                    dislikedAverage += rel.relation;
                    nDisliked++;
                }
            }
            average /= agentRelations.Count;
            likedAverage /= (nLiked != 0f ? nLiked : 1f);
            dislikedAverage /= (nDisliked != 0f ? nDisliked : 1f);


            float force =
                average - relation.relation
                +(nLiked != 0f ? likedAverage - relation.relation : 0f)
                - ((nDisliked != 0f ? dislikedAverage - relation.relation : 0f))
                ;

            relation.relation += force * Time.fixedDeltaTime * RELATION_DELTA_MOD;
            //var likedRelations = agentRelations.FindAll(x => tracker.relations.Find(y => y.a == agent && y.b == x.a).relation >= 0);
            
        
            //var dislikedRelations = agentRelations.FindAll(x => !likedRelations.Contains(x));
        }
        Debug.Log("[][][][][][][][][][][]");
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
