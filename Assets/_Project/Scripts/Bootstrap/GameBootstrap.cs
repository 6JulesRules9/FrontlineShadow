using FrontlineShadow.Config;
using FrontlineShadow.Core;
using FrontlineShadow.Save;
using FrontlineShadow.Tank;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FrontlineShadow.Bootstrap
{
    /// <summary>
    /// App-Einstieg: verdrahtet alle Services im <see cref="ServiceLocator"/> und
    /// lädt anschließend die erste Spielszene (Garage).
    ///
    /// Setup: In die <c>Bootstrap</c>-Szene ein leeres GameObject legen, diese
    /// Komponente anhängen, eine <see cref="BalanceConfig"/> zuweisen. Beide
    /// Szenen (Bootstrap, Garage) in die Build Settings aufnehmen.
    /// </summary>
    public class GameBootstrap : MonoBehaviour
    {
        [SerializeField] BalanceConfig _balanceConfig;
        [SerializeField] ComponentCatalog _componentCatalog;
        [SerializeField] string _firstScene = "Garage";
        [SerializeField] bool _loadFirstScene = true;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
            WireServices();
        }

        void Start()
        {
            if (_loadFirstScene
                && !string.IsNullOrEmpty(_firstScene)
                && Application.CanStreamedLevelBeLoaded(_firstScene))
            {
                SceneManager.LoadScene(_firstScene);
            }
            else
            {
                Debug.Log($"[Bootstrap] Erste Szene '{_firstScene}' nicht geladen " +
                          "(noch nicht in den Build Settings?). Services laufen trotzdem.");
            }
        }

        void WireServices()
        {
            ServiceLocator.Clear();

            var save = new SaveService();
            save.LoadOrCreate();
            ServiceLocator.Register(save);

            var cfg = _balanceConfig != null ? _balanceConfig.name : "—(nicht zugewiesen)";

            if (_balanceConfig != null && _componentCatalog != null)
            {
                var loadout = new LoadoutService(_componentCatalog, _balanceConfig);
                loadout.LoadOrCreate();
                ServiceLocator.Register(loadout);
            }
            else
            {
                Debug.LogWarning("[Bootstrap] LoadoutService nicht verdrahtet " +
                                  "(BalanceConfig oder ComponentCatalog fehlt).");
            }

            Debug.Log($"[Bootstrap] Services bereit. BalanceConfig: {cfg}. " +
                      $"Save v{save.Data.Version}, zuletzt {save.Data.LastSavedUtc}.");
        }
    }
}
