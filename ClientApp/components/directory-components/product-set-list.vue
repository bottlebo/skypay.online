<template>
    <div style="padding:20px; position:relative">
        <div style="padding-bottom:10px">
            <q-btn color="skypay-primary" @click="addProductSet" v-ripple>
                Добавить
            </q-btn>

        </div>
        <div class="relative-position">
            <q-data-table :data="productsets"
                          :config="config"
                          :columns="columns" class="tav">

                <template slot="col-id" slot-scope="cell">
                    <q-btn color="skypay-primary" flat small round @click="editProductSet(cell.data)">
                        <q-icon name="edit" size="18px" />
                    </q-btn>
                    <q-btn color="skypay-primary" round small flat @click="confirmDeleteProductSet(cell.data)">
                        <q-icon name="delete" size="18px" />
                    </q-btn>

                </template>
            </q-data-table>
            <q-inner-loading :visible="tvisible">
                <q-spinner-mat size="50px" color="skypay-primary"></q-spinner-mat>
            </q-inner-loading>

        </div>
        <q-modal v-model="addedit">
            <div style="min-height:300px; min-width:600px; padding:20px" class="relative-position">
                <p class="bold caption text-skypay-primary">
                    Изменение набора
                </p>
                <div v-show="visible">
                    <div style="min-height:200px;min-width:600px; padding:10px 0;">
                        <q-tabs inverted align="left">
                            <q-tab color="skypay-primary" default slot="title" name="tab-21" label="Набор" />
                            <q-tab color="skypay-primary" slot="title" name="tab-22" label="Состав" />
                            <q-tab-pane name="tab-21">
                                <div class="row pad">
                                    <q-field class="col-6">
                                        <q-input v-model="productset.name" @blur="$v.productset.name.$touch" :error="$v.productset.name.$error" float-label="Наименование" color="skypay-primary" />
                                    </q-field>
                                    <div class="col-6">
                                        <q-field class="">
                                            <q-input v-model="productset.barCode" @blur="$v.productset.barCode.$touch" :error="$v.productset.barCode.$error" float-label="Штрихкод" color="skypay-primary" />
                                        </q-field>
                                    </div>
                                    <div class="col-6">
                                        <q-field class="">
                                            <q-input v-model="productset.vendorCode" float-label="Артикул" color="skypay-primary" />
                                        </q-field>
                                    </div>

                                    <div class="col-6">
                                        <q-checkbox v-model="productset.byCash" left-label label="Касса" />
                                    </div>
                                </div>
                            </q-tab-pane>
                            <q-tab-pane name="tab-22">
                                <div class="row pad" style="padding-bottom:10px">
                                    <div class="col-6">
                                        <q-search v-model="terms" :error="$v.terms.$error" placeholder="Выбор продукта" color="skypay-primary">
                                            <q-autocomplete :filter="filterProducts" :static-data="{field: 'value', list: productList}" @selected="selectProduct" />
                                        </q-search>
                                    </div>
                                    <div class="col-4">
                                        <q-field>

                                            <q-input v-model="addQty" @blur="$v.addQty.$touch" :error="$v.addQty.$error" float-label="" placeholder="Количество" color="skypay-primary" />
                                        </q-field>
                                    </div>
                                    <div class="col-2">
                                        <q-btn color="skypay-primary" flat small round @click="addProduct">
                                            <q-icon name="add" size="18px" />
                                        </q-btn>
                                    </div>
                                </div>
                                <div class="relative-position">
                                    <q-data-table :data="setitems" style="max-width:600px"
                                                  :config="itemsconfig"
                                                  :columns="itemscolumns" class="tav">
                                        <template slot="col-Id" slot-scope="cell">
                                            <q-btn color="skypay-primary" round small flat @click="deleteItem(cell.data)">
                                                <q-icon name="delete" size="18px" />
                                            </q-btn>


                                        </template>
                                    </q-data-table>
                                    <q-inner-loading :visible="tablewait">
                                        <q-spinner-mat size="50px" color="skypay-primary"></q-spinner-mat>
                                    </q-inner-loading>
                                </div>
                            </q-tab-pane>
                        </q-tabs>
                    </div>

                    <div>
                        <q-btn color="skypay-secondary" @click="saveProductSet" label="Close">
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
    import { numeric, required } from 'vuelidate/lib/validators'
    import {
        QDataTable,
        QTabs, QTab, QTabPane, QField, QInput, QFab, QFabAction,
        QSelect, QCheckbox,
        QInnerLoading,
        QSpinnerGears,
        QSpinnerMat,
        QBtn,
        QIcon, QModal, Dialog, Toast, QSearch, QAutocomplete
    } from 'quasar-framework'
    export default {
        name: "product-sets",
        data() {
            return {
                form: { qty: '' },
                action: '',
                tablewait: false,
                visible: false,
                spvisible: false,
                tvisible: false,
                terms: '',
                addQty: null,
                selectedProduct: null,
                companyProducts: [],
                productList: [],
                itemsconfig: {
                    title: '',
                    refresh: false,
                    noHeader: false,
                    columnPicker: false,
                    leftStickyColumns: 0,
                    rightStickyColumns: 0,
                    bodyStyle: {
                        maxHeight: '500px'
                    },
                    rowHeight: '30px',
                    responsive: true,
                },
                itemscolumns: [

                    {
                        label: 'Наименование',
                        field: 'Product',

                        width: '44%',
                        format(value, row) {
                            return value.Name
                        }
                        //classes: 'col-7'
                    },

                    {
                        label: 'Артикул',
                        field: 'Product',
                        //filter: true,
                        //sort: true,
                        //type: 'string',
                        width: '20%',
                        format(value, row) {
                            return value.vendorCode
                        }
                        //classes: 'col-2'
                    },
                    {
                        label: 'Количество',
                        field: 'Qty',
                        width: '20%',
                        //sort: true,
                        //type: 'number'
                        //classes:'col-2'
                    },
                    {
                        label: '',
                        field: 'Id',
                        width: '15%'
                        //classes: 'col-1'
                    }
                ],
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
                },
                columns: [
                    {
                        label: 'Наименование',
                        field: 'name',
                        width: '35%'
                        //classes: 'col-7'
                    },
                    {
                        label: 'Штрихкод',
                        field: 'barCode',
                        width: '25%',
                        //sort: true,
                        //type: 'number'
                        //classes:'col-2'
                    },
                    {
                        label: 'Артикул',
                        field: 'vendorCode',
                        //filter: true,
                        //sort: true,
                        //type: 'string',
                        width: '20%'
                        //classes: 'col-2'
                    },
                    {
                        label: '',
                        field: 'id',
                        width: '15%'
                        //classes: 'col-1'
                    }
                ],
                types: [{ label: 'Текст', value: 1 }, { label: 'Птичка', value: 2 }, { label: 'Число', value: 3 }],
                addedit: false,
                productsets: [],
                productset: { id: 0, name: '', barCode: '', vendorCode: '', byCash: false }
                , setitems: []


            }
        },
        computed: {
            ...mapGetters({ selectedCategoryId: "selectedCategoryId", selectedCompanyId: "selectedCompanyId" })
        },
        validations: {
            productset: {
                name: { required },
                barCode: { required }
            },
            addQty: { numeric, required },
            terms: { required }
        },
        watch: {
            selectedCategoryId: async function () {
                this.tvisible = true
                this.productsets = await api.productset.get(this.selectedCategoryId)
                this.tvisible = false
            }
        },
        methods: {
            addProduct: async function () {
                this.$v.addQty.$touch()
                this.$v.terms.$touch()

                if (this.selectedProduct != null && !this.$v.addQty.$error) {
                    this.$v.addQty.$reset()
                    this.$v.terms.$reset()
                    switch (this.action) {
                        case 'edit':
                            this.tablewait = true;
                            let pr = { Id: 0, ProductId: this.selectedProduct.Id, Qty: this.addQty }
                            try {
                                let id = await api.productset.addItem(this.productset.id, pr)
                                this.setitems = await api.productset.getItems(this.productset.id)
                                this.addQty = null;
                                this.terms = ''
                                this.selectedProduct = null
                                Toast.create.positive('Продукт добавлен в набор')
                            }
                            catch (error) {
                                Toast.create.warning('Ошибка: ' + error)
                            }
                            this.tablewait = false
                            break
                        case 'add':
                            let prn = { Id: 0, ProductId: this.selectedProduct.Id, Qty: this.addQty, Product: this.selectedProduct }
                            this.setitems.push(prn)
                            this.addQty = null;
                            this.terms = ''
                            this.selectedProduct = null
                            break
                    }
                }
                else {
                    Toast.create.warning('Необходимо выбрать продукт и количество')
                }
            },
            filterProducts: function (terms, { field, list }) {
                return this.productList
                    .filter(p => p.label.toLowerCase().indexOf(terms) === 0 ||
                        p.barCode.toLowerCase().indexOf(terms) === 0 ||
                        p.vendorCode.toLowerCase().indexOf(terms) === 0

                    )
            },
            selectProduct: function (val) {
                this.selectedProduct = this.companyProducts.find(p => p.Id == val.Id)
            },
            editItem: function (id) {

            },
            deleteItem: async function (id) {
                this.tablewait = true;
                try {
                    await api.productset.deleteItem(id);
                    this.setitems = await api.productset.getItems(this.productset.id)
                    Toast.create.positive('Продукт удален')
                }
                catch (error) {
                    Toast.create.warning('Ошибка: ' + error)
                }
                this.tablewait = false;
            },
            editProductSet: async function (id) {
                this.action = 'edit'
                this.visible = false;
                this.spvisible = true;
                this.addedit = true;
                this.productset = this.productsets.filter(c => c.id == id)[0];
                this.setitems = await api.productset.getItems(this.productset.id)
                this.visible = true;
                this.spvisible = false;
            },
            addProductSet: async function () {
                this.action = 'add'
                this.visible = true;
                this.spvisible = false;
                this.productset = { id: 0, name: '', barCode: '', vendorCode: '', byCash: false, CategoryId: this.selectedCategoryId }
                this.setitems = [];
                this.addedit = true;
            },
            closeModal: function () {
                this.addedit = false;
            },
            saveProductSet: async function () {
                this.$v.productset.$touch()
                if (!this.$v.productset.$error) {
                    this.$v.productset.$reset()
                    this.spvisible = true;
                    switch (this.action) {
                        case 'edit':
                            try {
                                await api.productset.update(this.productset)
                                Toast.create.positive('Набор обновлен')
                            }
                            catch (error) {
                                Toast.create.warning('Ошибка: ' + error)
                            }
                            break
                        case 'add':
                            try {
                                this.productset.ProductSetItems = this.setitems;
                                await api.productset.add(this.productset)
                                this.productsets = await api.productset.get(this.selectedCategoryId)
                                Toast.create.positive('Набор добавлен')
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
                    Toast.create.warning('Необходимо ввести имя набора и штрихкод')
                }
            },
            confirmDeleteProductSet: function (id) {
                var productset = this.productsets.filter(c => c.id == id)[0];
                Dialog.create({
                    title: 'Удаление набора',
                    message: `Будет удален набор: ${productset.name} !`,
                    buttons: [
                        {
                            label: 'Отмена',
                            color: 'skypay-secondary'
                        },
                        {
                            label: 'Удалить',
                            color: 'skypay-secondary',
                            handler: async () => {
                                try {
                                    this.tvisible = true
                                    await api.productset.delete(id)

                                    this.productsets = await api.productset.get(this.selectedCategoryId)
                                    this.tvisible = false
                                    Toast.create.positive('Удален набор: ' + productset.name)
                                }
                                catch (error) {
                                    Toast.create.warning('Ошибка: ' + error)
                                }
                            }
                        }
                    ]
                })
            }
        },
        async created() {
            if (this.selectedCategoryId) {
                var ps = await api.productset.get(this.selectedCategoryId);
                if (ps.length > 0)
                    this.productsets = ps;
            }

            this.companyProducts = await api.products.getCompanyProducts(this.selectedCompanyId)
            this.productList = this.companyProducts.map((el) => { return { Id: el.Id, value: el.Name, label: el.Name, sublabel: 'Штрихкод:' + el.BarCode, barCode: el.BarCode, vendorCode: el.VendorCode } })
        },
        components: {
            QDataTable,
            //QField,
            //QInput,
            //QCheckbox,
            QSelect,
            //QSlider,
            QInnerLoading,
            QSpinnerGears,
            QSpinnerMat,
            QBtn,
            QIcon, QModal, QCheckbox,
            QTabs, QTab, QTabPane, QField, QInput, QFab, QFabAction, Dialog, Toast, QSearch, QAutocomplete
            //QTooltip,
            //QCollapsible
        }
    }
</script>
