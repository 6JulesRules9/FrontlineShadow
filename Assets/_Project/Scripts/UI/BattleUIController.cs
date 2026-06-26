using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace FrontlineShadow.UI
{
    /// <summary>
    /// Phase-2-Battle-HUD (04_ROADMAP.md Phase 2): nur der Rückweg in die Garage —
    /// das eigentliche HUD (Reticle, Hp, Ammo …) kommt mit Phase 3.
    /// </summary>
    [RequireComponent(typeof(UIDocument))]
    public class BattleUIController : MonoBehaviour
    {
        [SerializeField] string _garageSceneName = "Garage";

        void OnEnable()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            root.Clear();
            root.style.flexGrow = 1;
            root.style.justifyContent = Justify.FlexStart;
            root.style.paddingLeft = root.style.paddingTop = 16;

            var button = new Button(BackToGarage) { text = "Zurück zur Garage" };
            button.style.backgroundColor = StyleTokens.BgPanel2;
            button.style.color = StyleTokens.Ink;
            button.style.paddingLeft = button.style.paddingRight = 12;
            button.style.paddingTop = button.style.paddingBottom = 6;

            root.Add(button);
        }

        void BackToGarage() => SceneManager.LoadScene(_garageSceneName);
    }
}
