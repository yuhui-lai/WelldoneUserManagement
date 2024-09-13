import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'

// Vuetify
import 'vuetify/styles'
import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'
import { aliases, mdi } from 'vuetify/iconsets/mdi'
import '@mdi/font/css/materialdesignicons.css'

//const wumTheme = {
//    dark: false,
//    colors: {
//        background: '#FFFFFF',
//        surface: '#FFFFFF',
//        primary: '#6200EE',
//        'primary-darken-1': '#3700B3',
//        secondary: '#03DAC6',
//        'secondary-darken-1': '#018786',
//        error: '#B00020',
//        info: '#2196F3',
//        success: '#4CAF50',
//        warning: '#FB8C00',
//    },
//}

const vuetify = createVuetify({
    components,
    directives
})

createApp(App)
    .use(router)
    .use(vuetify)
    .mount('#app')
