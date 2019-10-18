<template>
    <div style="padding:20px; position:relative">
        <div style="padding-bottom:10px">
            <q-btn color="skypay-primary" @click="addProduct"  v-ripple>
                Добавить
            </q-btn>
        </div>
        <div class="relative-position">
            <q-data-table :data="products"
                          :config="config"
                          :columns="columns" class="tav">
                <!--@refresh="refresh"
                @selection="selection"
                @rowclick="rowClick"-->
                <template slot="col-n" slot-scope="cell">
                    <q-icon class="dragabble" v-bind:data-n="cell.data" color="skypay-primary" name="arrow_back" size="18px" />
                </template>
                    <template slot="col-id" slot-scope="cell">

                        <q-btn color="skypay-primary" flat small round @click="editProduct(cell.data)">
                            <q-icon name="edit" size="18px" />
                        </q-btn>
                        <q-btn color="skypay-primary" round small flat @click="confirmDeleteProduct(cell.data)">
                            <q-icon name="delete" size="18px" />
                        </q-btn>
                        <!--<div class="relative-position"  style="overflow:visible">
            <q-fab color="skypay-primary" small class="absolute-center" style="z-index:1000" direction="left" size="12px" flat icon="keyboard_arrow_up">
                <q-fab-action color="skypay-primary"
                               :offset="[20, 20]"
                              @click="deleteProduct(cell.data)"
                              icon="delete" />
                <q-fab-action color="skypay-primary"
                               :offset="[20, 20]"
                              @click="editProduct(cell.data)"
                              icon="edit" />

            </q-fab>
                </div>-->
                        <!--<q-btn flat color="primary" @click="deleteRow(props)">
                <q-icon name="delete" />
            </q-btn>-->
                    </template>
</q-data-table>
            <q-inner-loading :visible="visible">
                <q-spinner-mat size="50px" color="primary"></q-spinner-mat>
            </q-inner-loading>
        </div>

        <q-modal v-model="addedit">
            <div style="min-height:300px; min-width:600px; padding:20px">
                <p class="bold caption text-skypay-primary">
                    Изменение товара
                </p>
                <div v-show="mvisible">
                    <div style="min-height:200px;min-width:600px; padding:10px 0;">
                        <q-tabs inverted align="left">

                            <q-tab color="skypay-primary" default slot="title" name="tab-1" label="Товар" />
                            <q-tab color="skypay-primary" slot="title" name="tab-2" label="Характеристики" />
                            <q-tab color="skypay-primary" slot="title" name="tab-3" label="Модификация" />

                            <!--<q-route-tab v-for="route in dirroutes" :label="route.display"
                            :to="route.path"
                            exact
                            slot="title"
                             color="skypay-primary" />-->
                            <q-tab-pane name="tab-1">
                                <div class="row pad">
                                    <q-field class="col-6">
                                        <q-input v-model="product.name" @blur="$v.product.name.$touch" :error="$v.product.name.$error" float-label="Наименование" color="skypay-primary" />
                                    </q-field>
                                    <div class="col-6">
                                        <q-field class="">
                                            <q-input v-model="product.barCode" @blur="$v.product.barCode.$touch" :error="$v.product.barCode.$error" float-label="Штрихкод" color="skypay-primary" />
                                        </q-field>
                                    </div>
                                    <div class="col-6">
                                        <q-field class="">
                                            <q-input v-model="product.vendorCode" float-label="Артикул" color="skypay-primary" />
                                        </q-field>
                                    </div>
                                    <div class="col-6">
                                        <q-select v-model="product.unitId" @blur="$v.product.unitId.$touch" :error="$v.product.unitId.$error" color="skypay-primary" float-label="Еденица измерения"
                                                  :options="units" />

                                    </div>
                                    <div class="col-6">
                                        <q-checkbox v-model="product.weighing" left-label label="Весовой" />
                                    </div>
                                </div>
                            </q-tab-pane>
                            <q-tab-pane name="tab-2">
                                <q-card>
                                    <q-card-main>
                                        <div class="row ">
                                            <div v-for="(model,index) in cprops" :key="index" class="col-6">
                                                <q-field class="" v-if="model.type==1">
                                                    <q-input v-bind:float-label="model.name" v-model="model.value" color="skypay-primary" />
                                                </q-field>
                                                <q-checkbox v-if="model.type==2" v-model="model.value" true-value="true"
                                                            false-value="false" color="skypay-primary" left-label v-bind:label="model.name" />

                                            </div>
                                        </div>
                                    </q-card-main>
                                        </q-card>
                                            
</q-tab-pane>
                            <q-tab-pane name="tab-3">
                                <div class="row">
                                    <div class="col" style="margin-right:15px">
                                        <q-field>

                                            <q-input v-model="addPropName" @blur="$v.addPropName.$touch" :error="$v.addPropName.$error" float-label="Добавить свойство" color="skypay-primary" />
                                        </q-field>
                                    </div>
                                    <div class="col" style="margin-right:15px; max-width:30%">
                                        <q-select v-model="addPropType" color="skypay-primary" float-label="Тип"
                                                  @blur="$v.addPropType.$touch" :error="$v.addPropType.$error"
                                                  :options="types" />
                                    </div>
                                    <div class="col" style="max-width:10%">
                                        <q-btn color="skypay-primary"   small round @click="addProp">
                                            <q-icon name="add" size="18px" />
                                        </q-btn>
                                    </div>
                                </div>
                                <div class="row pad">
                                    <div v-for="(model,index) in pprops" :key="index" class="col-6">
                                        <q-card>
                                            <q-card-main>
                                                <div class="row">
                                                    <div class="col-10">
                                                        <q-field class="" v-if="model.type==1">
                                                            <q-input v-bind:float-label="model.name" v-model="model.value" color="skypay-primary" />
                                                        </q-field>
                                                        <q-checkbox v-if="model.type==2" color="skypay-primary" v-model="model.value" true-value="true"
                                                                    false-value="false" left-label v-bind:label="model.name" />
                                                    </div>
                                                    <div class="col-2">
                                                        <q-btn color="skypay-primary" flat small round @click="deleteProp(model.id)">
                                                            <q-icon name="delete" size="18px" />
                                                        </q-btn>
                                                    </div>
                                                </div>
                                            </q-card-main>
                                            </q-card>
</div>
                                </div>
                            </q-tab-pane>

                        </q-tabs>
                    </div>
                    <div>
                        <q-btn color="skypay-secondary" @click="saveProduct" label="Close">
                            Ok
                        </q-btn>
                        <q-btn color="skypay-secondary" @click="closeModal" flat label="Close">
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
        //QField,
        //QInput,
        //QCheckbox,
        QSelect, QCheckbox,
        //QSlider,
        QInnerLoading,
        QSpinnerGears,
        QSpinnerMat,
        QBtn,
        QIcon, QModal, Dialog, Toast, QCard, QCardTitle, QCardMain
        //QTooltip,
        //QCollapsible,
        //clone
    } from 'quasar-framework'
    export default {
        name: "product-list",
        data() {
            return {
                //productgrid: null,
                mvisible: false, spvisible: false,
                product: { id: 0, name: '', barCode: '', vendorCode: '', weighing: false, categoryId: null, unitId: null },
                visible: false, addedit: false, action: '', cprops: [], pprops: [], addPropName: '', addPropType: null,
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
                    //pagination: {
                    //    rowsPerPage: 15,
                    //    options: [5, 10, 15, 30, 50, 500]
                    //},
                    //selection: 'none'
                },
                columns: [
                    //{
                    //    label: 'id',
                    //    field: 'id',
                    //    width: '70px',
                    //    //classes: 'bg-orange-2',
                    //    //filter: true,
                    //    //sort(a, b) {
                    //    //    return (new Date(a)) - (new Date(b))
                    //    //},
                    //    //format(value) {
                    //    //    return new Date(value).toLocaleString()
                    //    //}
                    //},
                    {
                        label: '',
                        field: 'n',
                        width: '5%'
                        //classes: 'col-1'
                    },
                    {
                        label: 'Наименование',
                        field: 'name',
                        //format(value) {
                        //    if (value === 'Informational') {
                        //        return '<i class="material-icons text-positive" style="font-size: 22px">info</i>'
                        //    }
                        //    return value
                        //},
                        //classes: 'dragabble',
                        width: '33%'
                        //classes: 'col-7'
                    },
                    {
                        label: 'Штрихкод',
                        field: 'barCode',
                        width: '22%',
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
                        width: '18%'
                        //classes: 'col-2'
                    },
                    {
                        label: '',
                        field: 'id',
                        width: '15%'
                        //classes: 'col-1'
                    }
                ],
                pagination: true,
                rowHeight: 50,
                bodyHeightProp: 'maxHeight',
                bodyHeight: 500,
                types: [{ label: 'Текст', value: 1 }, { label: 'Птичка', value: 2 }/*, { label: 'Число', value: 3 }*/]

            }
        },
        computed: {
            ...mapGetters({ selectedCategoryId: "selectedCategoryId", selectedProductId: "selectedProductId", products: "products" }),
            currentCategoryId: {
                get() {
                    return this.selectedCategoryId ? this.selectedCategoryId : ""
                }
            },
            currentProdictId: {
                get() {
                    return this.selectedProductId ? this.selectedProductId : 0
                }
            },
            units: {
                get() {
                    //console.log(api.units.get())
                    return api.units.get()
                }
            }
        },
        validations: {
            product: {
                name: { required },
                barCode: { required },
                unitId: { required },

            },
            addPropName: { required },
            addPropType: { required }
        },
        watch: {
            currentCategoryId: async function () {
                this.visible = true;
                await this.$store.dispatch("reloadProducts");
                this.visible = false
                //this.productgrid.dataSource = this.products
                //this.productgrid.dataBind()
                //this.productgrid.refresh()

            }
        },
        methods: {
            ...mapActions({ selectProduct: 'selectProduct' }),
            getSelectedProduct: function () {
                return this.products.filter(c => c.id == this.selectedProductId)[0];
            },
            addProp: function () {
                this.$v.addPropName.$touch()
                this.$v.addPropType.$touch()
                if (!this.$v.addPropType.$error && !this.$v.addPropType.$error) {
                    let p = { id: 0, name: this.addPropName, type: this.addPropType, productId: this.currentProdictId, value: '' }
                    try {
                        //let id = await api.addCategoryProperty(p)
                        //p.id = id
                        this.pprops.push(p)
                        //for (let i in this.props) {
                        //    console.log(this.props[i].name + ';' + this.props[i].Type)
                        //}
                        this.addPropName = ""
                        this.addPropType = 0
                    }
                    catch (error) {
                        alert(error)
                    }
                }
                else {
                    Toast.create.warning('Введите название свойства и его тип')
                }
            },
            deleteProp: function (id) {
                let del = this.pprops.filter(p => p.id == id)[0];
                let index = this.pprops.indexOf(del);
                this.pprops.splice(index, 1)

            },
            mapBool: function (arr) {
                return arr.map((el) => {
                    if (el.type == 2) { el.value = el.value == 'true' }; return el;
                })
            },
            editProduct: async function (id) {
                //console.log(id)
                this.$v.product.$reset()
                this.$v.addPropName.$reset()
                this.$v.addPropType.$reset()

                this.mvisible = false;
                this.spvisible = true;
                this.addedit = true

                this.selectProduct(id)
                this.action = 'edit'
                let props = await api.products.getProductCategoryProps(id)
                this.cprops = this.mapBool(props.categoryProductProps)
                this.pprops = this.mapBool(props.productProps)
                //console.log(this.cprops)
                //console.log(this.pprops)
                var selp = this.getSelectedProduct();
                var self = this
                this.product = {}
                Object.keys(selp).forEach(function (key) {
                    self.product[key] = selp[key]
                })
                //this.product = this.getSelectedProduct()
                this.mvisible = true;
                this.spvisible = false;

            },
            confirmDeleteProduct: function (id) {
                this.selectProduct(id);
                var selp = this.getSelectedProduct();
                Dialog.create({
                    title: 'Удаление продукта',
                    message: `Будет удален продукт: ${selp.name} !`,
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
                                    console.log(this.selectedProductId)
                                    await api.products.delete(this.selectedProductId)
                                    this.$store.dispatch("deleteProduct", this.selectedProductId)

                                    this.selectProduct(0);
                                    Toast.create.positive('Удален продукт: ' + selp.name)
                                }
                                catch (error) {
                                    Toast.create.warning('Ошибка: ' + error)
                                }
                            }
                        }
                    ]
                })
            },
            deleteProduct: async function (id) {

            },
            addProduct: async function () {
                this.action = 'add'
                //this.selectedProductId = 0;
                //this.props = []
                //this.editCatName = '';
                this.mvisible = true;
                this.spvisible = false
                this.product = { id: 0, name: '', vendorCode: '', barCode: '', weighing: false, unitId: null, categoryId: this.selectedCategoryId }
                let props = await /*api.products.getProductCategoryProps()*/ api.getCategoryProperties(this.selectedCategoryId)
                //let p = [];
                //for (let i in props) {
                //    let prop = props[i]
                //    p.push({ id: 0, name: prop.name, type: prop.type, value: null, productId: 0 })
                //}
                this.$v.product.$reset()
                this.$v.addPropName.$reset()
                this.$v.addPropType.$reset()
                this.cprops = this.mapBool(props)
                this.pprops = []
                this.addedit = true

            },
            saveProduct: async function () {
                this.$v.product.$touch()
                if (!this.$v.product.$error) {
                    this.spvisible = true;
                    if (this.action == 'add') {
                        try {
                            var productext = {};
                            productext.Product = this.product
                            productext.ProductProps = this.pprops;
                            productext.CategoryProductProps = this.cprops
                            console.log(productext)
                            let id = await api.products.add(productext)
                            this.product.id = id

                            this.$store.dispatch("addProduct", this.product);
                            Toast.create.positive('Продукт добавлен')
                            //this.selectCategory(0)
                            //this.selectCategory(this.selectedCategoryId)
                        }
                        catch (error) {
                            Toast.create.warning('Ошибка: ' + error)
                        }
                    }
                    else if (this.action == 'edit') {
                        try {
                            //console.log(this.product.id)
                            let productext = {};
                            productext.Product = this.product
                            productext.ProductProps = this.pprops;
                            productext.CategoryProductProps = this.cprops
                            //console.log(productext)
                            await api.products.update(productext)
                            //let p = {};
                            //var self = this
                            //Object.keys(this.product).forEach(function (key) {
                            // p['name'] = self.product.name;//[key]
                            //})
                            this.$store.dispatch("editProduct", this.product);
                            Toast.create.positive('Продукт изменен')
                            //console.log(p)
                        }
                        catch (error) {
                            Toast.create.warning('Ошибка: ' + error)
                        }
                    }
                    this.spvisible = false
                    this.addedit = false;

                }
                else {
                    Toast.create.warning('Введите наименование, штрихкод и ед. измерения продукта')
                }
            },
            closeModal: function () {
                this.addedit = false;
                //this.editCatName = '';
            },
            handleDragStart(e) {
                e.dataTransfer.effectAllowed = 'copy';
                e.dataTransfer.setData('productId', e.target.getAttribute('data-n'));
                //e.target.style.opacity = '0.4';  // this / e.target is the source node.
            }
        },
        mounted() {
            let cols = document.querySelectorAll('.dragabble');
            let self = this;
            [].forEach.call(cols, function (col) {
                col.setAttribute('draggable','true')
                col.addEventListener('dragstart', self.handleDragStart, false);
            });
            //this.productgrid = new Grid({
            //    dataSource: this.products,
            //    toolbar: ['add', 'edit', 'delete', 'print', 'excelexport', 'search'],
            //    editSettings: { allowEditing: true, allowAdding: true, allowDeleting: true },
            //    columns: [
            //        { field: 'id', headerText: 'ID', textAlign: 'left', width: 40, type: 'number' },
            //        { field: 'Name', width: 140, headerText: 'Name', type: 'string' },
            //        { headerText: 'Manage Records', width: 160, commands: [{ type: 'edit', buttonOption: { iconCss: ' e-icons e-edit', cssClass: 'e-flat' } }, { type: 'delete', buttonOption: { iconCss: 'e-icons e-delete', cssClass: 'e-flat' } }, { type: 'save', buttonOption: { iconCss: 'e-icons e-update', cssClass: 'e-flat' } }, { type: 'cancel', buttonOption: { iconCss: 'e-icons e-cancel-icon', cssClass: 'e-flat' } }] }
            //        //{ field: 'Freight', headerText: 'Freight', textAlign: 'right', width: 120, format: 'C' },
            //        //{ field: 'OrderDate', headerText: 'Order Date', width: 140, format: 'yMd' }
            //    ],
            //    height: 315
            //});
            //this.productgrid.appendTo('#productgrid')
        },
        async created() {
            await this.$store.dispatch("getProducts");
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
            QTabs, QTab, QTabPane, QField, QInput, QFab, QFabAction, Dialog, Toast, QCard, QCardTitle, QCardMain
            //QTooltip,
            //QCollapsible
        },
    }
</script>
<style>
    .pad > div {
        padding: 5px 15px 5px 0px;
    }
    [draggable] {
        -moz-user-select: none;
        -khtml-user-select: none;
        -webkit-user-select: none;
        user-select: none;
        cursor: move
    }
    .dragabble{ user-select:none; cursor:move}
    .vis {
        overflow: visible !important
    }

    .tav table > tbody > tr td:last-child {
        overflow: visible !important
    }
</style>
