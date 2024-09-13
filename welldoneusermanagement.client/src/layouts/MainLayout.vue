<template>
    <v-layout class="rounded rounded-md">
        <AppBar />
        <v-main class="justify-center">
            <RouterView />
        </v-main>
    </v-layout>
</template>

<script lang="js">
    import { defineComponent } from 'vue';
    import AppBar from '@/components/AppBar.vue';
    

    export default defineComponent({
        components: {
            AppBar
        },
        data() {
            return {
                info: 'Hello, Vue!',
            }
        },
        mounted() {
            this.test();
        },
        methods: {
            test() {
                console.log(`MainLayout: ${this.info}`);
            },
        },
    });
</script>

<style>
</style>