using System;
using UnityEngine;

public class ExpDrop : MonoBehaviour
{
    int animHash = Animator.StringToHash(nameof(ExpDrop));

    [SerializeField] float rotationSpeed = 45;
    [SerializeField] float modifier = 5;
    [SerializeField] float distanceToStartFollow = 5;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] TrailRenderer trailRenderer;


    public Color ExpDropColor
    {
        get => meshRenderer.material.GetColor("_EmissionColor");
        set
        {
            meshRenderer.material.SetColor("_EmissionColor", value);
            trailRenderer.startColor = value;
            trailRenderer.endColor = value;
        }
    }

    [field:SerializeField] public int ExpToGive { get; set; }

    public Transform Target { get; set; }

    public bool IsFollow { get; set; }

    Vector3 velocity = Vector3.zero;

    Animator animator;

    public event Action<ExpDrop> onTouchPlayer;

    private void Awake()
    {
        Target = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        velocity = Vector3.zero;

        if (animator)
            animator.CrossFade(animHash, 0);
    }

    public void StartFollow()
    {
        IsFollow = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsFollow && (Target.position - transform.position).sqrMagnitude < distanceToStartFollow * distanceToStartFollow)
            transform.position = Vector3.SmoothDamp(transform.position, Target.position, ref velocity, Time.deltaTime * modifier);

        transform.GetChild(0).Rotate(Vector3.up * rotationSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsFollow = false;
            onTouchPlayer?.Invoke(this);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, distanceToStartFollow);
    }
}