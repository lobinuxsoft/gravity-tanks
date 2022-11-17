using UnityEngine;

namespace HNW
{
    [RequireComponent(typeof(ObjectPool))]
    public class ExpDropManager : MonoBehaviour
    {
        [SerializeField] LongVariable exp;
        [SerializeField, GradientUsage(true)] Gradient gradient;
        [SerializeField] int minExp = 10;
        [SerializeField] int maxExp = 1000;
        ObjectPool pool;

        private void Awake()
        {
            pool = GetComponent<ObjectPool>();
        }

        public void DropExpInPlace(Vector3 position)
        {
            Random.InitState((int)Time.time);

            var temp = pool.GetFromPool(false);
            temp.transform.position = position;

            if(temp.TryGetComponent(out ExpDrop drop))
            {
                float rnd = Random.Range(0f, 1f);
                drop.ExpToGive = (int)Mathf.Lerp(minExp, maxExp, rnd);
                drop.ExpDropColor = gradient.Evaluate(rnd);
                drop.onTouchPlayer += OnTouchPlayer;
            }

            temp.SetActive(true);
        }

        private void OnTouchPlayer(ExpDrop drop)
        {
            exp.Value += drop.ExpToGive;
            drop.onTouchPlayer -= OnTouchPlayer;
            pool.ReturnToPool(drop.gameObject);
        }
    }
}