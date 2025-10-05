using Core.Web;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Web
{

    [CreateAssetMenu(fileName = "WindowsMessagesInfo", menuName = "Scriptable Objects/WindowsMessagesInfo")]
    public class WindowsMessagesInfo : ScriptableObject
    {
        [SerializeField]
        private List<WindowMess> messages = new List<WindowMess>();

        public List<WindowMess> Messages => messages;
    }


    [Serializable]
    public class WindowMess
    {
        [SerializeField]
        private string caption;
        [SerializeField]
        private List<string> conMessages = new List<string>();
        [SerializeField]
        private MESSAGE_TYPES messageType;

        public string Caption => caption;
        public List<string> ConMessages => conMessages;
        public MESSAGE_TYPES MessageType => messageType;
    }
}

