using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TriggerEvent : UnityEvent { }

[RequireComponent(typeof(Rigidbody2D))]
public class ShipDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _shipTriggerLayer;
    private int _layerAsInt;
    [SerializeField] private TriggerEvent _areaEntered;
    [SerializeField] private TriggerEvent _areaExited;

    private void Start()
    {
        if (this._shipTriggerLayer == 0)
        {
            Debug.Log("Error: no layer set in trigger detector attached to  " + this.gameObject.name + ".");
        }
        else
        {
            this._layerAsInt = this._shipTriggerLayer.value;
        }

    }


    // invoke trigger event when a trigger object on the specified layer is detected
    void OnTriggerEnter2D(Collider2D collision)
    {
        if ((_shipTriggerLayer.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            _areaEntered.Invoke();
        }
    }


    // invoke trigger event again when the trigger object is no longer detected
    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((_shipTriggerLayer.value & (1 << collision.transform.gameObject.layer)) > 0)
        {
            _areaExited.Invoke();
        }
    }
}
