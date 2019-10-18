<template>
    <div class="row" style="min-height: calc(-125px + 100vh);">

        <div id='treeparent' class="col-4" style="min-height:100%">

            <category-list></category-list>

        </div>
        <div class="col">
            <product-list></product-list>
        </div>
        <div class="clearfix"></div>
    </div>
</template>
<script>
    //import { mapActions, mapGetters } from 'vuex'
    //import { TreeView } from '@syncfusion/ej2-navigations';
    export default {
        name: "productrange",
        data() {
            return {
                //categorytree:null
            }
        },
        computed: {
            //...mapGetters({
            //    categories: "categories",
            //    products: "products"
            //    //selectedCompany: "selectedCompany"
            //}),
            //currentCompanyId: {
            //    get() {
            //        if (this.$store.state.companies.selectedCompanyId)
            //            return this.$store.state.companies.selectedCompanyId;
            //        else
            //            return null
            //    }
            //}
        },
        methods: {
            //...mapActions({ selectCategory: 'selectCategory' }),
            //changeCategory: async function () {
            //    //console.log(this.categorytree.selectedNodes)
            //    this.selectCategory(this.categorytree.selectedNodes)
            //    //await this.$store.dispatch("getProducts")
            //    //console.log(this.products)
            //}
        },
        watch: {
            //categories: function () {
            //    //this.categorytree.refresh()
            //    if (this.categorytree && this.categorytree.destroy) {
            //        this.categorytree.destroy()

            //    }
            //    this.categorytree = new TreeView({

            //        fields: { dataSource: this.categories, id: 'id', parentID: 'parentID', text: 'Name', hasChildren: 'hasChild' },
            //        nodeSelected: this.changeCategory
            //    });
            //    this.categorytree.appendTo('#tree');
            //},
            //currentCompanyId: async function () {
            //    await this.$store.dispatch("reloadCategories");
            //}
        },
        //},
        mounted() {
            //this.categorytree = new TreeView({

            //    fields: { dataSource: this.categories, id: 'id', parentID: 'parentID', text: 'Name', hasChildren: 'hasChild' },
            //    nodeSelected: this.changeCategory
            //});
            //this.categorytree.appendTo('#tree');
        },
        async created() {
            //await this.$store.dispatch("getCategories");
        }
    }
</script>
<style>
    /*#treeparent {
        box-shadow: 5px 0 5px -5px rgba(0,0,0,.2)
            0 0 10px 1px rgba(0,0,0,.14),
            0 0 14px 2px rgba(0,0,0,.12);
    }*/
</style>
