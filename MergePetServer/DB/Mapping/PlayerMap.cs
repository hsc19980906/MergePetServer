using Common.Model;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergePetServer.DB.Mapping
{
    public class PlayerMap:ClassMap<Player>
    {
        public PlayerMap()
        {
            Table("player");
            Id(x => x.id_player).GeneratedBy.Assigned();
            Map(x => x.Profile);
            Map(x => x.Account);
            Map(x => x.IsCreate);
            Map(x => x.Password);
            Map(x => x.PlayerName);
            Map(x => x.RegisterTime);
            Map(x => x.Sex);
            Map(x => x.Max_CE);
            Map(x => x.imgPet);
            Map(x => x.Level);
            Map(x => x.petName);
            //HasMany<Pet>(x => x.Pets).KeyColumn("id_player").Cascade.All();
        }
    }
}
