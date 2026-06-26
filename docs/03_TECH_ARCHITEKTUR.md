# Frontline Shadow — Technische Architektur

> Wie wir das Spiel coden. Engine ist durch die vorhandene Unity-`.gitignore`
> festgelegt: **Unity**. Dieses Dokument definiert Stack, Projektstruktur,
> Kern-Architektur, Datenmodell und die wichtigsten Systeme.

---

## 1. Stack-Entscheidungen

| Bereich | Wahl | Begründung |
|---------|------|-----------|
| **Engine** | **Unity 6 LTS** | Durch Repo vorgegeben; ideal für stylized 3D Vehicle-Combat, riesiges Ökosystem, C#, gute Multiplayer-Optionen. |
| **Render-Pipeline** | **URP** (Universal RP) | Bester Fit für *stylized* (nicht photoreal), performant, skaliert Richtung Multiplayer/mobile. HDRP wäre Overkill. |
| **Sprache** | **C#** | Unity-Standard; gut für KI-gestütztes Coding & Wartbarkeit. |
| **Input** | **Input System (neu)** | Rebindings, mehrere Devices, sauber für Vehicle-Controls. |
| **Daten/Definitionen** | **ScriptableObjects** | Komponenten, Skills, Munition, Gegner als designbare Assets (keine Hardcodes). |
| **Save/Persistenz** | **JSON + Versionierung** | Spielstand, Real-Time-Timer, Inventar, Presets, Tech-Tree-Fortschritt. |
| **Async/Loading** | **Addressables** | Asset-Streaming, Content-Updates ohne Vollbuild. |
| **Multiplayer (Phase 8)** | **server-authoritativ** — Kandidaten: Netcode for GameObjects (+ Unity Relay/Lobby/Game Server Hosting) **oder** Photon Fusion 2 | Kleine, taktische Matches; server-authoritativ gegen Cheating; Entscheidung später (siehe §9). |

> **Wichtig:** Multiplayer-Tauglichkeit ist **Architektur-Leitplanke ab Tag 1**
> (Simulation von Rendering trennen, deterministisch denkbare Stat-Berechnung),
> aber **kein Feature vor Phase 8**.

---

## 2. Architektur-Prinzipien

1. **Datengetrieben vor Code.** Jede Komponente/Skill/Ammo/Gegner ist ein
   ScriptableObject. Balancing = Asset-Tuning, kein Recompile.
2. **Komposition vor Vererbung.** Ein Hunter ist eine *Komposition* aus 5
   Modulen + ein Stat-Aggregator. Kein "TankTypeA : Tank".
3. **Simulation ⟂ Präsentation.** Gameplay-Logik (Stats, Schaden, Spotting)
   kennt kein Rendering. Erleichtert Tests **und** späteren server-autoritativen
   Multiplayer.
4. **Module entkoppelt via Assembly Definitions.** Schnelle Compiles, klare
   Abhängigkeiten, testbar.
5. **Events statt Spaghetti.** Lose Kopplung über ein leichtes Event-/Signal-
   System (z. B. ScriptableObject-Events oder ein zentraler EventBus).
6. **Single Source of Truth pro System.** Z. B. `EconomyService` besitzt alle
   Material-/Cap-Logik; UI liest nur.

---

## 3. Projektstruktur

```
Assets/
  _Project/
    Art/                 # vom Entwickler (Meshes, Materials, VFX, UI-Sprites, Icons)
    Audio/               # vom Entwickler (SFX, Musik)
    Data/                # ScriptableObject-Instanzen (Komponenten, Skills, Ammo, Gegner, Level, Configs)
      Components/  Skills/  Ammo/  Enemies/  Levels/  Balance/
    Prefabs/             # Hunter-Rig, Projektile, Gegner, UI-Prefabs
    Scenes/
      Bootstrap.unity    # lädt Services, dann Garage
      Garage.unity       # Meta: Loadout, Crafting, Tree, Presets, Reparatur
      Battle.unity       # Gefecht (additiv geladen, Map als Sub-Scene)
    Scripts/
      Core/              # asmdef: FrontlineShadow.Core — Utilities, EventBus, ServiceLocator, SaveSystem
      Tank/              # asmdef: ...Tank — Hunter-Komposition, Komponenten-Slots, StatAggregator
      Stats/             # asmdef: ...Stats — Stat-Definitionen, Modifier-Pipeline
      Skills/            # asmdef: ...Skills — Skill-Definition, Aktivierung, Effekte
      Crafting/          # asmdef: ...Crafting — Blueprints, Rezepte, Tech-Tree
      Economy/           # asmdef: ...Economy — Material, Inventar, Tagescap, Reparatur-Timer
      Combat/            # asmdef: ...Combat — Schaden, Projektile, Ammo-Verhalten
      Vision/            # asmdef: ...Vision — Sichtweite, Spotting, Tarnung
      AI/                # asmdef: ...AI — Gegnerverhalten (Behavior Trees / FSM)
      Missions/          # asmdef: ...Missions — Level-Defs, Wellen, Vorschau, Skalierung
      Player/            # asmdef: ...Player — Input, Steuerung Hull/Turm/Zielen
      UI/                # asmdef: ...UI — Garage-/Battle-UI (UI Toolkit)
      Save/              # asmdef: ...Save — Serialisierung, Versionierung, Real-Time-Persistenz
      Net/               # asmdef: ...Net — (Phase 8) Multiplayer-Abstraktion
      Bootstrap/         # asmdef: ...Bootstrap — App-Start, Service-Wiring
    Tests/
      EditMode/          # Unit-Tests (Stat-Aggregation, Reparatur-Mathe, Tree-Gating)
      PlayMode/          # Integrationstests (Hunter baut, fährt, schießt)
```

> **asmdef-Abhängigkeiten** zeigen *nach innen* auf `Core`/`Stats`; UI & AI
> hängen von Domänen-Assemblies ab, nie umgekehrt. So bleibt die Simulation
> UI-frei und (perspektivisch) netcode-fähig.

---

## 4. Datenmodell (ScriptableObjects)

### 4.1 Komponenten

```csharp
public enum ComponentSlot { Tracks, Core, Turret, Gun, Comms }

[CreateAssetMenu(menuName = "FS/Component")]
public class TankComponentDef : ScriptableObject {
    public string Id;                 // stabile ID für Saves
    public ComponentSlot Slot;
    public int Tier;
    public string PathId;             // Pfad-Identität im Tree (z.B. "gun.sniper")
    public List<StatModifier> Stats;  // Base-Stats + Multiplikatoren (inkl. Gewicht!)
    public CraftRecipe Recipe;        // Materialkosten
    public Sprite Icon;               // vom Entwickler / Claude-Design
    public GameObject VisualPrefab;   // Mesh-Variante
}
```

### 4.2 Stat-Pipeline

```csharp
public enum StatType { TopSpeed, Accel, HullTraverse, Hp, ArmorFront, Weight,
                       AlphaDamage, Penetration, ReloadTime, Dispersion, AimTime,
                       ViewRange, SpottingSpeed, SpottingPersistence, Camo, ... }

public enum ModOp { Flat, PercentAdd, PercentMult }

[Serializable] public struct StatModifier { public StatType Type; public ModOp Op; public float Value; }
```

`StatAggregator` sammelt alle Modifier der 5 Slots + aktiver Skill +
(temporäre) Buffs und berechnet Final-Stats. **Weight** koppelt Defense→Speed
(schwere Cores drücken Top-Speed) → erzeugt den WoT-Trade-off aus Daten.

### 4.3 Hunter-Instanz (Loadout)

```csharp
public class HunterLoadout {            // serialisierbar, gehört in den Save
    public string[] ComponentInstanceIds = new string[5]; // je Slot eine Bauteil-Instanz
    public string EquippedSkillInstanceId;                // 1 Skill, gebunden an EINE Komponenten-Instanz
    public AmmoLoadout Ammo;
    public string PresetName;
}
```

> **Wichtig (Vision-Treue):** Skills & Schaden kleben an **Bauteil-Instanzen**,
> nicht am Loadout. Eine `ComponentInstance` (im Save) trägt: `DefId`,
> `DamagePercent` (für Reparatur), optional `BoundSkillId`. So bleibt der Skill
> "auf dem alten Turm", wenn man den Turm tauscht.

### 4.4 Skills, Ammo, Gegner, Level
- `SkillDef`: Kategorie (Agility/Power/…), Ziel-Slot, Effekt-Daten, Cooldown,
  Wind-up/Dauer, Skalierung.
- `AmmoDef`: Typ (HS/EX/RE/…), Projektil-Verhalten, Schaden/Radius/Reveal.
- `EnemyDef`: Typ (Footsoldier/Hunter-Variante/Mega), Stats, KI-Profil.
- `LevelDef`: Wellen-Komposition (für Vorschau **und** Spawning), Map-Ref,
  Belohnungen.

---

## 5. Kern-Services (Bootstrap-gewired)

| Service | Verantwortung |
|---------|---------------|
| `SaveService` | Laden/Speichern, Schema-Versionierung, Real-Time-Timestamps |
| `EconomyService` | Material-Inventar, **Tagescap** (serverzeit-sicher), Crafting-Kosten |
| `RepairService` | **Pro-Komponente**-Schaden, Reparatur-Timer, anteilige Mathe, Preset-Ausweichen |
| `TechTreeService` | Freischaltungen, Pfade, Gating, Skill-Punkte |
| `LoadoutService` | Presets, Validierung (passt alles?), aktiver Hunter |
| `MissionService` | Level laden, Wellen-Vorschau, Skalierung, Ergebnis |
| `CombatSystem` | Schaden, Projektile, Ammo-Effekte (simulation-only) |
| `VisionSystem` | Sichtweite, Spotting, Persistenz, Tarnung |

> Zugriff via leichten **ServiceLocator** (oder DI). Services sind
> **UI-/Rendering-frei** und damit unit-testbar.

---

## 6. Real-Time-Persistenz (kritisch & knifflig)

Anforderungen aus der Vision: Tages-Material-Cap, Reparatur-Timer 15 min→24 h,
**pro Komponente** gemessen, anteilig, Preset-Ausweichen.

Umsetzung:
- Reparatur/Cap werden als **absolute Timestamps** (UTC) gespeichert, nicht als
  herunterzählende Sekunden → übersteht App-Schließen.
- Restzeit = `endTime − now`. Cap-Reset an Tagesgrenze (konfigurierbare TZ).
- **Anti-Cheat (Single-Player low-stakes):** zunächst Systemzeit; Hook für
  spätere Serverzeit (Multiplayer) vorgesehen.
- **Alles tunebar** über ein `BalanceConfig`-ScriptableObject (Caps, Zeiten,
  Deckel) → kein Code-Change fürs Balancing.

---

## 7. Combat & Vision (Phase 2–3)

- **Hunter-Rig-Prefab:** getrennte Transforms für Hull (Ketten/Core), Turm,
  Geschütz-Mündung. Controller mappt Input → Hull-Bewegung + Turm-Aim.
- **Schussberechnung:** Streuung aus Geschütz-Stats × Bewegungs-/Skill-Faktoren;
  Ray-/Projektil-Hit; Durchschlag vs. Panzerung; Alpha-Damage.
- **Ammo-Effekte:** EX = Radial-Damage; RE = Reveal-Puls über `VisionSystem`;
  HS = hohe Projektil-Velocity/Pen.
- **VisionSystem:** periodisches Spotting (Distanz ≤ Sichtweite, LoS), Persistenz-
  Timer pro gespottetem Ziel, Tarnwert reduziert Spot-Wahrscheinlichkeit.

---

## 8. KI (Phase 6)

- **Behavior Trees** (oder kompakte FSM) pro `EnemyDef`-Profil:
  Footsoldier (anrücken/schwärmen), Hunter (positionieren/feuern/kiten),
  Mega (Phasen, Schwachpunkte).
- KI nutzt **dieselben** Stat-/Combat-Systeme wie der Spieler-Hunter → KI in
  Hunter "sitzen" lassen ist trivial (Vision: "Roboter sitzen auch in Hunter").

---

## 9. Multiplayer-Pfad (Phase 8) — vorbereitet, nicht vorgebaut

- **Server-autoritativ.** Client schickt Input, Server simuliert,
  Clients interpolieren/predicten.
- **Tech-Entscheidung später**, Kandidaten:
  - **Netcode for GameObjects** + Unity **Relay/Lobby/Game Server Hosting** —
    nativ, gut integriert.
  - **Photon Fusion 2** — starke Prediction, skaliert gut Richtung 15v15/30v30.
- **Was wir schon jetzt richtig machen:** Simulation von Präsentation trennen,
  Stat-Berechnung deterministisch-fähig, keine Game-Logik in `Update()` von
  View-Komponenten.
- Modi-Reihenfolge: 2v2/5v5 (Classic) → Wave-Survival-Coop → Ranked → 15v15 →
  Events 30v30.

---

## 10. Qualität & Tooling

- **Tests:** EditMode für Stat-Aggregation, Reparatur-Mathe (anteilig, /5),
  Cap-Reset, Tree-Gating; PlayMode für "Hunter baut → fährt → schießt".
- **CI:** GitHub Actions (Unity-Builder Action / `game-ci`) für Build + Tests
  pro PR. (Optionaler SessionStart-Hook für Web-Sessions, siehe Roadmap.)
- **Editor-Tools:** Custom Inspectors für Tech-Tree-Authoring, Balance-Dashboard
  (Stat-Vorschau pro Build).
- **Style:** C#-Conventions, `nullable` wo sinnvoll, kleine Klassen, asmdef-
  Grenzen respektieren.

---

## 11. Risiken & Gegenmaßnahmen

| Risiko | Gegenmaßnahme |
|--------|---------------|
| Real-Time-Timer fühlt sich nach Mobile-Frust an | Presets entkoppeln Spielfluss; alles tunebar; faire Deckel; früh playtesten |
| Build-Balancing (Extrem-Builds) wird unfair | rein datengetrieben; Balance-Dashboard; viele kleine Stell-schrauben |
| Multiplayer-Refactor zu spät = teuer | Simulation/Präsentation ab Tag 1 trennen |
| Scope-Creep (zu viele Systeme früh) | strikte Phasen-Gates (siehe `04_ROADMAP.md`), spielbarer Kern zuerst |
| Solo-Dev-Asset-Last | Claude-Design für Icons/Concept (siehe `05_ART_AUDIO_PIPELINE.md`); Platzhalter-Art bis Systeme stehen |
