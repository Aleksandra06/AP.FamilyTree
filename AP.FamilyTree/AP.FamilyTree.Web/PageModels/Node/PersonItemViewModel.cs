using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AP.FamilyTree.Db.Models;

namespace AP.FamilyTree.Web.PageModels.Node
{
    public class PersonItemViewModel : ICloneable
    {
        private HumanModel _item;
        public HumanModel Item => _item;
        public PersonItemViewModel()
        {
            _item = new HumanModel();
        }
        public PersonItemViewModel(HumanModel model)
        {
            _item = model;
        }
        public int Id
        {
            get => _item.Id;
            set => _item.Id = value;
        }
        [MaxLength(20)]
        [Required]
        public string Name
        {
            get => _item.Name;
            set => _item.Name = value;
        }
        [MaxLength(20)]
        [Required]
        public string Surname
        {
            get => _item.Surname;
            set => _item.Surname = value;
        }
        [MaxLength(20)]
        public string MiddleName
        {
            get => _item.MiddleName;
            set => _item.MiddleName = value;
        }
        public DateTime? BirthDate
        {
            get => _item.BirthDate;
            set => _item.BirthDate = value;
        }
        public DateTime? DeathDate
        {
            get => _item.DeathDate;
            set => _item.DeathDate = value;
        }
        [Required]
        public int Gender
        {
            get => _item.Gender;
            set => _item.Gender = value;
        }
        public string Biography
        {
            get => _item.Biography;
            set => _item.Biography = value;
        }
        [MaxLength(50)]
        public string PlaceOfBirth
        {
            get => _item.PlaceOfBirth;
            set => _item.PlaceOfBirth = value;
        }
        [MaxLength(50)]
        public string Nationality
        {
            get => _item.Nationality;
            set => _item.Nationality = value;
        }
        public DateTime? WeddingDate
        {
            get => _item.WeddingDate;
            set => _item.WeddingDate = value;
        }
        [MaxLength(50)]
        public string PlaceOfDeath
        {
            get => _item.PlaceOfDeath;
            set => _item.PlaceOfDeath = value;
        }
        [MaxLength(50)]
        public string BurialPlace
        {
            get => _item.BurialPlace;
            set => _item.BurialPlace = value;
        }
        [MaxLength(100)]
        public string Works
        {
            get => _item.Works;
            set => _item.Works = value;
        }
        public byte[] Photo
        {
            get => _item.Photo;
            set => _item.Photo = value;
        }
        public object Clone()
        {
            PersonItemViewModel tempObject = (PersonItemViewModel)this.MemberwiseClone();
            tempObject._item = (HumanModel)_item.Clone();
            return tempObject;
        }
    }
}
