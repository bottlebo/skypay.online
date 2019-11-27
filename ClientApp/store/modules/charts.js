import * as types from '../mutation-types'
const chartViews = [
    { view: 'InCharts', charts: [{ name: 'DoughChart', data: 0 }, { name: 'BarChart', data: 0 }] },
    { view: 'OutCharts', charts: [{ name: 'DoughChart', data: 0 }, { name: 'DoughChart', data: 1 }, { name: 'BarChart', data: 0 }] }
]
const state = {
    selectedViewIndex: 0,
    selectedChartIndex: 0

}
const getters = {
    currentChartView: state => chartViews[state.selectedViewIndex].view,
    currentChartName: state => chartViews[state.selectedViewIndex].charts[state.selectedChartIndex].name,
    chartDataIndex: state => chartViews[state.selectedViewIndex].charts[state.selectedChartIndex].data
}
const actions = {
    setView({ commit, state }, index) {
        commit(types.SET_VIEW, index)
    },
    setChart({ commit, state }, index) {
        commit(types.SET_CHART, index)
    }
}
const mutations = {
    [types.SET_VIEW](state, index) {
        state.selectedViewIndex = index
    },
    [types.SET_CHART](state, index) {
        state.selectedChartIndex = index
    }
}
export default {
    state,
    getters,
    actions,
    mutations
}
