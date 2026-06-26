using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FrontlineShadow.Tank
{
    /// <summary>
    /// Liste aller im Spiel verfügbaren Bauteile. Phase 1 hat noch kein
    /// Crafting/Freischalten (Phase 4) — alles hier Gelistete ist sofort
    /// equippbar.
    /// </summary>
    [CreateAssetMenu(menuName = "Frontline Shadow/Component Catalog", fileName = "ComponentCatalog")]
    public class ComponentCatalog : ScriptableObject
    {
        public List<TankComponentDef> Components = new();

        public IEnumerable<TankComponentDef> ForSlot(ComponentSlot slot)
            => Components.Where(c => c != null && c.Slot == slot);

        public TankComponentDef FindById(string defId)
            => Components.FirstOrDefault(c => c != null && c.Id == defId);
    }
}
