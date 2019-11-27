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
                    <component :is="currentView"></component>
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
            getByCategory: {
                get() { return api.reports.byCategories() }
            }
        },
        async created() {
            this.shortstock = await api.reports.shortstock()
            this.shortsale = await api.reports.shortsale()

            console.log('** created')
        },
        async mounted() {

        },
        methods: {
            ...mapActions({
                setView: "setView", setChart: "setChart"
            }),
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

    .pointer {
        cursor: pointer
    }
</style>
