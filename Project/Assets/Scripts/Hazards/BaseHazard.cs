using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BaseHazard : MonoBehaviour
{
    void Awake()
    {
        gameObject.GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioManager.Instance.Play2DSound(HazardSoundsEnum.Electricity);

        IGetHit getHit = collision.GetComponent<IGetHit>();
        getHit?.GetHit();
    }
}
