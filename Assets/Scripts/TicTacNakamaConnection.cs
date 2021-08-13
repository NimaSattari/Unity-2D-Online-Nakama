using Nakama;
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
    [SerializeField] GameObject ticTacCanvas;
    [SerializeField] DoTweenActions playerSelectingPanel;
    [SerializeField] DoTweenActions findingPanel;
    [SerializeField] TicTacController controller;


    async void Start()
    {
        client = new Client(scheme, host, port, serverKey, UnityWebRequestAdapter.Instance);
        session = await client.AuthenticateDeviceAsync(SystemInfo.deviceUniqueIdentifier);
        socket = client.NewSocket();
        await socket.ConnectAsync(session, true);
        socket.ReceivedMatchmakerMatched += OnReceivedMatchmakerMatched;
        socket.ReceivedMatchState += OnReceivedMatchState;
        Debug.Log(session);
        Debug.Log(socket);
    }

    public async void FindMatch()
    {
        Debug.Log("Finding Match");
        var matchMakingTicket = await socket.AddMatchmakerAsync("*", 2, 2);
        ticket = matchMakingTicket.Ticket;
        findingPanel.gameObject.SetActive(true);
        findingPanel.DoAnimation();
    }

    private async void OnReceivedMatchmakerMatched(IMatchmakerMatched matchmakerMatched)
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
        await socket.LeaveMatchAsync(matchId);
        playerSelectingPanel.gameObject.SetActive(false);
        ticTacCanvas.gameObject.SetActive(false);
        player = null;
        playerText.text = player;
        Ping(9);
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
        }
    }

    private void OnReceivedMatchState(IMatchState matchState)
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
    }

    public void SetPlayer(string newPlayer)
    {
        player = newPlayer;
        playerText.text = player;
        playerSelectingPanel.gameObject.SetActive(false);
        ticTacCanvas.gameObject.SetActive(true);
        controller.SetStartingSide("X");
        controller.SetGameControllerReferenceOnButtons();
    }
}
