# Frontline Shadow — Game Design Document (GDD)

> Strukturierte Aufschlüsselung der Vision in konkrete Systeme. Wo die Vision
> offen ist, gibt es **[VORSCHLAG]**-Markierungen — das sind Defaults, die wir
> jederzeit ändern können. Quelle der Wahrheit für die *Idee* bleibt
> `01_VISION.md`.

---

## 1. Design-Säulen (Pillars)

1. **Ein Hunter, unendlich Builds.** Tiefe entsteht nicht durch viele Fahrzeuge,
   sondern durch radikal freie Spezialisierung *eines* Fahrzeugs.
2. **Echte Trade-offs.** Jede Stärke kostet woanders. Extrem-Builds (z. B.
   "Übermacht"-Full-Tank) sind erlaubt und fühlen sich extrem an.
3. **Vorbereitung schlägt Reflexe.** Man sieht die Gegnerwelle vorher und baut
   gezielt dagegen. Der "Aha-Moment" ist der Loadout-Plan, nicht nur das Gefecht.
4. **Zeit hat Gewicht.** Real-Time-Mechaniken (Material-Cap, Reparatur) machen
   Entscheidungen bedeutsam, ohne den Spieler zu bestrafen, der clever Presets
   nutzt.
5. **Einsamer Schatten.** Mood: dunkel, still, gejagt-jagend. Du existierst
   offiziell nicht.

---

## 2. Der Hunter — Komponenten-Modell

Ein Hunter = **Komposition aus 5 Komponenten-Slots**. Jede Komponente trägt
Stats bei; die Summe ergibt das Fahrverhalten.

| Slot | Primärer Einfluss | Build-Identität |
|------|-------------------|-----------------|
| **Ketten** | Speed, Beschleunigung, Drehrate (Hull), Traverse, Gelände | Mobilität |
| **Core** | HP, Panzerung, Gewicht, Modul-Slots/Energie | Defensive |
| **Turm** | Turm-Drehgeschwindigkeit, Turm-Panzerung, Zielgenauigkeit beim Drehen | Reaktion/Schutz |
| **Geschütz** | Schaden, Durchschlag, Reichweite, Nachladezeit, Streuung | Firepower |
| **Comms** | Sichtweite, Aufklär-Geschwindigkeit, Spotting-Persistenz, Tarnung | Information |

### 2.1 Stat-Modell [VORSCHLAG]

Jede Komponente liefert einen Satz **Base-Stats** + optionale **Modifier**.
Final-Stat = `(Σ Base) × (Π Multiplikatoren) + Σ Flat-Boni`. Damit lassen sich
Extrem-Builds sauber abbilden (z. B. Core mit +250 % HP, aber −40 % Speed-Malus
durch Gewicht).

Kern-Stats (erste Iteration):

- **Mobilität:** Top-Speed, Beschleunigung, Hull-Traverse, Geländegängigkeit.
- **Defense:** HP, Panzerung (Front/Seite/Heck), Gewicht (beeinflusst Speed).
- **Firepower:** Alpha-Damage, Durchschlag, DPM (über Reload), Streuung,
  Zielzeit, max. Reichweite.
- **Information:** Sichtweite, Spotting-Speed, Spotting-Persistenz, Tarnwert.

> **Gewicht** ist der zentrale Kopplungs-Stat: schwere Defense-Teile drücken die
> Mobilität — so entsteht der WoT-artige Trade-off automatisch aus Daten.

### 2.2 Komponenten-Tiers / Pfade

Jede Komponente existiert in **Tiers** (T1 → Tn) und in **Pfad-Varianten**
innerhalb des Skill-Trees (z. B. Geschütz-Pfad "Sniper" vs. "Brawler" vs.
"Autoloader"). Höhere Tiers ≠ strikt besser, sondern **spezialisierter**.

---

## 3. Skill-System (Aktive Fähigkeiten)

**Regel (aus Vision):** Genau **1 Skill aktiv pro Hunter**. Skills sind **an die
konkrete Komponente gebunden** (klebt am Bauteil, nicht am Tank). Upgradet man
den Turm, bleibt der Skill auf dem alten Turm.

### 3.1 Skill-Kategorien × Komponente

Der **Effekt einer Kategorie hängt vom Bauteil ab**:

| Kategorie | Ketten | Turm | Geschütz | Core | Comms |
|-----------|--------|------|----------|------|-------|
| **Agility** | "Turbo Start": 15s +100 % Speed | "Flick Flack": Turm ~instant 180° | Reload-Cooldown ↓ | (z. B. schnellere Reparatur-Tick) | schnelleres Spotting |
| **Power** | (z. B. Ramming-Boost) | "Concentrate": 1 Schuss extrem präzise, auch fahrend; 10s Fenster, sonst "Pech" | Alpha-Burst | "Stationary": 5s verankert, Präzision ↑↑ | Reveal-Puls |

> Tabelle ist eine **Erweiterung** der Vision-Beispiele auf das volle Raster.
> Vision-kanonische Skills: Turbo Start, Flick Flack, Concentrate, Stationary.
> Der Rest ist **[VORSCHLAG]** und kann frei umgestaltet werden.

### 3.2 Skill-Mechanik (gemeinsames Gerüst)

- **Aktivierung** per Tastendruck (Cooldown).
- **Cast/Wind-up** möglich (z. B. Concentrate: 10s Fenster, ein Schuss).
- **Dauer** + **Effekt-Stärke** skalieren ggf. mit Tier/Skill-Punkten.
- Da nur 1 Skill aktiv ist, ist die Wahl **strategisch** und Teil des Loadouts.

### 3.3 Skill-Punkte

Skill-Punkte schalten Skills frei / verstärken sie. Verteilung im Skill-Tree
(siehe §4). [VORSCHLAG] Punkte aus Level-/Missions-Fortschritt.

---

## 4. Skill-Tree & Crafting

Ein **Tech-/Skill-Tree** erfüllt zwei Funktionen gleichzeitig:

1. **Was kann ich herstellen?** Nodes = Blueprints für Komponenten/Munition.
2. **Wohin geht der Pfad?** Der Tree zeigt sichtbar die Richtung jeder
   Investition (Sniper, Brawler, Scout, Übermacht ...).

### 4.1 Aufbau [VORSCHLAG]

- Ein **Ast pro Komponente** (Ketten / Core / Turm / Geschütz / Comms).
- Innerhalb jedes Astes **Verzweigungen** (Pfad-Identitäten).
- **Nodes** schalten frei: Blueprint (craftbar), Stat-Upgrade, oder Skill.
- Tiefer investieren = stärkere, aber spezialisiertere Optionen + Trade-off-
  Malus sichtbar.

### 4.2 Crafting-Loop

```
Material sammeln  →  Blueprint im Tree freischalten  →  Komponente craften
        →  in Loadout einbauen  →  testen im Gefecht  →  weiter spezialisieren
```

- **Materialien:** mehrere Typen [VORSCHLAG] (z. B. Schrott, Legierung,
  Elektronik, seltene Kerne), die unterschiedliche Komponenten/Tiers gaten.
- **Crafting kann Zeit/Material kosten** und mit dem Real-Time-System koppeln
  (siehe §8).

---

## 5. Munition

Munition ist **craftbar** und Teil der Pre-Battle-Vorbereitung. Man wählt, was
und wie viel man mitnimmt.

| Typ | Bedeutung | Wirkung |
|-----|-----------|---------|
| **HS** | Highspeed | Hohe Projektilgeschwindigkeit / Durchschlag (gut gegen schnelle/gepanzerte Einzelziele) |
| **EX** | Explosive | Flächenschaden — gut gegen Gruppen (z. B. 10× Fußsoldaten) |
| **RE** | Revealing | Deckt nach Einschlag im Umkreis von X m Gegner auf |
| ... | weitere | erweiterbar (z. B. AP, Brand, EMP) [VORSCHLAG] |

[VORSCHLAG] **Munitions-Loadout:** begrenzte Slots/Mengen pro Gefecht; Wechsel
im Kampf mit Nachladezeit. Munition wird aus Material gecraftet → koppelt an
Economy & Real-Time-Cap.

---

## 6. Vision / Spotting-System (Comms-getrieben)

WoT-inspiriert, aber stylized lesbar:

- **Sichtweite:** maximale Distanz, auf der Gegner überhaupt aufgedeckt werden.
- **Spotting-Speed:** wie schnell ein Gegner im Sichtfeld aufgedeckt wird.
- **Spotting-Persistenz:** wie lange ein aufgedeckter Gegner sichtbar bleibt,
  nachdem er aus dem Sichtfeld ist (Vision: "langsamer wieder verschwinden").
- **Tarnung:** wie schwer man selbst zu spotten ist.
- **RE-Ammo** und Comms-Skills erzeugen aktive Reveal-Effekte.

→ Ermöglicht den **Sniper/Scout**-Fantasy: aus dem Unsichtbaren heraus treffen.

---

## 7. Kampf-Modell [VORSCHLAG]

- **Echtzeit-3D**, Third-Person, ein steuerbarer Hunter.
- **Getrennte Steuerung** Hull (Ketten) vs. Turm; Zielen mit Reticle.
- **Streuung/Zielzeit** abhängig von Bewegung, Geschütz, Skills (Concentrate /
  Stationary reduzieren Streuung drastisch).
- **Schadensmodell:** Durchschlag vs. Panzerung (Winkel optional später),
  Alpha-Damage, ggf. Modul-/Komponenten-Schaden.
- **Tower-Defense-Touch:** Gegner kommen wellen-/etappenartig; Map mit
  Verteidigungs-/Sniper-Positionen.

---

## 8. Real-Time-Meta (das "Catch")

Kern-Spannungsbogen der Vision — Zeit ist eine Ressource.

### 8.1 Material-Cap
- Pro **Tag** nur **X Materialien** sammelbar (Soft-Cap). Fördert tägliche
  Rückkehr, ohne Grind-Zwang.

### 8.2 Schaden & Reparatur — **pro Komponente**
- HP-Verlust pro Mission **bleibt bestehen**, bis man repariert.
- Reparatur dauert **real-time** (anfangs ~15 min, später bis 24 h).
- **Anteilig:** 100 Schaden → nur anteilige Reparaturzeit.
- **Beschädigung wird pro Bauteil gemessen.**

**Beispiel (aus Vision):** Skill klebt auf dem Turm → man tauscht alles **außer
dem Turm** aus. Gesamtbeschädigung `X` h / 5 Komponenten = `X/5` h, die nur der
Turm warten muss → man ist schneller wieder im Gefecht (mit anderer Config).

### 8.3 Presets
- **Preset-Slots** speichern komplette Loadouts → schneller Umbau vor dem
  Gefecht und Ausweichen auf "anderen Hunter", während Bauteile in Reparatur
  sind.

> **Design-Hinweis:** Diese Mechanik ist mächtig **und** heikel (Mobile-/Idle-
> Game-Gefühl vs. Frust). Wir bauen sie **datengetrieben & tunebar** (alle
> Zeiten/Caps als Config), damit Balancing leicht ist. Faire Defaults:
> Reparaturzeit deckelbar, Presets entkoppeln Spielfluss vom Timer.

---

## 9. Missions- / Kampagnen-Struktur

- Start **immer in der Garage**.
- **Level für Level** (Tower-Defense-artig), Gegner skalieren hoch.
- **Vorschau** der Gegnerzusammensetzung pro Level → gezielte Vorbereitung.

**Beispiel Level 20:** 10× Fußsoldaten, 3× Speed Hunter, 1× Comm Hunter,
1× Mega Hunter.

### 9.1 Gegner-Typen
- **Fußsoldaten** (Roboter, früh, schwach, in Gruppen).
- **Hunter** (KI in Panzern — Speed/Comm/etc.-Varianten, spiegeln Spieler-
  Builds).
- **Mega Hunter** (Boss, 3–5× Größe normaler Panzer).

### 9.2 Skalierung [VORSCHLAG]
- Frühe Level: Fußsoldaten + Mini-Tank-Spieler.
- Mittlere: gemischte Hunter.
- Späte: Mega-Bosse + komplexe Mischwellen.

---

## 10. Story / Setting

- Post-apokalyptisch, Ödland + Ruinen, wenige Städte, Nebel/Dunkelheit.
- Spieler = **Frontline Shadow**: einsamer Wolf, existiert offiziell nicht,
  jagt KI-gesteuerte Roboter.
- Protagonistin-Referenz: **"Panzer Kommandantin"** (Character-Sheet) als
  visueller/narrativer Anker. [VORSCHLAG] Story über Garage-Funde, Logs,
  Sammelobjekte erzählt — minimalistisch, mood-getragen.

---

## 11. Modi & Multiplayer (Zielbild)

- **Singleplayer** (zuerst, voller Fokus).
- **Multiplayer** (später, kleiner Maßstab → taktisch):
  - **Classic & Ranked:** 2v2, 5v5.
  - **Classic & Ranked Endless** (Wave Survival): Solo / Duo / 5 Spieler.
  - **Später / Events:** 15v15, Special 30v30.

> Multiplayer ist **Architektur-Leitplanke ab Tag 1** (siehe
> `03_TECH_ARCHITEKTUR.md` §Netcode), aber **Feature erst ab Phase 8**.

---

## 12. Offene Design-Fragen (zu klären, blockieren nicht)

1. **Material-Typen & Cap-Werte** — wie viele Sorten, welche Tagescaps?
2. **Steuerungs-Feeling** — arcade-näher oder simulationsnäher?
3. **Progression-Quelle der Skill-Punkte** — nur Kampagne, auch Challenges?
4. **Permadeath/Soft-Fail?** — was passiert bei Missions-Niederlage genau?
5. **Monetarisierung/Scope** — reines Premium-Singleplayer zuerst? (beeinflusst
   nichts an der Technik-Basis, aber an späteren Systemen).

> Diese Fragen sind bewusst geparkt. Sie sind **nicht** auf dem kritischen Pfad
> für den Prototyp und werden beim Balancing entschieden.
