# Setup — Frontline Shadow (Phase 0)

Dieses Repo enthält das **Code-Fundament** (Phase 0). Der C#-Code, die Module
(Assembly Definitions), Save-System, BalanceConfig, Tests, das Package-Manifest
und die CI sind fertig. Was nur **im Unity-Editor** geht (`.meta`-Dateien,
`Library/`, restliche `ProjectSettings/`, Szenen), machst du **einmalig** —
hier die Schritte.

> Hintergrund: Das Fundament wurde headless (ohne Editor) erstellt. Beim ersten
> Öffnen generiert Unity die fehlenden Editor-Dateien automatisch.

---

## 1. Unity installieren
- **Unity Hub** installieren, darüber **Unity 6 LTS** (`6000.0.x`).
- Die Version in `ProjectSettings/ProjectVersion.txt` (`6000.0.32f1`) ggf. an
  deine installierte 6000.0-Version anpassen — Unity bietet sonst beim Öffnen
  einen Upgrade an (ist ok).
- Beim Install **kein** zusätzliches Modul zwingend nötig (Build-Support später).

## 2. Projekt öffnen
1. Unity Hub ▸ **Add ▸ Add project from disk** ▸ diesen Repo-Ordner wählen.
2. Öffnen. Unity importiert, erzeugt `.meta`-Dateien + `Library/`, und löst die
   Packages aus `Packages/manifest.json` auf (URP, Input System, Addressables,
   Test Framework). Bei einer Versions-Nachfrage: **Resolve/Upgrade** bestätigen.

## 3. URP aktivieren
Falls noch keine Render-Pipeline gesetzt ist:
1. `Assets ▸ Create ▸ Rendering ▸ URP Asset (with Universal Renderer)`.
2. `Project Settings ▸ Graphics` ▸ das URP-Asset als **Default Render Pipeline**
   setzen (und unter `Quality` ebenfalls zuweisen).

## 4. BalanceConfig-Asset anlegen
- `Assets ▸ Create ▸ Frontline Shadow ▸ Balance Config`
- Ablage z. B. unter `Assets/_Project/Data/Balance/BalanceConfig.asset`.

## 5. Szenen anlegen (Bootstrap + Garage)
1. Zwei leere Szenen unter `Assets/_Project/Scenes/` speichern:
   `Bootstrap.unity` und `Garage.unity`.
2. In **Bootstrap**: leeres GameObject „Bootstrap" anlegen ▸ Komponente
   **GameBootstrap** anhängen ▸ das **BalanceConfig**-Asset zuweisen.
3. `File ▸ Build Settings`: **beide** Szenen hinzufügen —
   `Bootstrap` an **Index 0**, `Garage` an **Index 1**.
4. Play auf der Bootstrap-Szene → Console zeigt
   `"[Bootstrap] Services bereit. ..."` und lädt die (leere) Garage.

## 6. Tests laufen lassen
- `Window ▸ General ▸ Test Runner ▸ EditMode ▸ Run All`
- Erwartung: **10 grüne Tests** (ServiceLocator, EventBus, SaveService).

## 7. Erstes Commit nach dem Öffnen
Unity hat jetzt viele `.meta`- und `ProjectSettings/`-Dateien erzeugt. Diese
**committen** (sie gehören ins Repo). `Library/`, `Temp/`, `*.csproj`, `*.sln`
sind bereits über `.gitignore` ausgeschlossen.

```bash
git add -A
git commit -m "chore: Unity-generierte Projektdateien (meta, ProjectSettings, Szenen)"
git push
```

---

## 8. CI grün bekommen (GitHub Actions)
Die CI (`.github/workflows/ci.yml`) nutzt **game-ci** und braucht eine
Unity-Lizenz als Secrets. Einmalig einrichten:

1. **Aktivierungsdatei erzeugen** nach der game-ci-Anleitung
   (<https://game.ci/docs/github/activation>): erzeugt eine `.alf`, die du auf
   der Unity-Lizenzseite gegen eine `.ulf` eintauschst.
2. In GitHub ▸ **Settings ▸ Secrets and variables ▸ Actions** anlegen:
   - `UNITY_LICENSE` = kompletter Inhalt der `.ulf` (Personal-Lizenz), **oder**
   - `UNITY_EMAIL` + `UNITY_PASSWORD` (Plus/Pro).
3. `unityVersion` im Workflow muss zur Projekt-Version passen
   (`6000.0.32f1` — bei Anpassung in Schritt 1 hier mitziehen).

Danach läuft die CI bei jedem Push auf `claude/**` und `main` und führt die
EditMode-Tests aus.

---

## Phase-0-Gate ✅ (erfüllt, wenn …)
- [ ] Projekt öffnet in Unity 6 ohne Compile-Fehler.
- [ ] `Bootstrap` lädt die (leere) `Garage`-Szene.
- [ ] EditMode-Tests laufen **grün** (lokal und in CI).

Wenn beim Öffnen ein Compile-/Import-Fehler auftaucht: **schick mir die
Console-Meldung**, dann fixe ich's. Danach geht's an **Phase 1** (Hunter-
Komposition & Stats — das erste, was du in der Garage anfassen kannst).
