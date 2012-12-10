using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace LiteApp.MySpace.Views.Helpers
{
    public static class ButtonExtensions
    {
        public static void AutomationPeerInvoke(this Button button)
        {
            ButtonAutomationPeer peer = new ButtonAutomationPeer(button);
            IInvokeProvider invokeProv = (IInvokeProvider)peer.GetPattern(PatternInterface.Invoke);
            invokeProv.Invoke();
        }
    }
}
