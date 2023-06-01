using Unity.Entities;
using UnityEngine;

public struct BoidsReferenceComponent : IComponentData
{
	public Entity Prefab;
}

public class BoidsReferenceAuthoring : MonoBehaviour
{
	public GameObject Prefab;

	class Baker : Baker<BoidsReferenceAuthoring>
	{
		public override void Bake(BoidsReferenceAuthoring referenceAuthoring)
		{
			var entityPrefab = GetEntity(referenceAuthoring.Prefab, TransformUsageFlags.Dynamic);

			var entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent(entity, new BoidsReferenceComponent
			{
				Prefab = entityPrefab
			});
		}
	}
}

