using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;


[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class CollisionSystem : JobComponentSystem
{
    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;
    protected override void OnCreate()
    {
        base.OnCreate();
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    [BurstCompile]
    struct CollisionSystemJob : ICollisionEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<RockColiderTag> rockColiderGroup;
        [ReadOnly] public ComponentDataFromEntity<LaserColiderTag> laserColiderGroup;
        [ReadOnly] public ComponentDataFromEntity<ShipColiderTag> playerColiderGroup;
        public ComponentDataFromEntity<Destroy> destroyGroup;

        public void Execute(CollisionEvent collisionEvent)
        {
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;

            bool aIsLaser = laserColiderGroup.HasComponent(entityA);
            bool aIsRock = rockColiderGroup.HasComponent(entityA);
            bool aIsPlayer = playerColiderGroup.HasComponent(entityA);
            bool bIsLaser = laserColiderGroup.HasComponent(entityB);
            bool bIsRock = rockColiderGroup.HasComponent(entityB);
            bool bIsPlayer = playerColiderGroup.HasComponent(entityB);


            if (aIsRock && bIsLaser)
            {
                Destroy destroyRData = destroyGroup[entityA];
                Destroy destroyLData = destroyGroup[entityB];
                destroyRData.dead = true;
                destroyLData.dead = true;
                destroyGroup[entityA] = destroyRData;
                destroyGroup[entityB] = destroyLData;
            }
            
            if (bIsRock && aIsPlayer)
            {
                Destroy destroyPData = destroyGroup[entityA];
                destroyPData.life = destroyPData.life - 1;

                if (destroyPData.life <= 0) destroyPData.dead = true;

                destroyGroup[entityA] = destroyPData;
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new CollisionSystemJob();
        job.rockColiderGroup = GetComponentDataFromEntity<RockColiderTag>(true);
        job.laserColiderGroup = GetComponentDataFromEntity<LaserColiderTag>(true);
        job.playerColiderGroup = GetComponentDataFromEntity<ShipColiderTag>(true);
        job.destroyGroup = GetComponentDataFromEntity<Destroy>(false);

        JobHandle jobHandle = job.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);
        jobHandle.Complete();
        return jobHandle;
    }
}
