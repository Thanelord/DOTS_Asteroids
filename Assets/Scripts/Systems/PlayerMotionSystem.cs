using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;


[AlwaysSynchronizeSystem]
public class MotionSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;
        float inputTurn = Input.GetAxis("Horizontal");

        Entities.ForEach((ref Translation translation, ref Rotation rotation, ref PhysicsVelocity vel, in PlayerMovement moveData) =>
        {
            if (Input.GetKey(KeyCode.M)) {
                Vector3 newDir = math.mul(rotation.Value, new Vector3(0, 3, 0));
                vel.Linear.xyz = newDir;
            }

            float newRot = vel.Angular.z;
            newRot -= inputTurn * moveData.rotation * deltaTime;
            vel.Angular.z = newRot;

        }).Run();

        return default;
    }
}

