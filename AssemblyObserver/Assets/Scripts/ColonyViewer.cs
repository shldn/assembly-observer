using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assembly;

public class ColonyViewer : MonoBehaviour {

    public int size = 10;
    public GameObject visual = null;
    private Colony colony = null;
    private List<GameObject> visuals = new List<GameObject>();
	
	void Start () {
        ConsoleRedirect.Redirect();
        Assembly.Assembly.onCreation += OnAssemblyCreation;
        colony = Colony.Create(size);
    }
	
	void Update () {
        List<System.Numerics.Vector3> positions = colony.GetPositions();
        for (int i = 0; i < visuals.Count; ++i)
            visuals[i].transform.position = Vec3(positions[i]);
	}

    void OnAssemblyCreation(Assembly.Assembly assembly)
    {
        visuals.Add(GameObject.Instantiate(visual));
    }

    Vector3 Vec3(System.Numerics.Vector3 v) {
        return new Vector3(v.X, v.Y, v.Z);
    }
}
