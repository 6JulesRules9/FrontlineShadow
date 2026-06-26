# Frontline Shadow — Style Tokens (Visual System v0.1)

> Die gemeinsame Token-Basis, auf die sich **jeder** Asset-Auftrag bezieht, damit
> Icons/UI/Concept eine **Familie** bilden statt Einzelbilder. Abgeleitet aus dem
> Character-Sheet ("Panzer Kommandantin"): dunkel, militärisch, stylized,
> hart/kantig, hoher Kontrast.
>
> v0.1 ist ein **Startpunkt** — du (Design-Hoheit) feinst die Werte, ich ziehe sie
> hier nach. Alle anderen Assets folgen dann diesen Tokens.

---

## 1. Farben

| Token | Hex | Verwendung |
|-------|-----|-----------|
| `--bg-base` | `#0E0F11` | App-Hintergrund (fast Schwarz) |
| `--bg-panel` | `#15171A` | Panels, Karten, Garage-Flächen |
| `--bg-panel-2` | `#1D2024` | erhöhte Flächen, Hover |
| `--ink` | `#E8EAED` | Primär-Linien/Icons auf Dunkel (Off-White) |
| `--ink-muted` | `#8A9099` | Gunmetal — sekundäre Linien, Detail, Inaktiv |
| `--ink-dim` | `#565B62` | Disabled, Hilfslinien |
| `--accent` | `#62D2E0` | Kühles Cyan — Highlights, Aktiv-Zustand, „Energie" (sparsam!) |
| `--accent-2` | `#3A8C97` | Accent gedimmt (Pressed/Trail) |
| `--danger` | `#E0556A` | Schaden, Warnung, Reparatur nötig |
| `--warn` | `#E0A24F` | Cap erreicht, Achtung |

> **Regel:** Akzent (`--accent`) ist **selten** — pro Icon max. **ein** Akzent-
> Detail. Das hält den dunklen, ernsten Mood und lenkt den Blick gezielt.

---

## 2. Geometrie & Linienführung

| Token | Wert | Hinweis |
|-------|------|---------|
| Icon-Canvas | `256 × 256` viewBox | quadratisch, transparenter Hintergrund |
| Safe-Padding | `≥ 32` units | Inhalt nicht an den Rand |
| Stroke (primär) | `9` units | bold, gut lesbar (auch klein in UI) |
| Stroke (sekundär) | `7` units | Detail in `--ink-muted` |
| `stroke-linejoin` | `round` | weiche Ecken trotz kantiger Formen |
| `stroke-linecap` | `round` | |
| Corner-Radius | `6–20` units | **kantig-militärisch**, nicht verspielt |
| Stil | **Line-Icons**, minimale Flächen | klare Silhouette zuerst |

---

## 3. Icon-Konventionen

- **Format:** SVG (Quelle) → in Unity als Sprite (UI Toolkit). SVG bleibt
  editierbar; bei Bedarf später PNG-Export (256²/512²).
- **Tinting:** Primärlinien können auf `currentColor` umgestellt werden, damit UI
  Zustände (aktiv/disabled) einfärben kann. v0.1 nutzt feste Token-Hex für die
  Vorschau.
- **Naming:** `slot[_path][_tier].svg`, z. B. `gun.svg`, später
  `gun_sniper_t2.svg`. IDs matchen die `Id`-Felder der ScriptableObjects.
- **Ablage:** `Assets/_Project/Art/Icons/components/…`,
  `…/skills/…`, `…/ammo/…`, `…/ui/…`, `…/tree/…`.

---

## 4. Asset-Bestand & Quelle

Die ersten handgemachten Platzhalter-SVGs wurden verworfen (Stilqualität nicht
ausreichend). Das **echte Icon-Set** entsteht über **Claude Design** bzw. die
Arbeitsdatei **`docs/Frontline Shadow.ai`** und folgt den obigen Tokens. Finale
Icons landen unter `Assets/_Project/Art/Icons/...` (benannt nach den
ScriptableObject-IDs).

---

## 5. Nächste Token-Schritte
1. Du bestätigst/feinst **Farben** (v. a. ob Cyan der richtige Akzent ist — oder
   z. B. ein wärmeres/kälteres Signature-Color zur Emblem-Marke passt).
2. **Emblem-Regel** festlegen (Dreizack/Krone): Mindestgröße, Clearspace,
   Mono-Variante — dann kann ich Brand-Varianten + Tier-Marker ableiten.
3. Danach: Icon-Set auf **Skills** (Agility/Power × Slot), **Ammo** (HS/EX/RE)
   und **Tech-Tree-Nodes** ausrollen — alles nach diesen Tokens.
