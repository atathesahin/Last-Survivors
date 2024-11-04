using UnityEngine;
using System.Collections.Generic;

public class EffectPool : MonoBehaviour
{
    public static EffectPool Instance;
    public GameObject effectPrefab;
    public int poolSize = 10;

    private List<GameObject> effectPool;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        effectPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject effect = Instantiate(effectPrefab);
            effect.SetActive(false);
            effectPool.Add(effect);
        }
    }

    public GameObject GetEffect()
    {
        foreach (GameObject effect in effectPool)
        {
            if (!effect.activeInHierarchy)
            {
                return effect;
            }
        }
        return null;
    }
}