<template>
    <div style=" height:100%">
       
        <div>
            <div class="chart-holder">
                <!--<keep-alive>-->
                    <component :is="currentChartName"></component>
                    <!--</keep-alive>-->
</div>

        </div>
    </div>
</template>
<script>
    import { QBtn } from 'quasar-framework'
    import DoughChart from '../UI/DoughChart'
    import BarChart from '../UI/BarChart'

    import { mapGetters } from 'vuex'

    export default {
        name: 'in-charts',
        data() {
            return {
                currentView: 'DoughChart'
            }
        },
        computed: {
            ...mapGetters({
                currentChartView: "currentChartView",
                currentChartName: "currentChartName"
            })
        },
        //watch: {
        //    currentChartView: function () {
        //        console.log('***')
        //        if (this.currentChartView == 'InCharts')
        //            this.renderChart(this.chartData, this.options)

        //    }
        //},
        components: {
            QBtn, DoughChart,BarChart
        },
        //created() {
        //    console.log('in created')
        //}
    }
</script>
<style>
    .chart-holder {
        position: relative;
        width:60%
    }

        .chart-holder canvas {
            /*height: 500px !important;
            width: 500px !important*/
        }
</style>
