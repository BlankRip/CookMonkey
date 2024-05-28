using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public struct PlayerData: IEquatable<PlayerData>, INetworkSerializable
{
    public ulong clientId;
    public int colorId;

    public PlayerData(ulong clientId, int colorId)
    {
        this.clientId = clientId;
        this.colorId = colorId;
    }

    public bool Equals(PlayerData other)
    {
        return clientId == other.clientId && colorId == other.colorId;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref clientId);
        serializer.SerializeValue(ref colorId);
    }
}