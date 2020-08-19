using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackDummy : BeneathSelectable<MasterInterface>
{

    public override void OnSubmit(BaseEventData eventData)
    { }

    public override void OnCancel(BaseEventData eventData)
    {
        Master.NavigateBack();
    }
}
