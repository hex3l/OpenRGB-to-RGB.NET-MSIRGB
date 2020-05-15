using RGB.NET.Core;
using Hex3l.RGB.NET.Devices.Msirgb.Generic;
using Hex3l.RGB.NET.Devices.Msirgb.MysticLightController;
using Hex3l.RGB.NET.Devices.Msirgb.Native;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Hex3l.RGB.NET.Devices.Msirgb
{
    public class MsirgbDeviceProvider : IRGBDeviceProvider
    {
        #region Properties & Fields

        private static MsirgbDeviceProvider _instance;
        public static MsirgbDeviceProvider Instance => _instance ?? new MsirgbDeviceProvider();

        public static List<string> PossibleX86NativePaths { get; } = new List<string> { "x86/OpenRGB_MSIRGB.dll" };

        public static List<string> PossibleX64NativePaths { get; } = new List<string> { "x64/OpenRGB_MSIRGB.dll" };

        public bool IsInitialized { get; private set; }

        public string LoadedArchitecture => _OpenRGB_MSIRGB.LoadedArchitecture;

        public bool HasExclusiveAccess { get; private set; }

        public IEnumerable<IRGBDevice> Devices { get; private set; }

        public Func<CultureInfo> GetCulture { get; set; } = CultureHelper.GetCurrentCulture;

        public DeviceUpdateTrigger UpdateTrigger { get; }

        #endregion

        #region Constructors

        public MsirgbDeviceProvider()
        {
            if (_instance != null) throw new InvalidOperationException($"There can be only one instance of type {nameof(MsirgbDeviceProvider)}");
            _instance = this;

            UpdateTrigger = new DeviceUpdateTrigger();
        }

        #endregion

        #region Methods

        public bool Initialize(RGBDeviceType loadFilter = RGBDeviceType.All, bool exclusiveAccessIfPossible = false, bool throwExceptions = false)
        {
            IsInitialized = false;

            try
            {
                UpdateTrigger?.Stop();

                _OpenRGB_MSIRGB.Reload();

                IList<IRGBDevice> devices = new List<IRGBDevice>();

                _OpenRGB_MSIRGB.Initialize();

                int controllers = _OpenRGB_MSIRGB.GetNumberOfControllers();

                if(controllers > 0)
                {

                    for (int i = 0; i < controllers; i++)
                    {

                        string name = _OpenRGB_MSIRGB.GetControllerName(i);
                        int leds = _OpenRGB_MSIRGB.GetControllerLeds(i);

                        // OpenRGB_MSIRGB.dll will add 2 devices per controller. The second one uses the inverted colors fix
                        if(i%2 != 0)
                        {
                            name = name + " INVERTED";
                        }

                        MsirgbDeviceUpdateQueue updateQueue = new MsirgbDeviceUpdateQueue(UpdateTrigger, i);
                        IMsirgbRGBDevice motherboard = new MsirgbRGBDevice(new MsirgbRGBDeviceInfo(RGBDeviceType.Mainboard, i, "MSIRGB", name));

                        motherboard.Initialize(updateQueue, leds);
                        devices.Add(motherboard);
                        
                    }
                }

                UpdateTrigger?.Start();

                Devices = new ReadOnlyCollection<IRGBDevice>(devices);
                IsInitialized = true;
            }
            catch
            {
                if (throwExceptions)
                    throw;
                return false;
            }

            return true;
        }

        public void ResetDevices()
        {
            //TODO: Implement
        }

        public void Dispose()
        { }

        #endregion
    }
}
