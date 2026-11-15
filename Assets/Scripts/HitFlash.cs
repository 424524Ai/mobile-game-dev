
using UnityEngine;

// Controls the red screen flash when the player hits an obstacle.
public class HitFlash : MonoBehaviour
{
    [SerializeField] private GameObject flashImage;

    public void ShowFlash()
    {
        if (flashImage != null)
        {
            flashImage.SetActive(true);
        }
    }
}
