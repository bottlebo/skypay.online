<script>
    import { Line } from 'vue-chartjs'
    import api from '../../store/api'
    import { colors } from '../../helpers'
    //import {  mapGetters } from 'vuex'
    export default {
        extends: Line,
        //mixins: [mixins.reactiveProp],
        data() {
            return {
                chartData: {
                    labels: [],
                    datasets: [
                        {
                            label: 'Запасы по дням',
                            backgroundColor: colors[6],
                            borderColor: colors[7],
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
        //computed: {
        //    ...mapGetters({
        //        currentChartView: "currentChartView"
        //    })
        //},
        props: ['gorender'],
        //watch: {
        //    currentChartView: function () {
        //        console.log('***')
        //        if (this.currentChartView == 'InCharts')
        //        this.renderChart(this.chartData, this.options)

        //    }
        //},
        //created() {
        //    console.log('* chart created')
        //},
        async mounted() {
            //console.log('* chart mounted')
            // Переопределение базового рендер метода с реальными данными.
            let _data = await api.reports.byValue();
            this.chartData.labels = []
            this.chartData.datasets.data = []
            //let _d = new Date()
            for (let i in _data) {
                //console.log(_d)
                this.chartData.labels.push(_data[i].date)
                this.chartData.datasets[0].data.push(_data[i].value )
            }
            //console.log('chart mount')
            //for (let i in this.chartData.labels) {
            //    console.log(this.chartData.labels[i])
            //}
            this.renderChart(this.chartData, this.options)
        }
    }
</script>
