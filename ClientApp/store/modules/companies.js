import * as types from '../mutation-types'
import api from '../api'
const state = {
    all: [],
    selectedCompanyId: null
}
const getters = {
    companies: state => state.all,
    selectedCompanyId: state => state.selectedCompanyId
}
const actions = {
    async getCompanies({ commit, state }) {
        if (state.all.length > 0) {

            return state.all
        }
        else {
            try {
                let response = await api.getCompanies()
                let _data = response.data;
                let companies = _data.map(function (el) { return { value: el.id, label: el.name } })

                commit(types.LOAD_COMPANIES, { companies })
                commit(types.SELECT_COMPANY, { companyId: state.all[0].id })
            }
            catch (e) {
                console.log(e)
            }
        }
    },
    selectCompany({ commit, state }, id) {
        if (id)
            commit(types.SELECT_COMPANY, { companyId: id })
    },
    async loadCompanies({ state }) {
        try {
            let companies = await api.getCompanies()
            commit(types.LOAD_COMPANIES, { companies })
            return companies;
        }
        catch (e) {
            return null;
            console.log(e)
        }
    }
}
const mutations = {
    [types.LOAD_COMPANIES](state, { companies }) {
        state.all = companies
    },
    [types.SELECT_COMPANY](state, { companyId }) {
        state.selectedCompanyId = companyId
    },
}
export default {
    state,
    getters,
    actions,
    mutations
}
