require(`quasar-framework/dist/quasar.mat.css`)
import Vue from 'vue'
import axios from 'axios'
import router from './router'
import store from './store'
import { sync } from 'vuex-router-sync'
import App from 'components/app-root'
import Quasar, { Ripple } from 'quasar-framework'
import Vuelidate from 'vuelidate'
Vue.prototype.$http = axios;
sync(store, router)
Vue.use(Quasar, {
    directives: {
        Ripple
    }
})
Vue.use(Vuelidate)

const app = new Vue({
    el: '#q-app',
    store,
    router,
    ...App,
})
export {
    app,
    router,
    store
}
