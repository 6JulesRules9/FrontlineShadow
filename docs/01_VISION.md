# Frontline Shadow — Die Grund-Idee (Vision)

> **Status:** Kanonische Quelle. Dieses Dokument hält die ursprüngliche Idee
> möglichst originalgetreu und **ohne Inhaltsverlust** fest. Es ist die
> Referenz, auf die sowohl der Entwickler als auch Claude immer wieder
> zurückgreifen. Änderungen an der Vision werden hier nachgezogen, nicht in
> abgeleiteten Dokumenten.

---

## 1. Pitch / USP

Ein Single-Player-Game (später wahrscheinlich mit Multiplayer): **"Frontline
Shadow"**.

Der USP: **World of Tanks (WoT) meets Stylized**, aber mit einem
entscheidenden Twist:

- Man hat eine **Werkstatt** und nur **EINEN** Panzer.
- Die zentrale Side-Quest ist es, seinen Panzer **sehr stark** zu machen.
- Man sammelt **Materialien** ein und **craftet Bestandteile**.
- Man kann **selbst frei wählen**, wie man ihn upgraded. Jedes Upgrade wandelt
  seinen **"Hunter"** (so heißen die Panzer) in eine bestimmte Richtung.

Ähnlich wie in WoT gibt es Trade-offs: Baut man auf **Speed und Damage**, geht
die **Verteidigung stark verloren**. Baut man **full Defense**, macht man
**weniger Schaden und ist langsam**.

Weil man aber einen **frei customizebaren** Hunter hat, kann man theoretisch
auch Dinge bauen, die es in WoT **nicht** gäbe. Beispiel: **Alles in
Verteidigung** bauen → eine **Übermacht**, die unfassbar viel tanken kann —
nicht zu vergleichen mit Heavys in WoT, **viel** mehr. Dafür eben auch weniger
Schaden, sehr langsam etc.

→ Man hat mehr **unique Spielraum** und kann sich **seine Art von Tank selbst
zusammenbauen**.

---

## 2. Der Hunter — Aufbau & Bestandteile

Im Prinzip **"passt" jedes Bestandteil auf ein anderes** — denn man stellt die
Bestandteile ja **extra für seinen Hunter** her.

Jeder Hunter besteht aus **5 Komponenten**:

1. **Ketten**
2. **Core**
3. **Turm**
4. **Geschütz**
5. **Comms**

Rollen der Komponenten:

- **Core** wäre der Main-Fokus für **defensive** Hunter.
- **Comms** bestimmt, **wie weit man schauen kann**, **wie schnell Gegner
  aufgedeckt werden** und **wie langsam Gegner wieder aus dem Sichtfeld
  verschwinden**.

Man braucht natürlich **jede** Komponente, um einen guten Hunter zu haben.

---

## 3. Skill-Tree & Crafting

Es muss eine Art **Skill-Tree** geben, in dem man auswählt:

- **was** man herstellt,
- **sehen kann, wohin der Path geht**, wenn man mehr hineininvestiert, etc.

Über diesen Tree entscheidet man also die Richtung, in die die Komponenten
(und damit der Hunter) sich entwickeln.

---

## 4. Skills (Aktive Fähigkeiten)

Zusätzlich kann man **Skill-Punkte verteilen**.

**Regel:** Jeder Hunter kann immer nur **1 Skill** ausgerüstet haben.

**Skills sind Bestandteil-spezifisch** — egal, ob man Tank oder Glass Cannon
baut. Die Skills sind **immer an die Komponenten gebunden**.

Die Logik dahinter: Der Skill-Typ wirkt je nach Komponente unterschiedlich.

- **Agility auf Turm:** Der Turm macht etwas Schnelles.
- **Agility auf Ketten:** Man fährt schnell.
- **Agility auf Geschütz:** Cooldown geht runter.

### Beispiel-Skills

- **Agility / Ketten — "Turbo Start":** Man ist für die ersten **15s +100%
  Geschwindigkeit**.
- **Agility / Turm — "Flick Flack":** Dreht den Turm bei Aktivierung **fast
  instant um 180°**.
- **Power / Turm — "Concentrate":** Bei Aktivierung wird der Zielkreis
  **unfassbar präzise**, selbst beim Fahren, für **einen Schuss**. Läuft nach
  Aktivierung **10s** an — wenn man in der Zeit nicht schießt: **"Pech"**.
- **Power / Core — "Stationary":** Verankert den Tank für **5s** im Boden,
  erhöht die **Schuss-Präzision enorm**.

### Wichtiges Detail: Skills bleiben an der Komponente

Wenn man seinen Turm **upgraded**, **bleibt der Skill auf dem anderen Turm**.
Der Skill klebt also an der konkreten Komponente, nicht am Hunter.

---

## 5. Build-Beispiel: Ultra-Sniper-Tank

So kann man z. B. einen Ultra-Sniper-Tank bauen:

- **Fokus beim Ausbau auf Geschütz:** Extreme Firepower.
- **Secondary-Fokus auf Comms:** Hohe Sichtweite.
- **Power-Skill auf Core "Stationary":** Verankert den Tank für 5s im Boden,
  erhöht die Schuss-Präzision enorm.

**Ergebnis:** Der Hunter steht verankert und hat ein **10s-Time-Window**, um
einen **krassen Sniper-Damage-Schuss quer über die Map** zu wirken — dorthin,
wo der Gegner einen nicht sieht.

→ Und so ergeben sich **unzählige weitere Kombinationen**.

---

## 6. Pre-Battle: Anpassung & Presets

Man kann **vor jedem Gefecht** seinen Tank komplett anpassen, wie man will.

Beispiel: Runde 1 als **Variante 1 — Sniper-Tank** spielen, und nächste Runde
nach aufwändigem Umbau als **Variante 2 — Aufklärungs-/Speed-Tank**.

Damit der Umbau schnell geht, gibt es **Preset-Slots**.

---

## 7. Story

Eine **post-apokalyptische Welt**. Wenige **"Städte"** auf der Welt. Das meiste
sind nur **Ruinen und Ödland**.

Der Name **Frontline Shadow** ergibt sich daraus, dass man wie eine **einsame
Seele im Dunkeln** (und im **Nebel des Ödlands**) wie ein **Schatten** (keiner
weiß, dass es dich gibt) seine Ziele **jagt**.

Man ist ein **einsamer Wolf im Outback** — ein **"Frontline Shadow"** — eine
Bezeichnung für genau die Art Mensch, die man ist: ein **Überlebender**, der im
Grunde **"Roboter" jagt**.

Ein bisschen basic, aber die Gegner sind ja **"KI"-gesteuert** — ohne damit
KI im Sinne von ChatGPT zu meinen. Das passt gut zum Mood.

---

## 8. Gameplay-Loop

- Man **startet immer in seiner Garage** und kann so **"Level-artig"** die
  Etappen spielen.
- Fast ein bisschen **Tower-Defense-mäßig** schreitet man **Level für Level**
  fort, und die Gegner werden **immer stärker**.
- Anfangs sind es nur ein paar **fußläufige Roboter**, und man sitzt in einem
  **Mini-Tank** (quasi Level 1). **Alles startet klein.**
- Man wird stärker, und irgendwann sitzen auf einmal **Roboter auch in
  Hunter**.
- Und irgendwann gibt's z. B. **Mega-Tanks**, die riesig sind — vielleicht
  **3–5× so groß** wie normale Panzer (quasi als **Boss-Gegner**).

Man kann **vorher immer sehen**, gegen welche Panzer man ran muss.

**Beispiel Level 20:** 10× Fußsoldaten, 3× Speed Hunter, 1× Comm Hunter,
1× Mega Hunter.

→ Und kann sich **dementsprechend vorbereiten**.

---

## 9. Munition

Man kann sich auch überlegen, **welche Art von Munition** man herstellt. Es
gibt verschiedene mit verschiedener Wirkung, z. B.:

- **HS-Ammo (Highspeed)**
- **EX-Ammo (Explosive)** — gut gegen Flächen, z. B. 10× Fußsoldaten
- **RE-Ammo (Revealing Ammo)** — deckt nach Einschlag im Umkreis von X Metern
  die Gegner auf
- etc.

---

## 10. Real-Time-Catch (Zeit-/Ressourcen-Mechanik)

Es basiert auf **Real-Time**:

- Man kann nur **X Materialien am Tag** sammeln.
- Wenn die **Hunter-HP auf 0** sinken, muss man den Hunter **reparieren**. Das
  dauert **X** (anfangs vielleicht **15 Minuten**, später **24 Stunden**).
- Die HP, die man pro Mission verliert, **bleiben** sozusagen, bis man den
  Hunter in die Reparatur schickt. (Man kann das theoretisch auch nach nur
  **einer** Mission und nur **100 Schaden** machen — dann dauert es halt nur den
  **Anteil**.)

### Beschädigung pro Bestandteil

Die "Beschädigung" wird **pro Bestandteil** gemessen. D. h.:

- Wenn man **mehrere Türme** hat, kann man auf **Preset 2** wechseln und mit
  einem **"anderen" Hunter** spielen.
- Oder z. B. bei einem **Skill auf dem Turm** alles **außer den Turm**
  austauschen — dann sind es nur **X Stunden Gesamtbeschädigung geteilt durch 5
  Komponenten = X/5**, was der Turm warten muss. Somit geht's **schneller wieder
  los** — mit anderer Config außer dem Turm.

---

## 11. Multiplayer (später)

Später soll es natürlich **Multiplayer** geben. Da es etwas **taktischer** ist,
allerdings **nicht im großen Stil** wie in WoT, sondern in **kleinem Stil**:

- **2v2**, **5v5**
- Ganz später, wenn die Community es fordert, vielleicht auch ein **großer
  Modus** mit **15v15** (bei Special Events vielleicht sogar mal **30v30**).

### Alternative Modi

- **Wave Survival:** Solo / Duo oder mit **5 Spielern** auf einer Map mit immer
  mehr Gegnern — so lange wie möglich überleben.

### Finale Modus-Struktur (Zielbild)

- **Singleplayer**
- **Multiplayer**
  - **Classic & Ranked** (2v2 / 5v5)
  - **Classic & Ranked Endless**

---

## 12. Visuelle Richtung

Die visuelle Richtung orientiert sich am beigefügten **Character-Sheet**
("BRAT ICON — Panzer Kommandantin"): ein **dunkler, stylized** Look,
semi-realistisch, mit militärisch-cyberpunkigem Einschlag und einer
**Signatur-Marke** (Dreizack-/Kronen-Emblem). Dunkle Farbpalette
(Schwarz/Anthrazit mit wenigen kühlen Akzenten).

> (Siehe `05_ART_AUDIO_PIPELINE.md` für die ausführliche Art-Direction.)

---

## 13. Rollenverteilung & Arbeitsweise

- **Der Entwickler** übernimmt komplett: **UI- & Icon-Design, Stylized 3D Art,
  Sound & Musik.**
- **Claude** übernimmt komplett: **Coding.**
- Man muss sich gemeinsam überlegen, **wie man das aufbaut**.

Wichtiger Zusatz: Auch wenn der Entwickler Art/Sound/UI **selbst** kann, ist das
**nicht automatisch effizient** — alles selbst aufzubauen dauert sehr lange.
Daher der Wunsch, **mit Claude Design zu arbeiten**, um ggf. **Icons etc.**, die
**genau zum Stil passen**, schon **erzeugen** zu lassen.

---

## 14. Status der Vision

> Das ist die **Grund-Idee**. Schon viel — aber natürlich noch sehr wenig,
> wenn man sich vor Augen führt, was da alles später spezifiziert werden muss.
> Dieses Dokument ist der **Anker**, von dem aus alles Weitere abgeleitet wird.
