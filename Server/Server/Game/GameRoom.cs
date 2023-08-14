using Google.Protobuf;
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

        Dictionary<int, Player> _players = new Dictionary<int, Player>();

        Map _map = new Map();

        public void Init(int mapId)
        {
            _map.LoadMap(mapId);
        }

        public void EnterGame(Player newPlayer)  // 들어올때는 플레이어로받고 나갈때는 아이디만 받는이유가 뭘까..
        {
            if (newPlayer == null)
                return;

            lock (_lock)
            {
                _players.Add(newPlayer.Info.PlayerId, newPlayer);
                newPlayer.Room = this;

                // 본인한테 정보 전송

                S_EnterGame enterPacket = new S_EnterGame();
                enterPacket.Player = newPlayer.Info;
                newPlayer.Session.Send(enterPacket);

                S_Spawn spawnPacket1 = new S_Spawn();
                foreach(Player p in _players.Values)
                {
                    if (newPlayer != p)
                        spawnPacket1.Players.Add(p.Info);
                }
                newPlayer.Session.Send(spawnPacket1);

                // 타인한테 정보 전송

                S_Spawn spawnPacket2 = new S_Spawn();
                spawnPacket2.Players.Add(newPlayer.Info);
                foreach(Player p in _players.Values)
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
                Player player = null;
                if (_players.Remove(playerId, out player) == false)
                    return;

                player.Room = null;

                // 본인한테 정보 전송
                S_LeaveGame leavePakcet = new S_LeaveGame();
                player.Session.Send(leavePakcet);

                //타인한테 정보 전송
                S_Despawn despawnPacket = new S_Despawn();
                despawnPacket.PlayerIds.Add(player.Info.PlayerId);
                foreach(Player p in _players.Values)
                {
                    if (player != p)
                        p.Session.Send(despawnPacket);
                }

            }
        }

        public void HandleMove(Player player, C_Move movePacket)
        {
            if (player == null) return;

            lock (_lock)
            {

                // TODO : 검증

                // 서버에서 좌표이동
                PositionInfo movePosInfo = movePacket.PosInfo;
                PlayerInfo info = player.Info;

                // 갈 수 있는지 체크
                if(movePosInfo.PosX != info.PosInfo.PosX || movePosInfo.PosY != info.PosInfo.PosY)
                {
                    if (_map.CanGo(new Vector2Int(movePosInfo.PosX, movePosInfo.PosY)) == false)
                        return;
                }

                info.PosInfo.State = movePosInfo.State;
                info.PosInfo.MoveDir = movePosInfo.MoveDir;
                _map.ApplyMove(player , new Vector2Int(movePosInfo.PosX, movePosInfo.PosY));


                // 다른 플레이어에게 알림
                S_Move resMovePacket = new S_Move();
                resMovePacket.PlayerId = player.Info.PlayerId;
                resMovePacket.PosInfo = movePacket.PosInfo;

                Broadcast(resMovePacket);
            }
        }

        public void HandleSkill(Player player, C_Skill skillPacket)
        {
            if (player == null) return;

            lock (_lock)
            {
                PlayerInfo info = player.Info;
                if (info.PosInfo.State != CreatureState.Idle)
                    return;

                info.PosInfo.State = CreatureState.Skill;

                S_Skill skill = new S_Skill() { Info = new SkillInfo() };
                skill.PlayerId = info.PlayerId;
                skill.Info.SkillId = 1;
                Broadcast(skill);

                // 데미지 판정
            }
        }

        public void Broadcast(IMessage packet)
        {
            lock (_lock)
            {
                foreach(Player p in _players.Values)
                {
                    p.Session.Send(packet);
                }
            }
        }
    }
}
