<template>
    <div>
        <div class="container-12">

            <div v-show="menuIsOpen" @click="menuIsOpen=false" class="overlay"></div>

            <div class="container-fluid-1">

                <nav class="navbar navbar-skypay navbar-fixed-top navbar-main1">
                    <!--<header>-->
                    <div class="navbar-header">
                        <button type="button" @click="toggle" class="navbar-toggle hamburger is-closed" data-toggle="offcanvas" style="display:block">
                            <!--<span class="sr-only">Toggle navigation</span>-->
                            <span class="icon-bar"></span>
                            <span class="icon-bar middl"></span>
                            <span class="icon-bar"></span>
                        </button>

                    </div>

                    <div class="navbar-collapse collapse">

                        <ul class="nav navbar-nav">
                            <li>
                                <div class="comname">
                                    <!--<input type="text" tabindex="1" id="comp" />-->
                                    <q-select v-model="selectedCompany" color="#3d5480"
                                              :options="companies" />
                                </div>
                            </li>

                            <li class="pad">
                                <!--<select id="comp">
                                    <option v-for="c in companies" v-bind='{value:c.id}'>{{c.Name}}</option>
                                </select>-->
                                <!--<input type="text" tabindex="1" id="comp" />-->
                            </li>
                            <!--<li>
                                <button id="companyselect">Increment</button>
                            </li>-->
                            <li v-for="route in routes">
                                <!-- TODO: highlight active link -->
                                <router-link :to="route.path">
                                    <span :class="route.style"></span> {{ route.display }}
                                </router-link>
                            </li>
                        </ul>
                        <ul class="nav navbar-nav navbar-right">
                            <li>
                                <a href="~/">
                                    <img src="../css/images/grey-logo.png" />
                                </a>
                            </li>
                        </ul>
                    </div>

                </nav>
            </div>

        </div>


    </div>
</template>

<script>
    import { routes } from '../routes'
    import {
        QSelect,
        QField
    } from 'quasar-framework'
    
    import { mapActions, mapGetters } from 'vuex'
    export default {
        components: {
            QSelect,
            QField
        },
        data() {
            return {
                routes,
                collapsed: true

            }
        },
        methods: {
            ...mapActions({ selectCompany: 'selectCompany' }),
            toggleCollapsed: function (event) {
                this.collapsed = !this.collapsed;
            },
            toggle: function () {
                this.menuIsOpen = !this.menuIsOpen;
            }
        },
        computed: {
            ...mapGetters({
                companies: "companies",
                selectedCompanyId: "selectedCompanyId"
            }),
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
            }

        },
        
        //},
        mounted() {
            
        },
        async created() {
            await this.$store.dispatch("getCompanies");
            console.log(this.companies.length)
            this.selectedCompany = this.companies[0].value
        }
    }
</script>

<style>
    /*.comname {
        height: 43px;
        width: 200px;
        background-color: #3d5480;
        color: #3d5480;
        font-weight: 500;
        text-align: center;
        padding-top: 10px;
        position: relative;
        margin-right: 30px;
        cursor: pointer;
        padding-left: 25px;
    }

        .comname .q-if {
            margin-top: 0 !important;
            color: #3d5480 !important
        }

        .comname .q-input-target {
            color: white !important
        }

        .comname::after {
            content: "";
            height: 17px;
            width: 17px;
            position: absolute;
            background-color: #3d5480;
            right: -20px;
            transform: rotate(45deg);
            top: 8px;
            border-top: solid 20px #3d5480;
            border-right: solid 20px #3d5480;
        }

        .comname > span .e-search-icon {
            display: none !important;
        }

        .comname > span {
            border: none !important;
            margin-top: -10px
        }

            .comname > span::after, .comname > span:before {
                display: none !important
            }*/

    .pad {
        padding-top: 5px;
        padding-bottom: 5px;
        margin-right: 80px;
        margin-left: 30px;
    }

    .e-popup {
        z-index: 1040 !important
    }

    .compSelect .e-input, .compSelect .e-input-group-icon {
        color: white !important;
        font-weight: 500 !important;
    }

        .compSelect .e-input::-webkit-input-placeholder {
            color: #ccc !important
        }

    .compSelect {
        border-bottom-color: #ccc !important
    }

    .slide-enter-active, .slide-leave-active {
        transition: max-height .35s
    }

    .slide-enter, .slide-leave-to {
        max-height: 0px;
    }

    .slide-enter-to, .slide-leave {
        max-height: 20em;
    }
</style>
