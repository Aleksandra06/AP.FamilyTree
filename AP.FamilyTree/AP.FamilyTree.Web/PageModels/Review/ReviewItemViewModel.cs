using System;
using System.ComponentModel.DataAnnotations;
using AP.FamilyTree.Web.PageModels.Interfaces;

namespace AP.FamilyTree.Web.PageModels.Review
{
    public class ReviewItemViewModel : IIsRefreshed
    {
        private Db.Models.Review _item;
        public Db.Models.Review Item => _item;
        public ReviewItemViewModel()
        {
            _item = new Db.Models.Review();
        }
        public ReviewItemViewModel(Db.Models.Review model)
        {
            _item = model;
        }
        public int ReviewId
        {
            get => _item.ReviewId;
            set => _item.ReviewId = value;
        }
        [Required(ErrorMessage = "Это поле должно быть заполнено")]
        public string ReviewText
        {
            get => _item.ReviewText;
            set => _item.ReviewText = value;
        }
        public string UserId
        {
            get => _item.UserId;
            set => _item.UserId = value;
        }
        public bool Accepted
        {
            get => _item.Accepted;
            set => _item.Accepted = value;
        }
        public DateTime InsertDate
        {
            get => _item.InsertDate;
            set => _item.InsertDate = value;
        }

        public string UserName { get; set; }
        public bool IsRefreshed { get; set; } = false;
    }
}
