<template>
    <div style=" height:100%">

        <div>
            <div class="chart-holder">
                <component :is="currentChartName" ></component>
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
        
        methods: {
            
        },
        components: {
            QBtn, DoughChart, BarChart
        },
        async created() {
            
        }
    }
</script>
