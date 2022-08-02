using Common;
using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using log4net;
using log4net.Config;
using MergePetServer.Handlers;
using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MergePetServer
{
    /// <summary>
    /// 服务器端启动类
    /// </summary>
    public class MergePetApplication : ApplicationBase
    {
        //用于日志输出打印
        public static readonly ILogger log = (ILogger)ExitGames.Logging.LogManager.GetCurrentClassLogger();

        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            log.Info("一位玩家连接过来了~");
            return new MergePetClientPeer(initRequest);
        }

        protected override void Setup()
        {
            InitLogging();
            log.Info("服务器启动完毕。");
        }

        protected override void TearDown()
        {
            log.Info("服务器已关闭。");
        }

        protected void InitLogging()
        {
            //使用logger工厂 直接使用log4net实现的实例
            ExitGames.Logging.LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
            //放在application的根目录下 就是deploy目录下的log文件夹内
            GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(this.ApplicationRootPath, "log");
            GlobalContext.Properties["LogFileName"] = "MP" + this.ApplicationName;
            XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.Combine(this.BinaryPath, "log4net.config")));
        }
    }
}
