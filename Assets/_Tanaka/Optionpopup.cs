using UnityEngine;

public class Optionpopup : MonoBehaviour
{
    [SerializeField]
    [Header("ƒIƒvƒVƒ‡ƒ“‰æ–Ê")]
    private GameObject optionPanel;

   public void ShowOptions()
    {
        optionPanel.SetActive(true);
    }

    public void HideOptions()
    {
        optionPanel.SetActive(false);
    }

}
