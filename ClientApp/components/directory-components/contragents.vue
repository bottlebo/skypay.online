<template>
    <div style="padding:20px; position:relative">
        <div class="relative-position">
            <div class="row" style="padding-bottom:10px">
                <div class="col-2">
                    <q-btn color="skypay-primary" @click="addAgent" v-ripple label="Add">
                        Добавить
                    </q-btn>
                </div>
                <div class="col-3">
                    <q-select v-model="viewType" stack-label="Показывать:"
                              color="skypay-primary"
                              @change="changeViewType"
                              :options="viewTypes" />
                </div>
                <div class="col"></div>
                <div class="col-5" style="padding-top:11px">
                    <q-search color="skypay-primary"
                              @keyup="keyUpSearch"
                              @change="changeSearch"
                              placeholder="Поиск"
                              v-model="agentsearch" />
                </div>
            </div>
            <q-data-table :data="agents"
                          :config="config"
                          :columns="columns" class="tav">

                <template slot="col-id" slot-scope="cell">
                    <q-btn color="skypay-primary" flat small round @click="editAgent(cell.data)">
                        <q-icon name="edit" size="18px" />
                    </q-btn>
                    <q-btn color="skypay-primary" round small flat @click="confirmDeleteAgent(cell.data)">
                        <q-icon name="delete" size="18px" />
                    </q-btn>

                </template>
            </q-data-table>
            <q-inner-loading :visible="tvisible">
                <q-spinner-mat size="50px" color="skypay-primary"></q-spinner-mat>
            </q-inner-loading>

        </div>
        <q-modal v-model="addedit">
            <div style="min-height:300px; min-width:95vw; padding:20px;" class="relative-position">
                <p v-if="action=='edit'" class="bold caption text-skypay-primary">
                    Изменение контрагента
                </p>
                <div v-if="action=='add'" style="width:40%" class="hfield">
                    <q-field label="Добавить" color="skypay-primary" :labelWidth="3">
                        <q-select v-model="addType"
                                  color="skypay-primary"
                                  @change="changeViewType"
                                  :options="addTypes" />
                    </q-field>
                </div>

                <div v-show="visible">
                    <div style="min-height:200px;min-width:600px; padding:10px 0 20px; ">
                        <div class="row">
                            <div class="col-5 tl" style="">
                                <q-field>
                                    <q-input v-model="agent.name" @blur="$v.agent.name.$touch" :error="$v.agent.name.$error" float-label="Наименование" color="skypay-primary" />
                                </q-field>
                                <q-select v-model="agent.sob" stack-label="Форма собственности"
                                          color="skypay-primary"
                                          :options="sobVals" />
                                <q-select v-model="agent.nds" stack-label="НДС"
                                          color="skypay-primary"
                                          :options="ndsVals" />
                                <q-field>
                                    <q-input v-model="agent.contact" float-label="Контакт" color="skypay-primary" />
                                </q-field>
                                <q-field>
                                    <q-input v-model="agent.phone" float-label="Телефон" color="skypay-primary" />
                                </q-field>
                                <q-field>
                                    <q-input v-model="agent.email" float-label="e-mail" color="skypay-primary" />
                                </q-field>

                            </div>
                            <div class="col-1"></div>
                            <div class="col-5 tl">
                                <q-field>
                                    <q-input v-model="agent.address" float-label="Юридический адрес" color="skypay-primary" />
                                </q-field>
                                <q-field>
                                    <q-input v-model="agent.address1" float-label="Фактический адрес" color="skypay-primary" />
                                </q-field>
                                <q-field>
                                    <q-input v-model="agent.inn" float-label="ИНН" color="skypay-primary" />
                                </q-field>
                                <q-field>
                                    <q-input v-model="agent.kpp" float-label="КПП" color="skypay-primary" />
                                </q-field>
                                <q-field>
                                    <q-input v-model="agent.ogrn" float-label="ОГРН" color="skypay-primary" />
                                </q-field>
                                <q-field>
                                    <q-datetime :month-names="month_names" :day-names="day_names" format="DD-MM-YYYY" ok-label="Ok" no-clear cancel-label="Отмена" v-model="agent.ogrnDate" type="date" float-label="Дата выдачи ОГРН" color="skypay-primary" />
                                </q-field>
                                <q-field>
                                    <q-input v-model="agent.web" float-label="Вебсайт" color="skypay-primary" />
                                </q-field>
                            </div>
                        </div>
                    </div>

                    <div>
                        <q-btn color="skypay-secondary" @click="saveAgent" label="Close">
                            Ok
                        </q-btn>
                        <q-btn color="skypay-secondary" flat @click="closeModal" label="Close">
                            Отмена
                        </q-btn>
                    </div>
                </div>
                <q-inner-loading :visible="spvisible">
                    <q-spinner-mat size="50px" color="skypay-primary"></q-spinner-mat>
                </q-inner-loading>
            </div>

        </q-modal>


    </div>
</template>
<script>
    import { mapActions, mapGetters } from 'vuex'
    import api from '../../store/api'
    import { datetime } from '../../constants'

    import { required } from 'vuelidate/lib/validators'
    import {
        QDataTable,
        QTabs, QTab, QTabPane, QField, QInput, QFab, QFabAction,
        QSelect, QCheckbox,
        QInnerLoading,
        QSpinnerGears,
        QSpinnerMat,
        QBtn,
        QIcon, QModal, Dialog, Toast, QSearch, QAutocomplete, QDatetime
    } from 'quasar-framework'
    export default {
        name: "product-sets",
        data() {
            return {
                month_names: datetime.month_names,
                day_names: datetime.day_names,
                action: '',
                viewType: 0,
                addType: 1,
                agentsearch: '',
                visible: true,
                spvisible: false,
                tvisible: false,
                config: {
                    title: '',
                    refresh: false,
                    noHeader: false,
                    columnPicker: false,
                    leftStickyColumns: 0,
                    rightStickyColumns: 0,
                    bodyStyle: {
                        maxHeight: '500px'
                    },
                    rowHeight: '50px',
                    responsive: true,
                    pagination: {
                        rowsPerPage: 5,
                    },
                    labels: {
                        rows: 'Строк',
                        columns: 'Coluuuuumns'
                    }
                },
                columns: [
                    {
                        label: 'Наименование',
                        field: 'name',

                        width: '25%',
                    },
                    {
                        label: 'ИНН',
                        field: 'inn',

                        width: '15%'
                    },
                    {
                        label: 'Контакт',
                        field: 'contact',

                        width: '15%'
                    },
                    {
                        label: 'Телефон',
                        field: 'phone',

                        width: '15%'
                    },
                    {
                        label: 'E-mail',
                        field: 'email',

                        width: '15%'
                    },
                    {
                        label: '',
                        field: 'id',
                        width: '15%'
                    }
                ],
                viewTypes: [{ label: 'Всех', value: 0, id: 0 }, { label: 'Покупателей', value: 'Bbuyer', id: 1 }, { label: 'Поставщиков', value: 'Shipper', id: 2 }],
                addedit: false,
                addTypes: [{ label: 'Покупателя', value: 1 }, { label: 'Поставщика', value: 2 }],

                agents: [],
                agent: {
                    id: 0,
                    name: '',
                    inn: '',
                    kpp: '',
                    ogrn: '', ogrnDate: new Date(), sob: 0, address1: '', address: '', contact: '',
                    email: '', web: '', phone: '', nds: 0
                },
                ndsVals: [{ label: 'Нет', value: 0 }, { label: '10%', value: 10 }, { label: '18%', value: 18 }],
                sobVals: [{ label: 'ООО', value: 0 }, { label: 'ОАО', value: 1 }, { label: 'ИП', value: 2 }, { label: 'ЗАО', value: 3 }, { label: 'ГБОУ', value: 4 }, { label: 'ГБОУ ДОД', value: 5 }]
            }
        },
        computed: {
            ...mapGetters({ selectedCompanyId: "selectedCompanyId" })
        },
        validations: {
            agent: {
                name: { required }
            }
        },
        watch: {
        },
        methods: {
            keyUpSearch: function (evt) {
                console.log(this.agentsearch)
                console.log(evt.keyCode)
            },
            changeSearch: function (val) {
                if (val == '') {
                    console.log('clear')
                }
            },
            changeViewType: async function (val) {
                this.tvisible = true
                this.agents = await api.agents.get(this.selectedCompanyId, this.viewType)
                this.tvisible = false
            },
            editAgent: async function (id) {
                this.action = 'edit'
                this.addedit = true;
                this.agent = this.agents.filter(c => c.id == id)[0];
            },
            addAgent: async function () {
                this.addType = this.viewType == 0 ? 1 : this.viewTypes.find(z => z.value == this.viewType).id
                this.$v.agent.$reset()
                this.action = 'add'
                this.agent = {
                    id: 0, name: '', inn: '', kpp: '',
                    ogrn: '', ogrnDate: new Date(), sob: 0,
                    address1: '', address: '', contact: '',
                    email: '', web: '', phone: '', nds: 0,
                    CompanyId: this.selectedCompanyId
                }

                this.addedit = true;
            },
            closeModal: function () {
                this.addedit = false;
            },
            saveAgent: async function () {
                this.$v.agent.$touch()
                if (!this.$v.agent.$error) {
                    this.$v.agent.$reset()
                    this.spvisible = true;
                    switch (this.action) {
                        case 'edit':
                            try {
                                await api.agents.update(this.agent)
                                Toast.create.positive('Контрагент обновлен')
                            }
                            catch (error) {
                                Toast.create.warning('Ошибка: ' + error)
                            }
                            break
                        case 'add':
                            try {

                                this.agent.Type = this.addType
                                await api.agents.add(this.agent)
                                this.agents = await api.agents.get(this.selectedCompanyId, this.addType)
                                Toast.create.positive('Контрагент добавлен')
                                let vt = this.viewTypes.find(z => z.id == this.addType)
                                this.viewType = vt.value
                            }
                            catch (error) {
                                Toast.create.warning('Ошибка: ' + error)
                            }
                            break
                    }
                    this.spvisible = false;
                    this.addedit = false;
                }
                else {
                    Toast.create.warning('Необходимо ввести имя предприятия')
                }
            },
            confirmDeleteAgent: function (id) {
            }
        },
        async created() {
            this.agents = await api.agents.get(this.selectedCompanyId, this.viewType)
        },
        components: {
            QDataTable,
            QSelect,
            QInnerLoading,
            QSpinnerGears,
            QSpinnerMat,
            QBtn, QDatetime,
            QIcon, QModal, QCheckbox,
            QTabs, QTab, QTabPane, QField, QInput, QFab, QFabAction, Dialog, Toast, QSearch, QAutocomplete
        }
    }
</script>
<style>
    .tl > div {
        margin-bottom: 10px !important
    }

    .hfield .q-field-label-inner > span {
        color: #3f98a6
    }

    .q-pagination button {
        color: #3f98a6 !important
    }
</style>
