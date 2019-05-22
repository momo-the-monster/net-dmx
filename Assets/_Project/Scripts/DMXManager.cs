using UnityEngine;
using DMXtable;
using System.IO;


[System.Serializable]
public class FixtureDescription
{
    public string name;
    public string group;
    public int numberChannels;
    public int startingChannel;
}

[System.Serializable]
public class FixtureDescriptionArray
{
    public FixtureDescription[] fixtures;
}

public class DMXManager : MonoBehaviour
{
    [SerializeField] new private Camera camera;
    DMXserial serial;
    DMXmaster master;

    Color _colorDelta;
    bool _reconnect = false;
    static DMXManager instance;
    private const string defaultGroupName = "main";

    // Reconnection
    [SerializeField] private int checkConnectionPeriod = 3;

    void Start()
    {
        instance = this;

        serial = new DMXserial();
        master = new DMXmaster();

        LoadFixtures();

        InvokeRepeating(nameof(Reconnect), 0, checkConnectionPeriod);
    }

    private void LoadFixtures()
    {
        
        string fixturesPath = Path.Combine(Application.streamingAssetsPath, "fixtures.json");
        var jsonData = File.ReadAllText(fixturesPath);
        var fixtureDescriptions = JsonUtility.FromJson<FixtureDescriptionArray>(jsonData);
        foreach (var fixtureDescription in fixtureDescriptions.fixtures)
        {
            // add to default group if group name is blank
            string groupName = string.IsNullOrEmpty(fixtureDescription.group) ? defaultGroupName : fixtureDescription.group;
            master.addFixtures(fixtureDescription.name, fixtureDescription.numberChannels, fixtureDescription.startingChannel, groupName);
        }
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

    private void OnDisable()
    {
    }

    /// <summary>
    /// Send the color to OPPSK - RGBW on channels 4-7
    /// </summary>
    /// <param name="color"></param>
    public void SendColor(Color color, string name = defaultGroupName)
    {
        camera.backgroundColor = color;
        master.updateMainFade(Mathf.RoundToInt(SROptions.Current.FadeTime * 1000));
        master.updateFixtures(name, color);
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
