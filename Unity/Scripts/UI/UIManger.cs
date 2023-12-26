using System;
using Events;
using TMPro;
using UnityEngine;
public class UIManger : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public GameObject healthTextPrefab;
    public GameObject critDamageTextPrefab;
    public GameObject critText;
    public GameObject ShuDyeing;
    public GameObject crit;
    public GameObject text;


    public Canvas gameCanvas;

    private void OnEnable()
    {

        CharacterEvents.characterDamaged += CharrctedTookDamage;
        CharacterEvents.characterHealed += CharrctedHealed;
        CharacterEvents.characterCritDamaged += CharrctedTookCritDamage;
        CharacterEvents.characterText += CharrctedText;
        CharacterEvents.characterShuDyeing += CharacterShuDyeing;
        CharacterEvents.characterCrit += CharacterCrit;
        CharacterEvents.characterTextW += CharacterTextW;
    }
   
    private void OnDisable()
    {
        CharacterEvents.characterDamaged -= CharrctedTookDamage;
        CharacterEvents.characterHealed -= CharrctedHealed;
        CharacterEvents.characterCritDamaged -= CharrctedTookCritDamage;
        CharacterEvents.characterText -= CharrctedText;
        CharacterEvents.characterShuDyeing -= CharacterShuDyeing;
        CharacterEvents.characterCrit -= CharacterCrit;
        CharacterEvents.characterTextW -= CharacterTextW;
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
    public void CharacterShuDyeing(GameObject character, string Text)
    {

        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tmpText = Instantiate(ShuDyeing, spawnPosition, Quaternion.identity, gameCanvas.transform)
            .GetComponent<TMP_Text>();
        tmpText.text = Text;
    }
    
    public void CharacterCrit(GameObject character, string Text)
    {

        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tmpText = Instantiate(crit, spawnPosition, Quaternion.identity, gameCanvas.transform)
            .GetComponent<TMP_Text>();
        tmpText.text = Text;
    }
    
    public void CharacterTextW(GameObject character, string Text)
    {

        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tmpText = Instantiate(text, spawnPosition, Quaternion.identity, gameCanvas.transform)
            .GetComponent<TMP_Text>();
        tmpText.text = Text;
    }

}
