using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergePetServer.DB
{
     public class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    _sessionFactory = Fluently.Configure().Database(MySQLConfiguration.
                        Standard.ConnectionString(db => db.Server("localhost").
                        Database("mergepetserver").Username("root").Password("123456")))
                .Mappings(x => x.FluentMappings.AddFromAssemblyOf<NHibernateHelper>()).BuildSessionFactory();
                    //6565:HSCqtt@ 180.76.168.123 dubaibai 6565155@qtt
                    //var configuration = new Configuration();
                    //mysql57.rdsmv7874sjaozo.rds.bj.baidubce.com
                    //configuration.Configure();//解析hibernate.cfg.xml
                    //configuration.AddAssembly("NHibernate_MySQL");

                    ////创建会话
                    //_sessionFactory = configuration.BuildSessionFactory();
                }
                return _sessionFactory;
            }
        }

        public static ISession OpenSession()
        {
            //打开和数据库的会话
            return SessionFactory.OpenSession();
        }

    }
}
