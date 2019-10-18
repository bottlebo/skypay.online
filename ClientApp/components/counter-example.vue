<template>
    <div>
        <h1>Counter</h1>
        <p>This is a simple example of a Vue.js component & Vuex</p>
        <p>
            Current count (Vuex): <strong>{{ currentCount }}</strong>
        </p>
        <p>
            Auto count: <strong>{{ autoCount }}</strong>
        </p>
        <button @click="incrementCounter()" id="primarybtn">Increment</button>
        <button @click="resetCounter()">Reset</button>
    </div>
</template>
<script>
    import { mapActions, mapState } from 'vuex'
    //import { enableRipple } from '@syncfusion/ej2-base';
    //import { Button } from '@syncfusion/ej2-buttons';
    //import { Tab } from '@syncfusion/ej2-navigations';
    export default {
        data() {
            return {
                autoCount: 0,
            }
        },

        computed: {
            ...mapState({
                currentCount: state => state.counter
            })
        },

        methods: {
            ...mapActions(['setCounter']),

            incrementCounter: function () {
                var counter = this.currentCount + 1;
                this.setCounter({ counter: counter });
            },
            resetCounter: function () {
                this.setCounter({ counter: 0 });
                this.autoCount = 0;
            }
        },
        mounted() {
            //enableRipple(true)
            //var button = new Button({ iconCss: 'material-icons e-play-icon', cssClass: 'e-flat', isToggle: true });
            //console.log(document.querySelector('#primarybtn'))
            //try {
            //    button.appendTo('#primarybtn');
            //}
            //catch (e) {
            //    console.log(e)
            //}
        },
        created() {
            setInterval(() => {
                this.autoCount += 1
            }, 1000);
            //enableRipple(true)
           
        }
    }
</script>
<style>
    /*@import '../../node_modules/@syncfusion/ej2/material.css';*/
</style>
