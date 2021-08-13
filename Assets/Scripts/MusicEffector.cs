using UnityEngine;

public class MusicEffector : MonoBehaviour
{
    void Update()
    {
        UpdateEverySecond();
    }

    private void UpdateEverySecond()
    {
        float[] spectrum = new float[256];
        AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
        float highestBass = 0;
        for (int i = 1; i < spectrum.Length - 1; i++)
        {
            if (spectrum[i] > highestBass)
            {
                highestBass = spectrum[i];
            }
        }
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(300, highestBass * 2160);
    }
}
