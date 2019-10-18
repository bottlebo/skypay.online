require(`quasar-framework/dist/quasar.mat.css`)
//require('quasar-extras/roboto-font')
//import 'quasar-extras/material-icons'
//import 'quasar-extras/animate'
import Vue from 'vue'
import axios from 'axios'
import router from './router'
import store from './store'
import { sync } from 'vuex-router-sync'
import App from 'components/app-root'
import Quasar, { Ripple } from 'quasar-framework'
import Vuelidate from 'vuelidate'
Vue.prototype.$http = axios;
//import Units from './components/directory-components/units'
////Vue.component('productrange', productrange);
//Vue.component('units', Units);
sync(store, router)
Vue.use(Quasar, {
    directives: {
        Ripple
    }
})
Vue.use(Vuelidate)

const app = new Vue({
        el:'#q-app',
        store,
        router,
        ...App,
        //components: { units }
    })
//Quasar.start(() => {
//    app
//});
export {
    app,
    router,
    store
}
