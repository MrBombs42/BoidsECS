using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Assets.Scripts.Movement
{
	[Serializable]
	[GenerateAuthoringComponent]
	public struct VelocityComponent : IComponentData
	{
		public float MaxSpeed;
		public float MaxForce;
		public float3 Steering;
		public float3 Velocity;
	}
}
