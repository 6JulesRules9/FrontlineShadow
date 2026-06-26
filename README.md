# Frontline Shadow

**WoT meets Stylized** — aber du hast eine **Werkstatt** und nur **EINEN**
Panzer (deinen "Hunter"). Sammle Material, crafte Bauteile und baue dir aus
**Ketten · Core · Turm · Geschütz · Comms** frei deinen eigenen Tank: vom
Glass-Cannon-Sniper bis zur unzerstörbaren "Übermacht". Ein post-apokalyptischer
Single-Player (später Multiplayer), in dem du als einsamer Schatten im Ödland
KI-gesteuerte Roboter jagst.

> Gebaut mit **Unity** (URP). Code: Claude. Art/UI/Sound: Entwickler.

---

## 📚 Dokumentation

Die Idee und der Bauplan leben in `docs/`. Reihenfolge zum Lesen:

| Doc | Inhalt |
|-----|--------|
| [`docs/01_VISION.md`](docs/01_VISION.md) | **Die Grund-Idee, originalgetreu & ohne Inhaltsverlust.** Kanonische Quelle. |
| [`docs/02_GAME_DESIGN.md`](docs/02_GAME_DESIGN.md) | Strukturiertes Game Design (Systeme, Skills, Ammo, Economy, Modi). |
| [`docs/03_TECH_ARCHITEKTUR.md`](docs/03_TECH_ARCHITEKTUR.md) | Technische Architektur (Unity-Stack, Projektstruktur, Datenmodell). |
| [`docs/04_ROADMAP.md`](docs/04_ROADMAP.md) | Phasen 0–9 mit Akzeptanz-Gates (was wann gebaut wird). |
| [`docs/05_ART_AUDIO_PIPELINE.md`](docs/05_ART_AUDIO_PIPELINE.md) | Art-Direction + wie Claude bei Icons/Concept hilft. |

> **Regel:** Ändert sich die *Idee*, wird zuerst `01_VISION.md` aktualisiert,
> dann die abgeleiteten Dokumente.

---

## 🎯 Status

**Phase 0 — Fundament (Code steht).** Vision + Plan komplett; Unity-Code-Gerüst
angelegt (Module/asmdefs, ServiceLocator, EventBus, Save-System, BalanceConfig,
EditMode-Tests, CI). Einmaliger Editor-Schritt offen → siehe [`SETUP.md`](SETUP.md).
Danach: **Phase 1 — Hunter-Komposition & Stats**.

## 🧭 Kurz-Pitch der Systeme
- **Ein Hunter, unendlich Builds** — 5 Komponenten, freie Spezialisierung, echte
  Trade-offs (Speed/Damage ⇄ Defense).
- **Skills kleben am Bauteil** — genau 1 aktiver Skill pro Hunter; Effekt hängt
  vom Slot ab (Agility/Turm = schneller Turm, Agility/Ketten = schnell fahren …).
- **Tech-/Skill-Tree** — craften, freischalten, Richtung sichtbar wählen.
- **Munition mit Wirkung** — HS / EX / RE …
- **Real-Time-Catch** — Tages-Material-Cap, Reparatur pro Bauteil (anteilig),
  Presets zum schnellen Umbau/Ausweichen.
- **Kampagne** — Level für Level, Wellen-Vorschau, skalierende Gegner bis zum
  3–5× großen **Mega Hunter**-Boss.
- **Später Multiplayer** — 2v2/5v5 (Classic & Ranked), Wave Survival, Events.
