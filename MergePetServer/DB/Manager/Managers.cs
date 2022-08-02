using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergePetServer.DB.Manager
{
    /// <summary>
    /// 单例化
    /// </summary>
    public static class Managers
    {
        public static PlayerManager Player { get; set; }

        //静态对象被调用时 调用该方法初始化
        static Managers()
        {
            Player = new PlayerManager();
        }
    }
}
