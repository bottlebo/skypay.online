<template>
    <div style=" background-color:aliceblue">
        <!--<div id='loader'>LOADING....</div>-->


        <q-tabs inverted align="left" no-pane-border>

            <q-tab color="skypay-primary" class="pl-40" default slot="title" name="tab-1" label="Номенклатура" />
            <q-tab color="skypay-primary" slot="title" name="tab-2" label="Наборы" />

            <q-tab color="skypay-primary" slot="title" name="tab-3" label="Контрагенты" />
            <q-tab color="skypay-primary" slot="title" name="tab-4" label="Сотрудники" />

            <q-tab color="skypay-primary" slot="title" name="tab-5" label="Еденицы измерения" />
            <q-tab-pane name="tab-1">

                <product-range></product-range>

            </q-tab-pane>

            <q-tab-pane name="tab-2"><product-sets></product-sets></q-tab-pane>

            <q-tab-pane name="tab-3"><agents></agents></q-tab-pane>
            <q-tab-pane name="tab-4"><users></users></q-tab-pane>

            <q-tab-pane name="tab-5"><units></units></q-tab-pane>

        </q-tabs>



    </div>
</template>
<script>

    import { routes } from '../routes'
    import Units from './directory-components/units'
    import Agents from './directory-components/contragents'
    import ProductRange from './directory-components/product-range'
    import ProductSets from './directory-components/product-sets'
    import Users from './directory-components/users'

    import {
        QTabs, QTab, QTabPane, QRouteTab
    } from 'quasar-framework'
    export default {
        data() {
            return {
                currentView: 'productrange'
            }
        },
        methods: {
        },
        computed: {
            dirroutes: function () {
                return routes.filter(r => r.path == '/Manage/Directory')[0].children
            }
        },
        mounted() {

        },
        components: {
            ProductRange,
            Users,
            ProductSets,
            Units,
            Agents,
            QTabs,
            QTab,
            QTabPane,
            QRouteTab

        }
    }
</script>
<style>
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
