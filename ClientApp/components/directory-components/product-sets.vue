<template>
    <div class="row" style="min-height: calc(-125px + 100vh);">

        <div id='treeparent' class="col-3" style="min-height:100%">

            <category-list></category-list>

        </div>
        <div class="col-9">
            <product-set-list></product-set-list>
        </div>
        <div class="clearfix"></div>
    </div>
</template>
<script>
    //import { mapActions, mapGetters } from 'vuex'
    //import { TreeView } from '@syncfusion/ej2-navigations';
    export default {
        name: "productsets",
        data() {
            return {
                //categorytree:null
            }
        }
    }
</script>
