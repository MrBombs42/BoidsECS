﻿using Assets.Scripts;
using Assets.Scripts.Movement;
using Assets.Scripts.Wander;
using System.Diagnostics;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class BoidSpawnSystem : SystemBase
{
	//This will be our query for Asteroids
	private EntityQuery m_AsteroidQuery;

	//We will use the BeginSimulationEntityCommandBufferSystem for our structural changes
	private BeginSimulationEntityCommandBufferSystem m_BeginSimECB;

	//This will be our query to find GameSettingsComponent data to know how many and where to spawn Asteroids
	private EntityQuery m_GameSettingsQuery;

	//This will save our Asteroid prefab to be used to spawn Asteroids
	private Entity m_Prefab;

	protected override void OnCreate()
	{
		//This is an EntityQuery for our Asteroids, they must have an AsteroidTag
		m_AsteroidQuery = GetEntityQuery(ComponentType.ReadWrite<Boid>());

		//This will grab the BeginSimulationEntityCommandBuffer system to be used in OnUpdate
		m_BeginSimECB = World.GetOrCreateSystemManaged<BeginSimulationEntityCommandBufferSystem>();

		//This is an EntityQuery for the GameSettingsComponent which will drive how many Asteroids we spawn
		m_GameSettingsQuery = GetEntityQuery(ComponentType.ReadWrite<GameSettingsComponent>());

		//This says "do not go to the OnUpdate method until an entity exists that meets this query"
		//We are using GameObjectConversion to create our GameSettingsComponent so we need to make sure
		//The conversion process is complete before continuing
		RequireForUpdate(m_GameSettingsQuery);
	}

	protected override void OnUpdate()
	{
		//Here we set the prefab we will use
		if (m_Prefab == Entity.Null)
		{
			//We grab the converted PrefabCollection Entity's AsteroidAuthoringComponent
			//and set m_Prefab to its Prefab value
			m_Prefab = GetSingleton<BoidsReferenceComponent>().Prefab;

			//we must "return" after setting this prefab because if we were to continue into the Job
			//we would run into errors because the variable was JUST set (ECS funny business)
			//comment out return and see the error
			return;
		}

		//Because of how ECS works we must declare local variables that will be used within the job
		//You cannot "GetSingleton<GameSettingsComponent>()" from within the job, must be declared outside
		var settings = GetSingleton<GameSettingsComponent>();

		//Here we create our commandBuffer where we will "record" our structural changes (creating an Asteroid)
		var commandBuffer = m_BeginSimECB.CreateCommandBuffer();

		//This provides the current amount of Asteroids in the EntityQuery
		var count = m_AsteroidQuery.CalculateEntityCountWithoutFiltering();

		//We must declare our prefab as a local variable (ECS funny business)
		var asteroidPrefab = m_Prefab;

		//We will use this to generate random positions
		var rand = new Unity.Mathematics.Random((uint)Stopwatch.GetTimestamp());
		Entities
			//.WithReadOnly(settings)
			.ForEach(() =>
			{
				for (int i = count; i < settings.NumberOfBoids; ++i)
				{
					// this is how much within perimeter asteroids start
					var padding = 0.1f;

					// we are going to have the asteroids start on the perimeter of the level
					// choose the x, y, z coordinate of perimeter
					// so the x value must be from negative levelWidth/2 to positive levelWidth/2 (within padding)
					var xPosition = rand.NextFloat(-1f * ((settings.LevelWidth) / 2 - padding), (settings.LevelWidth) / 2 - padding);
					// so the y value must be from negative levelHeight/2 to positive levelHeight/2 (within padding)
					var yPosition = rand.NextFloat(-1f * ((settings.LevelHeight) / 2 - padding), (settings.LevelHeight) / 2 - padding);
					// so the z value must be from negative levelDepth/2 to positive levelDepth/2 (within padding)
					var zPosition = rand.NextFloat(-1f * ((settings.LevelDepth) / 2 - padding), (settings.LevelDepth) / 2 - padding);

					//We now have xPosition, yPostiion, zPosition in the necessary range
					//With "chooseFace" we will decide which face of the cube the Asteroid will spawn on
					var chooseFace = rand.NextFloat(0, 6);

					//Based on what face was chosen, we x, y or z to a perimeter value
					//(not important to learn ECS, just a way to make an interesting prespawned shape)
					if (chooseFace < 1) { xPosition = -1 * ((settings.LevelWidth) / 2 - padding); }
					else if (chooseFace < 2) { xPosition = (settings.LevelWidth) / 2 - padding; }
					else if (chooseFace < 3) { yPosition = -1 * ((settings.LevelHeight) / 2 - padding); }
					else if (chooseFace < 4) { yPosition = (settings.LevelHeight) / 2 - padding; }
					else if (chooseFace < 5) { zPosition = -1 * ((settings.LevelDepth) / 2 - padding); }
					else if (chooseFace < 6) { zPosition = (settings.LevelDepth) / 2 - padding; }

					//we then create a new translation component with the randomly generated x, y, and z values
					var pos = new LocalTransform() { Position = new float3(xPosition, yPosition, zPosition) };

					//on our command buffer we record creating an entity from our Asteroid prefab
					var e = commandBuffer.Instantiate(asteroidPrefab);

					//we then set the Translation component of the Asteroid prefab equal to our new translation component
					commandBuffer.SetComponent(e, pos);

					var randomVel = new Vector3(rand.NextFloat(-1f, 1f), rand.NextFloat(-1f, 1f), rand.NextFloat(-1f, 1f));
					//next we normalize it so it has a magnitude of 1
					randomVel.Normalize();
					//now we set the magnitude equal to the game settings
					randomVel = randomVel * settings.AsteroidVelocity;
					//here we create a new VelocityComponent with the velocity data
					var vel = new VelocityComponent { Velocity = randomVel, MaxSpeed = 100, MaxForce = 100 };
					//now we set the velocity component in our asteroid prefab
					commandBuffer.SetComponent(e, vel);

					commandBuffer.AddComponent(e, new WanderComponent());
				}
			}).Schedule();

		//This will add our dependency to be played back on the BeginSimulationEntityCommandBuffer
		m_BeginSimECB.AddJobHandleForProducer(Dependency);
	}
}