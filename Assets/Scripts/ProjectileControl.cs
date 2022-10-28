using System;
using UnityEngine;

public class ProjectileControl : MonoBehaviour
{
    [SerializeField, Range(1, 360)] int emmitAmount = 1; 
    [SerializeField] float shootRate = .5f;
    [SerializeField] LayerMask layerToCollide;

    [SerializeField]float arcShootAngle = 0;
    ParticleSystem particle;
    ParticleSystem.EmissionModule emissionModule;
    ParticleSystem.CollisionModule collisionModule;
    ParticleSystem.ShapeModule shapeModule;

    public event Action<GameObject> OnProjectileHit;

    public int EmmitAmount
    {
        get => emmitAmount;
        set 
        { 
            emmitAmount = value;
            ArcShootAngle = emmitAmount < 2 ? 0 : emmitAmount;

            if (emissionModule.enabled)
            {
                ParticleSystem.Burst burst = new ParticleSystem.Burst(0, emmitAmount);
                emissionModule.SetBursts(new ParticleSystem.Burst[] { burst });
            }
        }
    }

    public float ArcShootAngle
    {
        get => arcShootAngle;
        private set
        {
            arcShootAngle = Mathf.Clamp(value, 0, 360);

            if (shapeModule.enabled)
            {
                shapeModule.radius = 0;
                shapeModule.radiusThickness = 0;
                shapeModule.arcMode = ParticleSystemShapeMultiModeValue.BurstSpread;
                shapeModule.arcSpread = 0;
                shapeModule.shapeType = ParticleSystemShapeType.Circle;

                shapeModule.arc = arcShootAngle;
                shapeModule.rotation = new Vector3(90f, -90f + (shapeModule.arc / 2), 0f);
            }
        }
    }

    public float ShootRate
    {
        get => shootRate;
        set => shootRate = value;
    }

    public LayerMask LayerToCollide
    {
        get => layerToCollide;
        set
        {
            layerToCollide = value;

            if(collisionModule.enabled)
            {
                collisionModule.collidesWith = layerToCollide;
                collisionModule.sendCollisionMessages = true;
            }
        }
    }

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();

        emissionModule = particle.emission;
        collisionModule = particle.collision;
        shapeModule = particle.shape;

        EmmitAmount = emmitAmount;
        LayerToCollide = layerToCollide;
        ArcShootAngle = emmitAmount;
    }

    void OnParticleCollision(GameObject other)
    {
        Debug.Log($"Cillide with {other.name}");
        OnProjectileHit?.Invoke(other);
    }
}