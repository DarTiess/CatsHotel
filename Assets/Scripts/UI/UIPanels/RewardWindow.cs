using System;

namespace UI.UIPanels
{
    public class RewardWindow: PanelBase
    {
        public override event Action ClickedPanel;
        protected override void OnClickedPanel()
        {
            ClickedPanel?.Invoke();
        }
    }
}