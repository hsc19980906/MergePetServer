using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    [Serializable]
    public class Player
    {
        public virtual int id_player { get; set; }
        public virtual string Account { get; set; }
        public virtual string Password { get; set; }
        public virtual string Sex { get; set; }
        public virtual string Profile { get; set; }
        public virtual DateTime RegisterTime { get; set; }
        public virtual bool IsCreate { get; set; }
        public virtual string PlayerName { get; set; }
        public virtual double Max_CE { get; set; }
        public virtual string imgPet { get; set; }
        public virtual string petName { get; set; }
        public virtual int Level { get; set; }

        //public virtual IList<Pet> Pets { get; set; }

        //public Player()
        //{
        //    Pets = new List<Pet>();
        //}

        //public virtual void AddPet(Pet pet)
        //{
        //    pet.Player = this;
        //    Pets.Add(pet);
        //}

        public virtual void Change(string sex,string profile,string name,bool isCreate)
        {
            this.Sex = sex;
            this.Profile = profile;
            this.PlayerName = name;
            this.IsCreate = isCreate;
        }

        public virtual void ChangeRank(PlayerRank rank)
        {
            this.Max_CE = rank.Max_CE;
            this.imgPet = rank.imgPet;
            this.Level = rank.Level;
            this.petName = rank.petName;
        }
    }
}
