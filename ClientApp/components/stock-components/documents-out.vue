<template>
    <div style="padding:20px; position:relative">
        <div class="relative-position">
            <div class="row" style="padding-bottom:10px">
                <div class="col-2">
                    <q-btn color="skypay-primary" @click="addDocument" v-ripple label="Add">
                        Добавить
                    </q-btn>
                </div>
                <div class="col-3">
                </div>
                <div class="col"></div>
                <div class="col-5" style="padding-top:11px">
                </div>
            </div>
            <q-data-table :data="documents"
                          :config="config"
                          :columns="columns" class="tav">
                <template slot="col-lock" slot-scope="cell">
                    <q-btn v-if="!cell.data.locked" color="skypay-primary" flat small round @click="editDocument(cell.data.Id)">
                        <q-icon name="edit" size="18px" />
                    </q-btn>
                    <q-btn color="skypay-primary" flat small round @click="editItems(cell.data)">
                        <q-icon name="list" size="18px" />
                    </q-btn>
                    <q-btn v-if="!cell.data.locked" color="skypay-primary" round small flat @click="confirmDeleteDocument(cell.data.Id)">
                        <q-icon name="delete" size="18px" />
                    </q-btn>
                    <q-btn v-if="!cell.data.locked" color="skypay-primary" flat small round @click="lockDocument(cell.data.Id)">
                        <q-icon name="lock_open" size="18px" />
                    </q-btn>
                    <q-icon v-else name="lock" style="margin:0 11px;" color="skypay-primary" size="18px" />
                </template>
            </q-data-table>
            <q-inner-loading :visible="tvisible">
                <q-spinner-mat size="50px" color="skypay-primary"></q-spinner-mat>
            </q-inner-loading>
        </div>
        <q-modal v-model="addEditDoc">
            <div style="min-height:300px; min-width:500px; padding:20px;" class="relative-position">
                <p class="bold caption text-skypay-primary">
                    <span v-if="action=='addDoc'">Добавление</span><span v-else>Изменение</span> документа
                </p>
                <div style="min-height:200px;min-width:600px; padding:10px 0 20px; ">
                    <div class="row">
                        <div class="col-5 pad">
                            <q-select v-model="agent" float-label="Покупатель" :error="$v.agent.$error"
                                      color="skypay-primary" @change="changeAgent"
                                      :options="agents" />
                        </div>
                        <div class="col-5" style="padding:10px">
                            <q-btn color="skypay-secondary" flat @click="changePrivate" label="Close">
                                <q-icon v-if="private" name="done" />
                                <span>&nbsp;Частное лицо</span>
                            </q-btn>
                        </div>
                    </div>

                </div>
                <div>
                    <q-btn color="skypay-secondary" @click="saveDoc" label="Close">
                        Ok
                    </q-btn>
                    <q-btn color="skypay-secondary" flat @click="addEditDoc=false" label="Close">
                        Отмена
                    </q-btn>
                </div>
            </div>
        </q-modal>
        <q-modal v-model="showItems">
            <div style="min-height:300px; width:95vw; padding:20px;" class="relative-position">
                <p class="bold caption text-skypay-primary">
                    <span v-if="canEditItems">Изменение</span><span v-else>Просмотр</span> документа
                </p>

                <div v-show="mvisible">
                    <div style="min-height:200px;min-width:600px; padding:10px 0 20px; ">
                        <div v-if="canEditItems" class="row" style="padding-bottom:10px">
                            <q-btn color="skypay-primary" @click="addItem" v-ripple label="Add">
                                Добавить
                            </q-btn>
                        </div>
                        <q-data-table :data="items"
                                      :config="item_config"
                                      :columns="item_columns" class="tav">

                            <template slot="col-Id" slot-scope="cell">

                                <div class="tabcrud" style="padding-top:10px">
                                    <q-fab v-if="canEditItems" color="skypay-primary" flat small class="pull-right" style="margin-top:-10px!important"
                                           icon="more_horiz"
                                           direction="left">
                                        <q-fab-action color="skypay-primary" size="10px"
                                                      @click="editItem(cell.data)"
                                                      icon="edit" />
                                        <q-fab-action color="skypay-primary"
                                                      @click="confirmDeleteItem(cell.data)"
                                                      icon="delete" />
                                    </q-fab>
                                </div>
                            </template>
                        </q-data-table>
                        <div class="row" style="padding-top:10px">
                            <div class="col"></div>
                            <div class="col-3">
                                ИТОГО: <span class="money">{{total.toFixed(2)}}</span>
                            </div>
                        </div>

                    </div>

                    <div>
                        <q-btn color="skypay-secondary" @click="showItems = false" label="Close">
                            Закрыть
                        </q-btn>
                    </div>
                </div>
                <q-inner-loading :visible="itemvisible">
                    <q-spinner-mat size="50px" color="skypay-primary"></q-spinner-mat>
                </q-inner-loading>
            </div>

        </q-modal>
        <q-modal v-model="addEditItems">
            <div style="min-height:300px; min-width:600px; padding:20px;" class="relative-position">
                <p v-if="action=='edit'" class="bold caption text-skypay-primary">
                    Добавление продукта
                </p>
                <div class=" pad" style="padding-bottom:10px">
                    <q-search v-model="terms" :error="$v.terms.$error" placeholder="Выбор продукта" color="skypay-primary">
                        <q-autocomplete :filter="filterProducts" :static-data="{field: 'value', list: productList}" @selected="selectProduct" />
                    </q-search>
                    <div class="row">
                        <div class="col" style="margin-right:10px">
                            <q-field>

                                <q-input v-model="item.addQty" @blur="$v.item.addQty.$touch" :error="$v.item.addQty.$error" float-label="Количество" placeholder="" color="skypay-primary" />
                            </q-field>
                        </div>
                    </div>
                    <div style="position:absolute;bottom:10px">
                        <q-btn color="skypay-secondary" @click="saveItem" label="Close">
                            Ok
                        </q-btn>
                        <q-btn color="skypay-secondary" flat @click="addEditItems=false" label="Close">
                            Отмена
                        </q-btn>
                    </div>

                </div>
                <q-inner-loading :visible="pvisible">
                    <q-spinner-mat size="50px" color="skypay-primary"></q-spinner-mat>
                </q-inner-loading>
            </div>
        </q-modal>
    </div>
</template>
<script>

    import { mapActions, mapGetters } from 'vuex'
    import api from '../../store/api'

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
        QIcon, QModal, Dialog, Toast, QSearch, QAutocomplete, QDatetime
        //QTooltip,
        //QCollapsible,
        //clone
    } from 'quasar-framework'
    import { numeric, required } from 'vuelidate/lib/validators'

    export default {
        name: "documents-out",
        data() {
            return {
                selectedItem: {},

                showItems: false,
                itemvisible: false,
                canEditItems: false,
                mvisible: true,
                items: [],
                total: 0,
                item: { addQty: null, outputPrice: null },
                addEditItems: false,
                stockProducts: [],
                productList: [],
                terms: '',
                selectedProduct: null,
                pvisible: false,
                action: 'edit',
                addEditDoc: false,
                documents: [],
                selectedDocument: {},
                tvisible: false,
                productList: [],
                agents: [],
                agent: {},
                private: false,
                selectedAgent: {},
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
                        //options: [5, 10, 15, 30, 50, 500]
                    },
                    //selection: 'none'
                    labels: {
                        rows: 'Строк',
                        columns: 'Coluuuuumns'
                    }
                },
                columns: [

                    {
                        label: 'Номер',
                        field: 'Number',

                        width: '5%',
                        //filter: true
                        //classes: 'col-7'
                    },
                    {
                        label: 'Дата',
                        field: 'DocumentDate',

                        width: '10%',
                        format(value, row) {
                            return new Date(value).toLocaleDateString();
                        }
                    },
                    {
                        label: 'Покупатель',
                        field: 'agentEx',

                        width: '10%',
                        format(value, row) {
                            if (value.private)
                                return 'Частное лицо'
                            else
                                return value.name
                        }
                    },
                    //{
                    //    label: 'Частное лицо',
                    //    field: 'Private',

                    //    width: '5%',
                    //    format(value, row) {
                    //        return value
                    //    }
                    //},
                    //{
                    //    label: 'НДС',
                    //    field: 'nds',
                    //    width: '5%',
                    //    format(value, row) {
                    //        return value + '%'
                    //    }
                    //},
                    {
                        label: ' ',
                        field: 'lock',
                        width: '10%',
                        classes: 'text-right'
                    }
                ],
                item_config: {
                    title: '',
                    refresh: false,
                    noHeader: false,
                    columnPicker: false,
                    leftStickyColumns: 0,
                    rightStickyColumns: 0,
                    bodyStyle: {
                        maxHeight: '450px'
                    },
                    rowHeight: '50px',
                    responsive: true,
                    //pagination: {
                    //    rowsPerPage: 5,
                    //    //options: [5, 10, 15, 30, 50, 500]
                    //},
                    //selection: 'none'
                    labels: {
                        rows: 'Строк',
                        columns: 'Coluuuuumns'
                    }
                },
                item_columns: [
                    {
                        label: 'Товар',
                        field: 'Product',

                        width: '20%',
                        format(value, row) {
                            return value.Name
                        }
                    },
                    //{
                    //    label: 'Закупочная цена',
                    //    field: 'InputPrice',

                    //    width: '12%',
                    //    //filter: true
                    //    //classes: 'col-7'
                    //},
                    {
                        label: 'Цена продажи',
                        field: 'OutputPrice',
                        classes: 'money',
                        width: '15%'
                    },
                    {
                        label: 'Количество',
                        field: 'Qty',

                        width: '12%'
                    },
                    {
                        label: 'Ед. изм.',
                        field: 'Product',

                        width: '12%',
                        format(value, row) {
                            return value.Unit.Name
                        }
                    },
                    {
                        label: 'Сумма',
                        field: 'Summ',
                        classes: 'money',

                        width: '20%'
                    },
                    {
                        label: 'НДС',
                        field: 'nds',
                        width: '15%'
                    },
                    {
                        label: 'Сумма с НДС',
                        field: 'Summ_nds',
                        classes: 'money',

                        width: '15%'
                    },
                    {
                        label: ' ',
                        field: 'Id',
                        width: '10%'
                    }
                ],
            }
        },
        methods: {
            addItem: function () {
                this.item.addQty = null
                this.item.outputPrice = null
                this.terms = ''
                this.itemAction = 'add'
                this.$v.item.$reset()
                this.$v.terms.$reset()

                this.addEditItems = true
            },
            editItems: async function (data) {
                this.showItems = true
                this.itemvisible = true
                let id = data.Id
                this.canEditItems = !data.locked;
                let _doc = this.documents.find(z => z.Id == id)
                this.selectedDocument = _doc
                let _items = await api.documents.getItems(id)
                this.total = 0;
                let self = this;
                this.items = this.computeItems(_doc, _items)
                this.mvisible = true
                this.itemvisible = false

            },
            editItem: function (id) {
                this.selectedItem = this.items.find(z => z.Id == id)
                this.item.addQty = this.selectedItem.Qty
                this.selectedProduct = this.selectedItem.Product
                this.terms = this.selectedItem.Product.Name
                this.itemAction = 'edit'
                this.addEditItems = true

            },
            saveItem: async function () {
                this.$v.item.$touch()
                this.$v.terms.$touch()

                if (this.selectedProduct != null && !this.$v.item.$error) {
                    this.$v.item.$reset()
                    this.$v.terms.$reset()
                    this.pvisible = true
                    switch (this.itemAction) {
                        case 'add':
                            let _item = { id: 0, DocumentId: this.selectedDocument.Id, ProductId: this.selectedProduct.Id, Qty: this.item.addQty, OutputPrice: this.selectedProduct.OutputPrice, InputPrice: 0 }
                            console.log(_item)
                            try {
                                await api.documents.addItem(_item)
                                let _items = await api.documents.getItems(this.selectedDocument.Id)
                                this.items = this.computeItems(this.selectedDocument, _items)
                                this.addEditItems = false
                                Toast.create.positive('Продукт добавлен в документ')
                            }
                            catch (error) {
                                Toast.create.warning('Ошибка: ' + error)

                            }
                            break
                        case 'edit':
                            let _itemu = { id: this.selectedItem.Id, DocumentId: this.selectedDocument.Id, ProductId: this.selectedProduct.Id, Qty: this.item.addQty, OutputPrice: this.selectedProduct.OutputPrice, InputPrice: 0 }
                            try {
                                await api.documents.updateItem(_itemu)
                                let _items = await api.documents.getItems(this.selectedDocument.Id)
                                this.items = this.computeItems(this.selectedDocument, _items)

                                Toast.create.positive('Продукт изменен')
                            }
                            catch (error) {
                                Toast.create.warning('Ошибка: ' + error)

                            }
                            break
                    }
                    this.pvisible = false
                    this.addEditItems = false
                }
                else {
                    Toast.create.warning('Необходимо заполнить все поля')
                }
            },
            selectProduct: function (val) {
                this.selectedProduct = this.productList.find(p => p.Id == val.Id)
            },
            filterProducts: function (terms, { field, list }) {
                let _f = this.productList
                    .filter(p => p.label.toLowerCase().indexOf(terms) === 0 ||
                        p.barCode.toLowerCase().indexOf(terms) === 0 ||
                        p.vendorCode.toLowerCase().indexOf(terms) === 0

                    )
                return _f
            },
            addDocument: function () {
                this.$v.agent.$reset()

                this.action = 'addDoc'
                this.selectedAgent = {}
                this.agent = ''
                this.private = false
                this.addEditDoc = true

            },
            editDocument: async function (id) {
                this.$v.agent.$reset()

                this.action = 'editDoc'
                let _doc = this.documents.find(z => z.Id == id)
                this.selectedDocument = _doc
                this.private = _doc.Private
                if (_doc.Private) {
                    this.selectedAgent = null
                    this.agent = 0
                }
                else {
                    this.selectedAgent = this.agents.find(z => z.value == _doc.AgentId)
                    this.agent = this.selectedAgent.value
                }
                this.addEditDoc = true

            },
            saveDoc: async function () {
                this.$v.agent.$touch()
                if (!this.$v.agent.$error) {
                    this.$v.agent.$reset()
                    this.addEditDoc = false
                    this.tvisible = true
                    switch (this.action) {
                        case 'addDoc':
                            let _document = {
                                id: 0, type: 2, nds: 0, makrkup: 0, AgentId: this.agent == 0 ? null : this.agent, stockId: this.selectedStockId, DocumentDate: new Date, Private: this.private
                            }
                            try {
                                await api.documents.add(_document)
                                let docs = await api.documents.get(this.selectedStockId, 'Out')
                                this.documents = this.computeDocs(docs)
                                Toast.create.positive('Документ добавлен')

                            }
                            catch (error) {
                                Toast.create.warning('Ошибка: ' + error)

                            }
                            break
                        case 'editDoc':
                            let _doc = {
                                id: this.selectedDocument.Id, type: 2, nds: 0, makrkup: 0, AgentId: this.agent == 0 ? null : this.agent, stockId: this.selectedStockId, DocumentDate: this.selectedDocument.DocumentDate, Private: this.private
                            }
                            try {
                                await api.documents.update(_doc)
                                let docs1 = await api.documents.get(this.selectedStockId, 'Out')
                                this.documents = this.computeDocs(docs1)
                                Toast.create.positive('Документ изменен')

                            }
                            catch (error) {
                                Toast.create.warning('Ошибка: ' + error)

                            }
                            break
                    }

                    this.tvisible = false

                }
                else {
                    Toast.create.warning('Необходимо заполнить все поля')
                }
            },
            computeItems: function (doc, items) {
                let self = this
                this.total = 0

                return items.map(
                    function (el) {
                        el.OutputPrice = el.OutputPrice.toFixed(2);
                        el.Summ = (el.OutputPrice * el.Qty).toFixed(2);
                        el.nds = doc.nds + '%'
                        let _s1 = (el.Summ * (1 + (doc.nds / 100)))
                        el.Summ_nds = _s1.toFixed(2)
                        self.total += _s1
                        return el
                    })
            },
            confirmDeleteItem: function (id) {
                let _item = this.items.find(z => z.Id == id)
                Dialog.create({
                    title: 'Удаление продукта',
                    message: `Будет удален продукт ${_item.Product.Name} !`,
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
                                    this.itemvisible = true

                                    await api.documents.deleteItem(id)
                                    let _items = await api.documents.getItems(this.selectedDocument.Id)
                                    this.items = this.computeItems(this.selectedDocument, _items)

                                    this.itemvisible = false
                                    Toast.create.positive('Удален продукт ' + _item.Product.Name)
                                }
                                catch (error) {
                                    Toast.create.warning('Ошибка: ' + error)
                                }
                            }
                        }
                    ]
                })
            },
            changeAgent: function (val) {
                if (val == 0) {
                    this.private = true
                }
                else {
                    this.private = false
                }
                console.log(this.private)
                this.selectedAgent = this.agents.find(z => z.id == val);
            },
            computeDocs: function (docs) {
                return docs.map(function (el) { el.Number = el.Id; el.agentEx = { name: (el.Agent ? el.Agent.Name : 'Частное лицо'), private: el.Private }; el.lock = { locked: el.Locked, Id: el.Id }; return el })
            },
            changePrivate: function () {
                this.private = !this.private
                if (this.private)
                    this.agent = 0
                else
                    this.agent = ''
                console.log(this.private)
            },
            lockDocument: function (id) {
                let _doc = this.documents.find(z => z.Id == id)
                Dialog.create({
                    title: 'Утверждение документа',
                    message: `Будет утвержден документ № ${_doc.Id} !`,
                    buttons: [
                        {
                            label: 'Отмена',
                            color: 'skypay-secondary'
                        },
                        {
                            label: 'Утвердить',
                            color: 'skypay-secondary',
                            handler: async () => {
                                try {
                                    await api.documents.lock(id)

                                    //
                                    let docs = await api.documents.get(this.selectedStockId, 'Out')
                                    this.documents = this.computeDocs(docs)

                                    Toast.create.positive('Утвержден документ № ' + _doc.Id)
                                }
                                catch (error) {
                                    Toast.create.warning('Ошибка: ' + error)
                                }
                            }
                        }
                    ]
                })
            },
            confirmDeleteDocument: function (id) {
                let _doc = this.documents.find(z => z.Id == id)
                Dialog.create({
                    title: 'Удаление документа',
                    message: `Будет удален документ № ${_doc.Id} !`,
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
                                    await api.documents.delete(id)
                                    let docs = await api.documents.get(this.selectedStockId, 'Out')
                                    this.documents = this.computeDocs(docs)
                                    this.tvisible = false
                                    Toast.create.positive('Удален документ № ' + _doc.Id)
                                }
                                catch (error) {
                                    Toast.create.warning('Ошибка: ' + error)
                                }
                            }
                        }
                    ]
                })
            },
        },
        validations: {
            //productset: {
            //    name: { required },
            //    barCode: { required }
            //},
            item: {
                addQty: { numeric, required },
                //inputPrice: { decimal, required }
            },
            terms: { required },
            agent: { required },
            //selectedDocument: {
            //    makrkup: { numeric, required }
            //},
            //selectedAgent: {
            //    nds: { numeric, required }
            //}
        },
        computed: {
            ...mapGetters({ selectedStockId: "selectedStockId", selectedCompanyId: "selectedCompanyId" })
        },
        watch: {
            selectedStockId: async function () {
                let docs = await api.documents.get(this.selectedStockId, 'Out')
                this.documents = this.computeDocs(docs)

            },
        },

        async created() {
            this.tvisible = true;
            this.stockProducts = await api.products.getStockProducts(this.selectedStockId)
            this.productList = this.stockProducts.map((el) => { return { Id: el.Product.Id, value: el.Product.Name, label: el.Product.Name, sublabel: 'Штрихкод:' + el.Product.BarCode, barCode: el.Product.BarCode, vendorCode: el.Product.VendorCode, OutputPrice: el.OutputPrice } })
            let _agents = await api.agents.get(this.selectedCompanyId, 1)
            this.agents = _agents.map(function (el) { el.value = el.id, el.label = el.name; return el })
            this.agents.push({ value: 0, label: 'Частное лицо' })
            let docs = await api.documents.get(this.selectedStockId, 'Out')
            this.documents = this.computeDocs(docs)
            this.tvisible = false

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
            QBtn, QDatetime,
            QIcon, QModal, QCheckbox,
            QTabs, QTab, QTabPane, QField, QInput, QFab, QFabAction, Dialog, Toast, QSearch, QAutocomplete
            //QTooltip,
            //QCollapsible
        }
    }
</script>
<style>
    .money::after {
        content: "\20bd";
        font-style: normal;
        margin-left: 4px
    }
</style>
