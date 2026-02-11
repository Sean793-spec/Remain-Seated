using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class DropdownEventRouter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Dropdown dropdown;

    [Header("Per-Index Events (set Size in Inspector)")]
    [SerializeField] private List<UnityEvent> onIndexSelected = new List<UnityEvent>();

    [Header("Behavior")]
    [Tooltip("Warn if the selected index exists but its UnityEvent has no Inspector-assigned listeners.")]
    [SerializeField] private bool warnIfNoEventAssigned = true;

    private void Reset()
    {
        dropdown = GetComponent<TMP_Dropdown>();
    }

    private void Awake()
    {
        if (!dropdown) dropdown = GetComponent<TMP_Dropdown>();

        if (!dropdown)
        {
            Debug.LogError($"[{nameof(DropdownEventRouter)}] No TMP_Dropdown found on '{name}'.", this);
            enabled = false;
        }
    }

    /// <summary>
    /// Hook this to TMP_Dropdown -> On Value Changed (Int).
    /// Dropdown automatically passes the selected index into this method.
    /// </summary>
    public void RouteDropdownIndex(int index)
    {
        // If you didn't provide an event slot for this index, do nothing.
        if (index < 0 || index >= onIndexSelected.Count)
            return;

        UnityEvent evt = onIndexSelected[index];

        if (warnIfNoEventAssigned && (evt == null || evt.GetPersistentEventCount() == 0))
        {
            string label = (dropdown != null && dropdown.options != null && index >= 0 && index < dropdown.options.Count)
                ? dropdown.options[index].text
                : "Unknown";

            Debug.LogWarning(
                $"[{nameof(DropdownEventRouter)}] No event assigned for '{(dropdown ? dropdown.name : name)}' index {index} ('{label}').",
                this
            );
        }

        evt?.Invoke();
    }
}
