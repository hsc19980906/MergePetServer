using System;
using System.Collections.Generic;
using Common;
using Common.Model;
using Common.Tool;
using MergePetServer.DB.Manager;
using MergePetServer.Tools;
using Photon.SocketServer;

namespace MergePetServer.Handlers
{
    public class PlayerHandler : HandlerBase
    {
        private PlayerManager playerManager = Managers.Player;
        OperationResponse response = new OperationResponse();

        public override OperationResponse OnHandlerMessage(OperationRequest request, MergePetClientPeer client)
        {
            response.OperationCode = request.OperationCode;
            Dictionary<byte, object> data = request.Parameters;

            switch (request.OperationCode)
            {
                case (byte)OpCode.Register:
                    Register(data);
                    break;
                case (byte)OpCode.Login:
                    MergePetApplication.log.Info("登陆");
                    Login(data,client);
                    break;
                case (byte)OpCode.Create:
                    Create(data, client);
                    break;
                case (byte)OpCode.Refresh:
                    Refresh(client);
                    break;
                case (byte)OpCode.Rank:
                    RankCE(client);
                    break;
                case (byte)OpCode.Update:
                    UpdateRank(data,client);
                    break;
                default:
                    break;
            }
            return response;
        }

        //下线
        public void Offline(MergePetClientPeer client)
        {
            if (playerManager.IsOnline(client))
                playerManager.Offline(client);
        }

        private void UpdateRank(Dictionary<byte, object> data, MergePetClientPeer client)
        {
            Player player = playerManager.GetPlayer(client);
            PlayerRank playerRank = EncodeTool.DecodeObject((byte[])data[(byte)ParameterCode.player]) as PlayerRank;
            player.ChangeRank(playerRank);
            playerManager.UpdatePlayer(player);
        }

        private void RankCE(MergePetClientPeer client)
        {
            response.Parameters = new Dictionary<byte, object>();
            List<PlayerRank> playerRanks = new List<PlayerRank>();
            IList<Player> players = playerManager.GetPlayerRankMsgs();
            for (int i = 0; i < players.Count; i++)
            {
                if(players[i].IsCreate)
                {
                    PlayerRank playerRank = new PlayerRank();
                    if (i == 0)
                        playerRank.ChangeTitle(PlayerRank.Title.first);
                    if (i == 1)
                        playerRank.ChangeTitle(PlayerRank.Title.second);
                    if (i == 2)
                        playerRank.ChangeTitle(PlayerRank.Title.third);
                    if (i > 2 && i < 10)
                        playerRank.ChangeTitle(PlayerRank.Title.beforeten);
                    playerRank.Set(players[i].Max_CE, players[i].imgPet, players[i].Profile, players[i].petName, players[i].Level, players[i].PlayerName);
                    if(playerManager.IsOnline(client)&&playerRank.playerName == playerManager.GetPlayer(client).PlayerName)
                        response.Parameters.Add((byte)ParameterCode.pet, EncodeTool.EncodeObject(playerRank));
                    playerRanks.Add(playerRank);
                }
            }
            response.Parameters.Add((byte)ParameterCode.player, EncodeTool.EncodeObject(playerRanks));
        }

        private void Refresh(MergePetClientPeer client)
        {
            //向客户端发送玩家及其宠物信息
            Player player = playerManager.GetPlayer(client);
            response.Parameters = new Dictionary<byte, object>();
            response.Parameters.Add((byte)ParameterCode.player,EncodeTool.EncodeObject(player));
           // response.Parameters.Add((byte)ParameterCode.pet, EncodeTool.EncodeObject(petManager.GetPets(player.id_player)));
        }

        private void Login(Dictionary<byte, object> data, MergePetClientPeer client)
        {
            response.Parameters = new Dictionary<byte, object>();
            if (!data.ContainsKey((byte)ParameterCode.pet))
            {
                MergePetApplication.log.Info("账号登陆");
                Player player = EncodeTool.DecodeObject((byte[])data[(byte)ParameterCode.player]) as Player;
                //MergePetApplication.log.Info(player.Password);
                if (!playerManager.IsExist(player.Account))
                {
                    response.DebugMessage = "该账号未注册！";
                    response.ReturnCode = (short)ReturnCode.Fail;
                }
                else if (playerManager.IsOnline(player.Account))
                {
                    response.DebugMessage = "该账号已登录！";
                    response.ReturnCode = (short)ReturnCode.Fail;
                }
                else if (!playerManager.VerifyAccount(player.Account, player.Password))
                {
                    response.DebugMessage = "账号或密码不正确";
                    response.ReturnCode = (short)ReturnCode.Fail;
                }
                else
                {
                    playerManager.Online(client, player.Account);//登录成功后上线
                                                                 //告诉客户端是否跳转到创建角色页面
                    bool isCreate;
                    if (data.ContainsKey((byte)ParameterCode.Created))
                        isCreate = false;
                    else
                        isCreate = playerManager.GetPlayer(player.Account).IsCreate;
                    if (isCreate)
                    {
                        // MergePetApplication.log.Info(isCreate);
                        response.Parameters.Add((byte)ParameterCode.Created, null);
                    }
                    response.ReturnCode = (short)ReturnCode.Success;
                }
            }
            else
            { 
                Player player = playerManager.GetPlayer((int)data[(byte)ParameterCode.player]);
                if (player == null)
                {
                    MergePetApplication.log.Info("id不存在:" + (int)data[(byte)ParameterCode.player]);
                    response.Parameters.Add((byte)ParameterCode.player, null);
                    response.DebugMessage = "该账号未注册！";
                    response.ReturnCode = (short)ReturnCode.Fail;
                }
                else
                {
                    if (!playerManager.IsOnline(player.Account))
                    {
                        MergePetApplication.log.Info("id登陆" + player.id_player);
                        playerManager.Online(client, player.Account);//登录成功后上线
                        response.Parameters.Add((byte)ParameterCode.player, null);
                        response.ReturnCode = (short)ReturnCode.Success;
                    }
                }
            }
        }

        private void Register(Dictionary<byte, object> data)
        {
            Player player=EncodeTool.DecodeObject((byte[])data[(byte)ParameterCode.player]) as Player;
            //账号已注册
            if(playerManager.GetPlayer(player.Account)!=null)
            {
                response.DebugMessage = "该账号已注册！";
                response.ReturnCode = (short)ReturnCode.Fail;
            }
            else
            {
                player.Password = MD5Tool.GetMD5(player.Password);
                player.IsCreate = false;
                player.RegisterTime = DateTime.Now;
                playerManager.AddPlayer(player);
                response.ReturnCode = (short)ReturnCode.Success;
            }
            
        }

        private void Create(Dictionary<byte, object> data, MergePetClientPeer client)
        {
            byte[] value = (byte[])data[(byte)ParameterCode.player];
            Player player = EncodeTool.DecodeObject(value) as Player;
            //PetAttris petAttris = petManager.GetModelByPetkind(data[(byte)ParameterCode.petkind].ToString());
            //随机生成成长值 1~1.5 保留两位小数   

            //double cc = new System.Random(DateTime.Now.Millisecond).Next(100, 150) / 100.0;
            //player.AddPet(pet);
            //string account = playerManager.GetAccountByClient(client);
            //TODO 再创建
            Player playerDB = playerManager.GetPlayer(client);
            playerDB.Change(player.Sex, player.Profile, player.PlayerName, true);

            string petname = (string)data[(byte)ParameterCode.pet];
            if (petname == "木波姆")
                petname = "绿波姆";
            //Pet pet = new Pet() { CC = cc, Level = 1, Exp = 0, PetName = data[(byte)ParameterCode.pet].ToString(),id_player=playerDB.id_player};
            //playerDB.AddPet(pet);
            playerManager.UpdatePlayer(playerDB);
            //petManager.AddPet(pet);
            //创建完成 需要刷新
            player = playerManager.GetPlayer(client);
            response.Parameters = new Dictionary<byte, object>();
            response.Parameters.Add((byte)ParameterCode.player, EncodeTool.EncodeObject(player));
            response.Parameters.Add((byte)ParameterCode.pet, petname);
            ////此时 玩家第一只宠物创建完成 返回该宠物id
            //response.Parameters = new Dictionary<byte, object>();
            //response.Parameters.Add((byte)ParameterCode.pet, petManager.GetId(player.id_player));
        }
    }
}
