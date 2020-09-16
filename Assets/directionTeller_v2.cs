using UnityEngine;

// Transform.Rotate example
//
// This script creates two different cubes: one red which is rotated using Space.Self; one green which is rotated using Space.World.
// Add it onto any GameObject in a scene and hit play to see it run. The rotation is controlled using xAngle, yAngle and zAngle, modifiable on the inspector.

public class directionTeller_v2 : MonoBehaviour
{
    public float xAngle;
    public float yAngle;
    public float zAngle;
    public Vector3 scaleChange;

    private GameObject cube1;

    public Transform parent;

    void Awake()
    {
        cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cube1.transform.SetParent(parent, false);
        cube1.transform.position = new Vector3(0.0f, -1.0f, 0.0f);
        scaleChange = new Vector3(-0.9f, -0.9f, -0.9f);
        cube1.transform.localScale += scaleChange;
        cube1.transform.Rotate(90.0f, 0.0f, 0.0f, Space.Self);
        cube1.GetComponent<Renderer>().material.color = Color.red;
        cube1.name = "Self";
    }

    void Update()
    {
        cube1.transform.Rotate(xAngle, yAngle, zAngle, Space.Self);
    }
}