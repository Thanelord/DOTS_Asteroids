using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class MoveforwardSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float deltaTime = Time.DeltaTime;

        Entities.ForEach((ref PhysicsVelocity vel, in GoForward moveData) =>
        {
            Vector3 newDir = moveData.dir;
            newDir += moveData.dir * moveData.speed * deltaTime;

            vel.Linear.xyz = newDir;

        }).Run();

        return default;
    }
}
