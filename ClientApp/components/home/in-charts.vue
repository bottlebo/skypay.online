<template>
    <div style=" height:100%">

        <div>
            <div class="chart-holder">
                <component :is="currentChartName"></component>
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
        components: {
            QBtn, DoughChart, BarChart
        },
    }
</script>
<style>
    .chart-holder {
        position: relative;
        width: 60%
    }
        .chart-holder canvas {
        }
</style>
