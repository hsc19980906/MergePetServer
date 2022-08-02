using Common.Model;
using MergePetServer.Tools;
using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;

namespace MergePetServer.DB.Manager
{
    public class PlayerManager
    {
        //存放在线的玩家
        private Dictionary<string, MergePetClientPeer> accClientDict = new Dictionary<string, MergePetClientPeer>();
        private Dictionary<MergePetClientPeer, string> clientAccDict = new Dictionary<MergePetClientPeer, string>();

        /// <summary>
        /// 是否在线
        /// </summary>
        public bool IsOnline(string account)
        {
            return accClientDict.ContainsKey(account);
        }

        /// <summary>
        /// 是否在线
        /// </summary>
        public bool IsOnline(MergePetClientPeer client)
        {
            return clientAccDict.ContainsKey(client);
        }

        /// <summary>
        /// 用户上线
        /// </summary>
        /// <param name="client"></param>
        /// <param name="account"></param>
        public void Online(MergePetClientPeer client, string account)
        {
            accClientDict.Add(account, client);
            clientAccDict.Add(client, account);
        }

        /// <summary>
        /// 下线
        /// </summary>
        /// <param name="client"></param>
        public void Offline(MergePetClientPeer client)
        {
            string account = clientAccDict[client];
            clientAccDict.Remove(client);
            accClientDict.Remove(account);
        }

        /// <summary>
        /// 下线
        /// </summary>
        /// <param name="account"></param>
        public void Offline(string account)
        {
            MergePetClientPeer client = accClientDict[account];
            accClientDict.Remove(account);
            clientAccDict.Remove(client);
        }

        /// <summary>
        /// 根据账户名获取Player
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public Player GetPlayer(string account)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(Player));
                criteria.Add(Restrictions.Eq("Account", account));
                return criteria.UniqueResult<Player>();
            }
        }

        public Player GetPlayer(MergePetClientPeer client)
        {
            if (GetPlayer(clientAccDict[client]) != null)
                return GetPlayer(clientAccDict[client]);
            else
                return null;
        }

        public Player GetPlayer(int id)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(Player));
                criteria.Add(Restrictions.Eq("id_player", id));
                return criteria.UniqueResult<Player>();
            }
        }

        public string GetAccountByClient(MergePetClientPeer client)
        {
            return clientAccDict[client];
        }

        public bool IsExist(string account)
        {
            if (GetPlayer(account) == null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 判断账号是否正确
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool VerifyAccount(string account, string password)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(Player));
                criteria.Add(Restrictions.Eq("Account", account));
                criteria.Add(Restrictions.Eq("Password", MD5Tool.GetMD5(password)));
                Player player = criteria.UniqueResult<Player>();
                if (player == null) return false;
                else return true;
            }
        }

        /// <summary>
        /// 添加：创建角色
        /// </summary>
        /// <param name="user"></param>
        public void AddPlayer(Player player)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Save(player);
                    transaction.Commit();
                }
            }
        }

        public void Delete(Player player)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Delete(player);
                    transaction.Commit();
                }
            }
        }

        /// <summary>
        /// 更新玩家信息
        /// </summary>
        /// <param name="player"></param>
        public void UpdatePlayer(Player player)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Update(player);
                    transaction.Commit();
                }
            }
        }

        public IList<Player> GetPlayerRankMsgs()
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    IList<Player> players = session.QueryOver<Player>().OrderBy(p => p.Max_CE).Desc.List();
                    transaction.Commit();
                    return players;
                }
            }
        }

    }
}
