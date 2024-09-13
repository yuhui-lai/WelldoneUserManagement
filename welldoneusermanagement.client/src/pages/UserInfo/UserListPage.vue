<template>
    <v-sheet class="d-flex flex-wrap px-4 py-8 mb-4"
             elevation="4"
             height="40vh"
             rounded>
        <div>
            <h2 class="text-h4 font-weight-black">使用者列表</h2>

            <v-text-field clearable label="用戶名" variant="outlined" width="60vw" class="mt-4"></v-text-field>
            <v-select label="狀態"
                      :items="['啟用', '暫停']"></v-select>
            <v-text-field clearable label="電子郵件" variant="outlined"></v-text-field>

            <v-btn prepend-icon="$vuetify" elevation="4" class="mr-4">
                查詢
            </v-btn>
            <v-btn prepend-icon="$vuetify" elevation="4">
                清除
            </v-btn>
        </div>
    </v-sheet>

    <v-sheet class="d-flex flex-wrap px-4 py-8 mb-4"
             elevation="4"
             height="40vh"
             rounded>
        <v-data-table :headers="tableHeader" :items="userinfos" :search="searchKey" item-key="id" :loading="loading">
            <template v-slot:item.edit="{ item }">
                <v-btn color="primary" rounded>修改</v-btn>
            </template>
            <template v-slot:item.delete="{ item }">
                <v-btn color="error" rounded>刪除</v-btn>
            </template>
        </v-data-table>
    </v-sheet>
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
                loading: false,
                userInfoApi: '/api/UserInfo',
                userinfos: [],

                tableHeader: [
                    { title: '#', key: 'id' },
                    { title: '姓名', key: 'displayname' },
                    { title: '用戶名', key: 'username' },
                    { title: 'Email', key: 'email' },
                    { title: '狀態', key: 'status' },
                    { title: '權限', key: 'roles' },
                    { title: '編輯', key: 'edit', sortable: false },
                    { title: '刪除', key: 'delete', sortable: false },
                ],

                searchKey: '',

            }
        },
        computed: {
        },
        watch: {
        },
        beforeCreate() {
        },
        created() {
        },
        beforeMount() {
        },
        mounted() {
            this.GetUserInfos();
        },
        updated() {
        },
        activated() {
        },
        deactivated() {
        },
        beforeDestroy() {
        },
        destroyed() {
        },
        methods: {
            GetUserInfos() {
                this.loading = true;
                let token = Cookies.get('login-token');

                let headers = {
                    'Authorization': `Bearer ${token}`
                };

                fetch(this.userInfoApi, {
                    method: 'GET',
                    headers: headers,
                })
                    .then(r => r.json())
                    .then(json => {
                        this.userinfos = json.data;
                        console.log(this.userinfos);
                        this.loading = false;
                        return;
                    })
                    .catch(error => {
                        console.error('Error:', error);
                    });
            },

        },
    });
</script>

<style>
</style>