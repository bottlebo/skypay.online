//const tarnsformTreeData_1 = function (categories, handler, currentid) {
//    for (let i in categories) {
//        categories[i].handler = handler
//        categories[i].expanded = categories[i].id == currentid
//        if (categories[i].children.length > 0)
//        {
//            tarnsformTreeData(categories[i].children, handler, currentid)
//        }
//    }
//}
const _cat = [
    { id: 1, name: "Телевизоры", hasChild: true, companyId: 1 },
    { id: 2, name: "Телевизоры LED", hasChild: false, parentID: 1, companyId: 1 },
    { id: 3, name: "Телевизоры PLAZMA", hasChild: true, parentID: 1, companyId: 1 },
    { id: 30, name: "Телевизоры PLAZMA-1", hasChild: false, parentID: 3, companyId: 1 },
    { id: 31, name: "Телевизоры PLAZMA-2", hasChild: false, parentID: 3, companyId: 1 },
    //
    { id: 4, name: "Кофеварки", hasChild: true, companyId: 2 },
    { id: 5, name: "Esspresso", hasChild: false, parentID: 4, companyId: 2 },
]
const tarnsformTreeData = function (categories, handler, currentid) {
    //let categories = _cat.filter(s => s.companyId == 1)
    //console.log(typeof categories.filter)
    let res = {}
    
    res.name = 'Категории'
    res.id = 0
    res.open = true
    res.children = []
    if (typeof categories.filter == 'undefined') {
        //alert('[[[')
        return res
    }
    else {
        let parents = categories.filter(c => !c.parentId)
        res.children = parents.slice(0)
        //debugger
        for (let i in res.children) {
            
            let node = res.children[i]
            //node.open = false
            if (node.hasChild) {
                //node.open = true
                node.children = []
                findChild(categories, node)
            }
        }
        //console.log(res)
        return res;
    }
}
const setSelected = function (cat, id) {
    console.log(id)
    for (let i in cat) {
        if (cat[i].id == id) {
            //console.log('**')
            cat[i].icon = "send"
            //console.log(cat)
        }
        else {
            cat[i].icon = ""
        }
        if (cat[i].children && cat[i].children.length > 0)
            setSelected(cat[i].children,id)
    }
}
const expandParent = function (categories, parentid) {
    let parent = categories.filter(n => n.id == parentid)[0]
    if (parent ) {
        parent.opened = true
        if (parent.parentID) expandParent(categories, parent.parentID)
    }
    return
}
const findChild = function (categories, node) {
    let childs = categories.filter(n => n.parentId == node.id)
    if (childs.length == 0) {
        //node.expanded = false
        //node.open = false
        node.children = []; return;
    }
    node.children = childs.slice(0)
    //node.open = true
    //node.expanded = false
    for (let i in childs) {

        findChild(categories, childs[i])


    }
    //categories.filter(n => n.parentID == node.id)
}
const colors= ['#61EFCD', '#CDDE1F', '#FEC200', '#CA765A', '#2485FA', '#F57D7D', '#C152D2', '#8854D9', '#3D4EB8', '#00BCD7']
export  {
    tarnsformTreeData, colors
}
