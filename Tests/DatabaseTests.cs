using NUnit.Framework;
using TFortisDeviceManager.Database;
using TFortisDeviceManager.Services;

namespace Tests
{
    internal class DatabaseTests
    {
        [Test]
        public void GetDeviceName_Test() 
        {
            string expectedName = "PSW-2G+UPS-Box";

            var actualName = PGDataAccess.GetDeviceName(10).ToString();

            Assert.AreEqual(expectedName, actualName);
        }

        [Test]
        public void GetPortsCount_Test() 
        {
            int expectedPorts = 6;

            var actualPorts = PGDataAccess.GetPortsCount(10);

            Assert.AreEqual((int)expectedPorts, actualPorts);
        }

        [Test]
        public void LoadOidsForMonitroing_Test()
        {
            int expectedCount = 10;

            var actualCount = PGDataAccess.LoadOidsForMonitroing().Count;

            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public void LoadOidsForMonitroingWithIP_Test()
        {
            int expectedCount = 10;

            var actualCount = PGDataAccess.LoadOidsForMonitroing("192.168.0.1").Count;

            Assert.AreEqual(expectedCount, actualCount);
        }
    }
}
