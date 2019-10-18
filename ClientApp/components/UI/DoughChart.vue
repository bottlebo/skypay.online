<script>
    import { Doughnut, mixins } from 'vue-chartjs'
    import api from '../../store/api'
    import { colors } from '../../helpers'
    import { mapGetters } from 'vuex'
    //const { reactiveData } = mixins
    export default {
        extends: Doughnut,
        mixins: [mixins.reactiveData],
        data() {
            return {
                //chartData: null,
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
            //    for (var i = 0; i < this.chartData.labels.length; i++) {
            //        this.chartData.labels.pop()
            //    }
            //    for (var i = 0; i < this.chartData.datasets[0].data; i++) {
            //        this.chartData.datasets[0].data.pop()
            //    }
            //    //= []
            //    //this.chartData.datasets[0].data = []
            //    for (let i in _data) {
            //        this.chartData.labels.push(_data[i].categoryName)
            //        this.chartData.datasets[0].data.push(_data[i].amount)
            //    }
            //    //console.log('chart mount')
            //    //for (let i in this.chartData.labels) {
            //    //    console.log(this.chartData.labels[i])
            //    //}
            //    console.log(this)
            //    this.renderChart(this.chartData, this.options)
            //}
        },
        computed: {
            ...mapGetters({
                currentChartView: "currentChartView",
                currentChartName: "currentChartName",
                chartDataIndex: "chartDataIndex"
            })
        },
        //props: ['chartData'],
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
                   // _data = await api.reports.saleByCategories();
                this.chartData.labels = []
                this.chartData.datasets[0].data = []
                for (let i in _data) {
                    this.chartData.labels.push(_data[i].categoryName)
                    this.chartData.datasets[0].data.push(_data[i].amount)
                }
                //
                this.$data._chart.update()
                //this.renderChart(this.chartData, this.options)
            }
        },
        //created() {
        //    console.log('* chart created')
        //},
        async mounted() {
            // Переопределение базового рендер метода с реальными данными.
            //this.makeChart()
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
           
            //this.renderChart(this.chartData, this.options)
            this.renderChart(this.chartData, this.options)
        }
    }
</script>
