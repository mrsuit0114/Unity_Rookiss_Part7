using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game
{
    public class GameRoom
    {
        object _lock = new object();
        public int RoomId { get; set; }

        List<Player> _players = new List<Player>();

        public void EnterGame(Player newPlayer)  // 들어올때는 플레이어로받고 나갈때는 아이디만 받는이유가 뭘까..
        {
            if (newPlayer == null)
                return;

            lock (_lock)
            {
                _players.Add(newPlayer);
                newPlayer.Room = this;

                // 본인한테 정보 전송

                S_EnterGame enterPacket = new S_EnterGame();
                enterPacket.Player = newPlayer.Info;
                newPlayer.Session.Send(enterPacket);

                S_Spawn spawnPacket1 = new S_Spawn();
                foreach(Player p in _players)
                {
                    if (newPlayer != p)
                        spawnPacket1.Players.Add(p.Info);
                }
                newPlayer.Session.Send(spawnPacket1);

                // 타인한테 정보 전송

                S_Spawn spawnPacket2 = new S_Spawn();
                spawnPacket2.Players.Add(newPlayer.Info);
                foreach(Player p in _players)
                {
                    if(newPlayer != p)
                        p.Session.Send(spawnPacket2);
                }

            }
        }

        public void LeaveGame(int playerId)
        {
            lock (_lock)
            {
                Player player =_players.Find(p=>p.Info.PlayerId == playerId);
                if(player == null) return;

                _players.Remove(player);
                player.Room = null;

                // 본인한테 정보 전송
                S_LeaveGame leavePakcet = new S_LeaveGame();
                player.Session.Send(leavePakcet);

                //타인한테 정보 전송
                S_Despawn despawnPacket = new S_Despawn();
                despawnPacket.PlayerIds.Add(player.Info.PlayerId);
                foreach(Player p in _players)
                {
                    if (player != p)
                        p.Session.Send(despawnPacket);
                }

            }
        }

    }
}
