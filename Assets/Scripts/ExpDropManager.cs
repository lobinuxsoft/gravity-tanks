using UnityEngine;

namespace HNW
{
    [RequireComponent(typeof(ObjectPool))]
    public class ExpDropManager : MonoBehaviour
    {
        [SerializeField] LongVariable exp;
        public ExpData[] exps;
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
                int rnd = Random.Range(0, exps.Length);
                drop.ExpDropColor = exps[rnd].color;
                drop.ExpToGive = exps[rnd].value;
                drop.onTouchPlayer += OnTouchPlayer;
            }

            temp.SetActive(true);
        }

        public void GrabAllActiveExpDrop()
        {
            var expDrops = GetComponentsInChildren<ExpDrop>();

            for (int i = 0; i < expDrops.Length; i++)
            {
                expDrops[i].ForceToFollow();
            }
        }

        private void OnTouchPlayer(ExpDrop drop)
        {
            exp.Value += drop.ExpToGive;
            drop.onTouchPlayer -= OnTouchPlayer;
            pool.ReturnToPool(drop.gameObject);
        }
    }
}

[System.Serializable]
public struct ExpData
{
    public int value;
    [ColorUsage(true, true)] public Color color;
}