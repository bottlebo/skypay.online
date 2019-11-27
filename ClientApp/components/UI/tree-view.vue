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
            }
        },
        computed: {
        },
        methods: {
            itemClick: function (e) {
                this.$emit('itemClick', e)
            },
            treeData: function() {
                return this.model
            }
        }
    }
</script>
