using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Chat;
using ExitGames.Client.Photon;
#if PHOTON_UNITY_NETWORKING
using Photon.Pun;
#endif
using System;


public class ChatManager : MonoBehaviour, IChatClientListener
{
    public ChatClient chatClient;
    private string worldChat;
    [SerializeField] InputField p1rName;
    [SerializeField] Text connectionState;
    [SerializeField] InputField msgInput;
    [SerializeField] Text msgArea;
    [SerializeField] GameObject introPanel;
    [SerializeField] GameObject msgPanel;

    protected internal ChatSettings chatAppSettings;
    public string UserName { get; set; }

    void Start()
    {
        Application.runInBackground = true;
        // if (string.IsNullOrEmpty(PhotonNetwork.PhotonServerSettings.ChatAppID))
        // {
        //     print(" No chat ID provided  ");
        //     return;
        // }
        if (string.IsNullOrEmpty(this.UserName))
		{
		    this.UserName = "user" + Environment.TickCount%99; //made-up username
		}
        #if PHOTON_UNITY_NETWORKING
        this.chatAppSettings = PhotonNetwork.PhotonServerSettings.AppSettings;
        #else
        if (this.chatAppSettings == null)
        {
            this.chatAppSettings = ChatSettings.Instance;
        }
        #endif
        
        print(" Have ID provided ");
         
        connectionState.text = "Connecting....";
        worldChat = "world";
    }
    void Update()
    {
        if (this.chatClient != null)
        this.chatClient.Service();
    }
    
    public void GetConnected()
    {
        print("Trying to connect");
        UserName = p1rName.text;
        this.chatClient = new ChatClient(this);
        
        //this.chatClient.Connect(PhotonNetwork.PhotonServerSettings.ChatAppID, "anything",new ExitGames.Client.Photon.Chat.AuthenticationValues(p1rName.text));
        this.chatClient.Connect(this.chatAppSettings.AppId, "1.0", new Photon.Chat.AuthenticationValues(this.UserName));
                
        connectionState.text = "Connecting to chat";
    }

    public void SendMsg()
    {
        Debug.Log("提交信息");
        this.chatClient.PublishMessage(worldChat,msgInput.text);
        msgInput.text ="";
    }

    
    public void DebugReturn(DebugLevel level, string message)
    {}

    public void OnDisconnected()
    {
        Debug.Log("*****************");
    }

    public void OnConnected()
    {
        msgArea.text = "";
        introPanel.SetActive(false);
        msgPanel.SetActive(true);
        connectionState.text = "Conncted";
        Debug.Log("** Connected");
        this.chatClient.Subscribe(new string[] {"world"});//worldChat = "world"
        this.chatClient.SetOnlineStatus(ChatUserStatus.Online);
        //onsubscribed(new string[] f worldChat J,new bool[Jftrue ]);//worldChat = "world"
    }

    public void OnChatStateChange(ChatState state)
    {}

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        // Debug.Log(" msgArea  "tmsgArea);
        if (msgArea!=null)
        {
            for (int i = 0; i < senders.Length; i++)
            {
                msgArea.text += senders[i] + " : " + messages[i] + "\n" ;
            }
        }        
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {}
    public void OnSubscribed(string[] channels, bool[] results)
    {
        connectionState.text = "In World Chat";
        this.chatClient.PublishMessage(worldChat, "Joined" );
    }

    public void OnUnsubscribed(string[] channels)
    {}

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {}

    public void OnUserSubscribed(string channel, string user)
    {}

    public void OnUserUnsubscribed(string channel, string user)
    {}
}