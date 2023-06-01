using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

namespace DefaultNamespace
{
	[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public partial class InteractionSystem : SystemBase
    {
        private const float CastRadius = 0.3f;
        private const float CastMaxDistance = 4f;

        private BuildPhysicsWorld _buildPhysicsWorldSystem;
        private CollisionFilter _collisionFilter;
        private EntityQuery _playerEntityQuery;

        protected override void OnCreate()
        {
            _buildPhysicsWorldSystem = World.GetOrCreateSystem<BuildPhysicsWorld>();
            _collisionFilter = GetCollisionFilterByLayerName("Interaction");

            // // Player.
            // _playerEntityQuery = GetEntityQuery(ComponentType.ReadOnly<InteractorComponent>());
            //
            // RequireForUpdate(_playerEntityQuery);

            EntityManager.CreateEntity(typeof(InteractionTargetCast));

            if (!HasSingleton<InteractionTargetCast>())
            {
                Debug.LogError($"{nameof(InteractionTargetCast)} is not a singleton.");
            }
        }

        protected override void OnUpdate()
        {
            var collisionWorld = _buildPhysicsWorldSystem.PhysicsWorld.CollisionWorld;

		// Camera.
		 var cameraPlayerEntity = GetSingletonEntity<CameraPlayer>();
		  var cameraPlayer = EntityManager.GetComponentObject<CameraPlayer>(cameraPlayerEntity);
		  var cameraPosition = EntityManager.GetComponentData<LocalToWorld>(cameraPlayerEntity).Position;
		  var cameraForward = (float3)cameraPlayer.GameObject.transform.forward;

		// Player.
		var playerEntity = GetSingletonEntity<InteractorComponent>();
		var playerPosition = EntityManager.GetComponentData<LocalToWorld>(playerEntity).Position;

		//var ignoreList = new NativeList<Entity>(Allocator.Temp);
		var allHits = new NativeList<Unity.Physics.RaycastHit>(Allocator.Temp);

		// Approximate Camera/Player(Head) distance.
		var headDistance = math.distance(playerPosition, cameraPosition);

		var fromPosition = cameraPosition + (cameraForward * headDistance);
		var toPosition = fromPosition + (cameraForward * CastMaxDistance);

		//ignoreList.Add(playerEntity);

		var raycast = new RaycastInput()
		{
			Start = fromPosition,
			End = toPosition,
			Filter = _collisionFilter
		};

		collisionWorld.CastRay(raycast, ref allHits);

		Entity hitEntity = Entity.Null;
		var min = math.INFINITY;

		foreach (var hit in allHits)
		{
			if (hit.Entity == playerEntity)
			{
				continue;
			}

			float hitDistance = math.distance(fromPosition, hit.Position);
			if (hitDistance < min)
			{
				min = hitDistance;
				hitEntity = hit.Entity;
			}
		}

		SetSingleton(new InteractionTargetCast { Value = hitEntity });

		allHits.Dispose();
        }




        public static CollisionFilter GetCollisionFilterByLayer(int layer)
        {
            var filter = CollisionFilter.Default;

            if (layer > 0)
            {
                var colliderLayerMask = 1u << layer;
                filter.BelongsTo = colliderLayerMask;
                filter.CollidesWith = colliderLayerMask;
            }

            return filter;
        }

        public static CollisionFilter GetCollisionFilterByLayerName(string layerName)
        {
            return GetCollisionFilterByLayer(LayerMask.NameToLayer(layerName));
        }
    }


}