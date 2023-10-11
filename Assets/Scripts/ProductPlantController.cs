using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductPlantController : MonoBehaviour
{
    private bool isReadyToPick;
    private Vector3 originalScale;

    [SerializeField] private ProductData productData;//kasamýz
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

    IEnumerator ProductsPicked()//leantween kütüphanesi sayesinde alttaki productpickedýn aynýsýný saðlayabiliyoruz
    {
        Vector3 targetScale = originalScale / 3;
        transform.gameObject.LeanScale(targetScale, 1f);

        yield return new WaitForSeconds(growUpTime);//fidemizin büyüme süresini burdan deðiþtirebiliriz

        transform.gameObject.LeanScale(originalScale, 1f).setEase(LeanTweenType.easeOutBack);
        isReadyToPick = true;
    }


    /*IEnumerator ProductPicked()
    {
        float duration = 1f;
        float timer = 0;

        Vector3 targetScale = originalScale / 3;

        while(timer < duration)//küçülme iþlemi
        {
            float t = timer / duration;      //0            //100       /%?
            Vector3 newScale = Vector3.Lerp(originalScale, targetScale, t);
                                    //ilk veriden ikinci veriye geçiþi bir yüzdelik deðerle saðlýyoruz (lerp)
            transform.localScale = newScale;
            timer += Time.deltaTime;
            yield return null;//coroutine olduðu için belirrtik
        }

        yield return new WaitForSeconds(5f);

        timer = 0;
        float growBackDuration = 1f;

        while(timer <  growBackDuration)//büyüme iþlemi
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
