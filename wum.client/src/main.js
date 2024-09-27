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

const wumTheme = {
    dark: false,
    colors: {
        background: '#f9f9ff',
        surface: '#f9f9ff',
        primary: '#769CDF',
        'primary-darken-1': '#5178C0',
        secondary: '#565f71',
        'secondary-darken-1': '#f5b0f9',
        error: '#ba1a1a',
        info: '#705575',
        success: '#4CAF50',
        warning: '#FB8C00',
        'on-background': '#1A1C1E',
        'on-surface': '#1A1C1E',
        'on-primary': '#FFFFFF',
        'on-secondary': '#ffffff',
        'on-error': '#FFFFFF',
        'on-info': '#FFFFFF',
        'on-success': '#FFFFFF',
        'on-warning': '#1A1C1E',
        accent: '#fad8fd',
    },
}

const vuetify = createVuetify({
    components,
    directives,
    theme: {
        defaultTheme: 'wumTheme',
        themes: {
            wumTheme,
        },
    }
})

createApp(App)
    .use(router)
    .use(vuetify)
    .mount('#app')
