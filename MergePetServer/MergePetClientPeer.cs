using Common;
using MergePetServer.DB.Manager;
using MergePetServer.Handlers;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;

namespace MergePetServer
{
    /// <summary>
    /// 与客户端交互
    /// </summary>
    public class MergePetClientPeer : ClientPeer
    {
        private OperationResponse response;
        private PlayerHandler playerHandler;

        public MergePetClientPeer(InitRequest initRequest) :base(initRequest)
        {
            playerHandler = new PlayerHandler();
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            playerHandler.Offline(this);
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            response = playerHandler.OnHandlerMessage(operationRequest,this);
            SendOperationResponse(response, sendParameters);
        }


    }
}
