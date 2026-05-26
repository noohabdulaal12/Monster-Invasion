using UnityEngine;

public class LocalMultiplayerGun : MonoBehaviour
{
    public int playerNumber = 1;
    public Camera fpsCam;
    public float range = 50f;
    public int damage = 25;

    void Update()
    {
        if (playerNumber == 1 && Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }

        if (playerNumber == 2 && Input.GetKeyDown(KeyCode.RightControl))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (fpsCam == null)
        {
            Debug.LogWarning("FPS Camera is missing");
            return;
        }

        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log("Hit: " + hit.collider.name);

            hit.collider.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
        }
    }
}