﻿/// <summary>
/// Temporal Triggers. These are the types of temporal triggers that can affect a temporal object
/// </summary>
public enum eTemporalTriggerType
{
    None = -1,
    Hour,
    Day,
    Month,
    Year,
    Count = Year - 1
}

/// <summary>
/// Temporal Trigger Outcomes. This list will probably get really large.
/// </summary>
public enum eTemporalTriggerOutcome
{
    None = -1,
    Meat_Spoil,
    Crop_Grow,
    Crop_Harvestable,
    Crop_Spoiled,
    Crop_Dead,
    Count = Crop_Dead - 1
}

/// <summary>
/// Resource Types. These will be paired with a quantity and consumed by resource dependent objects.
/// Note: We can also use the resource system to do vitals.
/// </summary>
public enum eResourceType
{
    None = -1,
    Health,
    Stamina,
    Heat,
    Water,
    Food,
    Fuel,
    Flameable,
    Count = Flameable - 1
}

public enum eImpactType
{
    None = -1,
    Blunt,
    Sharp,
    Pierce
}