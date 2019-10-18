<template>
    <div style=" background-color:aliceblue">


        <div class="row" style="padding:8px 0 0 4px;">
            <!--<div class="col-3">
                <div class="comname">
                    <q-select v-model="selectedStock" color="#3d5480"
                              :options="stocks" />
                </div>
            </div>-->
            <div class="" style="position:relative;width:100%">
                <div class="comname stock" style="position:absolute; top:-4px; z-index:1000">
                    
                            <q-select v-model="selectedStock" color="#3d5480"
                                      :options="stocks" />
                        
                </div>
                <div class="" style="position:absolute; right:10px; top:0px; z-index:1000">
                    <q-btn ref="target" flat round small  color="skypay-primary" icon="more_vert">
                        

                        <!-- Direct child of target -->
                        <q-popover ref="popover">
                            
                            <q-list separator link>
                                <q-item @click=" $refs.popover.close()">
                                    <q-item-side icon="add"  color="skypay-primary"/>
                                    <q-item-main label="Добавить" />
                                </q-item>
                                <q-item @click=" $refs.popover.close()">
                                    <q-item-side   color="skypay-primary">
                                        <q-icon  color="skypay-primary" name="edit" size="20px"></q-icon>
                                    </q-item-side>
                                    <q-item-main label="Изменить" />
                                </q-item>
                            </q-list>
                        </q-popover>
                    </q-btn>
                </div>
                <q-tabs inverted align="left" no-pane-border>
                    <q-tab color="skypay-primary" style="margin-left:240px" default slot="title" name="tab-1" label="Поступление" />
                    <q-tab color="skypay-primary" default slot="title" style="margin-right:50px" name="tab-2" label="Реализация" />

                    <!--<q-tab color="skypay-primary" slot="title" name="tab-3" label="Контрагенты" />
                    <q-tab color="skypay-primary" default slot="title" name="tab-4" label="Сотрудники" />

                    <q-tab color="skypay-primary" slot="title" name="tab-5" label="Еденицы измерения" />-->


                    <q-tab-pane name="tab-1"><documents-in></documents-in></q-tab-pane>
                    <q-tab-pane name="tab-2"><documents-out></documents-out></q-tab-pane>

                    <!--<q-tab-pane name="tab-3"><agents></agents></q-tab-pane>
                    <q-tab-pane name="tab-4"><users></users></q-tab-pane>

                    <q-tab-pane name="tab-5"><units></units></q-tab-pane>-->

                </q-tabs>
            </div>
        </div>





    </div>
</template>
<script>

    //import { Toolbar } from '@syncfusion/ej2-navigations';
    //import { routes } from '../routes'
    //import Units from './directory-components/units'
    //import Agents from './directory-components/contragents'
    //import ProductRange from './directory-components/product-range'
    //import ProductSets from './directory-components/product-sets'
    //import DocumentIn from './stock-components/documents-in'
    import { mapActions, mapGetters } from 'vuex'

    import {
        QTabs, QTab, QTabPane, QRouteTab, QSelect, QBtn, QPopover, QList, QItem, QItemSide, QItemMain, QIcon
    } from 'quasar-framework'
    export default {
        name: "stocks",
        data() {
            return {

            }
        },
        //props: ['id'],
        methods: {
            ...mapActions({ selectStock: 'selectStock' })
            //tb_click: function (e) {
            //    this.currentView = e.item.properties.htmlAttributes['component']
            //}
        },
        computed: {
            ...mapGetters({
                stocks: "stocks",
                selectedStockId: "selectedStockId",
                selectedCompanyId: "selectedCompanyId"
            }),
            selectedStock: {
                get() {
                    return this.selectedStockId
                },
                set(val) {
                    this.selectStock(val)
                }
            },
            currentCompanyId: {
                get() {
                    return this.selectedCompanyId ? this.selectedCompanyId : ""
                }
            },
            //dirroutes: function () {
            //    return routes.filter(r => r.path == '/Manage/Directory')[0].children
            //}
            //currentCompanyId: {
            //    get() {
            //        if (this.$store.state.companies.selectedCompany)
            //            return this.$store.state.companies.selectedCompany.id;
            //        else
            //            return null
            //    }
            //}
        },
        watch: {
            currentCompanyId: async function () {
                //var _this = this
                await this.$store.dispatch("reloadStocks");
                //axios.get('/odata/Companies(' + '1' + ')/Categories', { timeout: 10000 }).then((response) => {
                //    console.log(response.data)
                //    _this.cats1 = response.data
                //    console.log(_this.cats1)
                //    //this.cats = helpers.tarnsformTreeData((response.data), this.changeCategory, this.currentCategoryId)
                //    console.log('-----')
                //})
            }
        },
        mounted() {

        },
        async created() {
            //await this.$store.dispatch("getStocks");

        },
        //},
        components: {
            //productrange,
            //ProductRange,
           // DocumentIn,
            //ProductSets,
            //Units,
            //Agents,
            QTabs,
            QTab,
            QTabPane, QIcon,
            QRouteTab, QSelect, QBtn, QPopover, QList, QItem, QItemSide, QItemMain

        }
    }
</script>
<style>
    /*#container {
        visibility: hidden;
    }*/
    .comname .q-input-target {
        white-space: nowrap
    }

    .comname.stock {
        padding-left: 20px
    }

        .comname.stock, .comname.stock::after {
            background-color: #3f98a6 !important
        }

            .comname.stock::after {
                border-top: solid 20px #3f98a6 !important;
                border-right: solid 20px #3f98a6 !important;
            }

            .comname.stock .q-if {
                margin-top: 0 !important;
                color: #3f98a6 !important
            }

                .comname.stock .q-if:before {
                    color: #3f98a6 !important
                }

    .pl-40 {
        margin-left: 50px
    }

    .text-skypay-primary {
        color: #3f98a6 !important
    }

    #loader {
        color: #008cff;
        height: 40px;
        left: 45%;
        position: absolute;
        top: 45%;
        width: 30%;
    }

    /*.e-content .e-item {
        font-size: 12px;
        margin: 10px;
        text-align: justify;
    }*/

    .container {
        min-width: 350px;
        max-width: 500px;
        margin: 0 auto;
    }
</style>
