using UnityEngine;

public class DimensionGearUI : MonoBehaviour
{
    [SerializeField] private GameObject DimensionGearUsableObject;
    [SerializeField] private GameObject DimensionGearNotUsableObject;

    // Start is called before the first frame update
    private void Start()
    {
        this.DimensionGearUsableObject.SetActive(false);
        this.DimensionGearNotUsableObject.SetActive(GameManager.Instance.HasDimensionGear);

        EventManager.Instance.FOnDimensionGearAvailable += SetDimensionGearState;
        EventManager.Instance.FOnDimensionGearPosses += ActivateDimensionGear;
    }

    private void ActivateDimensionGear(bool isAvailable)
    {
        this.DimensionGearNotUsableObject.SetActive(isAvailable);
    }

    /// <summary>
    /// Set the state of the dimension gear
    /// </summary>
    /// <param name="isUsable">True Can be used, False Cant be used</param>
    private void SetDimensionGearState(bool isUsable)
    {
        if (!GameManager.Instance.HasDimensionGear) return;
        
        this.DimensionGearUsableObject.SetActive(isUsable);
        this.DimensionGearNotUsableObject.SetActive(!isUsable);

    }
}