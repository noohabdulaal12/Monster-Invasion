using UnityEngine;

public class BuildSystem : MonoBehaviour
{
    public GameObject shieldPrefab;
    public Transform playerCamera;
    public BreakManager breakManager;

    public float placeDistance = 4f;
    public int shieldCost = 10;
    public float wallYOffset = 1f;

    private GameObject previewObject;
    private bool buildMode = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (breakManager == null || !breakManager.IsBreakTime)
                return;

            ToggleBuildMode();
        }

        if (!buildMode) return;

        UpdatePreviewPosition();

        if (Input.GetMouseButtonDown(0))
        {
            PlaceShield();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancelBuildMode();
        }
    }

    void ToggleBuildMode()
    {
        buildMode = !buildMode;

        if (buildMode)
        {
            previewObject = Instantiate(shieldPrefab);
            SetPreviewMode(previewObject, true);
        }
        else
        {
            CancelBuildMode();
        }
    }

    void UpdatePreviewPosition()
    {
        Vector3 forward = playerCamera.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 targetPos = transform.position + forward * placeDistance;

        if (Physics.Raycast(targetPos + Vector3.up * 5f, Vector3.down, out RaycastHit hit, 10f))
        {
            previewObject.transform.position = hit.point + Vector3.up * wallYOffset;
            previewObject.transform.rotation = Quaternion.Euler(0, playerCamera.eulerAngles.y, 0);
        }
    }

    void PlaceShield()
    {
        if (GameManager.Instance.coins < shieldCost)
            return;

        GameManager.Instance.coins -= shieldCost;
        GameManager.Instance.UpdateCoinsUI();

        SetPreviewMode(previewObject, false);
        previewObject = null;
        buildMode = false;
    }

    void CancelBuildMode()
    {
        buildMode = false;

        if (previewObject != null)
            Destroy(previewObject);
    }

    void SetPreviewMode(GameObject obj, bool isPreview)
    {
        Collider[] colliders = obj.GetComponentsInChildren<Collider>();

        foreach (Collider col in colliders)
        {
            col.enabled = !isPreview;
        }
    }
}