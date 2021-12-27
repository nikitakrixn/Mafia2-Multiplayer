using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mafia2_Server
{
    class Server
    {
        private enum PacketID : byte
        {
            None,
            ConnectionAccepted,
            PlayerDisconnected,
            PlayerJoined,
            PlayerUpdatePacket
        }

        static NetManager server;
        static void Main(string[] args)
        {
            Console.Title = "Mafia2 Multiplayer 0.1 Server";
            Console.WriteLine("Mafia2 Multiplayer 0.1 Server - Alpha Build");
            Console.WriteLine("============================================");
            Console.WriteLine("[NETLOG] Starting server at port 9050...");
            EventBasedNetListener listener = new EventBasedNetListener();
            server = new NetManager(listener);
            server.Start(9050);
            Console.WriteLine("[NETLOG] Server started!");

            // Events
            listener.ConnectionRequestEvent += Listener_ConnectionRequestEvent;
            listener.PeerConnectedEvent += Listener_PeerConnectedEvent;
            listener.PeerDisconnectedEvent += Listener_PeerDisconnectedEvent;
            listener.NetworkReceiveEvent += Listener_NetworkReceiveEvent;
            while (true)
            {
                server.PollEvents();
            }
            server.Stop();
        }

        private static void Listener_NetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
        {
            PacketID id = (PacketID)reader.GetByte();
            switch (id)
            {
                case PacketID.PlayerUpdatePacket:

                    break;
            }
        }

        private static void Listener_ConnectionRequestEvent(ConnectionRequest request)
        {
            if (server.ConnectedPeersCount < 10) request.AcceptIfKey("M2MP"); else request.Reject();
        }

        private static void Listener_PeerConnectedEvent(NetPeer peer)
        {
            Console.WriteLine($"[NETLOG] Player connected : {peer.EndPoint}");
            NetDataWriter ToConnector = new NetDataWriter(), ToOthers = new NetDataWriter();

            // This message is sent to who connected only to the server
            ToConnector.Put((byte)PacketID.ConnectionAccepted);
            peer.Send(ToConnector, DeliveryMethod.ReliableOrdered);

            // This message is sent to others to notify about the other guy connecting
            ToOthers.Put((byte)PacketID.PlayerJoined);
            server.SendToAll(ToOthers, DeliveryMethod.ReliableOrdered, peer);
        }


        private static void Listener_PeerDisconnectedEvent(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            Console.WriteLine($"[NETLOG] Player Disconnected : {peer.EndPoint} ({disconnectInfo.Reason})");
        }
    }
}
