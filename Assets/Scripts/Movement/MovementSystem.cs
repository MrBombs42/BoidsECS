using Unity.Entities;
using Unity.Transforms;
using Assets.Scripts.Movement;
using UnityEngine;

public partial class MovementSystem : SystemBase
{
//FixedGroup
    protected override void OnUpdate()
    {
        var deltaTime = UnityEngine.Time.deltaTime;

        Entities
        .ForEach((ref Translation position, ref VelocityComponent velocity) =>
        {
            Vector3 vel = velocity.Velocity;
            var steering = Vector3.ClampMagnitude(velocity.Steering, velocity.MaxForce);
            Debug.DrawRay(position.Value, steering, Color.red, 2);
            // mass can be added steering  = steering / mass
            vel = Vector3.ClampMagnitude(vel + steering, velocity.MaxSpeed);
            velocity.Velocity = vel;
            Debug.DrawRay(position.Value, vel, Color.blue, 2);
            position.Value += velocity.Velocity * deltaTime;
        }).ScheduleParallel();
    }
}
