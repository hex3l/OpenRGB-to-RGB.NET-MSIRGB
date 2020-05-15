using RGB.NET.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hex3l.RGB.NET.Devices.Msirgb.Generic
{
    interface IMsirgbRGBDevice : IRGBDevice
    {
        void Initialize(MsirgbDeviceUpdateQueue updateQueue, int ledCount);
    }
}
