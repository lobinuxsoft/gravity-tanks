using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ExpDrop : MonoBehaviour
{
    int animHash = Animator.StringToHash(nameof(ExpDrop));

    [SerializeField] float rotationSpeed = 45;
    [SerializeField] float modifier = 5;
    [SerializeField] float distanceToStartFollow = 5;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] TextMeshPro textMesh;

    int expToGive;

    Coroutine followRoutine;

    public Color ExpDropColor
    {
        get => meshRenderer.material.GetColor("_Color");
        set
        {
            meshRenderer.material.SetColor("_Color", value);
            trailRenderer.startColor = value;
            trailRenderer.endColor = value;
            textMesh.color = value;
        }
    }

    public int ExpToGive 
    {
        get => expToGive;
        set
        {
            expToGive = value;
            textMesh.text = $"{expToGive}<sprite name=\"token_icon\" color=#{ColorUtility.ToHtmlStringRGB(textMesh.color)}>";
        }
    }

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
        if (followRoutine == null && IsFollow && (Target.position - transform.position).sqrMagnitude < distanceToStartFollow * distanceToStartFollow)
            ForceToFollow();

        transform.GetChild(0).Rotate(Vector3.up * rotationSpeed);
    }

    public void ForceToFollow()
    {
        followRoutine = StartCoroutine(FollowRoutine());
    }

    IEnumerator FollowRoutine()
    {
        while ((Target.position - transform.position).sqrMagnitude > 0)
        {
            transform.position = Vector3.SmoothDamp(transform.position, Target.position, ref velocity, Time.unscaledDeltaTime * modifier);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsFollow = false;
            followRoutine = null;
            onTouchPlayer?.Invoke(this);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, distanceToStartFollow);
    }
}