using System.Collections.Generic;
using static UnityEditor.Progress;
using UnityEngine;
using System.Linq;
using System;
using static UnityEditor.PlayerSettings;
using Newtonsoft.Json;

public static class CharacterStats 
{
    public static int HitDamage { get; } = 200;
    public static float AttackTime { get; } = 1.22f;
    public static float AttackSpeedMultiplyer { get; } = 1.8f;
    public static int Health { get; set; } = 50;
    public static int HealthMax { get; set; } = 100;
    public static int Energy { get; set; } = 200;
    public static int EnergyMax { get; set; } = 400;
    public static int XP { get; set; } = 200;
    public static int XPMax { get; set; } = 400;
}