using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuffItem
{
    [SerializeField] string titleEn;
    public string TitleEn => titleEn;
    [SerializeField] string title;
    public string Title => title;
    [SerializeField] float minRange;
    public float MinRange => minRange;
    [SerializeField] float maxRange;
    public float MaxRange => maxRange;

}
