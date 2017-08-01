using UnityEngine;
using UnityEngine.UI;

public class HealthBarExample : MonoBehaviour
{
    public GameObject player;

    private Image image;

    // Use this for initialization
    private void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    private void Update()
    {
        float time = player.GetComponent<Timer>().currentTime;
        float maxTime = player.GetComponent<Timer>().maxTime;

        image.fillAmount = 1f - (time / maxTime);
    }
}
