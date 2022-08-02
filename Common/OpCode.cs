using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 区分不同模块
    /// </summary>
    public enum OpCode:byte
    {
        Login,Register,Create,Refresh,Rank,Update
    }
}
