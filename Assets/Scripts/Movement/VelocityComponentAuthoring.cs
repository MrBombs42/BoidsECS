using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts.Movement
{
	public class VelocityComponentAuthoring : MonoBehaviour
	{
		public float MaxSpeed;
		public float MaxForce;
		public float3 Steering;
		public float3 Velocity;

		class Baker : Baker<VelocityComponentAuthoring>
		{
			public override void Bake(VelocityComponentAuthoring authoring)
			{
				var entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent(entity, new VelocityComponent
				{
					MaxForce = authoring.MaxForce,
					MaxSpeed = authoring.MaxSpeed,
					Steering = authoring.Steering,
					Velocity = authoring.Velocity
				});
			}
		}
	}
}
