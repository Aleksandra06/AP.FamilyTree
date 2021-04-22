import('basicprimitives/css/primitives.css');
import { FamConfig, FamItemConfig, Enabled, FamDiagram, SelectionPathMode, NeighboursSelectionMode } from 'basicprimitives';

var photos = {
    a: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADIAAAA8CAIAAACrV36WAAAAAXNSR0IArs4c6QAAAARn' +
        'QU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAGnSURBVGhD7dnBbQJBDAVQk1o2QjlQwKYGzpSwKQfq4IxIC' +
        'RTB9jLZHCJFwWv7/7EiDt6zmX2yPYMHNq01eb7n5flI36JiIXWpbFW2kAwgsdVblS0kA0hs9db/ZWs+vW/Wno9PxPE3dh' +
        'ls6Od+HI1XT1d64Sb8R5utEulwdbA8VY+LZ/kqkfF456pBHxDz5Xxze/p2vsxukBbAshTVOE0PO4B2cUlWKrgUTKsrV0e' +
        'ut3RVU/cm5aKKqPXVbjuIDPtDUh2JImq1+jmjkupIFNFStXadHncWXkecpb3393me4oJZnionXyjLV6W4QFZEleHCWNG+' +
        '0eKggQJiRVV6vhAXwoqrul0AC1H1uuIsTLUyukYH1jBL7WJ8lgq6oqwkVXSQDrLSVEFXjJWoirlCrFRVyBVhJasirgCr6' +
        '5tEv7a5A5jL0tcN7vNl9OVcHqtXRbocVr+Kc9k3H/3qPL69Ise7dh0SsS+2JmtFddgvdy/gGbY7Jdp2GRcyrlu1BfUjxt' +
        'iPRm/lqVbGHOMHnU39zQm0I/UbBLA+GVosJHGVrcoWkgEktnoLydYXkF/LiXG21MwAAAAASUVORK5CYII='
};

var control;
//var timer = null;

//window.Test = {
//    functionOne: function () {
//        var options = new OrgConfig();

//        var items = [
//            new OrgItemConfig({
//                id: 0,
//                parents: [],
//                title: "James Smith",
//                description: "VP, Public Sector",
//                image: photos.a
//            }),
//            new OrgItemConfig({
//                id: 1,
//                parents: [],
//                title: "Ted Lucas",
//                description: "VP, Human Resources",
//                image: photos.a
//            }),
//            new OrgItemConfig({
//                id: 2,
//                parents: [0,1],
//                title: "Fritz Stuger",
//                description: "Business Solutions, US",
//                image: photos.a
//            })
//        ];

//        options.items = items;
//        options.cursorItem = 0;
//        options.hasSelectorCheckbox = Enabled.True;

//        control = OrgDiagram(document.getElementById("basicdiagram"), options);

//    }
//}

window.Test = {
    functionOne: function (ids, names, dates, parents, count) {
        var options = new FamConfig();

        var item = new Array(count);
        for (var i = 0; i < count; i++) {
            item[i] = new FamItemConfig({
                id: ids[i],
                parents: parents[i],
                title: names[i],
                description: dates[i],
                image: photos.a,
                link: "https://www.basicprimitives.com/reactusecases/firstorganizationalchart"
            });
        }
        //var items = [
        //    new FamItemConfig({
        //        id: 0,
        //        parents: null,
        //        title: "James Smith",
        //        description: "Co-CEO",
        //        image: photos.a
        //    }),
        //    new FamItemConfig({
        //        id: 100,
        //        parents: null,
        //        title: "James Smith 2",
        //        description: "Co-CEO",
        //        image: photos.a
        //    }),
        //    new FamItemConfig({ id: 1, parents: [0, 100], hasSelectorCheckbox: false, templateName: "DepartmentTitleTemplate", title: "Finance", itemTitleColor: "Green" }),
        //    new FamItemConfig({
        //        id: 2,
        //        parents: [1],
        //        title: "Ted Lucas",
        //        description: "VP, Human Resources",
        //        image: photos.a
        //    }),
        //    new FamItemConfig({ id: 3, parents: [0, 100], hasSelectorCheckbox: false, templateName: "DepartmentTitleTemplate", title: "Sales", itemTitleColor: "Navy" }),
        //    new FamItemConfig({
        //        id: 4,
        //        parents: [3],
        //        title: "Fritz Stuger",
        //        description: "VP, Human Resources",
        //        image: photos.a
        //    }),
        //    new FamItemConfig({ id: 5, parents: [0, 100], hasSelectorCheckbox: false, templateName: "DepartmentTitleTemplate", title: "Operations", itemTitleColor: "Magenta" }),
        //    new FamItemConfig({
        //        id: 6,
        //        parents: [5],
        //        title: "Brad Whitt",
        //        description: "VP, Human Resources",
        //        image: photos.a
        //    }),
        //    new FamItemConfig({ id: 7, parents: [0, 100], hasSelectorCheckbox: false, templateName: "DepartmentTitleTemplate", title: "IT", itemTitleColor: "Orange" }),
        //    new FamItemConfig({
        //        id: 8,
        //        parents: [7],
        //        title: "Ted Whitt",
        //        description: "VP, Human Resources",
        //        image: photos.a
        //    }),
        //    new FamItemConfig({
        //        id: 18,
        //        parents: [7],
        //        title: "Ted Whitt 2",
        //        description: "VP, Human Resources",
        //        image: photos.a
        //    }),
        //    { id: 19, parents: [2], isVisible: true, description: "VP, Security Technology Unit (STU)", email: "robemorg@name.com", groupTitleColor: "#4169e1", image: photos.a, itemTitleColor: "#4b0082", phone: "308-532-6548", title: "Robert Morgan" },
        //    { id: 20, parents: [2], isVisible: true, description: "GM, Software Serviceability", email: "idabene@name.com", groupTitleColor: "#4169e1", image: photos.a, itemTitleColor: "#4b0082", phone: "765-723-1327", title: "Ida Benefield" },
        //    { id: 21, parents: [4], isVisible: true, description: "GM, Core Operating System Test", email: "vadaduho@name.com", groupTitleColor: "#4169e1", image: photos.a, itemTitleColor: "#4b0082", phone: "303-333-9215", title: "Vada Duhon" },
        //    { id: 22, parents: [4], isVisible: true, description: "GM, Global Platform Technologies and Services", email: "willloyd@name.com", groupTitleColor: "#4169e1", image: photos.a, itemTitleColor: "#4b0082", phone: "585-309-6253", title: "William Loyd" },
        //    { id: 23, parents: [6], isVisible: true, description: "Sr. VP, NAME & Personal Services Division", email: "craiblue@name.com", groupTitleColor: "#4169e1", image: photos.a, itemTitleColor: "#4b0082", phone: "915-355-4705", title: "Craig Blue" },
        //    { id: 24, parents: [6], isVisible: true, description: "VP, NAME Communications Services and Member Platform", email: "joelcraw@name.com", groupTitleColor: "#4169e1", image: photos.a, itemTitleColor: "#4b0082", phone: "650-623-3302", title: "Joel Crawford" },
        //    { id: 25, parents: [8], isVisible: true, description: "VP & CFO, NAME", email: "barblang@name.com", groupTitleColor: "#4169e1", image: photos.a, itemTitleColor: "#4b0082", phone: "618-822-7345", title: "Barbara Lang" },
        //    { id: 26, parents: [8], isVisible: true, description: "VP, NAME Operations", email: "barbfaul@name.com", groupTitleColor: "#4169e1", image: photos.a, itemTitleColor: "#4b0082", phone: "641-678-7646", title: "Barbara Faulk" },
        //    { id: 27, parents: [18], isVisible: true, description: "VP, NAME Global Sales & Marketing", email: "stewwill@name.com", groupTitleColor: "#4169e1", image: photos.a, itemTitleColor: "#4b0082", phone: "803-746-8733", title: "Stewart Williams" },
        //    { id: 28, parents: [18], isVisible: true, description: "Sr. VP, NAME Information Services & Merchant Platform", email: "robelemi@name.com", groupTitleColor: "#4169e1", image: photos.a, itemTitleColor: "#4b0082", phone: "425-590-4308", title: "Robert Lemieux" }
        //];

        options.items = item;
        options.cursorItem = 0;
        //options.selectionPathMode = SelectionPathMode.FullStack;
        options.neighboursSelectionMode = NeighboursSelectionMode.ParentsChildrenSiblingsAndSpouses;
        //options.templates = [getDepartmentTitleTemplate()];
        //options.onItemRender = onTemplateRender;
        //options.hasSelectorCheckbox = Enabled.True;
        options.normalLevelShift = 20;
        options.dotLevelShift = 20;
        options.lineLevelShift = 10;
        options.normalItemsInterval = 10;
        options.dotItemsInterval = 10;
        options.lineItemsInterval = 4;
        //options.hasSelectorCheckbox = primitives.Enabled.True;
        

        control = FamDiagram(document.getElementById("basicdiagram"), options);

    }

    //onTemplateRender: function (event, data) {
    //    switch (data.renderingMode) {
    //        case primitives.RenderingMode.Create:
    //            /* Initialize template content here */
    //            break;
    //        case primitives.RenderingMode.Update:
    //            /* Update template content here */
    //            break;
    //    }

    //    var itemConfig = data.context;
    //    var element = data.element;

    //    if (data.templateName == "DepartmentTitleTemplate") {
    //        element.firstChild.style.backgroundColor = itemConfig.itemTitleColor || primitives.Colors.RoyalBlue;
    //        element.firstChild.firstChild.textContent = itemConfig.title;
    //    }
    //},

    //getDepartmentTitleTemplate: function () {
    //    var result = new primitives.TemplateConfig();
    //    result.name = "DepartmentTitleTemplate";
    //    result.isActive = false;
    //    result.itemSize = new primitives.Size(200, 30);
    //    result.minimizedItemSize = new primitives.Size(3, 3);

    //    /* the following example demonstrates JSONML template see http://http://www.jsonml.org/ for details: */
    //    result.itemTemplate = ["div",
    //        {
    //            "style": {
    //                width: result.itemSize.width + "px",
    //                height: result.itemSize.height + "px"
    //            },
    //            "class": ["bp-item", "bp-corner-all", "bt-item-frame"]
    //        },
    //        ["div",
    //            {
    //                "name": "titleBackground",
    //                "style": {
    //                    top: "2px",
    //                    left: "2px",
    //                    width: "196px",
    //                    height: "25px"
    //                },
    //                "class": ["bp-item", "bp-corner-all", "bt-title-frame"]
    //            },
    //            ["div",
    //                {
    //                    name: "title",
    //                    "class": ["bp-item", "bp-title"],
    //                    style: {
    //                        top: "3px",
    //                        left: "6px",
    //                        width: "188px",
    //                        height: "23px",
    //                        textAlign: "center"
    //                    }
    //                }
    //            ]
    //        ]
    //    ];

    //    return result;
    //}
}
