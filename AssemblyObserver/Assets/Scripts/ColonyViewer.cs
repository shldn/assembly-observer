using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EGL;
using System;

public class ColonyViewer : MonoBehaviour {

	public static ColonyViewer Inst = null;

    [SerializeField] private int size = 10;
    private Colony colony = null;

	[SerializeField] private GameObject nodeVisual = null; public GameObject NodeVisual { get { return nodeVisual; } }
	[SerializeField] private Color basicNodeColor = Color.gray;
	[SerializeField] private Color detectorNodeColor = Color.green;
	[SerializeField] private Color processorNodeColor = Color.blue;
	[SerializeField] private Color motorNodeColor = Color.red;



	private List<AssemblyViewer> assemblyViewers = new List<AssemblyViewer>();
	public class AssemblyViewer {
		private Assembly assembly = null;
		private GameObject gameObject = null;
		private NodeViewer[] nodeViewers = null;

		public AssemblyViewer(Assembly assembly){
			this.assembly = assembly;
			Inst.assemblyViewers.Add(this);

			// Create assembly container.
			gameObject = new GameObject("Assembly-" + assembly.Id);
			gameObject.transform.position = Vec3(assembly.Position);
			gameObject.transform.rotation = Quat(assembly.Rotation);

			// Create nodes.
			nodeViewers = new NodeViewer[assembly.NodesList.Count];
			for (int i = 0; i < assembly.NodesList.Count; ++i)
				nodeViewers[i] = new NodeViewer(assembly.NodesList[i], assembly, gameObject.transform);
			
			// DEBUG - randomized initiial velocities
			Vector3 randomVelocity = UnityEngine.Random.insideUnitSphere * 1f;
			Vector3 randomAngularVelocity = UnityEngine.Random.insideUnitSphere * 1f;

			assembly.ApplyForce(randomVelocity.x, randomVelocity.y, randomVelocity.z);
			assembly.ApplyTorque(randomAngularVelocity.x, randomAngularVelocity.y, randomAngularVelocity.z);

		} // End of Ctor.

		public void Update(){
			gameObject.transform.position = Vec3(assembly.Position);
			gameObject.transform.rotation = Quat(assembly.Rotation);

			Debug.DrawRay(Vec3(assembly.Position), Quat(assembly.Rotation) * Vector3.forward * 2f, Color.blue);
			Debug.DrawRay(Vec3(assembly.Position), Quat(assembly.Rotation) * Vector3.right * 2f, Color.green);
			Debug.DrawRay(Vec3(assembly.Position), Quat(assembly.Rotation) * Vector3.up * 2f, Color.red);


			// DEBUG - drive around assembly
			Vector3 throttle = Vector3.zero;
			if(Input.GetKey(KeyCode.Keypad4))
				throttle.x += 1f;
			if(Input.GetKey(KeyCode.Keypad6))
				throttle.x -= 1f;
			if(Input.GetKey(KeyCode.Keypad8))
				throttle.y += 1f;
			if(Input.GetKey(KeyCode.Keypad5))
				throttle.y -= 1f;
			if(Input.GetKey(KeyCode.Keypad7))
				throttle.z += 1f;
			if(Input.GetKey(KeyCode.Keypad9))
				throttle.z -= 1f;
			assembly.ApplyForce(throttle.x * Inst.colony.TimeStep, throttle.y * Inst.colony.TimeStep, throttle.z * Inst.colony.TimeStep);

			Vector3 rotationThrottle = Vector3.zero;
			if(Input.GetKey(KeyCode.A))
				rotationThrottle.x += 1f;
			if(Input.GetKey(KeyCode.D))
				rotationThrottle.x -= 1f;
			if(Input.GetKey(KeyCode.W))
				rotationThrottle.y += 1f;
			if(Input.GetKey(KeyCode.S))
				rotationThrottle.y -= 1f;
			if(Input.GetKey(KeyCode.Q))
				rotationThrottle.z += 1f;
			if(Input.GetKey(KeyCode.E))
				rotationThrottle.z -= 1f;
			assembly.ApplyTorque(rotationThrottle.x * Inst.colony.TimeStep, rotationThrottle.y * Inst.colony.TimeStep, rotationThrottle.z * Inst.colony.TimeStep);
			
		} // End of Update().

	} // End of AssemblyViewer.


	public class NodeViewer {
		private Node node = null;
		private Assembly assembly = null;
		private GameObject gameObject = null;

		public NodeViewer(Node node, Assembly assembly, Transform parent){
			this.node = node;
			this.assembly = assembly;
			gameObject = Instantiate(Inst.nodeVisual, parent);
			gameObject.transform.localPosition = Vec3(node.LocalRealPos);

			Color nodeColor;
			switch(node.MyNodeType){
				case NodeType.detector : nodeColor = Inst.detectorNodeColor; break;
				case NodeType.processor : nodeColor = Inst.processorNodeColor; break;
				case NodeType.motor : nodeColor = Inst.motorNodeColor; break;
				default : nodeColor = Inst.basicNodeColor; break;
			}
			gameObject.GetComponent<Renderer>().material.color = nodeColor;
			gameObject.GetComponent<TrailRenderer>().startColor = new Color(nodeColor.r, nodeColor.g, nodeColor.b, 0.5f);
			gameObject.GetComponent<TrailRenderer>().endColor = new Color(nodeColor.r, nodeColor.g, nodeColor.b, 0f);
		} // Ctor.

		public void Update(){
		} // End of Update().

	} // End of NodeViewer.


	private void Awake() {
		Inst = this;
	} // End of Awake().

	private void Start () {
        ConsoleRedirect.Redirect();
        Assembly.onCreation += OnAssemblyCreation;
        colony = Colony.Create(size);
    } // End of Start().
	
	private void Update () {
        for (int i = 0; i < assemblyViewers.Count; i++)
			assemblyViewers[i].Update();
	} // End of Update().

    private void OnAssemblyCreation(Assembly assembly){
        new AssemblyViewer(assembly);
    } // End of OnAssemblyCreation().


    public static Vector3 Vec3(System.Numerics.Vector3 v) {
        return new Vector3(v.X, v.Y, v.Z);
    }

    public static Vector3 Vec3(Triplet v){
        return new Vector3(v.x, v.y, v.z);
    }

	public static Quaternion Quat(System.Numerics.Quaternion q) {
        return new Quaternion(q.X, q.Y, q.Z, q.W);
    }

} // End of ColonyViewer.
