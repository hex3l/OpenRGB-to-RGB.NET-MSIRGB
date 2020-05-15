using RGB.NET.Core;
using System;

namespace Hex3l.RGB.NET.Devices.Msirgb.Generic
{
    public class MsirgbRGBDeviceInfo : IRGBDeviceInfo 
    {
        #region Properties & Fields
        public RGBDeviceType DeviceType { get; }

        public int MsiDeviceID { get; }

        public string DeviceName { get; }

        public string Manufacturer { get; }

        public string Model { get; }

        public Uri Image { get; set; }

        public bool SupportsSyncBack => false;

        public RGBDeviceLighting Lighting => RGBDeviceLighting.Key;

        #endregion

        #region Constructors

        internal MsirgbRGBDeviceInfo(RGBDeviceType deviceType, int deviceID, string manufacturer = "MSIRGB", string model = "Generic I/O Superchip")
        {
            this.DeviceType = deviceType;
            this.MsiDeviceID = deviceID;
            this.Manufacturer = manufacturer;
            this.Model = model;

            DeviceName = $"{Manufacturer} {Model}";
        }

        #endregion
    }
}
