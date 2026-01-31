using UnityEngine;
using UnityEngine.Tilemaps;

public class ColorModeControl : MonoBehaviour
{
    private Material colorBlindMaterial; 

    public void Start()
    {
        GameControl.AllColorSwappingItems.Add(this);
    }

    public void OnDestroy()
    {
        GameControl.AllColorSwappingItems.Remove(this);
    }

    public void SetColorBlindType(bool isR, bool isG, bool isB)
    {
        // get material from this gameobject's renderer 2D
        if (colorBlindMaterial == null)
        {
            var renderer2D = GetComponent<SpriteRenderer>();
            if (renderer2D != null)
            {
                colorBlindMaterial = renderer2D.material;
            }
            var tileMapRenderer = GetComponent<TilemapRenderer>();
            if (tileMapRenderer != null)
            {
                colorBlindMaterial = tileMapRenderer.material;
            }
        }

        colorBlindMaterial.SetFloat("_IsR", isR ? 1f : 0f);
        colorBlindMaterial.SetFloat("_IsG", isG ? 1f : 0f);
        colorBlindMaterial.SetFloat("_IsB", isB ? 1f : 0f);
    }
}