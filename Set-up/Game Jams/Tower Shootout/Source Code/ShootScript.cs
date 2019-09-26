using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour
{
    public float firerate = 0.5f;
    float nextFire = 0f;
    Vector2 bulletPos;
    public GameObject Bullet;
    public float veloX = 0f;
    float veloY = 0f;

    float chargeTimeMultiplier = 2f;
    float maxChargeTimeValue = 2f;
    float minBulletSpeed = 4f;
    float exponentialSpeedValue = 4f;
    float SpawnDistanceBulletFromPlayer = 1.5f;
    Vector3 pSystemCompensationFactor = new Vector3(1, 0, 0);
    public float chargeTimeInSec = 0f;
    public bool playerFacingRight = true;

    public Rigidbody2D compensatingFactor;
    public ParticleSystem pSystem;
    public float works = 1f;
    public float amountOfParticles = 0f;

    float bulletScaleDivider = 2f;
    public float bulletScale = 1f;
    float minBulletScale = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Both charging and the bulletscale will be affected by the chargeTimer value
    public void Shoot(float chargeTimer)
    {
        chargeTimeInSec = chargeTimer;
        float chargeTime = chargeTimeInSec*chargeTimeMultiplier;
        if(chargeTime > maxChargeTimeValue)
        {
            chargeTime = maxChargeTimeValue;
        }

        veloX = Mathf.Pow(exponentialSpeedValue, chargeTime+0.5f) + minBulletSpeed;
        if (!playerFacingRight)
        {
            veloX *= -1;
        }

        float bulletScaleDifference = chargeTime / bulletScaleDivider;
        bulletScale = bulletScaleDifference + minBulletScale;
        if (playerFacingRight)
        {
            bulletPos = transform.position + new Vector3(bulletScaleDifference, 0, 0);
        }
        else
        {
            bulletPos = transform.position - new Vector3(bulletScaleDifference, 0, 0);
        }
        
        bulletPos += new Vector2(SpawnDistanceBulletFromPlayer, 0f);
        GameObject bullet = Instantiate(Bullet, bulletPos, Quaternion.identity);

        
        bullet.transform.localScale = new Vector3(bulletScale, bulletScale, bulletScale);
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(veloX, veloY);
        pSystem.Stop();
    }

    public void ChangePlayerDirectionToLeft()
    {
        if (!playerFacingRight) return;
        playerFacingRight = false;
        SpawnDistanceBulletFromPlayer = -1.5f;
        var shape = pSystem.shape;
        //shape.rotation = new Vector3(0, 180, 0);
        //pSystem.transform.position -= pSystemCompensationFactor;
        
    }

    public void ChangePlayerDirectionToRight()
    {
        if (playerFacingRight) return;
        playerFacingRight = true;
        SpawnDistanceBulletFromPlayer = 1.5f;
        var shape = pSystem.shape;
        //shape.rotation = new Vector3(0, 0, 0);
        //pSystem.transform.position += pSystemCompensationFactor;
    }

    public void UseParticles(float chargeTimer)
    {
        if (!pSystem.isPlaying)
        {
            pSystem.Play();
        }
        amountOfParticles = chargeTimer * 10f;

        var em = pSystem.emission;
        em.rateOverTime = amountOfParticles;
    }
}
