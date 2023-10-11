using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UIManger : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public GameObject healthTextPrefab;
    public GameObject critDamageTextPrefab;
    public GameObject critText;


    public Canvas gameCanvas;

    private void Awake()
    {
        //gameCanvas = FindObjectOfType<Canvas>();
        GameObject foundCanvasObject = GameObject.Find("Canvas");
        gameCanvas = foundCanvasObject.GetComponent<Canvas>();
    
    }

    private void OnEnable()
    {

        CharacterEvents.characterDamaged += CharrctedTookDamage;
        CharacterEvents.characterHealed += CharrctedHealed;
        CharacterEvents.characterCritDamaged += CharrctedTookCritDamage;
        CharacterEvents.characterText += CharrctedText;

    }
   
    private void OnDisable()
    {
        CharacterEvents.characterDamaged -= CharrctedTookDamage;
        CharacterEvents.characterHealed -= CharrctedHealed;
        CharacterEvents.characterCritDamaged -= CharrctedTookCritDamage;
        CharacterEvents.characterText -= CharrctedText;
    }

    public void CharrctedTookDamage(GameObject character, int damageReceived)
    {
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tmpText = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform)
            .GetComponent<TMP_Text>();
        tmpText.text = "-" + damageReceived.ToString();
    }


    public void CharrctedTookCritDamage(GameObject character, int critdamageReceived)
    {
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tmpText = Instantiate(critDamageTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform)
            .GetComponent<TMP_Text>();
        tmpText.text = "-" + critdamageReceived.ToString();
    }

    public void CharrctedHealed(GameObject character, int HealedReceived)
    {

        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tmpText = Instantiate(healthTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform)
            .GetComponent<TMP_Text>();
        tmpText.text = "+" + HealedReceived.ToString();
    }


    public void CharrctedText(GameObject character, string Text)
    {

        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tmpText = Instantiate(critText, spawnPosition, Quaternion.identity, gameCanvas.transform)
            .GetComponent<TMP_Text>();
        tmpText.text = Text;
    }
    

}
