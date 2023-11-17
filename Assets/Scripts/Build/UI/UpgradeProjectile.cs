using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Build.UI
{
    public class UpgradeProjectile : MonoBehaviour
    {
        
        public void OnUpgradeSelected(Button button)
        {
            // button.interactable = playerHasSufficientCurrency; or something similar
            
            Debug.Log("Upgrade Clicked!");
            TMP_Text text = button.GetComponentInChildren<TMP_Text>(true);
            if(text != null){
                text.text = "clicked";
            }
            
            // TODO change button text to next upgrade - this data should be stored somewhere clean
            // TODO apply change to projectile- event?
        }
    }
}