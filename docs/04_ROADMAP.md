# Frontline Shadow — Roadmap & Meilensteine

> Inkrementeller Aufbau: **immer ein spielbarer/testbarer Kern** zuerst, dann
> Tiefe. Jede Phase hat ein **Gate** (Akzeptanzkriterium). Erst wenn das Gate
> grün ist, geht's weiter. Reihenfolge ist nach Abhängigkeiten optimiert.

Legende: 🎯 Ziel · ✅ Gate (fertig wenn …) · 🧱 Hauptarbeit (Claude/Coding) ·
🎨 Entwickler-Assets

---

## Phase 0 — Fundament & Projekt-Setup
🎯 Lauffähiges, leeres Unity-Projekt mit sauberer Architektur-Basis.
🧱
- Unity 6 LTS + URP einrichten, Input System, Addressables.
- Ordnerstruktur + Assembly Definitions (`Core`, `Stats`, … siehe Architektur).
- `Bootstrap`-Szene + ServiceLocator + leeres `SaveService` (JSON, versioniert).
- Test-Setup (EditMode/PlayMode) + GitHub Actions CI (Build + Tests).
- `BalanceConfig`-ScriptableObject-Skelett.

✅ **Gate:** Projekt buildet via CI; `Bootstrap` lädt `Garage` (leer); ein
Dummy-EditMode-Test läuft grün.

---

## Phase 1 — Hunter-Komposition & Stats (Meta, ohne Kampf)
🎯 In der Garage einen Hunter aus 5 Komponenten bauen und Final-Stats sehen.
🧱
- `TankComponentDef`, `StatModifier`, `StatAggregator` (inkl. Weight→Speed-Kopplung).
- `HunterLoadout` + `ComponentInstance` (Schaden/Skill-Bindung vorgesehen).
- `LoadoutService` + **Preset-Slots** (speichern/laden/validieren).
- Erste Garage-UI (UI Toolkit): Slots, Bauteil-Auswahl, Stat-Panel, Presets.
🎨 Platzhalter-Icons je Slot (oder Claude-Design-Erstcharge).

✅ **Gate:** Spieler tauscht Komponenten, sieht Stats live, speichert ≥2 Presets,
lädt sie nach Neustart korrekt (Save funktioniert).

---

## Phase 2 — Fahrbarer Hunter (spielbarer Kern!)
🎯 Hunter in einer Test-Map fahren, Turm drehen, zielen, schießen.
🧱
- Hunter-Rig-Prefab (Hull/Turm/Mündung getrennt), `PlayerController` (Input System).
- Bewegung aus Mobilitäts-Stats; Turm-Aim; einfacher Schuss + Projektil.
- `Battle`-Szene additiv; Wechsel Garage↔Battle.
🎨 Erstes Hunter-Mesh/Rig (oder Platzhalter-Box-Rig).

✅ **Gate:** Loadout-Änderungen wirken spürbar aufs Fahr-/Schussverhalten;
"Garage → Gefecht → Garage" Loop steht.

---

## Phase 3 — Kampf-Tiefe: Schaden, Ammo, Vision, Skills
🎯 Der Kampf bekommt Identität (Trade-offs werden fühlbar).
🧱
- Schadensmodell (Durchschlag vs. Panzerung, Alpha, Streuung/Zielzeit).
- **Ammo-System** (HS/EX/RE) + Loadout-Auswahl.
- **VisionSystem** (Sichtweite, Spotting, Persistenz, Tarnung) — Comms wirkt.
- **Skill-System**: 1 aktiver Skill, an Bauteil-Instanz gebunden; die vier
  Vision-Skills (Turbo Start, Flick Flack, Concentrate, Stationary).
🎨 VFX/SFX für Schuss, Treffer, Reveal, Skill-Aktivierung.

✅ **Gate:** Sniper-Build (Geschütz+Comms+Stationary) vs. Speed-Build (Ketten+
Turbo Start) spielen sich klar unterschiedlich; RE-Ammo deckt auf.

---

## Phase 4 — Crafting & Skill-/Tech-Tree
🎯 Spieler schaltet frei, craftet, spezialisiert die Richtung.
🧱
- `TechTreeService` (Nodes, Pfade, Gating, Skill-Punkte).
- `CraftRecipe` + Material-Inventar (Economy-Grundlage).
- Tech-Tree-UI (Pfade sichtbar: Sniper/Brawler/Scout/Übermacht …).
- Editor-Tool zum Authoring des Trees.
🎨 Node-/Pfad-Icons (starker Claude-Design-Kandidat).

✅ **Gate:** Spieler schaltet ein Bauteil frei, craftet es aus Material, baut es
ein — Pfad-Richtung ist im Tree sichtbar.

---

## Phase 5 — Real-Time-Meta (das "Catch")
🎯 Zeit & Reparatur bekommen Gewicht.
🧱
- `RepairService`: **pro-Komponente**-Schaden, anteilige Reparaturzeit (X/5-
  Logik), Timer als UTC-Timestamps.
- `EconomyService`: **Tages-Material-Cap** mit Reset.
- Preset-Ausweichen während Reparatur; UI für Timer/Cap.
- Alles über `BalanceConfig` tunebar.

✅ **Gate:** Beschädigter Turm blockiert nur diesen; mit Preset (anderer Turm)
sofort weiterspielbar; Reparaturzeit überlebt App-Neustart korrekt.

---

## Phase 6 — Kampagne, Gegner-KI & Bosse
🎯 Der eigentliche Singleplayer-Loop: Level für Level, skalierende Gegner.
🧱
- `MissionService` + `LevelDef` (Wellen-Komposition).
- **Wellen-Vorschau** vor dem Level (z. B. "10× Fußsoldaten, 3× Speed Hunter,
  1× Comm Hunter, 1× Mega Hunter").
- Gegner-KI (Behavior Trees): Footsoldier, Hunter-Varianten, **Mega Hunter**
  (3–5× Größe, Boss).
- Skalierungs-Kurve, Belohnungen → Economy/Tree.
🎨 Gegner-Meshes (Roboter, Hunter-Varianten, Mega), Map-Art.

✅ **Gate:** Vertikale Scheibe: 5–10 Level am Stück spielbar mit sichtbarer
Schwierigkeitssteigerung und einem Boss.

---

## Phase 7 — Content, Balancing & Polish
🎯 Aus dem Prototyp wird ein rundes Singleplayer-Spiel.
🧱
- Mehr Komponenten/Skills/Ammo/Gegner (datengetrieben).
- Balance-Dashboard, Progressionskurve, Tuning.
- Story-Layer (Garage-Funde/Logs), Menüs, Save-Slots, Optionen, Audio-Mix.
🎨 Volle UI, Icons, finaler Art-Pass, Musik.

✅ **Gate:** Spielbare Demo-Kampagne (Anfang→erster Mega-Boss) ohne Blocker;
Onboarding verständlich.

---

## Phase 8 — Multiplayer-Fundament
🎯 Kleiner, taktischer PvP-Kern.
🧱
- Netcode-Entscheidung (NGO+Relay vs. Photon Fusion 2) + Spike.
- Server-autoritative Simulation, Lobby/Matchmaking, 2v2 → 5v5.
- Anti-Cheat-Basis (Serverzeit für Real-Time-Systeme).

✅ **Gate:** Stabiles 2v2-Match über Netz mit korrekter Stat-/Schaden-
Synchronisation.

---

## Phase 9 — Modi-Ausbau & Live
🎯 Volle Modus-Struktur.
🧱
- Wave-Survival-Coop (Solo/Duo/5p), Classic **& Ranked**, Endless-Ranked.
- Später: 15v15; Event-Modus 30v30.
- Live-Ops-Basis (Seasons/Events), Telemetrie/Balancing-Pipeline.

✅ **Gate:** Singleplayer + Multiplayer (Classic & Ranked, Endless) live-fähig.

---

## Querschnitt (läuft mit, jede Phase)
- **Tests** für jedes neue System (besonders Stat-/Reparatur-/Cap-Mathe).
- **Doku aktuell halten:** Vision-Änderungen → `01_VISION.md` zuerst.
- **Asset-Pipeline:** Claude-Design für Icons/Concept parallel zu Systemen
  (siehe `05_ART_AUDIO_PIPELINE.md`).

---

## Sofort-nächste Schritte (sobald du "go" sagst)
1. **Phase 0 Scaffolding** im Repo anlegen (Ordner, asmdefs, Bootstrap, Save-
   Skelett, CI-Workflow, BalanceConfig).
2. Optional: **SessionStart-Hook** für Web-Sessions (damit Tests/Builds in
   Claude-Code-Web laufen).
3. Danach **Phase 1** (Komposition & Stats) — das erste, was du in der Garage
   wirklich anfassen kannst.

> Empfehlung: Wir gehen **eine Phase nach der anderen**, jeweils mit grünem
> Gate, statt breit anzufangen. Das hält das Projekt jederzeit spielbar/testbar.
