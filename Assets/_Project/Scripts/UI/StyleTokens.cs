using UnityEngine;

namespace FrontlineShadow.UI
{
    /// <summary>Farb-Tokens aus 06_STYLE_TOKENS.md §1 — eine Quelle für alle code-gebauten UIs.</summary>
    public static class StyleTokens
    {
        public static readonly Color BgBase = FromHex(0x0E, 0x0F, 0x11);
        public static readonly Color BgPanel = FromHex(0x15, 0x17, 0x1A);
        public static readonly Color BgPanel2 = FromHex(0x1D, 0x20, 0x24);
        public static readonly Color Ink = FromHex(0xE8, 0xEA, 0xED);
        public static readonly Color InkMuted = FromHex(0x8A, 0x90, 0x99);
        public static readonly Color InkDim = FromHex(0x56, 0x5B, 0x62);
        public static readonly Color Accent = FromHex(0x62, 0xD2, 0xE0);
        public static readonly Color Accent2 = FromHex(0x3A, 0x8C, 0x97);
        public static readonly Color Danger = FromHex(0xE0, 0x55, 0x6A);
        public static readonly Color Warn = FromHex(0xE0, 0xA2, 0x4F);

        static Color FromHex(byte r, byte g, byte b) => new Color32(r, g, b, 255);
    }
}
