using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Scripts
{
	public class EntityCreator : MonoBehaviour
	{

		[SerializeField] public Mesh theMesh;
		[SerializeField] public Material theMaterial;

		// Start is called before the first frame update
		void Start()
		{
			EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

			EntityArchetype eArch = entityManager.CreateArchetype(
					typeof(RenderMesh),
					typeof(LocalTransform),
					typeof(LocalToWorld),
					typeof(RenderBounds)
				);

			NativeArray<Entity> eArray = new NativeArray<Entity>(10, Allocator.Temp);

			entityManager.CreateEntity(eArch, eArray);

			foreach (Entity ent in eArray)
			{
				entityManager.SetComponentData(ent, new LocalTransform() { Position = new Vector3(0f, 0f, 0f) });
				entityManager.SetSharedComponentManaged(ent, new RenderMesh
				{
					mesh = theMesh,
					material = theMaterial
				});
			}
			eArray.Dispose();
		}

	}
}
