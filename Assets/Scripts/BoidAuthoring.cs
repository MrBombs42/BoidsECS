using System;
using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts
{
	[Serializable]
	public struct Boid : IComponentData
	{
	}


	public class BoidAuthoring : MonoBehaviour
	{
		class Baker : Baker<BoidAuthoring>
		{
			public override void Bake(BoidAuthoring authoring)
			{
				AddComponent<Boid>(GetEntity(TransformUsageFlags.Dynamic));
			}
		}
	}
}
