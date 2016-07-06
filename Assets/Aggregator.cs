using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class Aggregator : MonoBehaviour {
    private Bailey bailey;
    private BaileyBoid baileyBoid;

    public float collectionTime = 20f;
    private List<RelationPair> relations;

    private List<DataPoint> dataPoints;
    
    private class DataPoint
    {
        public float[] baileyData;
        public float[] boidBaileyData;
        public float time;
    }
    private class RelationPair
    {
        public string identifier
        {
            get; set;
        }
        public RelationTracker.AgentRelation baileyRelation { get; set; }
        public RelationTracker.AgentRelation boidBaileyRelation { get; set; }
    }
	// Use this for initialization
	void Start () {
        Debug.Log(Application.persistentDataPath);
        bailey = GetComponent<BaileyFramework>().bailey;
        baileyBoid = GetComponent<BaileyBoidFramework>().baileyBoid;
        InitRelations();
        StartCoroutine(DataCollector());
	}
	
    void InitRelations()
    {
        relations = new List<RelationPair>();
        foreach(var v in bailey.tracker.relations)
        {
            var match = baileyBoid.tracker.relations.Find(x => x.Contains(v.a, v.b));
            relations.Add(new RelationPair
            {
                baileyRelation = v,
                boidBaileyRelation = match,
                identifier = v.a.name + "-" + v.b.name
            });
        }
    }

	IEnumerator DataCollector()
    {
        dataPoints = new List<DataPoint>();
        float startTime = Time.time;
        float counter = 0f;
        while(Time.time - startTime < collectionTime)
        {
            float[] bData = new float[relations.Count];
            float[] bbData = new float[relations.Count];
            for (int i = 0; i < relations.Count; i++)
            {
                bData[i] = relations[i].baileyRelation.relation;
                bbData[i] = relations[i].boidBaileyRelation.relation;
            }
            dataPoints.Add(new DataPoint
            {
                baileyData = bData,
                boidBaileyData = bbData,
                time = counter
            });
            yield return new WaitForSeconds(0.5f);
            counter += 0.5f;
        }

        string agentConcat = "Time";
        foreach (var v in relations)
        {
            agentConcat += "," + v.identifier;
        }

        List<string> bDataArr = new List<string>();
        List<string> bbDataArr = new List<string>();
        bDataArr.Add(agentConcat);
        bbDataArr.Add(agentConcat);

        foreach(var d in dataPoints)
        {
            string bString =  d.time.ToString();
            string bbString = d.time.ToString();
            for (int i = 0; i < d.baileyData.Length; i++)
            {
                bString += ",";
                bString += d.baileyData[i];
            }
            for (int i = 0; i < d.boidBaileyData.Length; i++)
            {
                bbString += ",";
                bbString += d.boidBaileyData[i];
            }
            bDataArr.Add(bString);
            bbDataArr.Add(bbString);
        }

        File.WriteAllLines(Application.persistentDataPath + "/" + System.DateTime.Now.ToString("[MM-dd] hh-mm-ss") + " (bailey).csv", bDataArr.ToArray());
        File.WriteAllLines(Application.persistentDataPath + "/" + System.DateTime.Now.ToString("[MM-dd] hh-mm-ss") + " (baileyBoid).csv", bbDataArr.ToArray());
        Debug.Log("Aggregation Complete");
    }


}
