using UnityEngine;
using System.Collections;
using TagsUtility;

public class TagSelectorExample : MonoBehaviour
{
    [TagSelector]
    public string TagFilter = "";

    [TagSelector]
    public string[] TagFilterArray = new string[] { };
}