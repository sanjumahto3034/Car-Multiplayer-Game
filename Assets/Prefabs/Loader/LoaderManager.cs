using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoaderManager : MonoBehaviour
{
    [SerializeField] private TMP_Text HeaderText;
    [SerializeField] private TMP_Text DescriptionText;

    public void SetContent(string headerText, string descriptionText)
    {
        HeaderText.text = headerText;
        DescriptionText.text = descriptionText;
    }
    public void DestroyPopup()
    {
        Destroy(gameObject);
    }
}
