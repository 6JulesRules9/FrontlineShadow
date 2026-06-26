# CLAUDE.md — Kontext-Anker für Frontline Shadow

> Diese Datei orientiert Claude in **jeder** Session. Kurz, stabil, verweist auf
> die Tiefe in `docs/`.

## Was ist das?
**Frontline Shadow** — stylized Tank-Game (WoT-inspiriert) in Unity. Du hast
**eine Werkstatt + EINEN frei baubaren Panzer ("Hunter")** aus 5 Komponenten
(**Ketten · Core · Turm · Geschütz · Comms**). Material sammeln → Bauteile
craften → frei spezialisieren (echte Trade-offs). Post-apokalyptischer
Single-Player (später Multiplayer), KI-Roboter als Gegner.

## Kanonische Quellen (immer zuerst lesen)
- `docs/01_VISION.md` — **die Idee, originalgetreu. Source of Truth.**
- `docs/02_GAME_DESIGN.md` — Systeme im Detail.
- `docs/03_TECH_ARCHITEKTUR.md` — wie wir coden (Unity-Architektur, Datenmodell).
- `docs/04_ROADMAP.md` — Phasen 0–9 + Gates. **Hier steht, was als Nächstes dran ist.**
- `docs/05_ART_AUDIO_PIPELINE.md` — Art-Direction + Asset-Zusammenarbeit.

## Rollen
- **Claude:** komplettes **Coding** (C#/Unity).
- **Entwickler:** UI/Icon-Design, Stylized 3D Art, Sound & Musik (Claude
  unterstützt bei 2D-Icons/Concept, siehe Art-Doc).

## Tech-Eckpunkte
- Unity 6 LTS, **URP**, C#, Input System, **ScriptableObject-getrieben**,
  Addressables, JSON-Save mit Real-Time-Timestamps.
- Prinzipien: **Komposition vor Vererbung**, **Daten vor Code**, **Simulation ⟂
  Präsentation** (für Tests & späteren server-autoritativen Multiplayer),
  **asmdef-Module**, alles **tunebar via BalanceConfig**.

## Arbeitsweise
- **Phasenweise** vorgehen (Roadmap), jede Phase mit grünem **Gate**, Projekt
  bleibt immer spielbar/testbar.
- Vision-Änderungen **zuerst in `01_VISION.md`** nachziehen, dann abgeleitete Docs.
- Tests für Mathe-lastige Systeme (Stat-Aggregation, Reparatur X/5, Tages-Cap).

## Git
- Entwicklungs-Branch: `claude/frontline-shadow-game-iy0cyp`.
- Keine PRs ohne explizite Aufforderung.
