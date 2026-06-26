using System;
using System.Collections.Generic;
using System.Linq;
using FrontlineShadow.Core;
using FrontlineShadow.Stats;
using FrontlineShadow.Tank;
using UnityEngine;
using UnityEngine.UIElements;

namespace FrontlineShadow.UI
{
    /// <summary>
    /// Phase-1-Garage-Screen (04_ROADMAP.md Phase 1): 5 Bauteil-Slots
    /// durchschalten, Final-Stats live sehen, Presets speichern/laden.
    /// Komplett code-gebaut (kein UXML) — UI Toolkit über <see cref="UIDocument"/>.
    /// </summary>
    [RequireComponent(typeof(UIDocument))]
    public class GarageUIController : MonoBehaviour
    {
        static readonly Dictionary<ComponentSlot, string> SlotLabels = new()
        {
            { ComponentSlot.Tracks, "Ketten" },
            { ComponentSlot.Core, "Core" },
            { ComponentSlot.Turret, "Turm" },
            { ComponentSlot.Gun, "Geschütz" },
            { ComponentSlot.Comms, "Comms" },
        };

        LoadoutService _loadout;
        readonly Dictionary<ComponentSlot, Label> _slotValueLabels = new();
        readonly Dictionary<StatType, Label> _statValueLabels = new();
        readonly List<Label> _presetNameLabels = new();
        Label _statusBar;

        void OnEnable()
        {
            if (!ServiceLocator.TryGet(out _loadout))
            {
                Debug.LogError("[GarageUI] LoadoutService nicht registriert — zuerst die Bootstrap-Szene laden.");
                return;
            }

            Build();
            RefreshAll();
        }

        void Build()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            root.Clear();
            root.style.flexGrow = 1;
            root.style.flexDirection = FlexDirection.Row;
            root.style.backgroundColor = StyleTokens.BgBase;
            root.style.paddingLeft = root.style.paddingRight = 24;
            root.style.paddingTop = root.style.paddingBottom = 24;

            var leftColumn = new VisualElement();
            leftColumn.style.flexDirection = FlexDirection.Column;
            leftColumn.style.flexGrow = 1;
            leftColumn.style.marginRight = 16;
            leftColumn.Add(BuildSlotsPanel());
            leftColumn.Add(BuildPresetsPanel());
            leftColumn.Add(BuildStatusBar());

            root.Add(leftColumn);
            root.Add(BuildStatsPanel());
        }

        VisualElement BuildSlotsPanel()
        {
            var panel = Panel();
            panel.Add(Title("Hunter — Bauteile"));

            foreach (ComponentSlot slot in Enum.GetValues(typeof(ComponentSlot)))
            {
                var row = new VisualElement { style = { flexDirection = FlexDirection.Row, alignItems = Align.Center, marginTop = 8 } };

                var name = new Label(SlotLabels[slot]);
                name.style.color = StyleTokens.InkMuted;
                name.style.width = 90;
                row.Add(name);

                var prev = SmallButton("◀", () => Cycle(slot, -1));
                row.Add(prev);

                var value = new Label();
                value.style.color = StyleTokens.Ink;
                value.style.flexGrow = 1;
                value.style.unityTextAlign = TextAnchor.MiddleCenter;
                _slotValueLabels[slot] = value;
                row.Add(value);

                var next = SmallButton("▶", () => Cycle(slot, 1));
                row.Add(next);

                panel.Add(row);
            }

            return panel;
        }

        VisualElement BuildStatsPanel()
        {
            var panel = Panel();
            panel.style.width = 280;
            panel.style.marginRight = 0;
            panel.Add(Title("Final-Stats"));

            var scroll = new ScrollView();
            scroll.style.flexGrow = 1;

            foreach (StatType type in Enum.GetValues(typeof(StatType)))
            {
                var row = new VisualElement { style = { flexDirection = FlexDirection.Row, justifyContent = Justify.SpaceBetween, marginTop = 4 } };

                var name = new Label(type.ToString());
                name.style.color = StyleTokens.InkMuted;
                row.Add(name);

                var value = new Label("0");
                value.style.color = StyleTokens.Ink;
                _statValueLabels[type] = value;
                row.Add(value);

                scroll.Add(row);
            }

            panel.Add(scroll);
            return panel;
        }

        VisualElement BuildPresetsPanel()
        {
            var panel = Panel();
            panel.style.marginTop = 16;
            panel.Add(Title("Presets"));

            for (var i = 0; i < _loadout.PresetSlotCount; i++)
            {
                var index = i;
                var row = new VisualElement { style = { flexDirection = FlexDirection.Row, alignItems = Align.Center, marginTop = 8 } };

                var nameLabel = new Label();
                nameLabel.style.color = StyleTokens.Ink;
                nameLabel.style.flexGrow = 1;
                _presetNameLabels.Add(nameLabel);
                row.Add(nameLabel);

                var save = SmallButton("Speichern", () => SavePreset(index));
                row.Add(save);

                var load = SmallButton("Laden", () => LoadPreset(index));
                row.Add(load);

                panel.Add(row);
            }

            return panel;
        }

        VisualElement BuildStatusBar()
        {
            _statusBar = new Label();
            _statusBar.style.color = StyleTokens.Accent;
            _statusBar.style.marginTop = 12;
            return _statusBar;
        }

        void Cycle(ComponentSlot slot, int direction)
        {
            var options = _loadout.Catalog.ForSlot(slot).ToList();
            if (options.Count == 0) return;

            var current = _loadout.GetEquipped(slot);
            var currentIndex = current == null ? -1 : options.FindIndex(c => c.Id == current.Id);
            var nextIndex = ((currentIndex + direction) % options.Count + options.Count) % options.Count;

            _loadout.Equip(slot, options[nextIndex].Id);
            _loadout.Save();
            RefreshAll();
        }

        void SavePreset(int slotIndex)
        {
            _loadout.SavePreset(slotIndex);
            _loadout.Save();
            SetStatus($"Preset {slotIndex + 1} gespeichert.");
            RefreshAll();
        }

        void LoadPreset(int slotIndex)
        {
            if (_loadout.LoadPreset(slotIndex))
            {
                _loadout.Save();
                SetStatus($"Preset {slotIndex + 1} geladen.");
            }
            else
            {
                SetStatus($"Preset {slotIndex + 1} ist leer.");
            }

            RefreshAll();
        }

        void SetStatus(string message)
        {
            if (_statusBar != null) _statusBar.text = message;
        }

        void RefreshAll()
        {
            foreach (ComponentSlot slot in Enum.GetValues(typeof(ComponentSlot)))
            {
                var def = _loadout.GetEquipped(slot);
                _slotValueLabels[slot].text = def != null ? def.Id : "—";
            }

            var stats = _loadout.ComputeFinalStats();
            foreach (var (type, label) in _statValueLabels.Select(kv => (kv.Key, kv.Value)))
                label.text = stats.GetValueOrDefault(type).ToString("0.##");

            for (var i = 0; i < _presetNameLabels.Count; i++)
            {
                var name = _loadout.GetPresetName(i);
                _presetNameLabels[i].text = string.IsNullOrEmpty(name) ? $"Preset {i + 1}: leer" : $"Preset {i + 1}: {name}";
            }
        }

        static VisualElement Panel()
        {
            var panel = new VisualElement();
            panel.style.backgroundColor = StyleTokens.BgPanel;
            panel.style.paddingLeft = panel.style.paddingRight = 16;
            panel.style.paddingTop = panel.style.paddingBottom = 16;
            panel.style.borderTopLeftRadius = panel.style.borderTopRightRadius = 8;
            panel.style.borderBottomLeftRadius = panel.style.borderBottomRightRadius = 8;
            return panel;
        }

        static Label Title(string text)
        {
            var label = new Label(text);
            label.style.color = StyleTokens.Ink;
            label.style.fontSize = 16;
            label.style.unityFontStyleAndWeight = FontStyle.Bold;
            return label;
        }

        static Button SmallButton(string text, Action onClick)
        {
            var button = new Button(onClick) { text = text };
            button.style.backgroundColor = StyleTokens.BgPanel2;
            button.style.color = StyleTokens.Ink;
            button.style.borderTopLeftRadius = button.style.borderTopRightRadius = 4;
            button.style.borderBottomLeftRadius = button.style.borderBottomRightRadius = 4;
            button.style.marginLeft = button.style.marginRight = 4;
            return button;
        }
    }
}
