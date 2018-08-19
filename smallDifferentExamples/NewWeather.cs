using UnityEngine;

public class NewWeather : MonoBehaviour
{
    public float changeTimer = 30.0f;
    private int currentEffect = -1;

    public GameObject[] effects;

    private void ChangeEffect()
    {
        currentEffect += 1;
        if (currentEffect >= effects.Length)
        {
            currentEffect = -1;
        }
        foreach (var c in effects)
        {
            c.SetActive(false);
        }
        if (currentEffect >= 0)
        {
            effects[currentEffect].SetActive(true);
        }
    }

    private void Update()
    {
        changeTimer -= Time.deltaTime;
        if (changeTimer <= 0.0f)
        {
            ChangeEffect();
            changeTimer = 30.0f;
        }
    }
}