using FrontlineShadow.Save;
using NUnit.Framework;

namespace FrontlineShadow.Tests
{
    public class SaveServiceTests
    {
        [Test]
        public void NewSaveData_HasCurrentVersion()
        {
            var data = new SaveData();
            Assert.AreEqual(SaveData.CurrentVersion, data.Version);
        }

        [Test]
        public void Serialize_Then_Deserialize_RoundTrips()
        {
            var data = new SaveData
            {
                Version = SaveData.CurrentVersion,
                LastSavedUtc = "2026-06-26T00:00:00.0000000Z"
            };

            var json = SaveService.Serialize(data);
            var back = SaveService.Deserialize(json);

            Assert.AreEqual(data.Version, back.Version);
            Assert.AreEqual(data.LastSavedUtc, back.LastSavedUtc);
        }

        [Test]
        public void Serialize_ProducesNonEmptyJson()
        {
            var json = SaveService.Serialize(new SaveData());
            Assert.IsNotNull(json);
            Assert.IsNotEmpty(json);
            StringAssert.Contains("Version", json);
        }
    }
}
