using UnityEngine;

public class DimensionGearUI : MonoBehaviour
{
    [SerializeField] private GameObject DimensionGearUsableObject;
    [SerializeField] private GameObject DimensionGearNotUsableObject;
    [SerializeField] private GameObject HeartOne;
    [SerializeField] private GameObject HeartTwo;
    [SerializeField] private GameObject HeartThree;

    // Start is called before the first frame update
    private void Start()
    {
        this.DimensionGearUsableObject.SetActive(false);
        this.DimensionGearNotUsableObject.SetActive(GameManager.Instance.HasDimensionGear);

        EventManager.Instance.FOnDimensionGearAvailable += SetDimensionGearState;
        EventManager.Instance.FOnDimensionGearPosses += ActivateDimensionGear;
        EventManager.Instance.FOnDamagePlayer += DamagePlayer;
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
    
    private void DamagePlayer()
    {
        if (this.HeartThree.activeSelf)
        {
            this.HeartThree.SetActive(false);
            return;
        }

        if (this.HeartTwo.activeSelf)
        {
            this.HeartTwo.SetActive(false);
            return;
        }
        
        if (this.HeartOne.activeSelf)
        {
            this.HeartOne.SetActive(false);
            
        }
        
        
    }
}