using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMessagePrefab : MonoBehaviour
{
   public TextMeshProUGUI messageText;
   public Image messageImage;
   [SerializeField]private bool isFade = false;
   private Color textFadeColor => new Color(messageText.color.r, messageText.color.g, messageText.color.b, 0); 
   private Color ImageFadeColor => new Color(messageImage.color.r, messageImage.color.g, messageImage.color.b, 0); 
   public void UpdateMessageText(string text)
   {
      messageText.text = text;
   }
   public void DestroyOrFadeAnimator(float delayTime)
   {
      if (!isFade)
      {
         if (delayTime != 0)
         {
            isFade = true;
            DOTween.To(() => messageText.color, x => messageText.color = x, textFadeColor, delayTime).SetEase(Ease.OutQuad);
            DOTween.To(() => messageImage.color, x => messageImage.color = x, ImageFadeColor, delayTime).SetEase(Ease.OutQuad).OnComplete((
               () =>
               {
                  Destroy(gameObject);
               }));
         }
         else
         {
            Destroy(gameObject);
         }
      }
   }
}
