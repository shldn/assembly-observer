using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assembly;
using System;

public class ColonyViewer : MonoBehaviour {

    public int size = 10;
    public GameObject visual = null;
    private Colony colony = null;
    private Dictionary<int, GameObject> visuals = new Dictionary<int, GameObject>();
	
	void Start () {
        ConsoleRedirect.Redirect();
        Assembly.Assembly.onCreation += OnAssemblyCreation;
        colony = Colony.Create(size);
    }
	
	void Update () {
        List<System.Tuple<int,System.Numerics.Vector3>> positionData = colony.GetPositions();
        GameObject visual = null;
        for (int i = 0; i < positionData.Count; ++i)
        {
            if (visuals.TryGetValue(positionData[i].Item1, out visual))
                visual.transform.position = Vec3(positionData[i].Item2);
        }
	}

    void OnAssemblyCreation(Assembly.Assembly assembly)
    {
        visuals.Add(assembly.id, GameObject.Instantiate(visual));
    }

    Vector3 Vec3(System.Numerics.Vector3 v) {
        return new Vector3(v.X, v.Y, v.Z);
    }
}
