# Setup — Frontline Shadow (Phase 0)

## ⚠️ Das Wichtigste zuerst
**Dieses Repo IST das Unity-Projekt.** Du legst **kein** separates Projekt an.
Du **öffnest diesen Ordner** in Unity — dann generiert Unity alles Fehlende
(`.meta`, `Library/`, restliche `ProjectSettings/`) selbst.

> Wenn du vorhin ein eigenes/leeres Unity-Projekt erstellt hast: das war ein
> Umweg, du kannst es ignorieren/löschen. (Hattest du dort schon echte Arbeit
> drin? Dann sag Bescheid — dann führen wir stattdessen *dein* Projekt mit
> meinem Code zusammen.)

---

## In 4 Schritten zum grünen Phase-0-Gate

### 1. Unity installieren
Unity Hub installieren → darüber **Unity `6000.4.0f1`** (deine Projekt-Version,
steht in `ProjectSettings/ProjectVersion.txt`). Hast du eine leicht andere
6000.x-Version? Auch ok — Unity passt die Projektdatei beim Öffnen an.

### 2. Diesen Ordner als Projekt öffnen
Unity Hub ▸ **Add ▸ Add project from disk** ▸ **diesen Repo-Ordner** wählen ▸
öffnen. Unity importiert, lädt die Packages (URP, Input System, Addressables,
Test Framework) und erzeugt die fehlenden Dateien. Bei Nachfragen
(Versions-Upgrade / Package-Resolve): **bestätigen**.

### 3. Ein Menüklick erledigt den Rest
Oben im Menü: **Tools ▸ Frontline Shadow ▸ Setup Phase 0**

Das legt automatisch an und verdrahtet:
- `BalanceConfig`-Asset
- Szenen **Bootstrap** + **Garage**
- `GameBootstrap` (mit zugewiesener BalanceConfig)
- Build-Settings-Reihenfolge (Bootstrap = 0, Garage = 1)

### 4. Prüfen
- `Assets/_Project/Scenes/Bootstrap.unity` öffnen ▸ **Play** ▸ in der Console
  erscheint `"[Bootstrap] Services bereit. ..."` und die (leere) Garage lädt.
- **Window ▸ General ▸ Test Runner ▸ EditMode ▸ Run All** → **10 grüne Tests**.

✅ **Gate erfüllt**, wenn: öffnet ohne Compile-Fehler · Bootstrap lädt Garage ·
Tests grün.

---

## Danach committen
Unity hat jetzt viele `.meta`- und `ProjectSettings/`-Dateien erzeugt — die
gehören ins Repo:

```bash
git add -A
git commit -m "chore: Unity-generierte Projektdateien + Phase-0-Szenen"
git push
```

`Library/`, `Temp/`, `*.csproj`, `*.sln` sind über `.gitignore` bereits
ausgeschlossen — die kommen nicht mit rein (ist korrekt so).

---

## Wenn etwas hakt
Bei **Compile-/Import-Fehlern** beim Öffnen: **Console-Meldung kopieren und mir
schicken** — ich fixe es. (Ich habe kein Unity hier und konnte daher nicht
selbst kompilieren; deshalb ist deine erste Rückmeldung wichtig.)

---

## Optional / später (nicht nötig fürs Phase-0-Gate)
- **URP-Look:** Erst relevant, wenn echte Visuals kommen (Phase 2+). Dann legen
  wir ein URP-Asset an und weisen es zu. Für Phase 0 (nur Services + leere
  Szenen + Tests) ist es egal.
- **CI grün (GitHub Actions):** braucht eine Unity-Lizenz als Secret
  (`UNITY_LICENSE`) — Anleitung: <https://game.ci/docs/github/activation>.
  Die `unityVersion` im Workflow ggf. an deine Version anpassen.
