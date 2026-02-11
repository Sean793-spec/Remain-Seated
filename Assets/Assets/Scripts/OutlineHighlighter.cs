using UnityEngine;

public class OutlineHighlighter : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;

    [Header("Detection Settings")]
    public LayerMask interactableLayerMask;  // For objects you can look at initially
    public string outlineLayerName = "Outline";
    public string interactableLayerName = "Interactable";
    public float maxDistance = 10f;

    private GameObject currentTarget;
    private int outlineLayer;
    private int interactableLayer;

    void Awake()
    {
        outlineLayer = LayerMask.NameToLayer(outlineLayerName);
        interactableLayer = LayerMask.NameToLayer(interactableLayerName);

        if (outlineLayer == -1)
            Debug.LogError($"Layer '{outlineLayerName}' not found!");
        if (interactableLayer == -1)
            Debug.LogError($"Layer '{interactableLayerName}' not found!");
    }

    void Update()
    {
        if (playerCamera == null) return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        // Include both Interactable and Outline layers in the raycast mask
        int combinedMask = interactableLayerMask | (1 << outlineLayer);

        if (Physics.Raycast(ray, out hit, maxDistance, combinedMask))
        {
            GameObject hitObj = hit.collider.gameObject;

            if (hitObj != currentTarget)
                SetCurrentTarget(hitObj);
        }
        else
        {
            ClearCurrentTarget();
        }
    }

    private void SetCurrentTarget(GameObject newTarget)
    {
        // Restore previous target if different
        if (currentTarget != null && currentTarget != newTarget)
            SetLayerRecursively(currentTarget, interactableLayer);

        currentTarget = newTarget;

        // Only change layer if it's currently Interactable
        if (currentTarget.layer == interactableLayer)
            SetLayerRecursively(currentTarget, outlineLayer);
    }

    private void ClearCurrentTarget()
    {
        if (currentTarget == null) return;

        // Only reset layer if currently Outline
        if (currentTarget.layer == outlineLayer)
            SetLayerRecursively(currentTarget, interactableLayer);

        currentTarget = null;
    }

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
            SetLayerRecursively(child.gameObject, layer);
    }
}
