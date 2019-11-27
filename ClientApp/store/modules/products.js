import * as types from '../mutation-types'
import api from '../api'
import axios from 'axios'

const state = {
    all: [],
    currentCategoryId: null,
    selectedProductId: null
}
const getters = {
    products: state => state.all,
    selectedProductId: state => state.selectedProductId
}
const actions = {
    async getProducts({ commit, state, rootState }, categoryId) {
        return new Promise(function (resolve, reject) {
            if (state.all.length > 0 && rootState.categories.selectedCategoryId == state.currentCategoryId) {

                resolve(state.all);
            }
            else {
                try {
                    axios
                        .get(api.host + '/odata/Products?$filter=categoryId eq ' + rootState.categories.selectedCategoryId, { timeout: 10000 })
                        .then(function (response) {
                            let products = response.data.map(function (el) { el.n = el.id; return el })
                            commit(types.LOAD_PRODUCTS, { products })
                            commit(types.PRODUCT_CATEGORY, { categoryId: rootState.categories.selectedCategoryId })
                            resolve(products)
                        })
                        .catch(function (error) {
                            reject(error)
                        });
                }
                catch (e) {
                    //return null;
                    reject(e)
                }
            }
        })
    },
    async reloadProducts({ commit, state, rootState }) {
        return new Promise(function (resolve, reject) {
            try {
                axios
                    .get(api.host + '/odata/Products?$filter=categoryId eq ' + rootState.categories.selectedCategoryId, { timeout: 10000 })
                    .then(function (response) {
                        let products = response.data.map(function (el) { el.n = el.id; return el })
                        commit(types.LOAD_PRODUCTS, { products })
                        commit(types.PRODUCT_CATEGORY, { categoryId: rootState.categories.selectedCategoryId })
                        resolve(products)
                    })
                    .catch(function (error) {
                        reject(error);
                    });
            }
            catch (e) {
                reject(e)
            }
        })

    },
    addProduct({ commit, state }, product) {
        commit(types.ADD_PRODUCT, { product })
    },
    selectProduct({ commit, state }, id) {
        commit(types.SELECT_PRODUCT, { productId: id })
    },
    editProduct({ commit, state }, product) {
        commit(types.EDIT_PRODUCT, { product })
    },
    deleteProduct({ commit }, productId) {
        commit(types.DELETE_PRODUCT, { productId })
    }
}
const mutations = {
    [types.LOAD_PRODUCTS](state, { products }) {
        state.all = products
    },
    [types.PRODUCT_CATEGORY](state, { categoryId }) {
        state.currentCategoryId = categoryId
    },
    [types.ADD_PRODUCT](state, { product }) {
        state.all.push(product)
    },
    [types.SELECT_PRODUCT](state, { productId }) {
        state.selectedProductId = productId
    },
    [types.EDIT_PRODUCT](state, { product }) {
        let p = state.all.filter(c => c.id == product.id)[0];
        Object.keys(p).forEach(function (key) {
            p[key] = product[key]
        })
    },
    [types.DELETE_PRODUCT](state, { productId }) {
        let del = state.all.filter(p => p.id == productId)[0];
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
