<template>
    <div style="padding:0px; position:relative; height:95%">
        <!--<v-jstree :data="cats" text-Field-Name="title"  value-Field-Name="id" class="catclass" noDots draggable @item-click="changeCategory"></v-jstree>-->
        <!--<q-tree :model="cats"
                contract-html="<i class='material-icons'>expand_less</i>"
                expand-html="<i class='material-icons'>expand_more</i>"></q-tree>-->

            <div class="treecrud" style="background-color:red">
                <q-fab color="skypay-primary" flat small class="pull-right" style="margin-top:-10px!important"
                       icon="more_vert"
                       direction="down">
                    <q-fab-action color="skypay-primary"
                                  v-bind:disabled="!(selectedCategoryId>=0)" @click="addCat"
                                  icon="add" />

                    <q-fab-action color="skypay-primary"
                                  v-bind:disabled="!(selectedCategoryId>0)" @click="editCat"
                                  icon="edit" />
                    <q-fab-action color="skypay-primary"
                                  v-bind:disabled="!(selectedCategoryId>0)" @click="deleteCat"
                                  icon="delete" />
                </q-fab>
            </div>
            <tree-view style="margin-right:40px" :model="buildTree()" @itemClick="changeCategory"></tree-view>
            <!--<div style="position:absolute; bottom:20px; padding:10px; width:100%;">
            <q-btn color="skypay-primary" round small v-bind:disabled="!(selectedCategoryId>=0)" @click="addCat">
                <q-icon name="add" size="18px" />
            </q-btn>
            <q-btn color="skypay-primary" round small v-bind:disabled="!(selectedCategoryId>0)" @click="editCat">
                <q-icon name="edit" size="18px" />
            </q-btn>
            <q-btn color="skypay-primary" round small v-bind:disabled="!(selectedCategoryId>0)" @click="deleteCat">
                <q-icon name="delete" size="18px" />
            </q-btn>
        </div>-->


        <q-modal v-model="addedit">
            <div style="min-height:300px; min-width:650px; padding:20px">
                <p class="bold caption text-skypay-primary" style="width:80%">
                    Изменение категории
                </p>
               
                <div style="min-height:200px; padding:10px 0;">
                    <q-field>
                        <q-input v-model="editCatName" float-label="Наименование категории" color="skypay-primary" />
                    </q-field>
                    <div class="caption text-skypay-primary"  style="max-width:400px; padding:20px 0 10px">Свойства товаров в категории</div>
                   
                    <!--<q-card>
                        <q-card-title>-->
                            <div class="row" style=" padding:10px 0">
                                <div class="col-5" style="margin-right:15px">
                                    <q-field>

                                        <q-input v-model="addPropName" float-label="" placeholder="Добавить" color="skypay-primary"/>
                                    </q-field>
                                </div>
                                <div class="col-5" style="margin-right:15px; max-width:30%">
                                    <q-select v-model="addPropType" color="skypay-primary"
                                              :options="types"  />
                                </div>
                                <div class="col" style="max-width:10%">
                                    <q-btn color="skypay-primary"  small round @click="addProp">
                                        <q-icon name="add" size="18px" />
                                    </q-btn>
                                </div>
                            </div>
                        <!--</q-card-title>
                        <q-card-separator />
                        <q-card-main>-->
                            <q-scroll-area style=" height: 200px;margin-top:15px; border-top:solid 1px #ccc; border-bottom:solid 1px #ccc; padding:15px 0 10px 0">
                               
                                <div class="" v-for="(model,index) in props" :key="index" style="padding-bottom:10px">
                                    <q-card style="width:95%">
                                        <q-card-main>
                                    <div class="row">
                                        <div class="col-5" style="margin-right:15px;">
                                            <q-field>
                                                <q-input v-model="model.name" float-label="" color="skypay-primary" />
                                            </q-field>
                                        </div>
                                        <div class="col-5" style="margin-right:15px;">
                                            <q-select v-model="model.type" color="skypay-primary"
                                                      :options="types" />
                                        </div>
                                        <div class="col-1" style="max-width:10%">
                                            <q-btn color="skypay-primary" flat small round @click="deleteProp(model.id)">
                                                <q-icon name="delete" size="18px" />
                                            </q-btn>
                                        </div>
                                    </div>
                                        </q-card-main>
                                    </q-card>
                                </div>
                                </q-scroll-area>
                        <!--</q-card-main>
                    </q-card>-->





                </div>
                <div>
                    <q-btn color="skypay-secondary" @click="saveCat" label="Close">
                        Ok
                    </q-btn>
                    <q-btn color="skypay-secondary" @click="closeModal" label="Close">
                        Отмена
                    </q-btn>
                </div>
            </div>
        </q-modal>
    </div>

</template>
<script>
    import { tarnsformTreeData } from '../../helpers'
    import axios from 'axios'
    import api from '../../store/api'
    import { mapActions, mapGetters } from 'vuex'
    //import { TreeView } from '@syncfusion/ej2-navigations';
    //import VJstree from 'vue-jstree'
    import {
        QBtn, QIcon, QModal, QField, QInput, QChipsInput, QList, QChip, QSelect, QCard, QCardMain, QCardTitle, QCardSeparator,
        QScrollArea,
        QItem,
        QItemSide,QFab,QFabAction,
        QItemMain, Dialog, Toast,
        clone
    } from 'quasar-framework'
    export default {
        name: "productrange",
        data() {
            return {
                cats: {},
                addedit: false,
                editCatName: "",
                action: '',
                props:[],// [{ id: 1, name: 'Jack', Type: 1 }, { id: 2, name: 'Jim', Type: 2 }],
                types: [{ label: 'Текст', value: 1 }, { label: 'Птичка', value: 2 }/*, { label: 'Число', value: 3 }*/],
                addPropName: "",
                addPropType: 0

            }
        },
        computed: {
            ...mapGetters({ selectedCompanyId: "selectedCompanyId", categories: "categories", selectedCategoryId: 'selectedCategoryId' }),

            currentCompanyId: {
                get() {
                    return this.selectedCompanyId ? this.selectedCompanyId : ""
                }
            },
            currentCategoryId: {
                get() {
                    return this.selectedCategoryId ? this.selectedCategoryId : null
                }
            }
        },
        methods: {
            ...mapActions({ selectCategory: 'selectCategory', getCategories: 'getCategories' }),
            changeCategory: async function (id) {
                //helpers.setSelected(this.cats, node.id)
                if (id === undefined) id = 0
                //console.log(id)
                this.selectCategory(id)
            },
            changeProp: function (val) {
                //console.log(Events)
            },
            buildTree: function () {
                return tarnsformTreeData(this.categories)
                //return this.cats
            },
            getSelectedCategory: function () {
                return this.categories.filter(c => c.id == this.selectedCategoryId)[0];
            },
            editCat: async function () {
                this.action = 'edit'
                this.addPropName = ""
                this.addPropType = 0
                let selCat = this.getSelectedCategory()
                this.editCatName = selCat.name
                this.props = await api.getCategoryProperties(selCat.id)
                //for (let i in this.props) {
                //    console.log(this.props[i].name + ';' + this.props[i].type)
                //}
                this.addedit = true
            },
            addCat: function () {
                this.action = 'add'
                this.props = []
                this.editCatName = '';
                this.addedit = true

            },
            deleteCat: async function () {
                let selCat = this.getSelectedCategory()
                Dialog.create({
                    title: 'Удаление категории',
                    message: `Будет удалена категория: ${selCat.name} и все продукты в этой категории!`,
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
                                    //..console.log(this.selectedProductId)
                                    //await api.products.delete(this.selectedProductId)
                                    //this.$store.dispatch("deleteProduct", this.selectedProductId)

                                    //this.selectProduct(0);
                                    await api.categories.deleteCategory(this.selectedCategoryId)
                                    this.$store.dispatch("deleteCategory", this.selectedCategoryId)
                                    this.selectCategory(0)
                                    Toast.create.positive('Удалена категория: ' + selCat.name)
                                }
                                catch (error) {
                                    Toast.create.warning('Ошибка: ' + error)
                                }
                            }
                        }
                    ]
                })




                //try {
                //    await api.categories.deleteCategory(this.selectedCategoryId)
                //    this.$store.dispatch("deleteCategory", this.selectedCategoryId)
                //    this.selectCategory(0)
                //}
                //catch (error) {
                //    alert(error)
                //}
            },
            deleteProp: function (id) {

                let del = this.props.filter(p => p.id == id)[0];
                let index = this.props.indexOf(del);
                this.props.splice(index, 1)
                //console.log(index)
                //for (let i in this.props) {
                //    console.log(this.props[i].name + ';' + this.props[i].Type)
                //}
            },
            addProp: function () {
                let p = { id: 0, name: this.addPropName, type: this.addPropType, categoryId: this.selectedCategoryId }
                try {
                    //let id = await api.addCategoryProperty(p)
                    //p.id = id
                    this.props.push(p)
                    //for (let i in this.props) {
                    //    console.log(this.props[i].name + ';' + this.props[i].Type)
                    //}
                    this.addPropName = ""
                    this.addPropType = 0
                }
                catch (error) {
                    alert(error)
                }
            },
            saveCat: async function () {
                let sel = this.getSelectedCategory();
                if (this.action == 'edit') {
                    let category = {
                        id: sel.id, name: this.editCatName, parentId: sel.id, hasChild: sel.hasChild, open: sel.open, companyId: sel.companyId, company: null, categoryProps: this.props, products: null
                    }
                    try {
                        //for (let i in this.props) {
                        //    console.log(this.props[i].name + ';' + this.props[i].Type)
                        //}
                        await api.categories.updateCategory(sel.id, category)
                        this.$store.dispatch("editCategory", category);
                    }
                    catch (error) {
                        alert(error)
                    }
                    //console.log(this.editCatName)
                   
                }
                else if (this.action == 'add') {
                    //let max = 1000
                    //let min = 10
                    //let r = Math.floor(Math.random() * (max - min)) + min
                    let parentId = this.selectedCategoryId == 0 ? null : this.selectedCategoryId
                    let category = {
                        id: 0, name: this.editCatName, parentId: parentId, hasChild: false, open:false, companyId: this.selectedCompanyId, company: null, categoryProps: null, products: null
                    }
                    category.categoryProps = this.props
                    //sel.hasChild = true
                    try {
                        let id = await api.categories.createCategory(category)
                        category.id = id
                        this.$store.dispatch("addCategory", category);
                        this.selectCategory(0)
                        this.selectCategory(this.selectedCategoryId)
                    }
                    catch (error) {
                        alert(error)
                    }
                    //this.$forceUpdate()
                }
                this.addedit = false;

            },
            closeModal: function () {
                this.addedit = false;
                this.editCatName = '';
            }
        },
        watch: {
            //categories: function () {
            //    console.log('cat changed')
            //    //this.cats = helpers.tarnsformTreeData(this.categories)
            //    //console.log(this.transform());
            //    //if (this.categorytree && this.categorytree.destroy) {
            //    //    this.categorytree.destroy()

            //    //}
            //    //this.categorytree = new TreeView({

            //    //    fields: { dataSource: this.categories, id: 'id', parentID: 'parentID', text: 'Name', hasChildren: 'hasChild' },
            //    //    nodeSelected: this.changeCategory
            //    //});
            //    //this.categorytree.appendTo('#categorytree');
            //},
            currentCompanyId: async function () {
                //var _this = this
                await this.$store.dispatch("reloadCategories");
                //axios.get('/odata/Companies(' + '1' + ')/Categories', { timeout: 10000 }).then((response) => {
                //    console.log(response.data)
                //    _this.cats1 = response.data
                //    console.log(_this.cats1)
                //    //this.cats = helpers.tarnsformTreeData((response.data), this.changeCategory, this.currentCategoryId)
                //    console.log('-----')
                //})
            }
        },
        //mounted() {
        //    //this.cat = api.TCat(1)
        //    //for (let i in this.cat) {
        //    //    this.cat[i].handler=this.hand
        //    //}
        //    //this.categorytree = new TreeView({

        //    //    fields: { dataSource: this.categories, id: 'id', parentID: 'parentID', text: 'Name', hasChildren: 'hasChild' },
        //    //    nodeSelected: this.changeCategory
        //    //});
        //    //this.categorytree.appendTo('#categorytree');
        //    //let response = await this.$http.get('/api/SampleData/WeatherForecasts')
        //    ////await axios.get('/odata/Companies(' + '1' + ')/Categories', { timeout: 10000 })
        //    //console.log(response.data)
        //    //console.log('created -----')
        //    //this.$store.dispatch("getCategories").then((data) => {
        //    //    console.log(data)
        //    //    console.log('created -----')
        //    //})
        //},
        async created() {
            //this.$store.dispatch("getCategories")
            //var _this = this
            //axios.get('/odata/Companies(' + '1' + ')/Categories', { timeout: 10000 })
            //    .then((cat) => {
            //console.log(cat.data)
            //    console.log('created -----')
            //    //this.$store.dispatch('saveCategories', categories)
            //    //let _thiscats = helpers.tarnsformTreeData(cat.data)
            //    _this.cats = cat.data
            //    console.log('created -----')
            //    //_this.cats1 = cat.data.slice(0)
            //    //console.log(_this.cats1)
            //    })

            await this.$store.dispatch("getCategories");
            //let response =
            //axios.get('/api/SampleData/WeatherForecasts').then((response) => {
            //try {
            //let response = await axios.get('/odata/Companies(' + '1' + ')/Categories', { timeout: 10000 })//.then((response) => {
            //this.$store.dispatch("getCategories").then((data) => {
            //    console.log(data)
            //    console.log('created -----')
            //})
            //}
            //catch (ee) {

            //}
            // })
            //this.cats = helpers.tarnsformTreeData((response.data), this.changeCategory, this.currentCategoryId)

            //console.log(helpers.tarnsformTreeData(clone(this.categories), this.changeCategory, 1))
        },
        components: {
            QBtn, QIcon, QModal, QField, QInput, QChipsInput, QList, QChip,
            QItem, QScrollArea,
            QItemSide,QFab, QFabAction,
            QItemMain, QSelect, QCard, QCardMain, QCardTitle, QCardSeparator, Dialog, Toast
        }
    }
</script>
<style>
    .catclass i.tree-themeicon {
        display: none !important
    }

    .q-chip {
        margin-right: 4px !important;
    }
    .treecrud .q-btn-standard{
        height:36px!important; width:36px!important
    }
    .treecrud .q-fab-actions button{
        margin-bottom:5px!important
    }
</style>
