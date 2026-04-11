using DG.Tweening;
using UnityEngine;

public class Bee : MonoBehaviour
{
    private Material bodyMaterial = null;
    public Hive CurrentHive { get; set; }
    public Vector3? CurrentSlot { get; set; }

    private void Awake()
    {
        bodyMaterial = transform.Find("Body").GetComponent<MeshRenderer>().sharedMaterial;
    }

    public Material GetMaterial() => bodyMaterial;

    public void SetMaterial(Material newMaterial)
    {
        bodyMaterial = newMaterial;
        transform.Find("Body").GetComponent<MeshRenderer>().sharedMaterial = bodyMaterial;
    }

    public void Move(Vector3 pos)
    {
        transform.DOMove(pos, 0.5f)
            .SetEase(Ease.Linear);
    }
}