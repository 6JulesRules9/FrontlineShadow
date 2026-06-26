# Frontline Shadow — Art Direction, Audio & Asset-Pipeline

> Du machst **UI/Icon-Design, Stylized 3D Art, Sound & Musik** selbst — aber das
> ist viel Arbeit. Dieses Dokument hält die **visuelle Richtung** fest und
> definiert, **wie Claude dir Asset-Arbeit abnimmt** (Icons, Concept, Specs),
> damit der Stil konsistent bleibt und du schneller bist.

---

## 1. Art-Direction (aus dem Character-Sheet)

Referenz: Character-Sheet **"BRAT ICON — Panzer Kommandantin"**.

- **Mood:** dunkel, militärisch, leicht cyberpunkig, "stylized realism".
  Einsam, kühl, gefährlich. Passt zum "Frontline Shadow / einsamer Wolf"-Setting.
- **Farbpalette:** überwiegend **Schwarz/Anthrazit/Gunmetal**, dezente kühle
  Akzente, wenige helle Hervorhebungen. Hoher Kontrast, viel Negativraum.
- **Signatur-Marke:** **Dreizack-/Kronen-Emblem** ("Untouchable"-Vibe) — als
  Brand-Element auf Tanks, UI, Icons, Loading-Screens wiederholbar.
- **Charakter-Anker:** Protagonistin als "Kommandantin"-Figur — INTJ,
  strategisch, dominant. Zitat-Vibe: *"I don't follow orders. I give them."*
- **Fahrzeug-Look:** der Hunter im Sheet (Nr. "01") ist matt-schwarz, kantig,
  mit dem Emblem — guter Stil-Referenzpunkt für **alle** Spieler-Hunter.

### Stil-Leitplanken für 3D/URP
- Stylized, nicht photoreal → URP, klare Silhouetten, kontrollierte Specular-
  Highlights, evtl. leichter Rim-Light/Toon-Einschlag für Lesbarkeit im Nebel.
- **Silhouetten-First:** Builds (Sniper/Brawler/Scout/Übermacht) sollen schon an
  der Silhouette erkennbar sein — wichtig fürs Gameplay-Reading.
- Ödland/Nebel als atmosphärischer Layer (Fog, Volumetrics dezent), damit das
  Spotting-System auch *visuell* trägt.

---

## 2. Wie Claude bei Assets hilft ("Claude Design")

Claude kann dir **stilkonsistente 2D-Assets generieren** (Icons, UI-Elemente,
Concept-Art, Marken-Variationen) und alle **technischen Specs/Templates**
liefern. So arbeiten wir zusammen:

### 2.1 Wofür Claude gut einsetzbar ist (sofort)
- **Komponenten-Icons** (Ketten/Core/Turm/Geschütz/Comms) in einheitlichem Set.
- **Skill-Icons** (Agility/Power × Slot) als konsistente Familie.
- **Ammo-Icons** (HS/EX/RE …).
- **Tech-Tree-Node-Icons** & Pfad-Marker (Sniper/Brawler/Scout/Übermacht).
- **UI-Iconographie** (Reparatur, Material, Cap-Timer, Presets, Reveal).
- **Emblem-/Brand-Varianten** (Dreizack/Krone) für Fraktionen/Tiers.
- **Concept-Art** für Hunter-Varianten, Mega-Boss, Gegner-Roboter, Maps.

### 2.2 Konsistenz-Setup (einmalig, dann wiederverwendbar)
Damit alle generierten Assets zusammenpassen, legen wir ein **Style-Token-
Dokument** an (Farb-Hex, Linienstärke, Corner-Radius, Grid, Emblem-Regeln,
Icon-Raster z. B. 256², transparenter Hintergrund). Jeder Asset-Auftrag
referenziert diese Tokens → konsistente Familie statt Einzelbilder.

### 2.3 Arbeitsablauf
```
Du nennst Bedarf (z.B. "5 Komponenten-Icons, Tier-1, Outline-Stil")
  → Claude generiert nach Style-Tokens
  → du reviewst/feinst (oder gibst Korrektur-Prompt)
  → Export als PNG (+ ggf. SVG) ins Repo unter Assets/_Project/Art/Icons/...
  → in Unity als Sprite (URP-UI) eingebunden
```

> **Hinweis:** Bildgenerierung kann in dieser Session via Design-/Image-Tools
> erfolgen, sofern verfügbar. Wenn ein Generierungs-Tool gerade nicht
> erreichbar ist, liefert Claude stattdessen **präzise Prompt-Specs** +
> Platzhalter-Assets, mit denen du oder ein Bildmodell sofort produzieren kann.
> So blockiert Asset-Arbeit nie die Code-Phasen (Platzhalter → später ersetzen).

### 2.4 Arbeitsteilung (klar)
| Asset-Typ | Primär | Claude unterstützt mit |
|-----------|--------|------------------------|
| 3D-Meshes/Materials/VFX | **Du** | Specs, Naming, Import-Settings, Shader-Hinweise |
| Musik & SFX | **Du** | Cue-Listen, Trigger-Mapping (welcher Sound wann) |
| UI-Layout & Stil | **Du** | UI-Toolkit-Struktur, Bindings, Komponenten-Templates |
| 2D-Icons (Komp./Skill/Ammo/Tree/UI) | **Du oder Claude** | direkte Generierung nach Style-Tokens |
| Concept-Art / Moodboards | **Du oder Claude** | direkte Generierung / Varianten |
| Branding/Emblem-Varianten | **Du oder Claude** | direkte Generierung |

---

## 3. Asset-Konventionen (für den Code-Anschluss)

- **Ordner:** `Assets/_Project/Art/{Icons,Concept,Meshes,Materials,VFX,UI,Branding}`.
- **Icons:** quadratisch (z. B. 256²), transparenter PNG-Hintergrund, optional
  SVG-Quelle; benannt nach `slot_path_tier` (z. B. `gun_sniper_t2.png`).
- **Naming:** stabile, sprechende IDs — matchen die `Id`-Felder der
  ScriptableObjects, damit Code & Art sauber verdrahtet sind.
- **Import-Settings:** Sprites für UI (Mip off, passende Compression);
  Meshes mit konsistenter Skala/Pivot (Hull/Turm-Trennung beachten).
- **Platzhalter-Strategie:** Code-Phasen nutzen simple Platzhalter; finaler
  Art-Pass ersetzt sie ohne Code-Änderung (IDs bleiben stabil).

---

## 4. Audio (Kurz-Leitplanke)
- **Mood:** dunkel/ambient fürs Ödland, druckvoll im Gefecht, klare Skill-Cues.
- **Cue-Mapping** (Claude liefert die Liste): Schuss/Treffer/Durchschlag,
  Reveal, Skill-Aktivierung (je Skill ein Signatur-Cue), Reparatur fertig,
  Cap erreicht, Boss-Auftritt.
- **Tech:** Unity Audio (später ggf. FMOD/Wwise, falls nötig).

---

## 5. Nächster Asset-Schritt (Vorschlag)
1. **Style-Token-Dokument** gemeinsam festlegen (10 Minuten Input von dir:
   Haupt-Hex-Werte + Emblem-Regel).
2. Erste **Icon-Charge**: die 5 Komponenten-Icons (Tier 1) als Test der
   Konsistenz — passend zum Garage-UI aus Phase 1.
