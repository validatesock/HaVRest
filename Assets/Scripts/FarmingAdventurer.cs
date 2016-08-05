﻿using UnityEngine;
using System;
using System.Collections;
using System.Runtime.Serialization;

/// <summary>
/// Player Class!
/// </summary>
[Serializable]
public class FarmingAdventurer : ISerializable
{
    // Keys
    private static readonly string kPlayerNameKey = "farmingGame_PlayerNameKey";
    private static readonly string kPlayerAgeKey = "farmingGame_PlayerAgeKey";

    // Values
    private string mPlayerName = "";
    public string PlayerName { get { return mPlayerName; } }

    private int mPlayerAge = 0;
    public int PlayerAge { get { return mPlayerAge; } }

    private SerializableList<int> mItemIndicies = new SerializableList<int>();
    public SerializableList<int> ItemIndicies { get { return mItemIndicies; } }

    public FarmingAdventurer()
    {
        // EMPTY CTOR FOR COMPLIATION
    }

    public FarmingAdventurer(string name, int age)
    {
        mPlayerName = name;
        mPlayerAge = age;
        mItemIndicies = new SerializableList<int>();
    }

    ////////////////////////////////////////////////////////////////////////////
    /// Implementation of ISerializable (C# .Net)
    ////////////////////////////////////////////////////////////////////////////

    public FarmingAdventurer(SerializationInfo information, StreamingContext context)
    {
        mPlayerAge = (int)information.GetValue(kPlayerAgeKey, typeof(int));
        mPlayerName = (string)information.GetValue(kPlayerNameKey, typeof(string));
        mItemIndicies = new SerializableList<int>(information, context);
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(kPlayerAgeKey, mPlayerAge);
        info.AddValue(kPlayerNameKey, mPlayerName);

        mItemIndicies.GetObjectData(info, context);
    }
}