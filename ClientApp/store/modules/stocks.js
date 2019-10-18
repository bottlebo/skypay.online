import * as types from '../mutation-types'
import api from '../api'
const state = {
    all: [],
    selectedStockId: null

}
const getters = {
    stocks: state => state.all,
    selectedStockId: state => state.selectedStockId
}
const actions = {
    //saveChildrens({ commit, state }, childrens) {
    //    commit(types.RECEIVE_CHILDRENS, { childrens })
    //},
    async getStocks({ commit, state, rootState }, companyId) {
        if (state.all.length > 0) {

            return state.all;//.find(s => s.companyId === companyId)
        }
        else {
            try {
                let _data = await api.stocks.get(rootState.companies.selectedCompanyId)
                if (_data.length > 0) {
                    let stocks = _data.map(function (el) { return { value: el.id, label: el.name } })

                    console.log(stocks)

                    commit(types.LOAD_STOCKS, { stocks })
                    commit(types.SELECT_STOCK, { stockId: state.all[0].value })
                }
                //return companies;
            }
            catch (e) {
                //return null;
                console.log(e)
            }
        }
    },
    async reloadStocks({ commit, state, rootState }, companyId) {
       
        try {
            let _data = await api.stocks.get(rootState.companies.selectedCompanyId)
                let stocks = _data.map(function (el) { return { value: el.id, label: el.name } })

                //console.log(stocks)
                commit(types.LOAD_STOCKS, { stocks })
                commit(types.SELECT_STOCK, { stockId: state.all[0].value })
                //return companies;
            }
            catch (e) {
                //return null;
                console.log(e)
            }
        
    },
    selectStock({ commit, state }, id) {
        if (id)
            commit(types.SELECT_STOCK, { stockId: id /*state.all.filter(c=>c.id == index)[0]*/ })
    }
    //,
    //async loadCompanies({ state }) {
    //    try {
    //        let companies = await api.getCompanies()
    //        commit(types.LOAD_COMPANIES, { companies })
    //        return companies;
    //    }
    //    catch (e) {
    //        return null;
    //        console.log(e)
    //    }
    //}
}
const mutations = {
    [types.LOAD_STOCKS](state, { stocks }) {
        state.all = stocks
    },
    [types.SELECT_STOCK](state, { stockId }) {
        state.selectedStockId = stockId
    }
}
export default {
    state,
    getters,
    actions,
    mutations
}
