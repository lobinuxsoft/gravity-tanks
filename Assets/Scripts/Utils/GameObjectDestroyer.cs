using UnityEngine;

namespace HNW.Utils
{
    public class GameObjectDestroyer : MonoBehaviour
    {
        public void DestroyGameobject() => Destroy(gameObject);
    }
}