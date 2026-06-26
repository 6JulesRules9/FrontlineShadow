using System;
using System.Collections.Generic;
using System.IO;
using FrontlineShadow.Config;
using FrontlineShadow.Stats;
using FrontlineShadow.Tank;
using NUnit.Framework;
using UnityEngine;

namespace FrontlineShadow.Tests
{
    public class LoadoutServiceTests
    {
        string _tempDir;
        ComponentCatalog _catalog;
        BalanceConfig _balance;

        [SetUp]
        public void SetUp()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), "fs_loadout_tests_" + Guid.NewGuid());
            Directory.CreateDirectory(_tempDir);

            _balance = ScriptableObject.CreateInstance<BalanceConfig>();
            _balance.PresetSlots = 3;

            _catalog = ScriptableObject.CreateInstance<ComponentCatalog>();
            _catalog.Components = new List<TankComponentDef>
            {
                MakeDef("tracks.a", ComponentSlot.Tracks, new StatModifier(StatType.TopSpeed, ModOp.Flat, 50)),
                MakeDef("tracks.b", ComponentSlot.Tracks, new StatModifier(StatType.TopSpeed, ModOp.Flat, 70)),
                MakeDef("core.a", ComponentSlot.Core, new StatModifier(StatType.Hp, ModOp.Flat, 300)),
                MakeDef("turret.a", ComponentSlot.Turret, new StatModifier(StatType.ArmorFront, ModOp.Flat, 40)),
                MakeDef("gun.a", ComponentSlot.Gun, new StatModifier(StatType.AlphaDamage, ModOp.Flat, 200)),
                MakeDef("comms.a", ComponentSlot.Comms, new StatModifier(StatType.ViewRange, ModOp.Flat, 380)),
            };
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_tempDir)) Directory.Delete(_tempDir, true);
        }

        static TankComponentDef MakeDef(string id, ComponentSlot slot, params StatModifier[] mods)
        {
            var def = ScriptableObject.CreateInstance<TankComponentDef>();
            def.Id = id;
            def.Slot = slot;
            def.Stats = new List<StatModifier>(mods);
            return def;
        }

        LoadoutService NewService() => new(_catalog, _balance, _tempDir);

        [Test]
        public void LoadOrCreate_WithoutSaveFile_EquipsFirstComponentPerSlot()
        {
            var service = NewService();
            service.LoadOrCreate();

            Assert.AreEqual("tracks.a", service.GetEquipped(ComponentSlot.Tracks).Id);
            Assert.AreEqual("core.a", service.GetEquipped(ComponentSlot.Core).Id);
        }

        [Test]
        public void Equip_SwapsComponentInSlot()
        {
            var service = NewService();
            service.LoadOrCreate();

            service.Equip(ComponentSlot.Tracks, "tracks.b");

            Assert.AreEqual("tracks.b", service.GetEquipped(ComponentSlot.Tracks).Id);
        }

        [Test]
        public void Equip_WrongSlotForDef_Throws()
        {
            var service = NewService();
            service.LoadOrCreate();

            Assert.Throws<ArgumentException>(() => service.Equip(ComponentSlot.Core, "tracks.a"));
        }

        [Test]
        public void ComputeFinalStats_SumsAcrossEquippedComponents()
        {
            var service = NewService();
            service.LoadOrCreate();

            var stats = service.ComputeFinalStats();

            Assert.AreEqual(50f, stats[StatType.TopSpeed], 0.001f);
            Assert.AreEqual(300f, stats[StatType.Hp], 0.001f);
        }

        [Test]
        public void SaveLoadPreset_RoundTripsActiveLoadout()
        {
            var service = NewService();
            service.LoadOrCreate();
            service.Equip(ComponentSlot.Tracks, "tracks.b");
            service.SavePreset(0, "Speed Build");

            service.Equip(ComponentSlot.Tracks, "tracks.a");
            Assert.AreEqual("tracks.a", service.GetEquipped(ComponentSlot.Tracks).Id);

            var loaded = service.LoadPreset(0);

            Assert.IsTrue(loaded);
            Assert.AreEqual("tracks.b", service.GetEquipped(ComponentSlot.Tracks).Id);
            Assert.AreEqual("Speed Build", service.Active.PresetName);
        }

        [Test]
        public void LoadPreset_EmptySlot_ReturnsFalse()
        {
            var service = NewService();
            service.LoadOrCreate();

            Assert.IsFalse(service.LoadPreset(1));
        }

        [Test]
        public void HasPreset_ReflectsSavedState()
        {
            var service = NewService();
            service.LoadOrCreate();

            Assert.IsFalse(service.HasPreset(0));

            service.SavePreset(0);

            Assert.IsTrue(service.HasPreset(0));
        }

        [Test]
        public void SavePreset_OutOfRange_Throws()
        {
            var service = NewService();
            service.LoadOrCreate();

            Assert.Throws<ArgumentOutOfRangeException>(() => service.SavePreset(99));
        }

        [Test]
        public void Save_Then_NewServiceInstance_RestoresPresetsAfterRestart()
        {
            var service = NewService();
            service.LoadOrCreate();
            service.Equip(ComponentSlot.Gun, "gun.a");
            service.SavePreset(0, "Alpha Build");
            service.Save();

            var restarted = NewService();
            restarted.LoadOrCreate();

            Assert.IsTrue(restarted.HasPreset(0));
            Assert.AreEqual("Alpha Build", restarted.GetPresetName(0));
            Assert.IsTrue(restarted.LoadPreset(0));
            Assert.AreEqual("gun.a", restarted.GetEquipped(ComponentSlot.Gun).Id);
        }

        [Test]
        public void ResizePresets_WhenBalanceSlotCountChanges_PreservesExistingPresets()
        {
            var service = NewService();
            service.LoadOrCreate();
            service.SavePreset(0, "Kept");
            service.Save();

            _balance.PresetSlots = 5;

            var resized = NewService();
            resized.LoadOrCreate();

            Assert.AreEqual(5, resized.PresetSlotCount);
            Assert.AreEqual("Kept", resized.GetPresetName(0));
        }
    }
}
