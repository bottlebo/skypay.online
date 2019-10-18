<template>
    <div style=" height:100%">

        <div>

            <div class="chart-holder">
                <!--<keep-alive>-->
                <component :is="currentChartName" ></component>
                <!--</keep-alive>-->
            </div>

        </div>
    </div>
</template>
<script>
    import { QBtn } from 'quasar-framework'
    import DoughChart from '../UI/DoughChart'
    import BarChart from '../UI/BarChart'
    import api from '../../store/api'
    import { mapGetters } from 'vuex'

    export default {
        name: 'out-charts',
        data() {
            return {
                currentView: 'DoughChart',
                chartData: {
                    labels: [],
                    datasets: [
                        {
                            label: 'GitHub Commits',
                            //backgroundColor: colors.reverse(),
                            data: []
                        }
                    ]
                }
            }
        },
        props: ['view'],
        computed: {
            ...mapGetters({
                currentChartView: "currentChartView",
                currentChartName: "currentChartName",
                chartDataIndex: "chartDataIndex"
            })
        },
        //watch: {
        //    //currentChartView: function () {
        //    //    console.log('***')
        //    //    if (this.currentChartView == 'InCharts')
        //    //        this.renderChart(this.chartData, this.options)

        //    //}
        //    chartDataIndex: async function () {
        //        console.log('***:' + this.chartDataIndex)
        //        this.chartData = {
        //            labels: [],
        //            datasets: [
        //                {
        //                    label: 'GitHub Commits',
        //                    //backgroundColor: colors.reverse(),
        //                    data: []
        //                }
        //            ]
        //        }
        //        let _data = []
        //        if (this.currentChartView == 'InCharts')
        //            _data = await api.reports.byCategories();
        //        else if (this.currentChartView == 'OutCharts') {
        //            if (this.chartDataIndex == 0)
        //                _data = await api.reports.saleByCategories();
        //            else if (this.chartDataIndex == 1)
        //                _data = await api.reports.saleByProducts();
        //        }
        //        //for (var i = 0; i < this.chartData.labels.length; i++) {
        //        //    this.chartData.labels.pop()
        //        //}
        //        //for (var i = 0; i < this.chartData.datasets[0].data; i++) {
        //        //    this.chartData.datasets[0].data.pop()
        //        //}
        //        //= []
        //        //this.chartData.datasets[0].data = []
        //        for (let i in _data) {
        //            this.chartData.labels.push(_data[i].categoryName)
        //            this.chartData.datasets[0].data.push(_data[i].amount)
        //        }
        //        //console.log('chart mount')
        //        for (let i in this.chartData.labels) {
        //            console.log(this.chartData.labels[i])
        //        }
        //        //if (this.currentChartView == 'InCharts')
        //        //this.renderChart(this.chartData, this.options)

        //    }
        //},
        methods: {
            //makeChart: async function () {
            //    let _data = []
            //    if (this.currentChartView == 'InCharts')
            //        _data = await api.reports.byCategories();
            //    else if (this.currentChartView == 'OutCharts') {
            //        if (this.chartDataIndex == 0)
            //            _data = await api.reports.saleByCategories();
            //        else if (this.chartDataIndex == 1)
            //            _data = await api.reports.saleByProducts();
            //    }
            //    //for (var i = 0; i < this.chartData.labels.length; i++) {
            //    //    this.chartData.labels.pop()
            //    //}
            //    //for (var i = 0; i < this.chartData.datasets[0].data; i++) {
            //    //    this.chartData.datasets[0].data.pop()
            //    //}
            //    //= []
            //    //this.chartData.datasets[0].data = []
            //    for (let i in _data) {
            //        this.chartData.labels.push(_data[i].categoryName)
            //        this.chartData.datasets[0].data.push(_data[i].amount)
            //    }
            //    //console.log('chart mount')
            //    for (let i in this.chartData.labels) {
            //        console.log(this.chartData.labels[i])
            //    }
            //    //console.log(this)
            //    //this.renderChart(this.chartData, this.options)
            //}
        },
        components: {
            QBtn, DoughChart, BarChart
        },
        async created() {
            //let _data = []

            //this.chartData = {
            //    labels: [],
            //    datasets: [
            //        {
            //            label: 'GitHub Commits',
            //            //backgroundColor: colors.reverse(),
            //            data: []
            //        }
            //    ]
            //}
            //if (this.currentChartView == 'InCharts')
            //    _data = await api.reports.byCategories();
            //else if (this.currentChartView == 'OutCharts') {
            //    if (this.chartDataIndex == 0)
            //        _data = await api.reports.saleByCategories();
            //    else if (this.chartDataIndex == 1)
            //        _data = await api.reports.saleByProducts();
            //}
            ////for (var i = 0; i < this.chartData.labels.length; i++) {
            ////    this.chartData.labels.pop()
            ////}
            ////for (var i = 0; i < this.chartData.datasets[0].data; i++) {
            ////    this.chartData.datasets[0].data.pop()
            ////}
            ////= []
            ////this.chartData.datasets[0].data = []
            //for (let i in _data) {
            //    this.chartData.labels.push(_data[i].categoryName)
            //    this.chartData.datasets[0].data.push(_data[i].amount)
            //}
            ////console.log('chart mount')
            //for (let i in this.chartData.labels) {
            //    console.log(this.chartData.labels[i])
            //}
            //console.log('out created')
        }
    }
</script>
