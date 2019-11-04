using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


/// <summary>
/// This class is used to show the text on screen letter by letter
/// </summary>
public class AutoTypeLogic : MonoBehaviour
{
    public float delay;
    public string fullText;
    public string currentText;
    public TextMeshProUGUI UiText;

    private void Start()
    {
        StartCoroutine(ShowText());
    }
    public IEnumerator ShowText()
    {
        for (int i = 0; i < fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i + 1);

            UiText.text = currentText + "_";

            // Last word dont type the underscore
            if (i == fullText.Length - 1) UiText.text = currentText;

            yield return new WaitForSeconds(delay);
        }
        yield return new WaitForSeconds(delay);

    }
}
