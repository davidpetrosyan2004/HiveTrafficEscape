using DG.Tweening;
using UnityEngine;

public class Bee : MonoBehaviour
{
    [SerializeField] private Material bodyMaterial = null;

    private void Awake()
    {
        bodyMaterial = transform.Find("Body").GetComponent<MeshRenderer>().sharedMaterial;
    }

    public Material GetMaterial()
    {
        return bodyMaterial;
    }
    public void SetMaterial(Material newMaterial)
    {
        bodyMaterial = newMaterial;
        transform.Find("Body").GetComponent<MeshRenderer>().sharedMaterial = bodyMaterial;
    }

    public void Move(Vector3 pos)
    {
        transform.DOMove(pos, 1f)
            .SetEase(Ease.InOutCirc);
    }
}