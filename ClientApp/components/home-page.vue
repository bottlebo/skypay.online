<template>
    <div style=" background-color:aliceblue; min-height: calc(-50px + 100vh);">
        <div>
            <q-card class="pointer" inline style="width: 300px; background-color:white" @click="currentView=0;setChartIndex(0)">
                <q-card-title>
                    <span slot="subtitle">Запасы</span>
                    <span style="color: #fb6d9d; font-size:20px; font-weight:500">{{format(shortstock.total)}} <i class="rub" /></span>
                </q-card-title>
                <q-card-separator />
                <q-card-main style=" height:76px">
                    <div v-for="item in shortstock.stockItems" class="row">
                        <div class="col-7">{{item.stockName}}</div>
                        <div class="col-4" style="color: #fb6d9d; font-weight:500">{{format(item.amount)}} <i class="rub" /></div>
                    </div>
 
                </q-card-main>
            </q-card>
            <q-card class="pointer" inline style="width: 300px; background-color:white" @click="currentView=1;setChartIndex(0)">
                <q-card-title>
                    <span slot="subtitle">Продажи1</span>
                    <span style="color: #81c868; font-size:20px; font-weight:500">{{format(shortsale.total)}} <i class="rub" /></span>
                </q-card-title>
                <q-card-separator />
                <q-card-main style=" height:76px">
                    <div v-for="item in shortsale.stockItems" class="row">
                        <div class="col-7">{{item.stockName}}</div>
                        <div class="col-4" style="color: #81c868; font-weight:500">{{format(item.amount)}} <i class="rub" /></div>
                    </div>

                </q-card-main>
            </q-card>
        </div>
        <div>
            <q-card style="min-height: 600px; background-color:white">
                <q-card-title>
                    <!--<span slot="subtitle">Продажи</span> #4d80f3-->
                    <div class="row">
                        <div class="col-1" style="">
                            <span v-if="currentChartView=='InCharts'" style="color:#fb6d9d;  font-weight:500">Запасы</span>
                            <span v-if="currentChartView=='OutCharts'" style="color:#81c868;  font-weight:500">Продажи</span>
                        </div>
                        <div class="col-10" style="margin-left:20px" v-if="currentChartView=='InCharts'">
                            <q-btn @click="setChartIndex(0)" push flat color="skypay-primary">По категориям</q-btn>
                            <q-btn @click="setChartIndex(1)" flat push color="skypay-primary">По датам</q-btn>
                        </div>
                        <div class="col-10" style="margin-left:20px" v-if="currentChartView=='OutCharts'">
                            <q-btn @click="setChartIndex(0)" push flat color="skypay-primary">По категориям</q-btn>
                            <q-btn @click="setChartIndex(1)" flat push color="skypay-primary">По товарам</q-btn>
                            <q-btn @click="setChartIndex(2)" flat push color="skypay-primary">По дням</q-btn>

                        </div>
                    </div>
                </q-card-title>
                <q-card-separator />
                <q-card-main>
                    <!--<div class="row">
                        <div class="col-7">Склад Игрушек №1</div>
                        <div class="col-4" style="color: #fb6d9d; font-weight:500">60 000 <i class="rub" /></div>
                    </div>
                    <div class="row">
                        <div class="col-7">Склад Игрушек №1</div>
                        <div class="col-4">60 000 руб</div>
                    </div>-->
                    <component :is="currentView"></component>
                    <!--<div style="clear:both; padding-bottom:20px">123</div>-->
                    <!--<div class="chart-holder">
                        <dough-chart :gorender="gor"  :options="chartOptions" style="max-height:450px"></dough-chart>
                    </div>-->
                </q-card-main>
            </q-card>
        </div>
    </div>
</template>
<script>
    import { QBtn, QIcon, QCard, QCardTitle, QCardMain, QCardSeparator, QBtnGroup } from 'quasar-framework'
    import api from '../store/api'
    import { colors } from '../helpers'
    import DoughChart from '../components/UI/DoughChart'
    import InCharts from './home/in-charts'
    import OutCharts from './home/out-charts'
    import { mapActions, mapGetters } from 'vuex'
    export default {
        data() {
            return {

                gor: 0,
                shortstock: { stockItems: [], total: 0 },
                shortsale: { stockItems: [], total: 0 },
                
            }
        },

        computed: {
            ...mapGetters({
                currentChartView: "currentChartView"
            }),
            currentView: {
                get() { return this.currentChartView },
                set(index) { this.setView(index) }
            },
            //currentView: {},
            getByCategory: {
                get() { return api.reports.byCategories() }
            }
            //chart_data: function () {
            //    return this.chartData
            //   //return {
            //   //     labels: [],
            //   //         datasets: [
            //   //             {
            //   //                 label: 'GitHub Commits',
            //   //                 backgroundColor: colors.reverse(),
            //   //                 //['#61EFCD', '#CDDE1F', '#FEC200', '#CA765A', '#2485FA', '#F57D7D', '#C152D2', '#8854D9', '#3D4EB8', '#00BCD7'],// ['#f87979', '#00ff00', '#0000ff'],
            //   //                 data: []
            //   //             }
            //   //         ]
            //   // }
            //}
        },
        async created() {
            this.shortstock = await api.reports.shortstock()
            this.shortsale = await api.reports.shortsale()

            //console.log(this.shortstock.total)
            console.log('** created')
            //this.setView(0)
        },
        async mounted() {
            //console.log('** mounted')
            //let _data = await api.reports.byCategories();

            //this.chartData.labels = []
            //this.chartData.datasets.data = []
            //for (let i in _data) {
            //    this.chartData.labels.push(_data[i].categoryName)
            //    this.chartData.datasets[0].data.push(_data[i].amount)
            //}

        },
        methods: {
            ...mapActions({
                setView: "setView", setChart: "setChart" }),
            setChartIndex: function (val) {
                this.setChart(val)
            },
            format: function (num) {
                return numeral(num).format('0,0.00')
            }
        },
        components: {
            QBtn, DoughChart, InCharts, OutCharts, QBtnGroup,
            QIcon, QCard, QCardTitle, QCardMain, QCardSeparator
        }
    }
</script>
<style>
    .rub::after {
        content: "\20bd";
        font-style: normal
    }
    .pointer {cursor:pointer}
</style>
