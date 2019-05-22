using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SRDebugger.Internal;

public class ShowSelectedOptions : MonoBehaviour {

    [SerializeField] private bool showOnStart = false;
    [SerializeField] private string[] options;
    [SerializeField] private KeyCode triggerKey;
    [SerializeField] private bool showConsole = false;
    internal bool show;

    void Start () {
        if (showOnStart)
            AddAll();

        if (showConsole)
            SRDebug.Instance.DockConsole.IsVisible = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(triggerKey))
        {
            if (show)
                RemoveAll();
            else
                AddAll();
        }
    }

    void AddAll()
    {
        show = true;
        foreach (var item in options)
            AddOption(item);
    }

    void RemoveAll()
    {
        show = false;
        foreach (var item in options)
            RemoveOption(item);
    }

    void RemoveOption(string name)
    {
        Service.PinnedUI.Unpin(Service.Options.Options.FirstOrDefault(p => p.Name == name));

    }

    void AddOption(string name)
    {
        Service.PinnedUI.Pin(Service.Options.Options.FirstOrDefault(p => p.Name == name));
    }
	
}