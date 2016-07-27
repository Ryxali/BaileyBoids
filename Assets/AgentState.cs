using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgentState {

	public float opinion { get; set; }
    
    public AgentState(float opinion)
    {
        this.opinion = opinion;
    }
}
