import * as types from '../mutation-types'
import api from '../api'
import axios from 'axios'
const host = '/skypay'
const state = {
    all: [],
    selectedCategoryId: null,
    currentCompanyId: null
}
const getters = {
    categories: state => state.all,
    selectedCategoryId: state => state.selectedCategoryId
}
const actions = {
    getCategories({ commit, state, rootState }) {


        return new Promise(function (resolve, reject) {
            if (state.all.length > 0 && rootState.companies.selectedCompanyId == state.currentCompanyId) {

                resolve(state.all)
            }
            else {

                axios
                    .get(api.host + '/odata/Companies(' + rootState.companies.selectedCompanyId + ')/Categories', { timeout: 10000 })
                    .then(function (response) {
                        let categories = response.data.map(function (el) {
                            el.open = true; return el
                        })
                        commit(types.LOAD_CATEGOTIES, { categories })
                        commit(types.CATEGORY_COMPANY, { companyId: rootState.companies.selectedCompanyId })
                        commit(types.SELECT_CATEGORY, { categoryId: null })
                        resolve(categories)
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
            }
        })
    },
    saveCategories({ commit }, categories) {
        commit(types.LOAD_CATEGOTIES, { categories })
    },
    editCategory({ commit, state }, category) {
        commit(types.EDIT_CATEGORY, { category })
    },
    addCategory({ commit, state }, category) {
        commit(types.ADD_CATEGORY, { category })
    },
    deleteCategory({ commit }, categoryId) {
        commit(types.DELETE_CATEGORY, { categoryId })
    },
    toggleCategory({ commit }, id) {
        console.log('***' + id)
        if (id > 0)
            commit(types.TOGGLE_CATEGORY, id)

    },
    reloadCategories({ commit, state, rootState }) {
        return new Promise(function (resolve, reject) {
            axios
                .get(api.host + '/odata/Companies(' + rootState.companies.selectedCompanyId + ')/Categories', { timeout: 10000 })
                .then(function (response) {
                    let _data = response.data;
                    let categories = response.data.map(function (el) {
                        el.open = true; return el
                    })
                    commit(types.LOAD_CATEGOTIES, { categories })
                    commit(types.CATEGORY_COMPANY, { companyId: rootState.companies.selectedCompanyId })
                    commit(types.SELECT_CATEGORY, { categoryId: null })
                    resolve(categories)
                })
                .catch(function (error) {
                    console.log(error);
                })
        })
    },
    selectCategory({ commit, state }, id) {
        commit(types.SELECT_CATEGORY, { categoryId: id })
    }
}
const mutations = {
    [types.TOGGLE_CATEGORY](state, id) {
        let cat = state.all.find(c => c.id == id);
        cat.open = !cat.open
    },
    [types.LOAD_CATEGOTIES](state, { categories }) {
        state.all = categories
    },
    [types.SELECT_CATEGORY](state, { categoryId }) {
        state.selectedCategoryId = categoryId
    },
    [types.CATEGORY_COMPANY](state, { companyId }) {
        state.currentCompanyId = companyId
    },
    [types.EDIT_CATEGORY](state, { category }) {
        let cat = state.all.filter(c => c.id == category.id)[0];
        cat.name = category.name;
    },
    [types.ADD_CATEGORY](state, { category }) {

        let cat = state.all.filter(c => c.id == category.parentId)[0]
        if (cat) {
            cat.hasChild = true;
        }
        state.all.push(category)
    },
    [types.DELETE_CATEGORY](state, { categoryId }) {
        let del = state.all.filter(p => p.id == categoryId)[0];
        let index = state.all.indexOf(del);
        state.all.splice(index, 1)
    }
}

export default {
    state,
    getters,
    actions,
    mutations
}
