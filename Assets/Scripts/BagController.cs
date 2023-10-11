using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BagController : MonoBehaviour
{
    [SerializeField] private Transform bag;

    public List<ProductData> productDataList;
    private Vector3 productSize;
    public int maxBagCapacity;
    //public GameObject addCapacity;//kapasiteyi arttýrma eklicez 

    [SerializeField] private TextMeshPro maxText;

    public static BagController instance;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //maxBagCapacity = 5;
        LoadBagCapacity();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("ShopPoint"))
        {
            PlayShopSound();
            for (int i = productDataList.Count - 1; i >= 0; i--)
            {
                SellProductsToShop(productDataList[i]);
                Destroy(bag.transform.GetChild(i).gameObject);
                productDataList.RemoveAt(i);
            }

            ControlBagCapacity();
        }

        if(other.CompareTag("UnlockedBakeryUnit"))
        {
            UnlockBakeryUnityController bakeryUnit = other.GetComponent<UnlockBakeryUnityController>();

            ProductType neededProduct = bakeryUnit.GetNeededProductType();//inspectorde pastanemizde belirlediðimiz ürünü neededprdocut diye belirttik

            for (int i = productDataList.Count - 1; i >= 0; i--)//taþýdýðýmýz ürünler
            {
                if (productDataList[i].productType == neededProduct)//taþýdýðýmýz ürünler pastanenin ihtiyacý olan ürünlerse
                                                                    //(taþýdýðýmýz ürün pastanenin ihtiyacý olan ürünle uyuþuyorsa)
                {
                    if (bakeryUnit.StoreProduct() == true)//pastanenin ürün ihtiyacý veya boþluðu varsa (al ve yok et)
                    {
                        Destroy(bag.transform.GetChild(i).gameObject);
                        productDataList.RemoveAt(i);
                    }
                }
            }

            StartCoroutine(PutProductsInOrder());//çantayý yeniden düzenle
            ControlBagCapacity();//iþlem tamamlandýðýnda çantayý kontrol et
        }
    }

    private void SellProductsToShop(ProductData productData)
    {
        //cashmanagera söyle ürün satýldý
        CashManager.instance.ExchangeProduct(productData);
    }

    public void AddProductToBag(ProductData productData)
    {
        /*if(!IsEmptySpace())//eðer çantada yer yoksa
        {
            return;//geri dön alttaki kodlarý çalýþtýrma
        }*/

        GameObject boxProduct = Instantiate(productData.productPrefab, Vector3.zero, Quaternion.identity);
        boxProduct.transform.SetParent(bag, true);

        CalculateObjectSize(boxProduct);
        float yPosition = CalculateNewYPositionOfBox();
        boxProduct.transform.localRotation = Quaternion.identity;
        boxProduct.transform.localPosition = Vector3.zero;
        boxProduct.transform.localPosition = new Vector3(0, yPosition, 0);
        productDataList.Add(productData);
        ControlBagCapacity();
    }

    private float CalculateNewYPositionOfBox()
    {
        //ürünün sahnedeki yüksekliði x ürünün adedi
        float newYPos = productSize.y * productDataList.Count;
        return newYPos;
    }

    private void CalculateObjectSize(GameObject gameObject)
    {
        if(productSize == Vector3.zero)
        {
            MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
            productSize = renderer.bounds.size;
        }
    }

    private void ControlBagCapacity()
    {
        if(productDataList.Count == maxBagCapacity)
        {
            SetMaxTextOn();
        }
        else
        {
            SetMaxTextOff();
        }
    }

    private void SetMaxTextOn()
    {
        if(!maxText.isActiveAndEnabled)
        {
            maxText.gameObject.SetActive(true);
        }
    }

    private void SetMaxTextOff()
    {
        if (maxText.isActiveAndEnabled)
        {
            maxText.gameObject.SetActive(false);
        }
    }

    public bool IsEmptySpace()
    {
        if(productDataList.Count < maxBagCapacity)
        {
            return true;
        }
        return false;
    }

    private IEnumerator PutProductsInOrder()
    {
        yield return new WaitForSeconds(0.15f);
        for(int i = 0; i < bag.childCount; i++)
        {
            float newPosY = productSize.y * i;
            bag.GetChild(i).transform.localPosition = new Vector3(0, newPosY, 0);
        }
    }

    public void SaveBagCapacity()
    {
        PlayerPrefs.SetInt("capacity", maxBagCapacity);
        Debug.Log("Kapasite kaydedildi");
    }

    private void LoadBagCapacity()
    {
        maxBagCapacity = PlayerPrefs.GetInt("capacity", 5);
    }

    private void PlayShopSound()
    {
        if(productDataList.Count > 0)
        {
            AudioManager.instance.PlayAudio(AudioClipType.shopClip);
        }
    }
}
