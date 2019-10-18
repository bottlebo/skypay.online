import Vue from 'vue'
import Vuex from 'vuex'
import sidemenu from './modules/sidemenu'
import companies from './modules/companies'
import stocks from './modules/stocks'
import categories from './modules/categories'
import products from './modules/products'
import charts from './modules/charts'

import createLogger from 'vuex/dist/logger'
Vue.use(Vuex)

// TYPES
const MAIN_SET_COUNTER = 'MAIN_SET_COUNTER'

// STATE
const state = {
    counter: 0
}

// MUTATIONS
const mutations = {
    [MAIN_SET_COUNTER](state, obj) {
        state.counter = obj.counter
    }
}

// ACTIONS
const actions = ({
    setCounter({ commit }, obj) {
        commit(MAIN_SET_COUNTER, obj)
    }
})
const debug = process.env.NODE_ENV !== 'production'
export default new Vuex.Store({
    modules: {
        sidemenu, companies, stocks, categories, products, charts
    },
    strict: debug,
    plugins: debug ? [createLogger()] : [],
    state,
    mutations,
    actions
});
