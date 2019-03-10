using UnityEngine;
using DMXtable;

public class DMXManager : MonoBehaviour
{
    [SerializeField] new private Camera camera;
    DMXserial serial;
    DMXmaster master;

    Color _colorDelta;
    bool _reconnect = false;
    static DMXManager instance;

    // Reconnection
    [SerializeField] private int checkConnectionPeriod = 3;

    void Start()
    {
        instance = this;

        serial = new DMXserial();
        master = new DMXmaster();

        AddCustomLightset();

        InvokeRepeating(nameof(Reconnect), 0, checkConnectionPeriod);
    }

    private void AddCustomLightset()
    {
        master.addFixtures(lightName, 4, 4, "main");
    }

    private string GetComPortName()
    {
        return $"COM{SROptions.Current.ComPort}";
    }

    /// <summary>
    /// Reconnect to the device, cancel ongoing invoke
    /// </summary>
    void Reconnect()
    {
        bool result = serial.connect(GetComPortName());
        Debug.Log($"Attempted to connect: {result}");
        if (result)
        {
            CancelInvoke(nameof(Reconnect));

            master.updateMainMaster(1);
            master.updateMainFade(1000);
        }
    }

    void Update()
    {
        if (!serial.isConnected)
        {
            return;
        }

        master.update();

        FixturesAndChannels f = master.getFixtureArray();
        if (f.byteArray.Length > 0)
        {
            serial.send(f.byteArray);
        }
    }

    internal static void TriggerClear()
    {
        instance.SendColor(Color.clear);
    }

    public static void TriggerRandom()
    {
        instance.SendColor(Random.ColorHSV(0, 1, 1, 1, 1, 1));
    }

    [SerializeField] private string lightName = "Oppsk1";

    private void OnDisable()
    {
    }

    /// <summary>
    /// Send the color to OPPSK - RGBW on channels 4-7
    /// </summary>
    /// <param name="color"></param>
    public void SendColor(Color color)
    {
        camera.backgroundColor = color;
        master.updateMainFade(Mathf.RoundToInt(SROptions.Current.FadeTime * 1000));
        master.updateFixtures(lightName, color);
    }

    /// <summary>
    /// Turn off the light, stop checking for reconnection
    /// </summary>
    private void OnDestroy()
    {
        if (serial != null && serial.isConnected) serial.disconnect();
        CancelInvoke(nameof(Reconnect));
    }
}
