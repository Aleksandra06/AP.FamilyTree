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

        private string b =
            "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADIAAAA8CAIAAACrV36WAAAAAXNSR0IArs4c6QAAAARn" +
            "QU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAGnSURBVGhD7dnBbQJBDAVQk1o2QjlQwKYGzpSwKQfq4IxIC" +
            "RTB9jLZHCJFwWv7/7EiDt6zmX2yPYMHNq01eb7n5flI36JiIXWpbFW2kAwgsdVblS0kA0hs9db/ZWs+vW/Wno9PxPE3dh" + 
            "ls6Od+HI1XT1d64Sb8R5utEulwdbA8VY+LZ/kqkfF456pBHxDz5Xxze/p2vsxukBbAshTVOE0PO4B2cUlWKrgUTKsrV0e" +
            "ut3RVU/cm5aKKqPXVbjuIDPtDUh2JImq1+jmjkupIFNFStXadHncWXkecpb3393me4oJZnionXyjLV6W4QFZEleHCWNG+" +
            "0eKggQJiRVV6vhAXwoqrul0AC1H1uuIsTLUyukYH1jBL7WJ8lgq6oqwkVXSQDrLSVEFXjJWoirlCrFRVyBVhJasirgCr6" +
            "5tEv7a5A5jL0tcN7vNl9OVcHqtXRbocVr+Kc9k3H/3qPL69Ise7dh0SsS+2JmtFddgvdy/gGbY7Jdp2GRcyrlu1BfUjxt" +
            "iPRm/lqVbGHOMHnU39zQm0I/UbBLA+GVosJHGVrcoWkgEktnoLydYXkF/LiXG21MwAAAAASUVORK5CYII=";

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Models = await Service.GetByTreeId(TreeId);
                var idList = Models.Select(x => x.HumanId).ToArray();
                var namesList = Models.Select(x => x.Human.Name).ToArray();
                var datesList = Models.Select(x => GetText(x.Human.Item)).ToArray();
                var parentsList = Models.Select(x => new List<int>() { x.MotherId.GetValueOrDefault(), x.FatherId.GetValueOrDefault() }).Select(y => y.ToArray()).ToArray();
                var photo = Models.Select(x => x.Human.PhotoSvg?.Length > 0 ? x.Human.PhotoSvg : b).ToArray();

                await Js.InvokeVoidAsync("Test.functionOne", idList, namesList, datesList, parentsList, Models.Count, photo);
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
