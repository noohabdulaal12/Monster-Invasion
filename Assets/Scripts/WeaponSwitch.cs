using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwitch : MonoBehaviour
{
    public int selectedWeapon = 0;

    void Start()
    {
        SelectWeapon();
    }

    void Update()
    {
        int previousSelected = selectedWeapon;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.digit1Key.wasPressedThisFrame)
                selectedWeapon = 0;

            if (Keyboard.current.digit2Key.wasPressedThisFrame)
                selectedWeapon = 1;

            if (Keyboard.current.digit3Key.wasPressedThisFrame)
                selectedWeapon = 2;
        }

        if (Mouse.current != null)
        {
            float scroll = Mouse.current.scroll.ReadValue().y;

            if (scroll > 0f)
            {
                selectedWeapon--;
                if (selectedWeapon < 0)
                    selectedWeapon = transform.childCount - 1;
            }
            else if (scroll < 0f)
            {
                selectedWeapon++;
                if (selectedWeapon >= transform.childCount)
                    selectedWeapon = 0;
            }
        }

        if (previousSelected != selectedWeapon)
        {
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            weapon.gameObject.SetActive(i == selectedWeapon);
            i++;
        }
    }
}