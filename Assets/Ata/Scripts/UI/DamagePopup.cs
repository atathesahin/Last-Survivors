using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private TextMeshPro damageText;
   
    public void ShowDamage(int damage)
    {
        damageText.text = damage.ToString();

       
        AnimateCharacterColors();

      
        transform.DOMoveY(transform.position.y + 1f, 0.5f).SetEase(Ease.OutQuad);
        damageText.DOFade(0, 0.5f).SetDelay(0.3f).OnComplete(() =>
        {
            DOTween.Kill(damageText);
            gameObject.SetActive(false);
        });
    }

    private void AnimateCharacterColors()
    {
        damageText.ForceMeshUpdate(); 
        TMP_TextInfo textInfo = damageText.textInfo;

     
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

       
            if (!charInfo.isVisible) continue;

        
            int vertexIndex = charInfo.vertexIndex;
            Color32[] vertexColors = textInfo.meshInfo[charInfo.materialReferenceIndex].colors32;

        
            Color32 startColor = new Color32(255, 0, 0, 255); 
            Color32 endColor = new Color32(0, 255, 0, 255);   

            DOTween.To(
                () => startColor,
                x =>
                {
             
                    vertexColors[vertexIndex + 0] = x;
                    vertexColors[vertexIndex + 1] = x;
                    vertexColors[vertexIndex + 2] = x;
                    vertexColors[vertexIndex + 3] = x;

               
                    damageText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                },
                endColor,
                1f 
            );
        }
    }
}