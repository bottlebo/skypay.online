<script>
    import { Bar } from 'vue-chartjs'
    import api from '../../store/api'
    import { colors } from '../../helpers'
    import {  mapGetters } from 'vuex'
    export default {
        extends: Bar,
        //mixins: [mixins.reactiveProp],
        data() {
            return {
                chartData: {
                    labels: [],
                    datasets: [
                        {
                            label: 'Запасы по дням',
                            backgroundColor: '#00BCD7',
                            borderColor: '#00BCD7',
                            data: []
                        }
                    ]
                },
                options: {
                    //scales: {
                    //    xAxes: [{
                    //        type: 'time',
                    //        time: {
                    //            //displayFormats: {
                    //            //    minute: 'MMM YYYY'
                    //            //}
                    //        }
                    //    }]
                    //},
                    responsive: true,
                    //legend: {
                    //    position: 'right',
                    //},
                    title: {
                        display: false,
                        text: 'Запасы по категориям'
                    },
                }
            }
        },
        computed: {
            ...mapGetters({
                currentChartView: "currentChartView",
                currentChartName: "currentChartName",
                chartDataIndex: "chartDataIndex"
            })
        },
        props: ['gorender'],
        watch: {
            chartDataIndex: async function () {
                let _data = []
                this.chartData.labels = []
                this.chartData.datasets[0].data = []
                if (this.currentChartView == 'InCharts') {
                    this.chartData.datasets[0].label = 'Запасы по дням'
                    _data = await api.reports.byValue();
                }
                else if (this.currentChartView == 'OutCharts') {
                    this.chartData.datasets[0].label = 'Продажи по дням'

                    _data = await api.reports.saleByDays();
                }
                
                for (let i in _data) {
                    this.chartData.labels.push(_data[i].date)
                    this.chartData.datasets[0].data.push(_data[i].value)
                }
                this.$data._chart.update()
            }
        },
        //created() {
        //    console.log('* chart created')
        //},
        async mounted() {
            let _data = []
            if (this.currentChartView == 'InCharts') {
                this.chartData.datasets[0].label = 'Запасы по дням'
                _data = await api.reports.byValue();
            }
            else if (this.currentChartView == 'OutCharts') {
                this.chartData.datasets[0].label = 'Продажи по дням'

                _data = await api.reports.saleByDays();
            }
            this.chartData.labels = []
            this.chartData.datasets[0].data = []
            for (let i in _data) {
                this.chartData.labels.push(_data[i].date)
                this.chartData.datasets[0].data.push(_data[i].value)
            }











            //let _data = await api.reports.byValue();

            //this.chartData.labels = []
            //this.chartData.datasets[0].data = []
            ////let _d = new Date()
            //for (let i in _data) {
            //    //console.log(_d)
            //    this.chartData.labels.push(_data[i].date)
            //    this.chartData.datasets[0].data.push(_data[i].value )
            //}
            //console.log('chart mount')
            //for (let i in this.chartData.labels) {
            //    console.log(this.chartData.labels[i])
            //}
            this.renderChart(this.chartData, this.options)
        }
    }
</script>
