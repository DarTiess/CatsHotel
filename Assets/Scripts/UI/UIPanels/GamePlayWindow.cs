using System;
using UnityEngine;

namespace UI.UIPanels
{
    public class GamePlayWindow: PanelBase
    {

	    public override event Action ClickedPanel;

	    protected override void OnClickedPanel()
        {
            ClickedPanel?.Invoke();
            Debug.Log("settings button");
        }
    }
}