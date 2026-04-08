using UnityEngine;

public class Bee : MonoBehaviour
{
    private MeshRenderer rendererMesh;

    private void Awake()
    {
        rendererMesh = GetComponent<MeshRenderer>();
    }

    public void SetColor(Color color)
    {
        rendererMesh.material.color = color;
    }
}