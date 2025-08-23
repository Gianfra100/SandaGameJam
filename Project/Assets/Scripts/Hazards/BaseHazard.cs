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
        IGetHit getHit = collision.GetComponent<IGetHit>();
        getHit?.GetHit();
    }
}
