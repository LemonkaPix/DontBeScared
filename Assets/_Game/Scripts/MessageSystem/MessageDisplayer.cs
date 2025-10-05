using Unity.VisualScripting;
using UnityEngine;

namespace Core.Web
{
    public class MessageDisplayer : Singleton<MessageDisplayer>
    {
        [SerializeField]
        private WindowsMessagesInfo TerminalMessages;

        [SerializeField]
        private WindowsMessagesInfo GameMessages;

        private int currentGame = 0;
        private int currentMessage = 0;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            DontDestroyOnLoad(this.gameObject);
        }

        
        public void DisplayNextMessage()
        {
            if(currentGame == 0)
            {
                for (int i = 0; i < TerminalMessages.Messages[currentMessage].ConMessages.Count; i++)
                {
                    WindowMess oneMessage = TerminalMessages.Messages[currentMessage];
                    WindowsMessage.Instance.Message_Box(oneMessage.ConMessages[i], oneMessage.Caption, oneMessage.MessageType);
                }

                currentMessage++;

                if(currentMessage == TerminalMessages.Messages.Count)
                {
                    currentMessage = 0;
                    currentGame = 1;
                }
            }
            else
            {
                for (int i = 0; i < GameMessages.Messages[currentMessage].ConMessages.Count; i++)
                {
                    WindowMess oneMessage = GameMessages.Messages[currentMessage];
                    WindowsMessage.Instance.Message_Box(oneMessage.ConMessages[i], oneMessage.Caption, oneMessage.MessageType);
                }

                currentMessage++;

                if (currentMessage == GameMessages.Messages.Count)
                {
                    currentMessage = 0;
                    currentGame = 2;
                }
            }
        }

        public void ResetCurrentMessage()
        {
            currentMessage = 0;
        }
    }
}
