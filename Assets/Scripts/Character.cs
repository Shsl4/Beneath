using System;
using Attributes;
using Events;
using Interfaces;
using UnityEngine;

public abstract class Character : MonoBehaviour, IIdentifiable
{
    
    public String characterName;

    public string GetIdentifiableName() { return characterName; }
    

}