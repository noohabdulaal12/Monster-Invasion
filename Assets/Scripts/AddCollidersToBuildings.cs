using UnityEngine;

public class AddCollidersToBuildings : MonoBehaviour
{
    [ContextMenu("Add Box Colliders To Children")]
    void AddBoxCollidersToChildren()
    {
        foreach (Transform child in GetComponentsInChildren<Transform>(true))
        {
            if (child == transform) continue;

            if (child.GetComponent<Collider>() == null)
            {
                MeshRenderer mr = child.GetComponent<MeshRenderer>();
                if (mr != null)
                {
                    child.gameObject.AddComponent<BoxCollider>();
                }
            }
        }

        Debug.Log("Finished adding BoxColliders.");
    }

    [ContextMenu("Add Mesh Colliders To Children")]
    void AddMeshCollidersToChildren()
    {
        foreach (Transform child in GetComponentsInChildren<Transform>(true))
        {
            if (child == transform) continue;

            if (child.GetComponent<Collider>() == null)
            {
                MeshFilter mf = child.GetComponent<MeshFilter>();
                if (mf != null && mf.sharedMesh != null)
                {
                    MeshCollider mc = child.gameObject.AddComponent<MeshCollider>();
                    mc.sharedMesh = mf.sharedMesh;
                    mc.convex = false;
                }
            }
        }

        Debug.Log("Finished adding MeshColliders.");
    }
}