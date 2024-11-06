using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reasource : MonoBehaviour
{
    public ItemData itemToGive;
    public int quantityPrtHit = 1;
    public int capacy;

    public void Gather(Vector3 hitPoint, Vector3 hitNoraml)
    {
        for (int i = 0; i < quantityPrtHit; i++)
        {
            if (capacy <= 0) break;
            capacy -= 1;
            Instantiate(itemToGive.dropPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNoraml, Vector3.up));
        }
    }

}
