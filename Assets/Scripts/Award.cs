using UnityEngine;

public class Award : MonoBehaviour
{
    public float speed = 1f;
    
    public AwardType awardType;

    void Start()
    {
        
    }
    
    void Update()
    {
        transform.Translate(Vector3.down * (speed * Time.deltaTime));

        if (transform.position.y < -Constants.DestroyYLimit)
        {
            Destroy(this.gameObject);
        }
    }
}
