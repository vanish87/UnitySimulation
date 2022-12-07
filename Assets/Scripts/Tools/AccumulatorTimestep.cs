using System;
using Simulation.Attributes;
using Unity.Mathematics;
using UnityEngine;

namespace Simulation.Tool
{
	[System.Serializable]
	public class AccumulatorTimestep
	{
		public enum State
		{
			DeltaTime,//use delta time for update
			PreferredTime,// use preferredTimestep
			PreferredTimeOverTime,// use preferredTimestep but can not catch real world time
			PreferredTimeUnderTime,// use preferredTimestep but too fast to exceed real world time
			
		}
		protected Action<float> updateActions;
		[SerializeField] protected float accumulator = 0f;
		[SerializeField] protected float preferredTimestep = 1 / 60f;
		[SerializeField] protected int maxIteration = 32;
		[SerializeField] protected const float MaxAccumulationTime = 1f;
		[SerializeField] protected bool logWarning = false;
		[SerializeField] protected bool forcePreferredTimestep = false;
		[SerializeField] protected bool forceIteration = false;
		[SerializeField, DisableEdit] protected State currentState = State.DeltaTime;
		public AccumulatorTimestep(float preferredTimestep = 1 / 60f, int maxIteration = 32, bool forcePreferredTimestep = false, bool forceIteration = false, bool logWarning = false)
        {
            this.preferredTimestep = preferredTimestep;
            this.maxIteration = maxIteration;
            this.accumulator = 0;
			this.forcePreferredTimestep = forcePreferredTimestep;
			this.forceIteration = forceIteration;

			this.logWarning = logWarning;
        }
		public void Update(float preferredTimestep, int maxIteration)
		{
            this.preferredTimestep = preferredTimestep;
            this.maxIteration = maxIteration;
            this.Update();
        }
		public void Update()
		{
			this.accumulator += Time.deltaTime;
            var iteration = 0;
			while ((this.accumulator >= this.preferredTimestep || this.forceIteration) && iteration < this.maxIteration)
			{
				var dt = this.forceIteration ? (this.forcePreferredTimestep ? this.preferredTimestep : math.min(this.preferredTimestep, Time.deltaTime)) : this.preferredTimestep;
				this.updateActions?.Invoke(dt);
				this.accumulator -= dt;
				iteration++;

				this.currentState = this.accumulator < 0 ? State.PreferredTimeUnderTime : State.PreferredTime;
			}
			if(iteration == 0) 
			{
				var dt = this.forcePreferredTimestep ? this.preferredTimestep : Time.deltaTime;
				this.updateActions?.Invoke(dt);
				this.accumulator -= dt;
				this.accumulator = math.clamp(this.accumulator, 0, MaxAccumulationTime);

				this.currentState = State.DeltaTime;
			}

			if (iteration + 1 > this.maxIteration)
            {
				if (this.logWarning) Debug.LogWarning("Max Iteration reached " + iteration);
                if(this.accumulator > MaxAccumulationTime)
				{
					if (this.logWarning) Debug.LogWarning("Reset accumulator");
					this.accumulator = 0;
				}
				this.currentState = State.PreferredTimeOverTime;
            }
		}
		public void OnUpdate(Action<float> update)
		{
			this.updateActions -= update;
			this.updateActions += update;
		}
	}
}
