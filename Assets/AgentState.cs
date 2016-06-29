using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgentState {

	public float opinion { get; private set; }
    
    public AgentState(float opinion)
    {
        this.opinion = opinion;
    }
}
