using UnityEngine;

namespace FrontlineShadow.Combat
{
    /// <summary>
    /// Reine Schadens-Mathe (02_GAME_DESIGN.md §7, [VORSCHLAG]). Bewusst ohne
    /// MonoBehaviour gehalten (Simulation ⟂ Präsentation,
    /// 03_TECH_ARCHITEKTUR.md) und damit testbar.
    ///
    /// Phase-3-Modell (Schwellenwert, stylized lesbar): Durchschlag ≥ Panzerung
    /// → voller Alpha-Schaden; knapp darunter → linear abfallender Teilschaden
    /// bis zur Abpraller-Schwelle; weit darunter → 0 (Abpraller). Winkel/Modul-
    /// Schaden kommen optional später (siehe Doc).
    /// </summary>
    public static class DamageResolver
    {
        /// <summary>Anteil der Panzerung, ab dessen Unterschreitung gar kein Schaden mehr durchkommt.</summary>
        public const float RicochetThreshold = 0.5f;

        /// <summary>
        /// Liefert den tatsächlich verursachten Schaden für einen Treffer auf ein
        /// Ziel mit gegebener effektiver Panzerung.
        /// </summary>
        public static float Resolve(DamageInfo hit, float armor)
        {
            if (hit.AlphaDamage <= 0f) return 0f;
            if (armor <= 0f) return hit.AlphaDamage;

            var ratio = hit.Penetration / armor;

            if (ratio >= 1f) return hit.AlphaDamage;
            if (ratio <= RicochetThreshold) return 0f;

            // Linearer Übergang zwischen Abpraller-Schwelle und vollem Durchschlag.
            var t = Mathf.InverseLerp(RicochetThreshold, 1f, ratio);
            return hit.AlphaDamage * t;
        }
    }
}
