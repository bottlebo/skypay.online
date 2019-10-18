<template>
    <ul id="demo" style="margin-top:0!important">
        <item class="item"
              :model="treeData()" v-on:itemClick="itemClick">
        </item>
    </ul>
</template>
<script>

    import Vue from 'vue'
    import item from './tree-item'
    Vue.component("item", item)
    export default {
        props: {
            model: {}
        },
        data() {
            return {
                //treeData: this.model
            }
        },
        computed: {
            //treeData: { get() { return this.model } }
        },
        //watch: {
        //    model: function () {
        //        console.log('TREE')
        //        console.log(this.model)
        //    }
        //},
        methods: {
            itemClick: function (e) {
                //console.log(0)
                this.$emit('itemClick', e)
            },
            treeData: function() {
                return this.model
            }
        }
        //,
        //created() {
        //    console.log('tree created')
        //}
    }
</script>
