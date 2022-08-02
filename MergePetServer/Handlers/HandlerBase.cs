using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergePetServer.Handlers
{
    /// <summary>
    /// 所有handler的父类 用来定义接口
    /// </summary>
    public abstract class HandlerBase
    {
        public abstract OperationResponse OnHandlerMessage(OperationRequest request,MergePetClientPeer client);
    }
}
