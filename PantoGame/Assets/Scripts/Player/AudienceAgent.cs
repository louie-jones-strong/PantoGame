using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AudienceAgent : Agent
{
	void Update()
    {
		var rangeSize = 16;
		
		if (NavMeshAgent.remainingDistance <= NavMeshAgent.stoppingDistance + 1.5f)
		{
			bool foundRoute = false;
			while (!foundRoute)
			{
				var target = new Vector3(Random.Range(-rangeSize, rangeSize), 0, Random.Range(-rangeSize, rangeSize));
				foundRoute = NavMeshAgent.SetDestination(target);
			}
		}
		UpdateVisuals();
    }
}
