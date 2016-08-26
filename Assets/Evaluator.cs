using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Evaluator : MonoBehaviour {
    private static int executions = 0;
    private static int[] agentsToSimulate = { 3, 5, 15 };
    private static int currentAgentToSimulate = 0;

    public static int AgentsToSimulate {  get { return agentsToSimulate[currentAgentToSimulate]; } }

    private struct ExecutionResult
    {
        public int nFailed, nTotal;

    }

    private struct ParallelExecutionResult
    {
        public ExecutionResult bailey;
        public ExecutionResult baileyBoid;
    }
    private List<ParallelExecutionResult> executionResults;
    private List<AggregationResult> aggregationResults;
    private float startTime;
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
        executionResults = new List<ParallelExecutionResult>();
        aggregationResults = new List<AggregationResult>();
        asyncOp = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    private AsyncOperation asyncOp = null;
	// Update is called once per frame
	void Update () {
        if (asyncOp != null && asyncOp.isDone)
        {
            asyncOp = null;
            startTime = Time.time;
        }
        if (Time.time - startTime > 60f && asyncOp == null)
        {
            var bailey = FindObjectOfType<BaileyFramework>().bailey.tracker;
            var baileyBoid = FindObjectOfType<BaileyBoidFramework>().baileyBoid.tracker;
            executionResults.Add(new ParallelExecutionResult
            {
                bailey = new ExecutionResult { nFailed = bailey.relations.FindAll(x => x.relation < 0).Count, nTotal = bailey.relations.Count },
                baileyBoid = new ExecutionResult { nFailed = baileyBoid.relations.FindAll(x => x.relation < 0).Count, nTotal = baileyBoid.relations.Count }
            });
            Debug.Log("EX: " + currentAgentToSimulate + "-" + executions + "(" + Time.time + ")");
            executions++;
            if (executions >= 5)
            {
                
                var baileyRes = ProduceAggregate(executionResults.ConvertAll<ExecutionResult>(x => x.bailey));
                var boidRes = ProduceAggregate(executionResults.ConvertAll<ExecutionResult>(x => x.baileyBoid));
                string[] strings = new[]
                {
                    "type,lowest,highest,median,low avg.,mid low avg,high avg,mid high avg,total",
                    "individual"+baileyRes.lowest + "," + baileyRes.highest + "," + baileyRes.median + "," + baileyRes.lowAvg + "," + baileyRes.midLowAvg + "," + baileyRes.midHighAvg + "," + baileyRes.highAvg + "," + baileyRes.total,
                    "collective"+boidRes.lowest + "," + boidRes.highest + "," + boidRes.median + "," + boidRes.lowAvg + "," + boidRes.midLowAvg + "," + boidRes.midHighAvg + "," + boidRes.highAvg + "," + boidRes.total
                };
                System.IO.File.WriteAllLines(Application.persistentDataPath + "/result (" + AgentsToSimulate + ") " + System.DateTime.Now.ToString("[MM-dd] hh-mm-ss")+".csv", strings);
                // TODO store total data and clear shit
                /*var baileyStrings = executionResults.ConvertAll<string>(x => (x.bailey.nTotal - x.bailey.nFailed) + ", " + x.bailey.nFailed + ", " + x.bailey.nTotal + ", " + ((float)x.bailey.nFailed / (float)x.bailey.nTotal));
                baileyStrings.Insert(0, "positive, negative, total, negative (%)");
                var baileyBoidStrings = executionResults.ConvertAll<string>(x => (x.baileyBoid.nTotal - x.baileyBoid.nFailed) + ", " + x.baileyBoid.nFailed + ", " + x.baileyBoid.nTotal + ", " + ((float)x.baileyBoid.nFailed / (float)x.baileyBoid.nTotal));
                baileyBoidStrings.Insert(0, "positive, negative, total, negative (%)");

                System.IO.File.WriteAllLines(Application.persistentDataPath + "/bailey (" + AgentsToSimulate + ") " + System.DateTime.Now.ToString("[MM-dd] hh-mm-ss"), baileyStrings.ToArray());
                System.IO.File.WriteAllLines(Application.persistentDataPath + "/baileyBoid (" + AgentsToSimulate + ") " + System.DateTime.Now.ToString("[MM-dd] hh-mm-ss"), baileyBoidStrings.ToArray());*/
                executionResults.Clear();
                executions = 0;
                currentAgentToSimulate++;
                if (currentAgentToSimulate >= agentsToSimulate.Length)
                {
                    currentAgentToSimulate = 0;
                    Application.Quit();
                } else
                {
                    asyncOp = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1, UnityEngine.SceneManagement.LoadSceneMode.Single);
                }
            } else
            {
                asyncOp = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1, UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
        }
	}

    private class AggregationResult
    {
        public int total, median, lowest, highest;
        public float lowAvg, midLowAvg, midHighAvg, highAvg;
    }

    private AggregationResult ProduceAggregate(List<ExecutionResult> results)
    {
        int size = results[0].nTotal;
        int stepSize = size / 4;
        results.Sort((a, b) => a.nFailed.CompareTo(b.nFailed));
        var median = results[results.Count / 2].nFailed;
        var lowest = results[0].nFailed;
        var highest = results[results.Count - 1].nFailed;
        float lowAverage = 0f;
        int i = 0;
        int nSum = 0;
        for(; i < results.Count / 4; i++)
        {
            nSum++;
            lowAverage += results[i].nFailed;
        }
        lowAverage /= nSum;

        nSum = 0;
        float midLowAverage = 0f;
        for (; i < results.Count / 4 * 2; i++)
        {
            nSum++;
            midLowAverage += results[i].nFailed;
        }
        midLowAverage /= nSum;

        nSum = 0;
        float midHighAverage = 0f;
        for (; i < results.Count / 4 * 3; i++)
        {
            nSum++;
            midHighAverage += results[i].nFailed;
        }
        midHighAverage /= nSum;

        nSum = 0;
        float highAverage = 0f;
        for (; i < results.Count; i++)
        {
            nSum++;
            highAverage += results[i].nFailed;
        }
        highAverage /= nSum;
        
        return new AggregationResult
        {
            total = executions,
            lowest = lowest,
            median = median,
            highest = highest,
            lowAvg = lowAverage,
            midLowAvg = midLowAverage,
            midHighAvg = midHighAverage,
            highAvg = highAverage

        };
    }

    
}
