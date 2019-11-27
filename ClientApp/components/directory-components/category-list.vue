<template>
    <div style="padding:0px; position:relative; height:95%">
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
        <q-modal v-model="addedit">
            <div style="min-height:300px; min-width:650px; padding:20px">
                <p class="bold caption text-skypay-primary" style="width:80%">
                    Изменение категории
                </p>

                <div style="min-height:200px; padding:10px 0;">
                    <q-field>
                        <q-input v-model="editCatName" float-label="Наименование категории" color="skypay-primary" />
                    </q-field>
                    <div class="caption text-skypay-primary" style="max-width:400px; padding:20px 0 10px">Свойства товаров в категории</div>
                    <div class="row" style=" padding:10px 0">
                        <div class="col-5" style="margin-right:15px">
                            <q-field>

                                <q-input v-model="addPropName" float-label="" placeholder="Добавить" color="skypay-primary" />
                            </q-field>
                        </div>
                        <div class="col-5" style="margin-right:15px; max-width:30%">
                            <q-select v-model="addPropType" color="skypay-primary"
                                      :options="types" />
                        </div>
                        <div class="col" style="max-width:10%">
                            <q-btn color="skypay-primary" small round @click="addProp">
                                <q-icon name="add" size="18px" />
                            </q-btn>
                        </div>
                    </div>
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
    import api from '../../store/api'
    import { mapActions, mapGetters } from 'vuex'
    import {
        QBtn, QIcon, QModal, QField, QInput, QChipsInput, QList, QChip, QSelect, QCard, QCardMain, QCardTitle, QCardSeparator,
        QScrollArea,
        QItem,
        QItemSide, QFab, QFabAction,
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
                props: [],
                types: [{ label: 'Текст', value: 1 }, { label: 'Птичка', value: 2 }],
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
                if (id === undefined) id = 0
                this.selectCategory(id)
            },
            changeProp: function (val) {
            },
            buildTree: function () {
                return tarnsformTreeData(this.categories)
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
            },
            deleteProp: function (id) {

                let del = this.props.filter(p => p.id == id)[0];
                let index = this.props.indexOf(del);
                this.props.splice(index, 1)
            },
            addProp: function () {
                let p = { id: 0, name: this.addPropName, type: this.addPropType, categoryId: this.selectedCategoryId }
                try {
                    this.props.push(p)
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
                        await api.categories.updateCategory(sel.id, category)
                        this.$store.dispatch("editCategory", category);
                    }
                    catch (error) {
                        alert(error)
                    }

                }
                else if (this.action == 'add') {
                    let parentId = this.selectedCategoryId == 0 ? null : this.selectedCategoryId
                    let category = {
                        id: 0, name: this.editCatName, parentId: parentId, hasChild: false, open: false, companyId: this.selectedCompanyId, company: null, categoryProps: null, products: null
                    }
                    category.categoryProps = this.props
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
                }
                this.addedit = false;

            },
            closeModal: function () {
                this.addedit = false;
                this.editCatName = '';
            }
        },
        watch: {
            currentCompanyId: async function () {
                await this.$store.dispatch("reloadCategories");
            }
        },
        async created() {
            await this.$store.dispatch("getCategories");
        },
        components: {
            QBtn, QIcon, QModal, QField, QInput, QChipsInput, QList, QChip,
            QItem, QScrollArea,
            QItemSide, QFab, QFabAction,
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

    .treecrud .q-btn-standard {
        height: 36px !important;
        width: 36px !important
    }

    .treecrud .q-fab-actions button {
        margin-bottom: 5px !important
    }
</style>
