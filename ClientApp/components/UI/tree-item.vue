<template id="item-template">
    <li>
        
        <div :class="{bold: isFolder(), dragOver:isDragOver, selected: isSelected, ind:!isFolder()}" v-ripple="!isFolder()" style="position:relative;user-select: none"
             @click="clickMe" @dragover="dragOverHandler" @dragleave="dragLeave" @drop="drop"
             >
            <q-icon v-if="isFolder()" name="expand_more" :class="{expand:open}" @click="toggle"></q-icon>
            <span style="padding:0 5px">{{ model.name }}</span>
            <!--<span style="font-size:8px" v-for="(model, index) in itemModel().children" :key="index">{{model.name}}</span>
            <span>{{isFolder()}}</span>-->
            <!--<span v-if="isFolder">[{{ open ? '-' : '+' }}]</span>-->
        </div>
        <transition name="slide">
            <ul v-show="open" v-if="isFolder()">
                <item class="item" v-on:itemClick="itemClick"
                      v-for="(model, index) in itemModel().children"
                      :key="index"
                      :model="model">
                </item>
                <!--<li class="add" @click="addChild">+</li>-->
            </ul>
            </transition>
</li>
</template>
<script>
    import Vue from 'vue'
    import {
        QIcon
    } from 'quasar-framework'
    export default {
        template: '#item-template',
        props: {
            model: {}
        },
        data: function () {
            return {
                open: true,
                isDragOver:false
            }
        },
        computed: {
            //open: {
            //    get(){return this.model.open}
            //},
            //isFolder: function () {
            //    return this.model.children != null && this.model.children.length >0
            //},
            isSelected: function () {
                return this.model.id == this.$store.state.categories.selectedCategoryId
            }
        },
        //watch: {
        //    model: function () {
        //        console.log('ITEM')
        //        console.log(this.model.name)
        //    }
        //},
        methods: {
            isFolder: function () {
                return this.model.children!=null && this.model.children.length >0
            },
            isOpen: function () {
                return this.model.open
            },
            toggle: function () {
                if (this.isFolder) {
                    //Vue.set(this.model,'open', !this.open)
                    this.$store.dispatch("toggleCategory", this.model.id);

                    //this.model.open = !this.model.open
                    this.open = !this.open
                }
                else {
                    //this.isSelected = true

                }
                //this.$emit('itemClick', this.model.id)
            },
            clickMe: function () {
                this.$emit('itemClick', this.model.id)
            },
            itemClick: function (e) {
                //if (!this.isFolder) {
                    this.$emit('itemClick',e)
                //}
            },
            changeType: function () {
                if (!this.isFolder) {
                    Vue.set(this.model, 'children', [])
                    this.addChild()
                    this.open = true
                }
            },
            addChild: function () {
                this.model.children.push({
                    name: 'new stuff'
                })
            },
            itemModel: function () {
               
                    return this.model
               
            },
            dragOverHandler: function (e) {
                if (e.preventDefault) e.preventDefault();
                e.dataTransfer.dropEffect = 'copy'
                this.isDragOver = true
                return false;
            },
            dragLeave: function (e) {
                this.isDragOver = false
            },
            drop: function (e) {
                if (e.stopPropagation) e.stopPropagation();
                this.isDragOver = false;
                console.log(e.dataTransfer.getData('productId'))
            }
        },
        created() {
            this.open = this.model.open
        },
        components: {
            QIcon
        }
    }
</script>
<style>
    .item {
        cursor: pointer;
    }
    .ind{
        margin-left:28px;
    }
    .expand{
        transform:rotate(180deg)
    }
    .bold {
        font-weight: 500;
    }
    .selected {
        background-color: #3f98a6; color:white
    }
    ul {
        padding-left: 1em;
        line-height: 1.5em;
        list-style-type: none;
    }
    .dragOver {
        background-color: #3d5480 !important;
        color: white
    }
    /*.slide-enter-active {
        transition: all .3s ease;
    }

    .slide-leave-active {
        transition: all .8s cubic-bezier(1.0, 0.5, 0.8, 1.0);
    }

    .slide-enter {
        height:unset
    }
    .slide-leave-to
 {
        height:0;

    }*/
    .slide-enter-active {
        animation: slide-in 0.25s ease-out;
        transform-origin: 0 0;
    }

    .slide-leave-active {
        animation: slide-in 0.2s reverse ease-in;
        transform-origin: 0 0;
    }

    @keyframes slide-in {
        0% {
            transform: scaleY(0);

        }

        /*50% {
            transform: scale(1.5);
        }*/

        100% {
            transform: scaleY(1);
        }
    }
</style>
