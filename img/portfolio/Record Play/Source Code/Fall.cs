using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour
{
    [SerializeField]
    private float fallTime = 0;
    [SerializeField]
    private bool falling = false;

    public float maxFallTime = 3f;
    public float fallSpeed = 1f;
    public bool destroyAfterFall;

    public Fallable fallable;

    public void PlayFall(Fallable fallable)
    {
        this.fallable = fallable;
        var playerController = fallable.fallableRoot.GetComponent<PlayerController>();
        if (playerController)
        {
            playerController.rb.constraints = RigidbodyConstraints2D.FreezePosition;
        }
        falling = true;
    }

    void Update()
    {
        if (falling)
        {
            if (fallTime <= maxFallTime)
            {
                var fallableRootTransform = fallable.fallableRoot.transform;
                fallableRootTransform.localScale = new Vector3(
                    Mathf.Clamp(fallableRootTransform.localScale.x - fallSpeed * Time.deltaTime, 0, fallableRootTransform.localScale.x),
                    Mathf.Clamp(fallableRootTransform.localScale.y - fallSpeed * Time.deltaTime, 0, fallableRootTransform.localScale.y));
                fallTime += Time.deltaTime;
            }
            else
            {
                fallTime = 0;
                falling = false;
                if (destroyAfterFall)
                {
                    Destroy(fallable.gameObject);
                    Destroy(fallable.fallableRoot.gameObject);
                }
            }
        }
    }
}
