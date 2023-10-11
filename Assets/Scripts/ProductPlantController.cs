using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductPlantController : MonoBehaviour
{
    private bool isReadyToPick;
    private Vector3 originalScale;

    [SerializeField] private ProductData productData;//kasam�z
    private BagController bagController;

    [SerializeField] private float growUpTime = 5f;
    // Start is called before the first frame update
    void Start()
    {
        isReadyToPick = true;
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && isReadyToPick)
        {
            bagController = other.GetComponent<BagController>();

            if(bagController.IsEmptySpace())
            {
                AudioManager.instance.PlayAudio(AudioClipType.grapClip);
                bagController.AddProductToBag(productData);
                isReadyToPick = false;
                StartCoroutine(ProductsPicked());
            }
        }
    }

    IEnumerator ProductsPicked()//leantween k�t�phanesi sayesinde alttaki productpicked�n ayn�s�n� sa�layabiliyoruz
    {
        Vector3 targetScale = originalScale / 3;
        transform.gameObject.LeanScale(targetScale, 1f);

        yield return new WaitForSeconds(growUpTime);//fidemizin b�y�me s�resini burdan de�i�tirebiliriz

        transform.gameObject.LeanScale(originalScale, 1f).setEase(LeanTweenType.easeOutBack);
        isReadyToPick = true;
    }


    /*IEnumerator ProductPicked()
    {
        float duration = 1f;
        float timer = 0;

        Vector3 targetScale = originalScale / 3;

        while(timer < duration)//k���lme i�lemi
        {
            float t = timer / duration;      //0            //100       /%?
            Vector3 newScale = Vector3.Lerp(originalScale, targetScale, t);
                                    //ilk veriden ikinci veriye ge�i�i bir y�zdelik de�erle sa�l�yoruz (lerp)
            transform.localScale = newScale;
            timer += Time.deltaTime;
            yield return null;//coroutine oldu�u i�in belirrtik
        }

        yield return new WaitForSeconds(5f);

        timer = 0;
        float growBackDuration = 1f;

        while(timer <  growBackDuration)//b�y�me i�lemi
        {
            float t = timer / growBackDuration;
            
            Vector3 newScale= Vector3.Lerp(targetScale, originalScale, t);
            transform.localScale = newScale;
            timer += Time.deltaTime;
            yield return null;
        }

        isReadyToPick = true;
        yield return null;
    }*/
}
