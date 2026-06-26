using System;
using System.Collections.Generic;

namespace FrontlineShadow.Tank
{
    /// <summary>
    /// Persistenz-Schema für <see cref="LoadoutService"/>. Eigene Datei,
    /// getrennt vom Kern-<c>SaveData</c> — Tank-Domäne besitzt ihren eigenen
    /// Persistenz-Slice (03_TECH_ARCHITEKTUR.md Prinzip "Single Source of Truth
    /// pro System").
    /// </summary>
    [Serializable]
    public class LoadoutSaveData
    {
        public const int CurrentVersion = 1;

        public int Version = CurrentVersion;
        public List<ComponentInstance> Instances = new();
        public HunterLoadout Active = new();
        public HunterLoadout[] Presets = Array.Empty<HunterLoadout>();
    }
}
