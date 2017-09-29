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
        GameObject assemblyObj = new GameObject("Assembly-" + assembly.id);
        assemblyObj.transform.position = Vec3(assembly.position);
        for (int i = 0; i < assembly.nodes.Count; ++i) {
            GameObject node = GameObject.Instantiate(visual, assemblyObj.transform);
            node.transform.localPosition = Vec3(assembly.nodes[i].localHexPos);
        }

        visuals.Add(assembly.id, assemblyObj);
    }

    Vector3 Vec3(System.Numerics.Vector3 v) {
        return new Vector3(v.X, v.Y, v.Z);
    }

    Vector3 Vec3(Triplet v)
    {
        return new Vector3(v.x, v.y, v.z);
    }

}
