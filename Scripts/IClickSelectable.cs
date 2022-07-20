using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClickSelectable
{
    void OnSelectionStart();
    void UpdateSelection();

    void OnSelectionEnd();


}
