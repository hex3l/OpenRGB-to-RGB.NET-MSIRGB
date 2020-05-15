using RGB.NET.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Hex3l.RGB.NET.Devices.Msirgb.Native
{
    internal static class _OpenRGB_MSIRGB
    {
        #region Libary Management

        private static IntPtr _dllHandle = IntPtr.Zero;

        internal static string LoadedArchitecture { get; private set; }

        internal static void Reload()
        {
            UnloadMsiusb();
            LoadMsiusb();
        }

        private static void LoadMsiusb()
        {
            if (_dllHandle != IntPtr.Zero) return;

            // HACK: Load library at runtime to support both, x86 and x64 with one managed dll
            List<string> possiblePathList = Environment.Is64BitProcess ? MsirgbDeviceProvider.PossibleX64NativePaths : MsirgbDeviceProvider.PossibleX86NativePaths;
            string dllPath = possiblePathList.FirstOrDefault(File.Exists);
            if (dllPath == null) throw new RGBDeviceException($"Can't find the OpenRGB_MSIRGB.dll at one of the expected locations:\r\n '{string.Join("\r\n", possiblePathList.Select(Path.GetFullPath))}'");

            SetDllDirectory(Path.GetDirectoryName(Path.GetFullPath(dllPath)));

            _dllHandle = LoadLibrary(dllPath);

            _initPointer = (InitializePointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "init"), typeof(InitializePointer));
            _getNumberOfControllersPointer = (GetNumberOfControllersPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "getNumberOfControllers"), typeof(GetNumberOfControllersPointer));
            _getControllerNamePointer = (GetControllerNamePointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "getControllerName"), typeof(GetControllerNamePointer));
            _getControllerLedsPointer = (GetControllerLedsPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "getControllerLeds"), typeof(GetControllerLedsPointer));
            _getControllerDescriptionPointer = (GetControllerDescriptionPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "getControllerDescription"), typeof(GetControllerDescriptionPointer));
            _getControllerZonesPointer = (GetControllerZonesPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "getControllerZones"), typeof(GetControllerZonesPointer));
            _setModePointer = (SetModePointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "setMode"), typeof(SetModePointer));
            _setLedColorPointer = (SetLedColorPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "setLedColor"), typeof(SetLedColorPointer));
            _setZoneColorPointer = (SetZoneColorPointer)Marshal.GetDelegateForFunctionPointer(GetProcAddress(_dllHandle, "setZoneColor"), typeof(SetZoneColorPointer));
        }

        private static void UnloadMsiusb()
        {
            if (_dllHandle == IntPtr.Zero) return;

            while (FreeLibrary(_dllHandle)) ;
            _dllHandle = IntPtr.Zero;
        }

        [DllImport("kernel32.dll")]
        private static extern bool SetDllDirectory(string lpPathName);

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        private static extern bool FreeLibrary(IntPtr dllHandle);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr dllHandle, string name);

        #endregion

        #region SDK-METHODS

        #region Pointers

        private static InitializePointer _initPointer;
        private static GetNumberOfControllersPointer _getNumberOfControllersPointer;
        private static GetControllerNamePointer _getControllerNamePointer;
        private static GetControllerLedsPointer _getControllerLedsPointer;
        private static GetControllerDescriptionPointer _getControllerDescriptionPointer;
        private static GetControllerZonesPointer _getControllerZonesPointer;
        private static SetModePointer _setModePointer;
        private static SetLedColorPointer _setLedColorPointer;
        private static SetZoneColorPointer _setZoneColorPointer;

        #endregion

        #region Delegates
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void InitializePointer();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int GetNumberOfControllersPointer();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.BStr)]
        private delegate string GetControllerNamePointer(
            [MarshalAs(UnmanagedType.I4)] int controller_idx);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int GetControllerLedsPointer(
            [MarshalAs(UnmanagedType.I4)] int controller_idx);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.BStr)]
        private delegate string GetControllerDescriptionPointer(
            [MarshalAs(UnmanagedType.I4)] int controller_idx);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void GetControllerZonesPointer(
            [MarshalAs(UnmanagedType.I4)] int controller_idx,
            [Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_BSTR)] out string[] zonesArray,
            [Out, MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_UINT)] out uint[] zonesLedsArray);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SetModePointer(
            [MarshalAs(UnmanagedType.I4)] int controller_idx,
            [MarshalAs(UnmanagedType.I4)] int mode_idx);


        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SetLedColorPointer(
            [MarshalAs(UnmanagedType.I4)] int controller_idx,
            [MarshalAs(UnmanagedType.I4)] int led_idx,
            [MarshalAs(UnmanagedType.I4)] int r,
            [MarshalAs(UnmanagedType.I4)] int g,
            [MarshalAs(UnmanagedType.I4)] int b);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SetZoneColorPointer(
            [MarshalAs(UnmanagedType.I4)] int controller_idx,
            [MarshalAs(UnmanagedType.I4)] int zone_idx,
            [MarshalAs(UnmanagedType.I4)] int r,
            [MarshalAs(UnmanagedType.I4)] int g,
            [MarshalAs(UnmanagedType.I4)] int b);
        #endregion

        internal static void Initialize() => _initPointer();
        internal static int GetNumberOfControllers() => _getNumberOfControllersPointer();
        internal static string GetControllerName(int controller_idx) => _getControllerNamePointer(controller_idx);
        internal static int GetControllerLeds(int controller_idx) => _getControllerLedsPointer(controller_idx);
        internal static string GetControllerDescription(int controller_idx) => _getControllerDescriptionPointer(controller_idx);
        internal static void GetControllerZones(int controller_idx, out string[] zonesArray, out uint[] zonesLedsArray) => _getControllerZonesPointer(controller_idx, out zonesArray, out zonesLedsArray);
        internal static void SetMode(int controller_idx, int mode_idx) => _setModePointer(controller_idx, mode_idx);
        internal static void SetLedColor(int controller_idx, int led_idx, int r, int g, int b) => _setLedColorPointer(controller_idx, led_idx, r, g, b);
        internal static void SetZoneColor(int controller_idx, int zone_idx, int r, int g, int b) => _setZoneColorPointer(controller_idx, zone_idx, r, g, b);

        #endregion
    }
}
