const tarnsformTreeData = function (categories, handler, currentid) {
    //let categories = _cat.filter(s => s.companyId == 1)
    //console.log(typeof categories.filter)
    let res = {}
    
    res.name = 'Категории'
    res.id = 0
    res.open = true
    res.children = []
    if (typeof categories.filter == 'undefined') {
        return res
    }
    else {
        let parents = categories.filter(c => !c.parentId)
        res.children = parents.slice(0)
        for (let i in res.children) {
            
            let node = res.children[i]
            if (node.hasChild) {
                node.children = []
                findChild(categories, node)
            }
        }
        return res;
    }
}
const setSelected = function (cat, id) {
    console.log(id)
    for (let i in cat) {
        if (cat[i].id == id) {
            cat[i].icon = "send"
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
        node.children = []; return;
    }
    node.children = childs.slice(0)
    for (let i in childs) {

        findChild(categories, childs[i])


    }
}
const colors= ['#61EFCD', '#CDDE1F', '#FEC200', '#CA765A', '#2485FA', '#F57D7D', '#C152D2', '#8854D9', '#3D4EB8', '#00BCD7']
export  {
    tarnsformTreeData, colors
}
