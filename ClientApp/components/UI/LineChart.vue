<script>
    import { Line } from 'vue-chartjs'
    import api from '../../store/api'
    import { colors } from '../../helpers'
    export default {
        extends: Line,
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
        props: ['gorender'],
        async mounted() {
            let _data = await api.reports.byValue();
            this.chartData.labels = []
            this.chartData.datasets.data = []
            for (let i in _data) {
                this.chartData.labels.push(_data[i].date)
                this.chartData.datasets[0].data.push(_data[i].value)
            }
            this.renderChart(this.chartData, this.options)
        }
    }
</script>
