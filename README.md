# OpenRGB-to-RGB.NET-MSIRGB
Compatibility layer for **[RGB.NET](https://github.com/DarthAffe/RGB.NET)**

This device provider uses OpenRGB's msi-rgb implementation to control MSI motherboard's Super I/O chips

**_Keep in mind that this project uses [OpenRGB](https://gitlab.com/CalcProgrammer1/OpenRGB) directly. As OpenRGB, I will not be liable for any damage._**

## Solution

The solution has two projects

#### _Hex3l.RGB.NET.Devices.MSIRGB_
RGB.NET DeviceProvider, it will take care of loading OpenRGB_MSIRGB.dll and communicate with it.

#### _OpenRGB_MSIRGB_
Minimal implementation of OpenRGB features required by this layer. It includes directly (or using git patches) the code/implementations made by _OpenRGB_


## Color inversion fixes

In order to fix color inversion _OpenRGB_MSIRGB.dll_ returns 2 devices for the same motherboard controller.
One has a normal behaviour while the other, flagged as `INVERTED` applies the colors space inversion.

## How to build and use

1. Clone the repo with its submodules.
2. Apply git patch in `OpenRGB_MSIRGB/ORGB-patches` to `OpenRGB_MSIRGB/OpenRGB`
3. Apply git patch in `OpenRGB_MSIRGB/ORGB-min-api-dll-patches` to `OpenRGB_MSIRGB/OpenRGB-min-api-dll-base`
4. Build `Hex3l.RGB.NET.Devices.Msiusb`(Release) (it will also build _OpenRGB_MSIRGB_)
5. Get the 2 dlls and place
   - `Hex3l.RGB.NET.Devices.MSIRGB.dll` in your `DeviceProvider` folder (the folder you are using to store your `DeviceProviders`)
   - `OpenRGB_MSIRGB.dll` in 
      - x86: `<app root>/x86`
      - x64: `<app root>/x64`
6. You will also need inpout32.dll/inpoutx64.dll in your root app directory, you can get it from 
   - x86:`OpenRGB_MSIRGB/OpenRGB/dependencies/inpout32_1501/Win32/inpout32.dll`
   - x64:`OpenRGB_MSIRGB/OpenRGB/dependencies/inpout32_1501/x64/inpoutx64.dll`
7. inpoutx64 requires the installation of a kernel driver. To install it the final user must run `DriverInstall.exe` located in `InpOutBinaries_1501\Win32` inside the binaries distribution http://www.highrez.co.uk/scripts/download.asp?package=InpOutBinaries
