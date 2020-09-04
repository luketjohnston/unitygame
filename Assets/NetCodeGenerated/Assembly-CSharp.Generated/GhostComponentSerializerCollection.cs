//THIS FILE IS AUTOGENERATED BY GHOSTCOMPILER. DON'T MODIFY OR ALTER.
using Unity.Entities;
using Unity.NetCode;
using Assembly_CSharp.Generated;

namespace Assembly_CSharp.Generated
{
    [UpdateInGroup(typeof(ClientAndServerInitializationSystemGroup))]
    public class GhostComponentSerializerRegistrationSystem : SystemBase
    {
        protected override void OnCreate()
        {
            var ghostCollectionSystem = World.GetOrCreateSystem<GhostCollectionSystem>();
            ghostCollectionSystem.AddSerializer(AgentComponentGhostComponentSerializer.State);
            ghostCollectionSystem.AddSerializer(AngleInputGhostComponentSerializer.State);
            ghostCollectionSystem.AddSerializer(BackwardModifierGhostComponentSerializer.State);
            ghostCollectionSystem.AddSerializer(BusyTimerGhostComponentSerializer.State);
            ghostCollectionSystem.AddSerializer(CanMoveGhostComponentSerializer.State);
            ghostCollectionSystem.AddSerializer(CooldownGhostComponentSerializer.State);
            ghostCollectionSystem.AddSerializer(DashGhostComponentSerializer.State);
            ghostCollectionSystem.AddSerializer(DestinationComponentGhostComponentSerializer.State);
            ghostCollectionSystem.AddSerializer(FreezeTimerGhostComponentSerializer.State);
            ghostCollectionSystem.AddSerializer(GameOrientationGhostComponentSerializer.State);
            ghostCollectionSystem.AddSerializer(GamePositionGhostComponentSerializer.State);
            ghostCollectionSystem.AddSerializer(HealthGhostComponentSerializer.State);
            ghostCollectionSystem.AddSerializer(LocationInputGhostComponentSerializer.State);
            ghostCollectionSystem.AddSerializer(OrientationInputGhostComponentSerializer.State);
            ghostCollectionSystem.AddSerializer(OwningPlayerGhostComponentSerializer.State);
            ghostCollectionSystem.AddSerializer(ReleasableGhostComponentSerializer.State);
            ghostCollectionSystem.AddSerializer(RotatingGhostComponentSerializer.State);
            ghostCollectionSystem.AddSerializer(SpeedGhostComponentSerializer.State);
            ghostCollectionSystem.AddSerializer(TargetOrientationGhostComponentSerializer.State);
            ghostCollectionSystem.AddSerializer(UsableGhostComponentSerializer.State);
        }

        protected override void OnUpdate()
        {
            var parentGroup = World.GetExistingSystem<InitializationSystemGroup>();
            if (parentGroup != null)
            {
                parentGroup.RemoveSystemFromUpdateList(this);
            }
        }
    }
}