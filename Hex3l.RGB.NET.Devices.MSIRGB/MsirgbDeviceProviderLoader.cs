using RGB.NET.Core;

namespace Hex3l.RGB.NET.Devices.Msirgb
{
    public class MsirgbDeviceProviderLoader : IRGBDeviceProviderLoader
    {
        #region Properties & Fields

        public bool RequiresInitialization => false;

        #endregion

        #region Methods

        public IRGBDeviceProvider GetDeviceProvider() => MsirgbDeviceProvider.Instance;

        #endregion
    }
}
