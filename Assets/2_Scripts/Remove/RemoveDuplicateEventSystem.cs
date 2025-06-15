using UnityEngine;
using UnityEngine.EventSystems;

public class RemoveDuplicateEventSystem : MonoBehaviour
{
    void Awake()
    {
        var eventSystems = UnityEngine.Object.FindObjectsByType<EventSystem>(FindObjectsSortMode.None);
        if (eventSystems.Length > 1)
        {
            foreach (var es in eventSystems)
            {
                if (es != EventSystem.current)
                    Destroy(es.gameObject);
            }
        }
    }
}