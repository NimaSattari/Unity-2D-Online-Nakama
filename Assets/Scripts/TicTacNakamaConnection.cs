using Nakama;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TicTacNakamaConnection : MonoBehaviour
{
    public string scheme = "http";
    public string host = "localhost";
    public int port = 7350;
    public string serverKey = "defaultkey";

    public IClient client;
    public ISession session;
    public ISocket socket;
    public IMatch match;

    public string ticket;
    public string matchId;

    [SerializeField] GridSpaceButton[] gridSpaceButtons;

    [SerializeField] public string player;

    [SerializeField] Text playerText;
    [SerializeField] Text chatTextPrefab;
    [SerializeField] TMP_InputField chatInputfield;

    [SerializeField] DoTweenActions ticTacCanvas;
    [SerializeField] GameObject chatPanel;

    [SerializeField] DoTweenActions playerSelectingPanel;
    [SerializeField] DoTweenActions findingPanel;

    [SerializeField] TicTacController controller;
    [SerializeField] Button xButton;
    [SerializeField] Button oButton;


    public async void Start()
    {
        client = new Client(scheme, host, port, serverKey, UnityWebRequestAdapter.Instance);
        session = await client.AuthenticateDeviceAsync(SystemInfo.deviceUniqueIdentifier);
        socket = client.NewSocket();
        await socket.ConnectAsync(session, true);
        Debug.Log(session);
        Debug.Log(socket);
        var mainThread = UnityMainThreadDispatcher.Instance();
        socket.ReceivedMatchmakerMatched += m => mainThread.Enqueue(() => OnReceivedMatchmakerMatched(m));
        socket.ReceivedMatchState += m => mainThread.Enqueue(() => OnReceivedMatchState(m));
    }

    public async void FindMatch()
    {
        Debug.Log("Finding Match");
        var matchMakingTicket = await socket.AddMatchmakerAsync("*", 2, 2);
        ticket = matchMakingTicket.Ticket;
        findingPanel.gameObject.SetActive(true);
        findingPanel.DoAnimation();
    }

    public async void OnReceivedMatchmakerMatched(IMatchmakerMatched matchmakerMatched)
    {
        match = await socket.JoinMatchAsync(matchmakerMatched);
        matchId = match.Id;
        playerSelectingPanel.gameObject.SetActive(true);
        playerSelectingPanel.DoAnimation();
        findingPanel.gameObject.SetActive(false);
        Debug.Log("Our Session ID" + match.Self.SessionId);
        foreach(var user in match.Presences)
        {
            Debug.Log("Connected USER session ID" + user.SessionId);
        }
    }

    public async void LeaveMatch()
    {
        Ping(9);
        controller.NakamaRestartGame();
        await socket.LeaveMatchAsync(matchId);
        playerSelectingPanel.gameObject.SetActive(false);
        ticTacCanvas.gameObject.SetActive(false);
        xButton.interactable = true;
        oButton.interactable = true;
        player = "";
        playerText.text = player;
    }


    public async void Ping(int buttonNumber)
    {
        switch (buttonNumber)
        {
            case 0:
                await socket.SendMatchStateAsync(matchId, 0, "", null);
                break;
            case 1:
                await socket.SendMatchStateAsync(matchId, 1, "", null);
                break;
            case 2:
                await socket.SendMatchStateAsync(matchId, 2, "", null);
                break;
            case 3:
                await socket.SendMatchStateAsync(matchId, 3, "", null);
                break;
            case 4:
                await socket.SendMatchStateAsync(matchId, 4, "", null);
                break;
            case 5:
                await socket.SendMatchStateAsync(matchId, 5, "", null);
                break;
            case 6:
                await socket.SendMatchStateAsync(matchId, 6, "", null);
                break;
            case 7:
                await socket.SendMatchStateAsync(matchId, 7, "", null);
                break;
            case 8:
                await socket.SendMatchStateAsync(matchId, 8, "", null);
                break;
            case 9:
                await socket.SendMatchStateAsync(matchId, 9, "", null);
                break;
            case 10:
                await socket.SendMatchStateAsync(matchId, 10, "", null);
                break;
            case 12:
                await socket.SendMatchStateAsync(matchId, 12, "", null);
                break;
            case 13:
                await socket.SendMatchStateAsync(matchId, 13, "", null);
                break;
        }
    }

    public void OnReceivedMatchState(IMatchState matchState)
    {
        if(0 <= matchState.OpCode && matchState.OpCode <= 8)
        {
            gridSpaceButtons[matchState.OpCode].NakamaSetSpace();
        }
        else if (matchState.OpCode == 9)
        {
            LeaveMatch();
        }
        else if(matchState.OpCode == 10)
        {
            controller.NakamaRestartGame();
        }
        else if(matchState.OpCode == 11)
        {
            var enc = System.Text.Encoding.UTF8;
            var content = enc.GetString(matchState.State);
            Text chatTextInstance = Instantiate(chatTextPrefab, chatPanel.transform);
            chatTextInstance.text = content;
        }
        else if(matchState.OpCode == 12)
        {
            xButton.interactable = false;
        }
        else if (matchState.OpCode == 13)
        {
            oButton.interactable = false;
        }
    }

    public void SetPlayer(string newPlayer)
    {
        player = newPlayer;
        if (newPlayer == "X")
        {
            Ping(12);
        }
        else if(newPlayer == "O")
        {
            Ping(13);
        }
        playerText.text = player;
        playerSelectingPanel.gameObject.SetActive(false);
        ticTacCanvas.gameObject.SetActive(true);
        ticTacCanvas.DoAnimation();
        controller.SetStartingSide("X");
        controller.SetGameControllerReferenceOnButtons();
    }

    public void OnEndEditChat()
    {
        Text chatTextInstance = Instantiate(chatTextPrefab, chatPanel.transform);
        chatTextInstance.text = chatInputfield.text;
        ChatPing(chatInputfield.text);
        chatInputfield.text = "";
    }
    public async void ChatPing(string chatText)
    {
        await socket.SendMatchStateAsync(matchId, 11, chatText, null);
    }
}
