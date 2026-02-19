using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public static class Utility
{
    public static Vector3 GetMousePosition()
    {
        Camera camera = Camera.main;
        Vector3 screenPosition = Input.mousePosition;

        screenPosition.z = -camera.transform.position.z;

        Vector3 worldPosition = camera.ScreenToWorldPoint(screenPosition);

        return worldPosition;
    }

    public static bool TryGetOverlapComponent<T>(Vector2 center, Vector2 size, LayerMask layerMask, out T foundComponent, float angle = 0f)
    {
        foundComponent = default;

        Collider2D hit = Physics2D.OverlapBox(center, size, angle, layerMask);
        if (hit != null)
        {
            if (hit.TryGetComponent(out foundComponent))
            {
                return true;
            }
        }

        return false;
    }
    
    public static string TakeLocalizationString(string key)
    {
        return LocalizationSettings.StringDatabase.GetLocalizedString(LocalizationKeys.Localization.TableName, key);
    }
}