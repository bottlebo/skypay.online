<template>
    <div id="q-app" class="">

        <q-layout ref="layout"
                  :view="'lHh Lpr lFf'"
                  :left-breakpoint="2000"
                  :right-breakpoint="1200"
                  :reveal="true"

                  >
            <q-toolbar slot="header" class="bg-skypay-primary ptb-0">
                <q-btn flat @click="$refs.layout.toggleLeft()" class="mtb-4">
                    <q-icon name="menu" />
                </q-btn>
                <q-toolbar-title>
                    <!--Quasar Layout
                    <span slot="subtitle">Empowering your app</span>
                    <div>
                        123
                    </div>-->
                    <div style="float:left; margin-right:30px">
                        <div class="comname">
                            <!--<input type="text" tabindex="1" id="comp" />-->
                            <q-select v-model="selectedCompany" color="#3d5480"
                                      :options="companies" />
                        </div>
                    </div>
                    <div style="float:left">
                        <q-tabs color="skypay-primary" no-pane-border>
                            <q-route-tab v-for="route in mainroutes" :label="route.display"
                                         :to="route.path"
                                         exact
                                         slot="title" />
                           
                        </q-tabs>
                    </div>
                    <!--<ul class="nav navbar-nav" style="display:none">
                        <li>
                            <div class="comname">
                                <q-select v-model="selectedCompany" color="#3d5480"
                                          :options="companies" />
                            </div>
                        </li>

                        <li class="pad">
                          
                        </li>
                        
                        <li v-for="route in mainroutes">
                            <router-link :to="route.path">
                                <span :class="route.style"></span> {{ route.display }}
                            </router-link>
                        </li>
                    </ul>-->
                </q-toolbar-title>
               
                <!--<q-btn class="within-iframe-hide" flat @click="$router.replace('/showcase')" style="margin-right: 15px">
                    <q-icon name="keyboard_arrow_left" />
                    Go back
                </q-btn>-->
                <!--<q-btn flat @click="$refs.layout.toggleRight()">
                    <q-icon name="menu" />
                </q-btn>-->
            </q-toolbar>
            <q-scroll-area slot="left" style="width: 100%; height: 100%">

                <q-list-header>Left Panel</q-list-header>
                <q-item tag="label">
                    <q-item-side>
                        <q-icon name="account circle"></q-icon>
                        <!--<q-checkbox v-model="checkboxModel" />-->
                    </q-item-side>
                    <q-item-main>
                        <q-item-tile label>Учетная запись</q-item-tile>
                        <!--<q-item-tile sublabel>Notify me about updates to apps or games that I downloaded</q-item-tile>-->
                        <!--<q-item>-->
                            <q-side-link item to="/showcase/layout/play-with-layout">
                                <!--<q-item-side icon="account circle" />-->
                                <q-item-main label="Управление" sublabel="" />
                                <!--<q-item-side right icon="thumb_up" />-->
                            </q-side-link>
                        <!--</q-item>-->
                    </q-item-main>
                </q-item>
                <q-side-link item to="/showcase/layout/play-with-layout">
                    <q-item-side icon="account circle" />
                    <q-item-main label="Учетная запись" sublabel="" />
                    <!--<q-item-side right icon="thumb_up" />-->
                </q-side-link>
                <q-side-link item to="/showcase/layout/drawer-panels">
                    <q-item-side icon="view_array" />
                    <q-item-main label="Drawer Panels" sublabel="Layout left/right sides" />
                </q-side-link>
                <q-side-link item to="/showcase/layout/fixed-positioning">
                    <q-item-side icon="pin_drop" />
                    <q-item-main label="Fixed Positioning" sublabel="...on a Layout" />
                </q-side-link>
                <q-side-link item to="/showcase/layout/floating-action-button">
                    <q-item-side icon="play_for_work" />
                    <q-item-main label="Floating Action Button" sublabel="For Page actions" />
                </q-side-link>

                <!--<div v-if="layoutStore.leftScroll" style="padding: 25px 16px 16px;">
                    <p class="caption" v-for="n in 50">
                        <em>Left Panel has intended scroll</em>
                    </p>
                </div>-->
            </q-scroll-area>
            <transition name="fade"  mode="out-in">
                <router-view></router-view>
                </transition>
</q-layout>




            
</div>
</template>
<script>
    import Vue from 'vue'
    import { routes } from '../routes'
    import {
        QLayout,
        QToolbar,
        QToolbarTitle,
        QSearch,
        QTabs,
        QRouteTab,
        QBtn,
        QIcon,
        QItem,
        QItemSide,
        QItemMain,
        QSideLink,
        QListHeader,
        QScrollArea,
        QSelect,
        QTransition
    } from 'quasar-framework'


    import CounterExample from './counter-example'
    import FetchData from './fetch-data'
    import HomePage from './home-page'
    //import NavMenu from './nav-menu'
    import Directory from './directory'
    import ProductList from './directory-components/product-list'
    import ProductSetList from './directory-components/product-set-list'
    import DocumentsIn from './stock-components/documents-in'
    import DocumentsOut from './stock-components/documents-out'

    import CategoryList from './directory-components/category-list'
    import TreeView from './UI/tree-view'
    Vue.component('tree-view', TreeView)
    import { mapActions, mapGetters } from 'vuex'
    Vue.component('counter-example', CounterExample);
    Vue.component('fetch-data', FetchData);
    Vue.component('home-page', HomePage);
    Vue.component('directory', Directory);
    Vue.component('product-list', ProductList)
    Vue.component('product-set-list', ProductSetList)
    Vue.component('documents-in', DocumentsIn)
    Vue.component('documents-out', DocumentsOut)

    Vue.component('category-list', CategoryList)

    export default {
        data() {
            return {
                openClass: "", routes
            }
        },
        methods: {
            ...mapActions({ selectCompany: 'selectCompany' })
            //...mapActions['getCompanies'],
            //loadCompanies: function () {
            //    //this.setCounter({ counter: 0 })
            //    console.log(this.$store)
                

            //    return '***'//this.closeMenu()
            //}
           
        },
        computed: {
            ...mapGetters({
                companies: "companies",
                selectedCompanyId: "selectedCompanyId"
            }),
            mainroutes: function() {
               
                    return routes.filter(r => r.slot == 'tollbar')
                
            },
            selectedCompany: {
                get() {
                    return this.selectedCompanyId
                },
                set(val) {
                    this.selectCompany(val)
                }
            },
            menuIsOpen: {
                get() {
                    return this.$store.state.sidemenu.open;
                },
                set(newValue) {
                    this.$store.commit("SIDEMENU_TOGGLE", newValue);
                }
            },
            appClass: {
                get() {
                    return this.$store.state.sidemenu.open ? "toggled" : ""
                }
            }
        },
        async created() {
            await this.$store.dispatch("getCompanies");
            this.selectedCompany = this.companies[0].value
            await this.$store.dispatch("getStocks");

        },
         components: {
            QLayout,
            QToolbar,
            QToolbarTitle,
            QSearch,
            QTabs,
            QRouteTab,
            QBtn,
            QIcon,
            QItem,
            QItemSide,
            QItemMain,
            QSideLink,
            QListHeader,
            QScrollArea,
            QSelect,
            QTransition
        },
    }
</script>
<style>
    .bg-skypay-primary {
        background-color: #3f98a6!important
    }
    .text-skypay-primary {
        color: #3f98a6 !important
    }
    .ptb-0{
        padding-top:0!important;
        padding-bottom:0!important
    }
    .mtb-4 {
        margin-top: 4px !important;
        margin-bottom: 4px !important
    }
    .comname {
        height: 50px;
        width: 200px;
        background-color: #3d5480;
        color: #3d5480;
        font-weight: 500;
        text-align: center;
        padding-top: 12px;
        position: relative;
        margin-right: 30px;
        cursor: pointer;
        padding-left: 35px;
    }
        .comname .q-if {
            margin-top: 0 !important;
            color: #3d5480 !important
        }
            .comname .q-if:before {
                color: #3d5480 !important
            }

            .comname .q-input-target {
                color: white !important
            }

        .comname::after {
            content: "";
            height: 34px;
            width: 36px;
            position: absolute;
            background-color: #3d5480;
            right: -18px;
            transform: rotate(45deg);
            top: 8px;
            border-top: solid 20px #3d5480;
            border-right: solid 20px #3d5480;
        }
    .fade-enter-active, .fade-leave-active {
        transition: opacity .5s;
    }

    .fade-enter, .fade-leave-to /* .fade-leave-active до версии 2.1.8 */ {
        opacity: 0;
    }
</style>
