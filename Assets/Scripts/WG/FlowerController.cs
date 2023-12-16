using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerController : MonoBehaviour
{
    [SerializeField] HPController hpController;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            ItemData itemData = collision.GetComponent<ItemMovement>()?.itemData;

            if (itemData != null)
                hpController.MaxHP += itemData.addHP;

            Destroy(collision.gameObject);
        }
    }
}
