using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    [Serializable]
    public class PlayerRank
    {
        public double Max_CE;
        public string imgPet;
        public string imgPlayer;
        public string petName;
        public int Level;
        public string playerName;
        public Title title = Title.notitle;

        public enum Title
        {
            first,
            second,
            third,
            beforeten,
            notitle
        }

        public void Set(double ce,string imgPet,string imgPlayer,string name,int level,string playername)
        {
            Max_CE = ce;
            this.imgPet = imgPet;
            this.imgPlayer = imgPlayer;
            petName = name;
            Level = level;
            playerName = playername;
        }

        public void ChangeTitle(Title title)
        {
            this.title = title;
        }


        //根据Title枚举类型返回对应的string
        public string WhichTitle()
        {
            switch (title)
            {
                case PlayerRank.Title.first:
                    return "至尊契约师";
                case PlayerRank.Title.second:
                    return "契约大师";
                case PlayerRank.Title.third:
                    return "契约小将";
                case PlayerRank.Title.beforeten:
                    return "契约者";
                case PlayerRank.Title.notitle:
                    return "契约新手";
                default:
                    return "NULL";
            }
        }
    }
}
