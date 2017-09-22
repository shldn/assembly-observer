using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assembly;

public class ColonyViewer : MonoBehaviour {

    public int size = 10;
    public GameObject visual = null;
    private Colony colony = null;
    private List<GameObject> visuals = new List<GameObject>();
	// Use this for initialization
	void Start () {
        ConsoleRedirect.Redirect();
        colony = Colony.Create(size);
        for (int i = 0; i < size; ++i)
            visuals.Add(GameObject.Instantiate(visual));
    }
	
	// Update is called once per frame
	void Update () {
        List<System.Numerics.Vector3> positions = colony.GetPositions();
        for (int i = 0; i < visuals.Count; ++i)
            visuals[i].transform.position = Vec3(positions[i]);
	}

    Vector3 Vec3(System.Numerics.Vector3 v) {
        return new Vector3(v.X, v.Y, v.Z);
    }
}
