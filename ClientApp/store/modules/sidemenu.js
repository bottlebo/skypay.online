import * as types from '../mutation-types'
const state = {
    open: false,
    openClass:""
}
const actions = {
    closeMenu({ commit, state }) {
        commit(types.SIDEMENU_TOGGLE, false)
    }
}
const mutations = {
    [types.SIDEMENU_TOGGLE](state, shouldOpen) {
        if (typeof shouldOpen === 'boolean') {
            state.open = shouldOpen
            //state.openClass = state.open ? "toggled" : ""
        } else {
            state.open = !state.open
            //state.openClass = state.open ? "toggled" : ""
        }
        //console.log(state.openClass)
    }
    //,
    //[types.SIDEMENU_TOGGLECLASS](state, shouldOpen) {
    //}
}
export default {
    state,
    //getters,
    actions,
    mutations
}
