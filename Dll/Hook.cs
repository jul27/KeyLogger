using System.Windows.Forms;

namespace DLL
{
    public static partial class Hook
    {
        private static event KeyPressEventHandler s_KeyPress;
        public static event KeyPressEventHandler KeyPress
        {
            add
            {
                SetHook();
                s_KeyPress += value;
            }
            remove
            {
                s_KeyPress -= value;
                UnHook();
            }
        }
    }
}
