using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Collections;
using Unity.NetCode;
using Unity.Physics;
using Unity.Transforms;

public struct AgentGhostSerializer : IGhostSerializer<AgentSnapshotData>
{
    private ComponentType componentTypeAgentComponent;
    private ComponentType componentTypeBackwardModifier;
    private ComponentType componentTypeCanMove;
    private ComponentType componentTypeDestinationComponent;
    private ComponentType componentTypeGameOrientation;
    private ComponentType componentTypeGamePosition;
    private ComponentType componentTypeHealth;
    private ComponentType componentTypeSpeed;
    private ComponentType componentTypeTargetOrientation;
    private ComponentType componentTypePhysicsCollider;
    private ComponentType componentTypePhysicsGravityFactor;
    private ComponentType componentTypePhysicsMass;
    private ComponentType componentTypePhysicsVelocity;
    private ComponentType componentTypeLocalToWorld;
    private ComponentType componentTypeRotation;
    private ComponentType componentTypeTranslation;
    // FIXME: These disable safety since all serializers have an instance of the same type - causing aliasing. Should be fixed in a cleaner way
    [NativeDisableContainerSafetyRestriction][ReadOnly] private ArchetypeChunkComponentType<AgentComponent> ghostAgentComponentType;
    [NativeDisableContainerSafetyRestriction][ReadOnly] private ArchetypeChunkComponentType<BackwardModifier> ghostBackwardModifierType;
    [NativeDisableContainerSafetyRestriction][ReadOnly] private ArchetypeChunkComponentType<CanMove> ghostCanMoveType;
    [NativeDisableContainerSafetyRestriction][ReadOnly] private ArchetypeChunkComponentType<DestinationComponent> ghostDestinationComponentType;
    [NativeDisableContainerSafetyRestriction][ReadOnly] private ArchetypeChunkComponentType<GameOrientation> ghostGameOrientationType;
    [NativeDisableContainerSafetyRestriction][ReadOnly] private ArchetypeChunkComponentType<GamePosition> ghostGamePositionType;
    [NativeDisableContainerSafetyRestriction][ReadOnly] private ArchetypeChunkComponentType<Health> ghostHealthType;
    [NativeDisableContainerSafetyRestriction][ReadOnly] private ArchetypeChunkComponentType<Speed> ghostSpeedType;
    [NativeDisableContainerSafetyRestriction][ReadOnly] private ArchetypeChunkComponentType<Rotation> ghostRotationType;
    [NativeDisableContainerSafetyRestriction][ReadOnly] private ArchetypeChunkComponentType<Translation> ghostTranslationType;


    public int CalculateImportance(ArchetypeChunk chunk)
    {
        return 1;
    }

    public int SnapshotSize => UnsafeUtility.SizeOf<AgentSnapshotData>();
    public void BeginSerialize(ComponentSystemBase system)
    {
        componentTypeAgentComponent = ComponentType.ReadWrite<AgentComponent>();
        componentTypeBackwardModifier = ComponentType.ReadWrite<BackwardModifier>();
        componentTypeCanMove = ComponentType.ReadWrite<CanMove>();
        componentTypeDestinationComponent = ComponentType.ReadWrite<DestinationComponent>();
        componentTypeGameOrientation = ComponentType.ReadWrite<GameOrientation>();
        componentTypeGamePosition = ComponentType.ReadWrite<GamePosition>();
        componentTypeHealth = ComponentType.ReadWrite<Health>();
        componentTypeSpeed = ComponentType.ReadWrite<Speed>();
        componentTypeTargetOrientation = ComponentType.ReadWrite<TargetOrientation>();
        componentTypePhysicsCollider = ComponentType.ReadWrite<PhysicsCollider>();
        componentTypePhysicsGravityFactor = ComponentType.ReadWrite<PhysicsGravityFactor>();
        componentTypePhysicsMass = ComponentType.ReadWrite<PhysicsMass>();
        componentTypePhysicsVelocity = ComponentType.ReadWrite<PhysicsVelocity>();
        componentTypeLocalToWorld = ComponentType.ReadWrite<LocalToWorld>();
        componentTypeRotation = ComponentType.ReadWrite<Rotation>();
        componentTypeTranslation = ComponentType.ReadWrite<Translation>();
        ghostAgentComponentType = system.GetArchetypeChunkComponentType<AgentComponent>(true);
        ghostBackwardModifierType = system.GetArchetypeChunkComponentType<BackwardModifier>(true);
        ghostCanMoveType = system.GetArchetypeChunkComponentType<CanMove>(true);
        ghostDestinationComponentType = system.GetArchetypeChunkComponentType<DestinationComponent>(true);
        ghostGameOrientationType = system.GetArchetypeChunkComponentType<GameOrientation>(true);
        ghostGamePositionType = system.GetArchetypeChunkComponentType<GamePosition>(true);
        ghostHealthType = system.GetArchetypeChunkComponentType<Health>(true);
        ghostSpeedType = system.GetArchetypeChunkComponentType<Speed>(true);
        ghostRotationType = system.GetArchetypeChunkComponentType<Rotation>(true);
        ghostTranslationType = system.GetArchetypeChunkComponentType<Translation>(true);
    }

    public void CopyToSnapshot(ArchetypeChunk chunk, int ent, uint tick, ref AgentSnapshotData snapshot, GhostSerializerState serializerState)
    {
        snapshot.tick = tick;
        var chunkDataAgentComponent = chunk.GetNativeArray(ghostAgentComponentType);
        var chunkDataBackwardModifier = chunk.GetNativeArray(ghostBackwardModifierType);
        var chunkDataCanMove = chunk.GetNativeArray(ghostCanMoveType);
        var chunkDataDestinationComponent = chunk.GetNativeArray(ghostDestinationComponentType);
        var chunkDataGameOrientation = chunk.GetNativeArray(ghostGameOrientationType);
        var chunkDataGamePosition = chunk.GetNativeArray(ghostGamePositionType);
        var chunkDataHealth = chunk.GetNativeArray(ghostHealthType);
        var chunkDataSpeed = chunk.GetNativeArray(ghostSpeedType);
        var chunkDataRotation = chunk.GetNativeArray(ghostRotationType);
        var chunkDataTranslation = chunk.GetNativeArray(ghostTranslationType);
        snapshot.SetAgentComponentPlayerId(chunkDataAgentComponent[ent].PlayerId, serializerState);
        snapshot.SetBackwardModifierValue(chunkDataBackwardModifier[ent].Value, serializerState);
        snapshot.SetCanMoveValue(chunkDataCanMove[ent].Value, serializerState);
        snapshot.SetDestinationComponentValue(chunkDataDestinationComponent[ent].Value, serializerState);
        snapshot.SetDestinationComponentValid(chunkDataDestinationComponent[ent].Valid, serializerState);
        snapshot.SetGameOrientationValue(chunkDataGameOrientation[ent].Value, serializerState);
        snapshot.SetGamePositionValue(chunkDataGamePosition[ent].Value, serializerState);
        snapshot.SetHealthValue(chunkDataHealth[ent].Value, serializerState);
        snapshot.SetHealthregen(chunkDataHealth[ent].regen, serializerState);
        snapshot.SetHealthmax(chunkDataHealth[ent].max, serializerState);
        snapshot.SetSpeedValue(chunkDataSpeed[ent].Value, serializerState);
        snapshot.SetRotationValue(chunkDataRotation[ent].Value, serializerState);
        snapshot.SetTranslationValue(chunkDataTranslation[ent].Value, serializerState);
    }
}
