# uDMX Scripts & Example

[uDMX](http://www.anyma.ch/research/udmx/) is an open source hardware & software solution for controlling DMX lights over USB.

This repo contains:
* WinUSB Driver for uDMX devices
* LibUsbDotNet plugin for working with libusb devices
* uDMX c# class for managing & controlling a uDMX controller
* DMXColorSender example script that sends a Unity Color to a particular [DMX Light](https://www.amazon.com/gp/product/B06Y2XC4MN)

### To Use With Your Own Light

Figure out which channels your light uses for receving R,G,B and W (if applicable) signals - this will be in your product manual. Modify the 'SendColor' method of DMXColorSender to send the channels starting at the correct offset and in the correct order. In the example below, the light is looking for brightness, red, green and then blue starting on channel 4:

``` 
private void SendColor(Color color)
    {
        if (dmx.IsOpen)
        {
            Color32 c = color;
            byte[] values = new byte[4] { c.a, c.r, c.g, c.b };
            short channel = (short)(2 + address);
            dmx.SetChannelRange(channel, values);
        }
    }
```

### Prerequisites:

This is only known to work with:
*[This uDMX Controller](https://www.amazon.com/gp/product/B00ZQNIAP8)
*[This DMX Light](https://www.amazon.com/gp/product/B06Y2XC4MN)
*The *uDMX_WinUSB* drivers included in this repo. Run "InstallDriver.exe" and then plug in the Lixada controller above before opening the Unity Scene

Why do we start on channel 2 above? Because our light uses address 1, and DMX is 1-based while Unity is 0-based, so this will result in values being sent on channels 4, 5, 6 and 7.

Acknowledgements:
[PavelBanksy's uDMX repository](https://github.com/PavelBansky/uDMX) was the basis for the uDMX class here. I had to change the vID and pID to match our particular controller, and I switched the way the constructor works to get it working on my machine.