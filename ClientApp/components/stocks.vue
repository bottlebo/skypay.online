<template>
    <div style=" background-color:aliceblue">
        <div class="row" style="padding:8px 0 0 4px;">
            <div class="" style="position:relative;width:100%">
                <div class="comname stock" style="position:absolute; top:-4px; z-index:1000">
                    <q-select v-model="selectedStock" color="#3d5480"
                              :options="stocks" />
                </div>
                <div class="" style="position:absolute; right:10px; top:0px; z-index:1000">
                    <q-btn ref="target" flat round small color="skypay-primary" icon="more_vert">
                        <q-popover ref="popover">
                            <q-list separator link>
                                <q-item @click=" $refs.popover.close()">
                                    <q-item-side icon="add" color="skypay-primary" />
                                    <q-item-main label="Добавить" />
                                </q-item>
                                <q-item @click=" $refs.popover.close()">
                                    <q-item-side color="skypay-primary">
                                        <q-icon color="skypay-primary" name="edit" size="20px"></q-icon>
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
                    <q-tab-pane name="tab-1"><documents-in></documents-in></q-tab-pane>
                    <q-tab-pane name="tab-2"><documents-out></documents-out></q-tab-pane>
                </q-tabs>
            </div>
        </div>
    </div>
</template>
<script>
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
        methods: {
            ...mapActions({ selectStock: 'selectStock' })
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
        },
        watch: {
            currentCompanyId: async function () {
                await this.$store.dispatch("reloadStocks");
            }
        },
        mounted() {

        },
        async created() {

        },
        components: {
            QTabs,
            QTab,
            QTabPane, QIcon,
            QRouteTab, QSelect, QBtn, QPopover, QList, QItem, QItemSide, QItemMain

        }
    }
</script>
<style>
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

    .container {
        min-width: 350px;
        max-width: 500px;
        margin: 0 auto;
    }
</style>
