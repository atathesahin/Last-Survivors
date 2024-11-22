using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private TextMeshPro damageText;
   
    public void ShowDamage(int damage)
    {
        
        damageText.text = damage.ToString();

        // Karakter rengi değişimi başlat
        AnimateCharacterColors();

        // Yukarı hareket + şeffaflık azaltma
        transform.DOMoveY(transform.position.y + 1f, 0.5f).SetEase(Ease.OutQuad);
        damageText.DOFade(0, 0.5f).SetDelay(0.3f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    private void AnimateCharacterColors()
    {
        damageText.ForceMeshUpdate(); // Mesh'i güncelle (TextMeshPro içi)
        TMP_TextInfo textInfo = damageText.textInfo;

        // Karakterlerin mesh bilgileri
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            // Geçerli karakter bilgisi
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

            // Karakter görünür mü? (örneğin boşlukları atla)
            if (!charInfo.isVisible) continue;

            // Vertex renklerini al
            int vertexIndex = charInfo.vertexIndex;
            Color32[] vertexColors = textInfo.meshInfo[charInfo.materialReferenceIndex].colors32;

            // Her karakter için animasyon başlat
            Color32 startColor = new Color32(255, 0, 0, 255); // Kırmızı
            Color32 endColor = new Color32(0, 255, 0, 255);   // Yeşil

            DOTween.To(
                () => startColor,
                x =>
                {
                    // Vertex renklerini güncelle
                    vertexColors[vertexIndex + 0] = x;
                    vertexColors[vertexIndex + 1] = x;
                    vertexColors[vertexIndex + 2] = x;
                    vertexColors[vertexIndex + 3] = x;

                    // Mesh'i güncelle
                    damageText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                },
                endColor,
                1f // Animasyon süresi
            );
        }
    }
}