using UnityEngine;

namespace GravityTanks.Utils
{
    public class GameObjectDestroyer : MonoBehaviour
    {
        public void DestroyGameobject() => Destroy(gameObject);
    }
}