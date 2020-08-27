using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatText : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private Text sctText;

    [SerializeField]
    private float lifeTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeOut());
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    public IEnumerator FadeOut()
    {
        float startAlpha = sctText.color.a;
        float rate = 1.0f / lifeTime;
        float progress = 0.0f;

        while (progress < 1)
        {
            Color tmp = sctText.color;
            tmp.a = Mathf.Lerp(startAlpha, 0, progress);
            sctText.color = tmp;
            progress += rate * Time.deltaTime;

            yield return null;
        }

        Destroy(gameObject);
        
    }
}

