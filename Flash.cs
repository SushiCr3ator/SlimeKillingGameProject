using System.Collections;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] private Material whiteFlashMaterial;
    [SerializeField] private float restoreDefaultMaterialTime = .2f;

    private Material _defaultMaterial;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        this._spriteRenderer = GetComponent<SpriteRenderer>();
        this._defaultMaterial = this._spriteRenderer.material;
    }

    public float GetRestoreMaterialTime()
    {
        return this.restoreDefaultMaterialTime;
    }

    public IEnumerator FlashRoutine()
    {
        this._spriteRenderer.material = this.whiteFlashMaterial;
        yield return new WaitForSeconds(this.restoreDefaultMaterialTime);
        this._spriteRenderer.material = this._defaultMaterial;
    }
    
}
