using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AP.FamilyTree.Db.Models;
using AP.FamilyTree.Web.Data;
using AP.FamilyTree.Web.Data.Services.TreesServices;
using AP.FamilyTree.Web.Globals;
using AP.FamilyTree.Web.PageModels;
using AP.FamilyTree.Web.PageModels.Node;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AP.FamilyTree.Web.Pages.Tree
{
    public class TreeViewModel : BaseViewModel
    {
        [Parameter] public int TreeId { get; set; }
        [Inject] protected TreeBuidingService Service { get; set; }
        protected List<NodeItemViewModel> Models { get; set; }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Models = await Service.GetByTreeId(TreeId);
                var idList = Models.Select(x => x.HumanId).ToArray();
                var namesList = Models.Select(x => x.Human.Name).ToArray();
                var datesList = Models.Select(x => GetText(x.Human.Item)).ToArray();
                var parentsList = Models.Select(x => new List<int>() { x.MotherId.GetValueOrDefault(), x.FatherId.GetValueOrDefault() }).Select(y => y.ToArray()).ToArray();

                await Js.InvokeVoidAsync("Test.functionOne", idList, namesList, datesList, parentsList, Models.Count);
            }
        }

        private string GetText(HumanModel model)
        {
            string str = model.MiddleName + " " + model.Surname;

            str += "\n";

            str += GlobalFunction.ConvertToLiveYear(model.BirthDate, model.DeathDate);

            return str;
        }
    }
}
