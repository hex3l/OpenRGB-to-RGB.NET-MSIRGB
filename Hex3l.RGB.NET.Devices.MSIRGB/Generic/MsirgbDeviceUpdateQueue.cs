using RGB.NET.Core;
using Hex3l.RGB.NET.Devices.Msirgb.Native;
using System.Collections.Generic;

namespace Hex3l.RGB.NET.Devices.Msirgb.Generic
{
    public class MsirgbDeviceUpdateQueue : UpdateQueue
    {
        #region Properties & Fields

        private int _deviceID;

        #endregion

        #region Constructors

        public MsirgbDeviceUpdateQueue(IDeviceUpdateTrigger updateTrigger, int deviceID)
            : base(updateTrigger)
        {
            this._deviceID = deviceID;
        }

        #endregion

        #region Methods

        protected override void Update(Dictionary<object, Color> dataSet)
        {
            foreach (KeyValuePair<object, Color> data in dataSet) {
                _OpenRGB_MSIRGB.SetLedColor(_deviceID, (int)data.Key, data.Value.GetR(), data.Value.GetG(), data.Value.GetB());
            }
        }

        #endregion
    }
}
