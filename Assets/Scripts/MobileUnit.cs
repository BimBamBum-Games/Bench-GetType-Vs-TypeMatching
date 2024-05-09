using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileUnit : UnitEntityBase
{
    public HideFlags editorFlag = HideFlags.HideInHierarchy;

    private void OnEnable() {
        gameObject.hideFlags = editorFlag;
    }
}
