using Unity.Networking.Transport;
using Unity.NetCode;
using Unity.Mathematics;
using Unity.Entities;

public struct DashSnapshotData : ISnapshotData<DashSnapshotData>
{
    public uint tick;
    private int Cooldowntimer;
    private int Cooldownduration;
    private int Dashmax_distance;
    private int Dashspeed;
    private int DashdirX;
    private int DashdirY;
    private int DashdirZ;
    private int OwningPlayerValue;
    private int OwningPlayerPlayerId;
    private int RotationValueX;
    private int RotationValueY;
    private int RotationValueZ;
    private int RotationValueW;
    private int TranslationValueX;
    private int TranslationValueY;
    private int TranslationValueZ;
    private uint Usablecanuse;
    uint changeMask0;

    public uint Tick => tick;
    public float GetCooldowntimer(GhostDeserializerState deserializerState)
    {
        return Cooldowntimer * 0.001f;
    }
    public float GetCooldowntimer()
    {
        return Cooldowntimer * 0.001f;
    }
    public void SetCooldowntimer(float val, GhostSerializerState serializerState)
    {
        Cooldowntimer = (int)(val * 1000);
    }
    public void SetCooldowntimer(float val)
    {
        Cooldowntimer = (int)(val * 1000);
    }
    public float GetCooldownduration(GhostDeserializerState deserializerState)
    {
        return Cooldownduration * 0.001f;
    }
    public float GetCooldownduration()
    {
        return Cooldownduration * 0.001f;
    }
    public void SetCooldownduration(float val, GhostSerializerState serializerState)
    {
        Cooldownduration = (int)(val * 1000);
    }
    public void SetCooldownduration(float val)
    {
        Cooldownduration = (int)(val * 1000);
    }
    public float GetDashmax_distance(GhostDeserializerState deserializerState)
    {
        return Dashmax_distance * 0.001f;
    }
    public float GetDashmax_distance()
    {
        return Dashmax_distance * 0.001f;
    }
    public void SetDashmax_distance(float val, GhostSerializerState serializerState)
    {
        Dashmax_distance = (int)(val * 1000);
    }
    public void SetDashmax_distance(float val)
    {
        Dashmax_distance = (int)(val * 1000);
    }
    public float GetDashspeed(GhostDeserializerState deserializerState)
    {
        return Dashspeed * 0.001f;
    }
    public float GetDashspeed()
    {
        return Dashspeed * 0.001f;
    }
    public void SetDashspeed(float val, GhostSerializerState serializerState)
    {
        Dashspeed = (int)(val * 1000);
    }
    public void SetDashspeed(float val)
    {
        Dashspeed = (int)(val * 1000);
    }
    public float3 GetDashdir(GhostDeserializerState deserializerState)
    {
        return GetDashdir();
    }
    public float3 GetDashdir()
    {
        return new float3(DashdirX * 0.001f, DashdirY * 0.001f, DashdirZ * 0.001f);
    }
    public void SetDashdir(float3 val, GhostSerializerState serializerState)
    {
        SetDashdir(val);
    }
    public void SetDashdir(float3 val)
    {
        DashdirX = (int)(val.x * 1000);
        DashdirY = (int)(val.y * 1000);
        DashdirZ = (int)(val.z * 1000);
    }
    public Entity GetOwningPlayerValue(GhostDeserializerState deserializerState)
    {
        if (OwningPlayerValue == 0)
            return Entity.Null;
        if (!deserializerState.GhostMap.TryGetValue(OwningPlayerValue, out var ghostEnt))
            return Entity.Null;
        if (Unity.Networking.Transport.Utilities.SequenceHelpers.IsNewer(ghostEnt.spawnTick, Tick))
            return Entity.Null;
        return ghostEnt.entity;
    }
    public void SetOwningPlayerValue(Entity val, GhostSerializerState serializerState)
    {
        OwningPlayerValue = 0;
        if (serializerState.GhostStateFromEntity.Exists(val))
        {
            var ghostState = serializerState.GhostStateFromEntity[val];
            if (ghostState.despawnTick == 0)
                OwningPlayerValue = ghostState.ghostId;
        }
    }
    public void SetOwningPlayerValue(int val)
    {
        OwningPlayerValue = val;
    }
    public int GetOwningPlayerPlayerId(GhostDeserializerState deserializerState)
    {
        return (int)OwningPlayerPlayerId;
    }
    public int GetOwningPlayerPlayerId()
    {
        return (int)OwningPlayerPlayerId;
    }
    public void SetOwningPlayerPlayerId(int val, GhostSerializerState serializerState)
    {
        OwningPlayerPlayerId = (int)val;
    }
    public void SetOwningPlayerPlayerId(int val)
    {
        OwningPlayerPlayerId = (int)val;
    }
    public quaternion GetRotationValue(GhostDeserializerState deserializerState)
    {
        return GetRotationValue();
    }
    public quaternion GetRotationValue()
    {
        return new quaternion(RotationValueX * 0.001f, RotationValueY * 0.001f, RotationValueZ * 0.001f, RotationValueW * 0.001f);
    }
    public void SetRotationValue(quaternion q, GhostSerializerState serializerState)
    {
        SetRotationValue(q);
    }
    public void SetRotationValue(quaternion q)
    {
        RotationValueX = (int)(q.value.x * 1000);
        RotationValueY = (int)(q.value.y * 1000);
        RotationValueZ = (int)(q.value.z * 1000);
        RotationValueW = (int)(q.value.w * 1000);
    }
    public float3 GetTranslationValue(GhostDeserializerState deserializerState)
    {
        return GetTranslationValue();
    }
    public float3 GetTranslationValue()
    {
        return new float3(TranslationValueX * 0.01f, TranslationValueY * 0.01f, TranslationValueZ * 0.01f);
    }
    public void SetTranslationValue(float3 val, GhostSerializerState serializerState)
    {
        SetTranslationValue(val);
    }
    public void SetTranslationValue(float3 val)
    {
        TranslationValueX = (int)(val.x * 100);
        TranslationValueY = (int)(val.y * 100);
        TranslationValueZ = (int)(val.z * 100);
    }
    public bool GetUsablecanuse(GhostDeserializerState deserializerState)
    {
        return Usablecanuse!=0;
    }
    public bool GetUsablecanuse()
    {
        return Usablecanuse!=0;
    }
    public void SetUsablecanuse(bool val, GhostSerializerState serializerState)
    {
        Usablecanuse = val?1u:0;
    }
    public void SetUsablecanuse(bool val)
    {
        Usablecanuse = val?1u:0;
    }

    public void PredictDelta(uint tick, ref DashSnapshotData baseline1, ref DashSnapshotData baseline2)
    {
        var predictor = new GhostDeltaPredictor(tick, this.tick, baseline1.tick, baseline2.tick);
        Cooldowntimer = predictor.PredictInt(Cooldowntimer, baseline1.Cooldowntimer, baseline2.Cooldowntimer);
        Cooldownduration = predictor.PredictInt(Cooldownduration, baseline1.Cooldownduration, baseline2.Cooldownduration);
        Dashmax_distance = predictor.PredictInt(Dashmax_distance, baseline1.Dashmax_distance, baseline2.Dashmax_distance);
        Dashspeed = predictor.PredictInt(Dashspeed, baseline1.Dashspeed, baseline2.Dashspeed);
        DashdirX = predictor.PredictInt(DashdirX, baseline1.DashdirX, baseline2.DashdirX);
        DashdirY = predictor.PredictInt(DashdirY, baseline1.DashdirY, baseline2.DashdirY);
        DashdirZ = predictor.PredictInt(DashdirZ, baseline1.DashdirZ, baseline2.DashdirZ);
        OwningPlayerValue = predictor.PredictInt(OwningPlayerValue, baseline1.OwningPlayerValue, baseline2.OwningPlayerValue);
        OwningPlayerPlayerId = predictor.PredictInt(OwningPlayerPlayerId, baseline1.OwningPlayerPlayerId, baseline2.OwningPlayerPlayerId);
        RotationValueX = predictor.PredictInt(RotationValueX, baseline1.RotationValueX, baseline2.RotationValueX);
        RotationValueY = predictor.PredictInt(RotationValueY, baseline1.RotationValueY, baseline2.RotationValueY);
        RotationValueZ = predictor.PredictInt(RotationValueZ, baseline1.RotationValueZ, baseline2.RotationValueZ);
        RotationValueW = predictor.PredictInt(RotationValueW, baseline1.RotationValueW, baseline2.RotationValueW);
        TranslationValueX = predictor.PredictInt(TranslationValueX, baseline1.TranslationValueX, baseline2.TranslationValueX);
        TranslationValueY = predictor.PredictInt(TranslationValueY, baseline1.TranslationValueY, baseline2.TranslationValueY);
        TranslationValueZ = predictor.PredictInt(TranslationValueZ, baseline1.TranslationValueZ, baseline2.TranslationValueZ);
        Usablecanuse = (uint)predictor.PredictInt((int)Usablecanuse, (int)baseline1.Usablecanuse, (int)baseline2.Usablecanuse);
    }

    public void Serialize(int networkId, ref DashSnapshotData baseline, ref DataStreamWriter writer, NetworkCompressionModel compressionModel)
    {
        changeMask0 = (Cooldowntimer != baseline.Cooldowntimer) ? 1u : 0;
        changeMask0 |= (Cooldownduration != baseline.Cooldownduration) ? (1u<<1) : 0;
        changeMask0 |= (Dashmax_distance != baseline.Dashmax_distance) ? (1u<<2) : 0;
        changeMask0 |= (Dashspeed != baseline.Dashspeed) ? (1u<<3) : 0;
        changeMask0 |= (DashdirX != baseline.DashdirX ||
                                           DashdirY != baseline.DashdirY ||
                                           DashdirZ != baseline.DashdirZ) ? (1u<<4) : 0;
        changeMask0 |= (OwningPlayerValue != baseline.OwningPlayerValue) ? (1u<<5) : 0;
        changeMask0 |= (OwningPlayerPlayerId != baseline.OwningPlayerPlayerId) ? (1u<<6) : 0;
        changeMask0 |= (RotationValueX != baseline.RotationValueX ||
                                           RotationValueY != baseline.RotationValueY ||
                                           RotationValueZ != baseline.RotationValueZ ||
                                           RotationValueW != baseline.RotationValueW) ? (1u<<7) : 0;
        changeMask0 |= (TranslationValueX != baseline.TranslationValueX ||
                                           TranslationValueY != baseline.TranslationValueY ||
                                           TranslationValueZ != baseline.TranslationValueZ) ? (1u<<8) : 0;
        changeMask0 |= (Usablecanuse != baseline.Usablecanuse) ? (1u<<9) : 0;
        writer.WritePackedUIntDelta(changeMask0, baseline.changeMask0, compressionModel);
        if ((changeMask0 & (1 << 0)) != 0)
            writer.WritePackedIntDelta(Cooldowntimer, baseline.Cooldowntimer, compressionModel);
        if ((changeMask0 & (1 << 1)) != 0)
            writer.WritePackedIntDelta(Cooldownduration, baseline.Cooldownduration, compressionModel);
        if ((changeMask0 & (1 << 2)) != 0)
            writer.WritePackedIntDelta(Dashmax_distance, baseline.Dashmax_distance, compressionModel);
        if ((changeMask0 & (1 << 3)) != 0)
            writer.WritePackedIntDelta(Dashspeed, baseline.Dashspeed, compressionModel);
        if ((changeMask0 & (1 << 4)) != 0)
        {
            writer.WritePackedIntDelta(DashdirX, baseline.DashdirX, compressionModel);
            writer.WritePackedIntDelta(DashdirY, baseline.DashdirY, compressionModel);
            writer.WritePackedIntDelta(DashdirZ, baseline.DashdirZ, compressionModel);
        }
        if ((changeMask0 & (1 << 5)) != 0)
            writer.WritePackedIntDelta(OwningPlayerValue, baseline.OwningPlayerValue, compressionModel);
        if ((changeMask0 & (1 << 6)) != 0)
            writer.WritePackedIntDelta(OwningPlayerPlayerId, baseline.OwningPlayerPlayerId, compressionModel);
        if ((changeMask0 & (1 << 7)) != 0)
        {
            writer.WritePackedIntDelta(RotationValueX, baseline.RotationValueX, compressionModel);
            writer.WritePackedIntDelta(RotationValueY, baseline.RotationValueY, compressionModel);
            writer.WritePackedIntDelta(RotationValueZ, baseline.RotationValueZ, compressionModel);
            writer.WritePackedIntDelta(RotationValueW, baseline.RotationValueW, compressionModel);
        }
        if ((changeMask0 & (1 << 8)) != 0)
        {
            writer.WritePackedIntDelta(TranslationValueX, baseline.TranslationValueX, compressionModel);
            writer.WritePackedIntDelta(TranslationValueY, baseline.TranslationValueY, compressionModel);
            writer.WritePackedIntDelta(TranslationValueZ, baseline.TranslationValueZ, compressionModel);
        }
        if ((changeMask0 & (1 << 9)) != 0)
            writer.WritePackedUIntDelta(Usablecanuse, baseline.Usablecanuse, compressionModel);
    }

    public void Deserialize(uint tick, ref DashSnapshotData baseline, ref DataStreamReader reader,
        NetworkCompressionModel compressionModel)
    {
        this.tick = tick;
        changeMask0 = reader.ReadPackedUIntDelta(baseline.changeMask0, compressionModel);
        if ((changeMask0 & (1 << 0)) != 0)
            Cooldowntimer = reader.ReadPackedIntDelta(baseline.Cooldowntimer, compressionModel);
        else
            Cooldowntimer = baseline.Cooldowntimer;
        if ((changeMask0 & (1 << 1)) != 0)
            Cooldownduration = reader.ReadPackedIntDelta(baseline.Cooldownduration, compressionModel);
        else
            Cooldownduration = baseline.Cooldownduration;
        if ((changeMask0 & (1 << 2)) != 0)
            Dashmax_distance = reader.ReadPackedIntDelta(baseline.Dashmax_distance, compressionModel);
        else
            Dashmax_distance = baseline.Dashmax_distance;
        if ((changeMask0 & (1 << 3)) != 0)
            Dashspeed = reader.ReadPackedIntDelta(baseline.Dashspeed, compressionModel);
        else
            Dashspeed = baseline.Dashspeed;
        if ((changeMask0 & (1 << 4)) != 0)
        {
            DashdirX = reader.ReadPackedIntDelta(baseline.DashdirX, compressionModel);
            DashdirY = reader.ReadPackedIntDelta(baseline.DashdirY, compressionModel);
            DashdirZ = reader.ReadPackedIntDelta(baseline.DashdirZ, compressionModel);
        }
        else
        {
            DashdirX = baseline.DashdirX;
            DashdirY = baseline.DashdirY;
            DashdirZ = baseline.DashdirZ;
        }
        if ((changeMask0 & (1 << 5)) != 0)
            OwningPlayerValue = reader.ReadPackedIntDelta(baseline.OwningPlayerValue, compressionModel);
        else
            OwningPlayerValue = baseline.OwningPlayerValue;
        if ((changeMask0 & (1 << 6)) != 0)
            OwningPlayerPlayerId = reader.ReadPackedIntDelta(baseline.OwningPlayerPlayerId, compressionModel);
        else
            OwningPlayerPlayerId = baseline.OwningPlayerPlayerId;
        if ((changeMask0 & (1 << 7)) != 0)
        {
            RotationValueX = reader.ReadPackedIntDelta(baseline.RotationValueX, compressionModel);
            RotationValueY = reader.ReadPackedIntDelta(baseline.RotationValueY, compressionModel);
            RotationValueZ = reader.ReadPackedIntDelta(baseline.RotationValueZ, compressionModel);
            RotationValueW = reader.ReadPackedIntDelta(baseline.RotationValueW, compressionModel);
        }
        else
        {
            RotationValueX = baseline.RotationValueX;
            RotationValueY = baseline.RotationValueY;
            RotationValueZ = baseline.RotationValueZ;
            RotationValueW = baseline.RotationValueW;
        }
        if ((changeMask0 & (1 << 8)) != 0)
        {
            TranslationValueX = reader.ReadPackedIntDelta(baseline.TranslationValueX, compressionModel);
            TranslationValueY = reader.ReadPackedIntDelta(baseline.TranslationValueY, compressionModel);
            TranslationValueZ = reader.ReadPackedIntDelta(baseline.TranslationValueZ, compressionModel);
        }
        else
        {
            TranslationValueX = baseline.TranslationValueX;
            TranslationValueY = baseline.TranslationValueY;
            TranslationValueZ = baseline.TranslationValueZ;
        }
        if ((changeMask0 & (1 << 9)) != 0)
            Usablecanuse = reader.ReadPackedUIntDelta(baseline.Usablecanuse, compressionModel);
        else
            Usablecanuse = baseline.Usablecanuse;
    }
    public void Interpolate(ref DashSnapshotData target, float factor)
    {
        SetRotationValue(math.slerp(GetRotationValue(), target.GetRotationValue(), factor));
        SetTranslationValue(math.lerp(GetTranslationValue(), target.GetTranslationValue(), factor));
    }
}
