using Unity.Networking.Transport;
using Unity.NetCode;
using Unity.Mathematics;
using Unity.Entities;

public struct SwordSnapshotData : ISnapshotData<SwordSnapshotData>
{
    public uint tick;
    private int Cooldowntimer;
    private int Cooldownduration;
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
    private int Child0RotationValueX;
    private int Child0RotationValueY;
    private int Child0RotationValueZ;
    private int Child0RotationValueW;
    private int Child0TranslationValueX;
    private int Child0TranslationValueY;
    private int Child0TranslationValueZ;
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
    public quaternion GetChild0RotationValue(GhostDeserializerState deserializerState)
    {
        return GetChild0RotationValue();
    }
    public quaternion GetChild0RotationValue()
    {
        return new quaternion(Child0RotationValueX * 0.001f, Child0RotationValueY * 0.001f, Child0RotationValueZ * 0.001f, Child0RotationValueW * 0.001f);
    }
    public void SetChild0RotationValue(quaternion q, GhostSerializerState serializerState)
    {
        SetChild0RotationValue(q);
    }
    public void SetChild0RotationValue(quaternion q)
    {
        Child0RotationValueX = (int)(q.value.x * 1000);
        Child0RotationValueY = (int)(q.value.y * 1000);
        Child0RotationValueZ = (int)(q.value.z * 1000);
        Child0RotationValueW = (int)(q.value.w * 1000);
    }
    public float3 GetChild0TranslationValue(GhostDeserializerState deserializerState)
    {
        return GetChild0TranslationValue();
    }
    public float3 GetChild0TranslationValue()
    {
        return new float3(Child0TranslationValueX * 0.01f, Child0TranslationValueY * 0.01f, Child0TranslationValueZ * 0.01f);
    }
    public void SetChild0TranslationValue(float3 val, GhostSerializerState serializerState)
    {
        SetChild0TranslationValue(val);
    }
    public void SetChild0TranslationValue(float3 val)
    {
        Child0TranslationValueX = (int)(val.x * 100);
        Child0TranslationValueY = (int)(val.y * 100);
        Child0TranslationValueZ = (int)(val.z * 100);
    }

    public void PredictDelta(uint tick, ref SwordSnapshotData baseline1, ref SwordSnapshotData baseline2)
    {
        var predictor = new GhostDeltaPredictor(tick, this.tick, baseline1.tick, baseline2.tick);
        Cooldowntimer = predictor.PredictInt(Cooldowntimer, baseline1.Cooldowntimer, baseline2.Cooldowntimer);
        Cooldownduration = predictor.PredictInt(Cooldownduration, baseline1.Cooldownduration, baseline2.Cooldownduration);
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
        Child0RotationValueX = predictor.PredictInt(Child0RotationValueX, baseline1.Child0RotationValueX, baseline2.Child0RotationValueX);
        Child0RotationValueY = predictor.PredictInt(Child0RotationValueY, baseline1.Child0RotationValueY, baseline2.Child0RotationValueY);
        Child0RotationValueZ = predictor.PredictInt(Child0RotationValueZ, baseline1.Child0RotationValueZ, baseline2.Child0RotationValueZ);
        Child0RotationValueW = predictor.PredictInt(Child0RotationValueW, baseline1.Child0RotationValueW, baseline2.Child0RotationValueW);
        Child0TranslationValueX = predictor.PredictInt(Child0TranslationValueX, baseline1.Child0TranslationValueX, baseline2.Child0TranslationValueX);
        Child0TranslationValueY = predictor.PredictInt(Child0TranslationValueY, baseline1.Child0TranslationValueY, baseline2.Child0TranslationValueY);
        Child0TranslationValueZ = predictor.PredictInt(Child0TranslationValueZ, baseline1.Child0TranslationValueZ, baseline2.Child0TranslationValueZ);
    }

    public void Serialize(int networkId, ref SwordSnapshotData baseline, ref DataStreamWriter writer, NetworkCompressionModel compressionModel)
    {
        changeMask0 = (Cooldowntimer != baseline.Cooldowntimer) ? 1u : 0;
        changeMask0 |= (Cooldownduration != baseline.Cooldownduration) ? (1u<<1) : 0;
        changeMask0 |= (OwningPlayerValue != baseline.OwningPlayerValue) ? (1u<<2) : 0;
        changeMask0 |= (OwningPlayerPlayerId != baseline.OwningPlayerPlayerId) ? (1u<<3) : 0;
        changeMask0 |= (RotationValueX != baseline.RotationValueX ||
                                           RotationValueY != baseline.RotationValueY ||
                                           RotationValueZ != baseline.RotationValueZ ||
                                           RotationValueW != baseline.RotationValueW) ? (1u<<4) : 0;
        changeMask0 |= (TranslationValueX != baseline.TranslationValueX ||
                                           TranslationValueY != baseline.TranslationValueY ||
                                           TranslationValueZ != baseline.TranslationValueZ) ? (1u<<5) : 0;
        changeMask0 |= (Usablecanuse != baseline.Usablecanuse) ? (1u<<6) : 0;
        changeMask0 |= (Child0RotationValueX != baseline.Child0RotationValueX ||
                                           Child0RotationValueY != baseline.Child0RotationValueY ||
                                           Child0RotationValueZ != baseline.Child0RotationValueZ ||
                                           Child0RotationValueW != baseline.Child0RotationValueW) ? (1u<<7) : 0;
        changeMask0 |= (Child0TranslationValueX != baseline.Child0TranslationValueX ||
                                           Child0TranslationValueY != baseline.Child0TranslationValueY ||
                                           Child0TranslationValueZ != baseline.Child0TranslationValueZ) ? (1u<<8) : 0;
        writer.WritePackedUIntDelta(changeMask0, baseline.changeMask0, compressionModel);
        if ((changeMask0 & (1 << 0)) != 0)
            writer.WritePackedIntDelta(Cooldowntimer, baseline.Cooldowntimer, compressionModel);
        if ((changeMask0 & (1 << 1)) != 0)
            writer.WritePackedIntDelta(Cooldownduration, baseline.Cooldownduration, compressionModel);
        if ((changeMask0 & (1 << 2)) != 0)
            writer.WritePackedIntDelta(OwningPlayerValue, baseline.OwningPlayerValue, compressionModel);
        if ((changeMask0 & (1 << 3)) != 0)
            writer.WritePackedIntDelta(OwningPlayerPlayerId, baseline.OwningPlayerPlayerId, compressionModel);
        if ((changeMask0 & (1 << 4)) != 0)
        {
            writer.WritePackedIntDelta(RotationValueX, baseline.RotationValueX, compressionModel);
            writer.WritePackedIntDelta(RotationValueY, baseline.RotationValueY, compressionModel);
            writer.WritePackedIntDelta(RotationValueZ, baseline.RotationValueZ, compressionModel);
            writer.WritePackedIntDelta(RotationValueW, baseline.RotationValueW, compressionModel);
        }
        if ((changeMask0 & (1 << 5)) != 0)
        {
            writer.WritePackedIntDelta(TranslationValueX, baseline.TranslationValueX, compressionModel);
            writer.WritePackedIntDelta(TranslationValueY, baseline.TranslationValueY, compressionModel);
            writer.WritePackedIntDelta(TranslationValueZ, baseline.TranslationValueZ, compressionModel);
        }
        if ((changeMask0 & (1 << 6)) != 0)
            writer.WritePackedUIntDelta(Usablecanuse, baseline.Usablecanuse, compressionModel);
        if ((changeMask0 & (1 << 7)) != 0)
        {
            writer.WritePackedIntDelta(Child0RotationValueX, baseline.Child0RotationValueX, compressionModel);
            writer.WritePackedIntDelta(Child0RotationValueY, baseline.Child0RotationValueY, compressionModel);
            writer.WritePackedIntDelta(Child0RotationValueZ, baseline.Child0RotationValueZ, compressionModel);
            writer.WritePackedIntDelta(Child0RotationValueW, baseline.Child0RotationValueW, compressionModel);
        }
        if ((changeMask0 & (1 << 8)) != 0)
        {
            writer.WritePackedIntDelta(Child0TranslationValueX, baseline.Child0TranslationValueX, compressionModel);
            writer.WritePackedIntDelta(Child0TranslationValueY, baseline.Child0TranslationValueY, compressionModel);
            writer.WritePackedIntDelta(Child0TranslationValueZ, baseline.Child0TranslationValueZ, compressionModel);
        }
    }

    public void Deserialize(uint tick, ref SwordSnapshotData baseline, ref DataStreamReader reader,
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
            OwningPlayerValue = reader.ReadPackedIntDelta(baseline.OwningPlayerValue, compressionModel);
        else
            OwningPlayerValue = baseline.OwningPlayerValue;
        if ((changeMask0 & (1 << 3)) != 0)
            OwningPlayerPlayerId = reader.ReadPackedIntDelta(baseline.OwningPlayerPlayerId, compressionModel);
        else
            OwningPlayerPlayerId = baseline.OwningPlayerPlayerId;
        if ((changeMask0 & (1 << 4)) != 0)
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
        if ((changeMask0 & (1 << 5)) != 0)
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
        if ((changeMask0 & (1 << 6)) != 0)
            Usablecanuse = reader.ReadPackedUIntDelta(baseline.Usablecanuse, compressionModel);
        else
            Usablecanuse = baseline.Usablecanuse;
        if ((changeMask0 & (1 << 7)) != 0)
        {
            Child0RotationValueX = reader.ReadPackedIntDelta(baseline.Child0RotationValueX, compressionModel);
            Child0RotationValueY = reader.ReadPackedIntDelta(baseline.Child0RotationValueY, compressionModel);
            Child0RotationValueZ = reader.ReadPackedIntDelta(baseline.Child0RotationValueZ, compressionModel);
            Child0RotationValueW = reader.ReadPackedIntDelta(baseline.Child0RotationValueW, compressionModel);
        }
        else
        {
            Child0RotationValueX = baseline.Child0RotationValueX;
            Child0RotationValueY = baseline.Child0RotationValueY;
            Child0RotationValueZ = baseline.Child0RotationValueZ;
            Child0RotationValueW = baseline.Child0RotationValueW;
        }
        if ((changeMask0 & (1 << 8)) != 0)
        {
            Child0TranslationValueX = reader.ReadPackedIntDelta(baseline.Child0TranslationValueX, compressionModel);
            Child0TranslationValueY = reader.ReadPackedIntDelta(baseline.Child0TranslationValueY, compressionModel);
            Child0TranslationValueZ = reader.ReadPackedIntDelta(baseline.Child0TranslationValueZ, compressionModel);
        }
        else
        {
            Child0TranslationValueX = baseline.Child0TranslationValueX;
            Child0TranslationValueY = baseline.Child0TranslationValueY;
            Child0TranslationValueZ = baseline.Child0TranslationValueZ;
        }
    }
    public void Interpolate(ref SwordSnapshotData target, float factor)
    {
        SetRotationValue(math.slerp(GetRotationValue(), target.GetRotationValue(), factor));
        SetTranslationValue(math.lerp(GetTranslationValue(), target.GetTranslationValue(), factor));
        SetChild0RotationValue(math.slerp(GetChild0RotationValue(), target.GetChild0RotationValue(), factor));
        SetChild0TranslationValue(math.lerp(GetChild0TranslationValue(), target.GetChild0TranslationValue(), factor));
    }
}
