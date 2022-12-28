using Unity.Entities;
using Unity.Transforms;
using Assets.Scripts.Movement;
using UnityEngine;

public partial class MovementSystem : SystemBase
{

    protected override void OnUpdate()
    {
        var deltaTime = UnityEngine.Time.deltaTime;

        Entities
        .ForEach((ref Translation position, ref VelocityComponent velocity) =>
        {
            Vector3 vel = velocity.Velocity;
            var steering = Vector3.ClampMagnitude(velocity.Steering, velocity.MaxForce);
            vel = Vector3.ClampMagnitude(vel + steering, velocity.MaxSpeed);
            velocity.Velocity = vel;

            position.Value += velocity.Velocity * deltaTime;
        }).ScheduleParallel();
    }
}
