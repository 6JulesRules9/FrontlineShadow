using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FrontlineShadow.Config;
using FrontlineShadow.Core;
using FrontlineShadow.Stats;
using UnityEngine;

namespace FrontlineShadow.Tank
{
    /// <summary>
    /// Verwaltet das aktive Hunter-Loadout, Bauteil-Instanzen und Preset-Slots
    /// (03_TECH_ARCHITEKTUR.md §5). Phase 1: jedes Bauteil im
    /// <see cref="ComponentCatalog"/> ist sofort equippbar (Crafting kommt in
    /// Phase 4) — pro Def existiert genau eine Instanz.
    /// </summary>
    public class LoadoutService : IService
    {
        const string FileName = "frontline_shadow.loadout.save.json";

        readonly string _path;
        readonly ComponentCatalog _catalog;
        readonly BalanceConfig _balance;

        LoadoutSaveData _data;

        public HunterLoadout Active => _data.Active;
        public int PresetSlotCount => _data.Presets.Length;
        public ComponentCatalog Catalog => _catalog;

        public LoadoutService(ComponentCatalog catalog, BalanceConfig balance, string directory = null)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _balance = balance ?? throw new ArgumentNullException(nameof(balance));
            var dir = string.IsNullOrEmpty(directory) ? Application.persistentDataPath : directory;
            _path = Path.Combine(dir, FileName);
        }

        /// <summary>Lädt den Loadout-Stand oder erzeugt einen Default (1. Bauteil je Slot aus dem Katalog).</summary>
        public LoadoutSaveData LoadOrCreate()
        {
            if (File.Exists(_path))
            {
                try
                {
                    var json = File.ReadAllText(_path);
                    _data = Deserialize(json) ?? CreateDefault();
                }
                catch (Exception e)
                {
                    Debug.LogError($"[Loadout] Laden fehlgeschlagen, starte neues Loadout. {e.Message}");
                    _data = CreateDefault();
                }
            }
            else
            {
                _data = CreateDefault();
            }

            ResizePresets();
            return _data;
        }

        public void Save()
        {
            _data ??= CreateDefault();
            File.WriteAllText(_path, Serialize(_data));
        }

        public TankComponentDef GetEquipped(ComponentSlot slot)
        {
            var instanceId = _data.Active.ComponentInstanceIds[(int)slot];
            if (string.IsNullOrEmpty(instanceId)) return null;

            var instance = FindInstance(instanceId);
            return instance == null ? null : _catalog.FindById(instance.DefId);
        }

        public void Equip(ComponentSlot slot, string defId)
        {
            var def = _catalog.FindById(defId);
            if (def == null || def.Slot != slot)
                throw new ArgumentException($"Kein Bauteil '{defId}' für Slot {slot} im Katalog.", nameof(defId));

            var instance = GetOrCreateInstance(defId);
            _data.Active.ComponentInstanceIds[(int)slot] = instance.Id;
        }

        public Dictionary<StatType, float> ComputeFinalStats()
        {
            var equipped = new List<TankComponentDef>(HunterLoadout.SlotCount);
            foreach (ComponentSlot slot in Enum.GetValues(typeof(ComponentSlot)))
            {
                var def = GetEquipped(slot);
                if (def != null) equipped.Add(def);
            }
            return StatAggregator.Compute(equipped, _balance);
        }

        public void SavePreset(int slotIndex, string presetName = null)
        {
            EnsurePresetIndex(slotIndex);
            var preset = _data.Active.Clone();
            preset.PresetName = string.IsNullOrEmpty(presetName) ? $"Preset {slotIndex + 1}" : presetName;
            _data.Presets[slotIndex] = preset;
        }

        public bool LoadPreset(int slotIndex)
        {
            EnsurePresetIndex(slotIndex);
            var preset = _data.Presets[slotIndex];
            if (preset == null || string.IsNullOrEmpty(preset.PresetName)) return false;

            _data.Active = preset.Clone();
            return true;
        }

        public bool HasPreset(int slotIndex)
        {
            EnsurePresetIndex(slotIndex);
            return _data.Presets[slotIndex] != null && !string.IsNullOrEmpty(_data.Presets[slotIndex].PresetName);
        }

        public string GetPresetName(int slotIndex)
        {
            EnsurePresetIndex(slotIndex);
            return _data.Presets[slotIndex]?.PresetName;
        }

        ComponentInstance GetOrCreateInstance(string defId)
        {
            var existing = _data.Instances.Find(i => i.DefId == defId);
            if (existing != null) return existing;

            var instance = new ComponentInstance(defId, defId);
            _data.Instances.Add(instance);
            return instance;
        }

        ComponentInstance FindInstance(string instanceId) => _data.Instances.Find(i => i.Id == instanceId);

        void EnsurePresetIndex(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= _data.Presets.Length)
                throw new ArgumentOutOfRangeException(nameof(slotIndex), slotIndex,
                    "Preset-Slot außerhalb von BalanceConfig.PresetSlots.");
        }

        void ResizePresets()
        {
            if (_data.Presets.Length == _balance.PresetSlots) return;

            var resized = new HunterLoadout[_balance.PresetSlots];
            Array.Copy(_data.Presets, resized, Math.Min(_data.Presets.Length, resized.Length));
            _data.Presets = resized;
        }

        LoadoutSaveData CreateDefault()
        {
            var data = new LoadoutSaveData { Presets = new HunterLoadout[_balance.PresetSlots] };

            foreach (ComponentSlot slot in Enum.GetValues(typeof(ComponentSlot)))
            {
                var first = _catalog.ForSlot(slot).FirstOrDefault();
                if (first == null) continue;

                var instance = new ComponentInstance(first.Id, first.Id);
                data.Instances.Add(instance);
                data.Active.ComponentInstanceIds[(int)slot] = instance.Id;
            }

            return data;
        }

        public static string Serialize(LoadoutSaveData data) => JsonUtility.ToJson(data, true);

        public static LoadoutSaveData Deserialize(string json) => JsonUtility.FromJson<LoadoutSaveData>(json);
    }
}
