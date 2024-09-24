<template>
    <v-container class="fill-height" fluid>
        <v-row align="center" justify="center">
            <v-col cols="12">
                <v-card class="elevation-12 rounded-lg">
                    <v-card-title class="text-center text-h5 py-4">
                        登出
                    </v-card-title>
                    <v-card-text class="text-center pb-0">
                        <p>已登出，是否回到登入畫面。</p>
                    </v-card-text>
                    <v-card-actions class="justify-center pa-4">
                        <v-btn color="primary" :loading="isLoggingOut" :disabled="isLoggingOut" :to="'/login'">
                            <v-icon start icon="mdi-login"></v-icon>
                            {{ isLoggingOut ? '登出中...' : '確認' }}
                        </v-btn>
                    </v-card-actions>
                </v-card>
            </v-col>
        </v-row>
    </v-container>
</template>

<script lang="js">
import { defineComponent } from 'vue';
import Cookies from 'js-cookie';

export default defineComponent({
    components: {
        
    },
    directives: {
    },
    filters: {
    },
    props: {
    },
    data() {
        return {
            isLoggingOut: true,
        }
    },
    mounted() {
        this.Logout();
    },
    methods: {
        Logout() {
            Cookies.remove(import.meta.env.VITE_COOKIE_LOGIN_TOKEN);
            this.isLoggingOut = false;
        }
    },
});
</script>

<style>

</style>