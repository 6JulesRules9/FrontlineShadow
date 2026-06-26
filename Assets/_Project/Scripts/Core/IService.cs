namespace FrontlineShadow.Core
{
    /// <summary>
    /// Marker-Interface für alle langlebigen Spiel-Services
    /// (Save, Economy, Repair, TechTree, ...). Werden vom Bootstrap im
    /// <see cref="ServiceLocator"/> registriert; Systeme/UI lesen nur.
    /// </summary>
    public interface IService { }
}
