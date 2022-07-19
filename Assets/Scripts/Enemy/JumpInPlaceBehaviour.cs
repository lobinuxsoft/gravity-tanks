using UnityEngine;

namespace GravityTanks.Enemy.Behaviour
{
    [CreateAssetMenu(menuName = "Enemy/Behaviour/Jump In Place")]
    public class JumpInPlaceBehaviour : IABehaviour
    {
        [SerializeField] float jumpTimer = 3f;
        [SerializeField] float jumpForce = 8;

        Rigidbody body;
        GroundDetector gDetector;
        float timer = 0;

        public override void DoBehaviour(GameObject owner)
        {
            if(!gDetector) gDetector = owner.GetComponent<GroundDetector>();
            if (!body) body = owner.GetComponent<Rigidbody>();

            body.angularDrag = 100;

            if (gDetector.OnGround)
            {
                if (timer > jumpTimer)
                    body.velocity = Vector3.up * jumpForce;
                else
                    timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
            }
        }
    }
}