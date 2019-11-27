import * as types from '../mutation-types'
const state = {
    open: false,
    openClass: ""
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
        } else {
            state.open = !state.open
        }
    }
}
export default {
    state,
    actions,
    mutations
}
