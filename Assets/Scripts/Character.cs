using System;
using Interfaces;
using UnityEngine;

public abstract class Character : MonoBehaviour, IIdentifiable
{
    
    public string characterName;

    public string GetIdentifiableName() { return characterName; }
    

}