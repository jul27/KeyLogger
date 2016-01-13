using System.ComponentModel;
using System.Windows.Forms;

namespace DLL
{
    public class GlobalEvents : Component
    {
        protected override bool CanRaiseEvents
        {
            get
            {
                return true;
            }
        }
        private event KeyPressEventHandler m_KeyPress;
        public event KeyPressEventHandler KeyPress//задает глоб соб-е
        {
            add
            {
                if (m_KeyPress == null)
                {
                    Hook.KeyPress += Hook_KeyPress;
                }
                m_KeyPress += value;
            }
            remove
            {
                m_KeyPress -= value;
                if (m_KeyPress == null)
                {
                    Hook.KeyPress -= Hook_KeyPress;
                }
            }
        }
        void Hook_KeyPress(object sender, KeyPressEventArgs e)//обработчик событий в новом потоке
        {
            if (m_KeyPress != null)
            {
                m_KeyPress.Invoke(this, e);
            }
        }
    }
}
