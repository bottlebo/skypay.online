<script>
    import { Doughnut, mixins } from 'vue-chartjs'
    import api from '../../store/api'
    import { colors } from '../../helpers'
    import { mapGetters } from 'vuex'
    export default {
        extends: Doughnut,
        mixins: [mixins.reactiveData],
        data() {
            return {
                chartData: {
                    labels: [],
                    datasets: [
                        {
                            label: 'GitHub Commits',
                            backgroundColor: colors.reverse(),
                            data: []
                        }
                    ]
                },
                options: {
                    responsive: true,
                    legend: {
                        position: 'right',
                    },
                    title: {
                        display: false,
                        text: 'Запасы по категориям'
                    },
                }
            }
        },
        methods: {
        },
        computed: {
            ...mapGetters({
                currentChartView: "currentChartView",
                currentChartName: "currentChartName",
                chartDataIndex: "chartDataIndex"
            })
        },
        watch: {
            chartDataIndex: async function () {
                let _data = []
                if (this.currentChartView == 'InCharts') {


                    _data = await api.reports.byCategories();
                }
                else if (this.currentChartView == 'OutCharts')
                {
                    if (this.chartDataIndex == 0)
                        _data = await api.reports.saleByCategories();
                    else if (this.chartDataIndex == 1)
                        _data = await api.reports.saleByProducts();
                }
                this.chartData.labels = []
                this.chartData.datasets[0].data = []
                for (let i in _data) {
                    this.chartData.labels.push(_data[i].categoryName)
                    this.chartData.datasets[0].data.push(_data[i].amount)
                }
                //
                this.$data._chart.update()
            }
        },
        async mounted() {
            let _data = []
            if (this.currentChartView == 'InCharts')
                _data = await api.reports.byCategories();
            else if (this.currentChartView == 'OutCharts')
                _data = await api.reports.saleByCategories();
            this.chartData.labels = []
            this.chartData.datasets[0].data = []
            for (let i in _data) {
                this.chartData.labels.push(_data[i].categoryName)
                this.chartData.datasets[0].data.push(_data[i].amount)
            }
           
            this.renderChart(this.chartData, this.options)
        }
    }
</script>
