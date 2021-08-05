using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

[AlwaysSynchronizeSystem]
[UpdateAfter(typeof(TransformSystemGroup))]
[UpdateAfter(typeof(SpawnSystem))]
public class DespawnSystem : JobComponentSystem

{

    private EndSimulationEntityCommandBufferSystem commandBufferSystem;

    protected override void OnCreate()
    {
        base.OnCreate();
        //Get comand buffer
        commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }


    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        EntityCommandBuffer entityCommandBuffer = commandBufferSystem.CreateCommandBuffer();

        Entities.WithAll<Decaying>().WithAll<Destroy>().ForEach((Entity entity, in Decaying toGo, in Destroy destroy) =>
        {
            if (toGo.despawn)
            {
                entityCommandBuffer.DestroyEntity(entity);
            }

        }).Run();

        Entities.
            WithAll<Destroy>().
            WithoutBurst().
            ForEach((Entity entity, in Destroy destroy) =>
        {
            ///*
            if (destroy.dead && destroy.split)
            {
                //Increment score and destroy rok
                GameManager.main.PlayerScore();
                entityCommandBuffer.DestroyEntity(entity);
            }
            //*/
        }).Run();

        return default;

        //commandBufferSystem.AddJobHandleForProducer(this.Dependency);
    }
}
