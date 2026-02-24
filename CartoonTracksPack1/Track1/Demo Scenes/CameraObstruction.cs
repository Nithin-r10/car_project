using UnityEngine;

public class CameraObstruction : MonoBehaviour
{
    [SerializeField] private float rayDistance = 10f;
    [SerializeField] private LayerMask obstructionLayer;

    private MeshRenderer lastRenderer;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, obstructionLayer))
        {
            MeshRenderer renderer = hit.collider.GetComponent<MeshRenderer>();
            Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.red);

            if (renderer != null)
            {
                if (lastRenderer != renderer)
                {
                    RestoreLast();
                    renderer.enabled = false;
                    lastRenderer = renderer;
                }
            }
        }
        else
        {
            RestoreLast();
        }
    }

    void RestoreLast()
    {
        if (lastRenderer != null)
        {
            lastRenderer.enabled = true;
            lastRenderer = null;
        }
    }
}