This Utility controls DMX lights via a USB to DMX adapter, accepting input over HTTP and Websockets so you can easily control lights from a variety of programs.

The program should work with any lights, but this demo is written assuming the following equipment:

- [DMXKing](https://amzn.to/30diTDZ) (any Enttec-compatible adapter should work but this is the only tested one)
- [Oppsk DMX Light](https://amzn.to/2E2KHla)

## DMX Adapter Config

Before you can use the NetDMX bridging app, you need to configure your DMX adapter and ensure that it's working correctly.

### Install The Driver

- Install this driver for the adapter: [FTDI driver](https://drive.google.com/open?id=1PHl0XAlvW4HGn9UWz_-bDmzZeouXlbay)
    - Unzip file
    - Right-click ftdiport.inf &gt; install
    - Right-click ftdibus.inf &gt; install

### Install the Config Tool

- Install this Configuration/Testing tool: [UltraDMX Configuration Utility](https://dmxking.com/downloads/ultraDMX_Configuration.zip)
    - Run the tool
    - Press 'Refresh' next to COM Port
    - In the COM Port dropdown, a COM Port should now be available, take note of this number.

### Test Light with Config Tool

You can test any light with the config tool, but you'll need to know the light's channel mappings. for the suggested RGBW light, the mapping we'll test is:

Channel 4: Brightness (W)  
Channel 5: Red  
Channel 6: Green  
Channel 7: Blue

- Plug the [Oppsk DMX Light](https://amzn.to/2E2KHla) into a power source.
- Plug the DMX &gt; USB Adapter into your machine after installing the driver.
- Plug the other end into the input on the light.
- Set the light to DMX Input (should read d001)
- Run the UltraDMX Configuration Utility
- Select the active COM Port for your adapter
- From the menu bar, choose View &gt; DMX Display
- Switch to Transmit mode
- Hover your mouse over Channel 4 and scroll your mouse wheel up until the box reads FF
- Hover your mouse over channel 5 and scroll your mouse up, the light should fade in red, scroll back down to fade it out
    - repeat for channels 6 and 7, these should turn your light green and blue, respectively.
    - dial channel 4 (brightness) down to 0 and close the DMX View window and the Config utillity

- To get fades to work well, I've found it helpful to change the TX mode.
    - Click on the 'Advanced' Tab
    - Change DMX TX Mode to 'Ultra Fast Update'
    - Press Apply and confirm the dummy-check dialog

### Test NetDMX

- Run NetDMX and set the ComPort in the on-screen settings to the one you discovered using the Utility (make sure UltraDMX Config Utility is closed so it releases the COM Port)
- Click “ReloadScene” to enable the new Com Port you set.
- You should see “Attempted to connect: True” in the console readout. If you see ‘False’, something is wrong. You can use the [UltraDMX Config Utility](https://dmxking.com/downloads-list) to configure and test the adapter.
- Once you’ve connected, you can press ‘TriggerRandom’ to set a random color, this should be set on your light as well.
- Set FadeTime to 0 if you have issues - it’s been fiddly with any kind of fade (even though that’s much nicer)