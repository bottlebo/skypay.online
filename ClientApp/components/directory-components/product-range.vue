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
    export default {
        name: "productrange",
        data() {
            return {
            }
        },
        computed: {
        },
        methods: {
        },
        watch: {
        },
        mounted() {
        },
        async created() {
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
