using Nakama;
using System.Collections.Generic;
using UnityEngine;

public class NakamaConnection : MonoBehaviour
{
    private string scheme = "http";
    private string host = "localhost";
    private int port = 7350;
    private string serverKey = "defaultkey";

    private IClient client;
    private ISession session;
    private ISocket socket;

    private string ticket;
    private string matchId;


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
        var matchMakingTicket = await socket.AddMatchmakerAsync("*", 2, 4);
        ticket = matchMakingTicket.Ticket;
    }

    private async void OnReceivedMatchmakerMatched(IMatchmakerMatched matchmakerMatched)
    {
        var match = await socket.JoinMatchAsync(matchmakerMatched);
        matchId = match.Id;
        Debug.Log("Our Session ID" + match.Self.SessionId);
        foreach(var user in match.Presences)
        {
            Debug.Log("Connected USER session ID" + user.SessionId);
        }
    }

    public async void Ping()
    {
        Debug.Log("Sending Ping");
        await socket.SendMatchStateAsync(matchId, 1, "", null);
    }

    private async void OnReceivedMatchState(IMatchState matchState)
    {
        if(matchState.OpCode == 1)
        {
            Debug.Log("Recived Ping");
            Debug.Log("Sending Pong");
            await socket.SendMatchStateAsync(matchId, 2, "", new[] { matchState.UserPresence });
        }
        if(matchState.OpCode == 2)
        {
            Debug.Log("Recived Pong");
            Debug.Log("Sending Ping");
            await socket.SendMatchStateAsync(matchId, 1, "", new[] { matchState.UserPresence });
        }
    }
}
