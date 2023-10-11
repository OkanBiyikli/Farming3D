using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Security.Cryptography;

public class LockedUnitController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int price;
    [SerializeField] private int ID; //bu idyi inspectorden vericez her ürün farklý idye sahip olcak 2 tane cabbagefield varsa birine 1 birine 2 vericez

    [Header("Objects")]
    [SerializeField] private TextMeshPro priceText;
    [SerializeField] private GameObject lockedUnit;
    [SerializeField] private GameObject unlockedUnit;
    private bool isPurchased;
    private string keyUnit = "keyUnit";
    // Start is called before the first frame update
    void Start()
    {
        priceText.text = price.ToString();
        LoadUnit();     
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPurchased)
        {
            UnlockUnit();
            //ürünü para yeterse aç
        }
    }

    private void UnlockUnit()
    {
        if(CashManager.instance.TryBuyThisUnit(price))
        {
            AudioManager.instance.PlayAudio(AudioClipType.shopClip);
            Unlock();
            SaveUnit();
        }
        //parasý var mý kontrol et
        //varsa ürünü aç
    }

    private void Unlock()
    {
        isPurchased = true;
        lockedUnit.SetActive(false);
        unlockedUnit.SetActive(true);
    }

    private void SaveUnit()
    {
        string key = keyUnit + ID.ToString();//keyunit2,  keyunit3 gibi
        PlayerPrefs.SetString(key, "saved");
    }

    private void LoadUnit()
    {
        string key = keyUnit + ID.ToString();
        string status = PlayerPrefs.GetString(key);

        if(status.Equals("saved"))
        {
            Unlock();
        }
    }
}
