using UnityEngine;

public class MoneyBall : MonoBehaviour
{
    Vector3 target;
    AlchemistElf alchemistElf;
    public void Init(Vector3 Target, AlchemistElf parent)
    {
        alchemistElf = parent;
        target = Target;
    }

    private void FixedUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        Vector3 direction = target - transform.position;
        float currentDistSqr = (transform.position - target).sqrMagnitude;
        if (currentDistSqr <= 0.35f)
        {
            alchemistElf.CreateMoney(10, 10, transform.position);
            Destroy(gameObject);
        }
        transform.Translate(10f * Time.deltaTime * direction.normalized);
    }
}
