<template>
    <v-container width="80vw">
        <v-sheet class="d-flex flex-wrap px-4 py-8 mb-4"
                 elevation="4"
                 rounded>
            <v-form>
                <v-row class="">
                    <v-col cols="12">
                        <h2 class="text-h4 font-weight-black">使用者列表</h2>
                    </v-col>
                    <v-col cols="12">
                        <v-text-field class="search-block-row"
                                      clearable
                                      label="使用者帳號"
                                      variant="outlined"></v-text-field>
                    </v-col>
                    <v-col cols="12">
                        <v-text-field class="search-block-row"
                                      clearable label="使用者名稱"
                                      variant="outlined"
                                      style="height:50px;"></v-text-field>
                    </v-col>
                    <v-col cols="12">
                        <v-select class="search-block-select-row"
                                  label="狀態"
                                  width="150"
                                  :items="['啟用', '暫停']"></v-select>
                    </v-col>
                    <v-col cols="12">
                        <v-select class="search-block-select-row"
                                  label="國籍"
                                  width="150"></v-select>
                    </v-col>
                    <v-col cols="12" class="search-block-row">
                        <v-btn prepend-icon="mdi-magnify"
                               elevation="4"
                               class="mr-4">
                            查詢
                        </v-btn>
                        <v-btn prepend-icon="mdi-broom"
                               elevation="4">
                            清除
                        </v-btn>
                    </v-col>
                </v-row>
            </v-form>
        </v-sheet>

        <v-sheet class="d-flex flex-wrap px-4 py-8 mb-4"
                 elevation="4"
                 height="60vh"
                 rounded>
            <v-data-table :headers="tableHeader" :items="userinfos" :search="searchKey" item-key="id" :loading="loading">
                <template v-slot:top>
                    <v-toolbar flat color="surface">
                        <v-spacer></v-spacer>
                        <v-btn class="mb-2"
                               prepend-icon="mdi-plus"
                               color="primary"
                               rounded
                               variant="outlined"
                               v-on:click="OpenCreateDialog">
                            新增
                        </v-btn>
                    </v-toolbar>
                </template>
                <template v-slot:item.status="{ item }">
                    <span>{{ item.status ? '啟用' : '未啟用' }}</span>
                </template>
                <template v-slot:item.edit="{ item }">
                    <v-btn color="primary"
                           prepend-icon="mdi-pencil"
                           rounded>修改</v-btn>
                </template>
                <template v-slot:item.delete="{ item }">
                    <v-btn color="error"
                           prepend-icon="mdi-close"
                           rounded>刪除</v-btn>
                </template>
            </v-data-table>
        </v-sheet>

        <UserCreateDialog v-if="isCreateDialogVisible" @close="CloseUserCreateDialog" />
            
            <!--v-model="create_dialog"
                      @update:modelValue="CloseUserCreateDialog"-->
        
    </v-container>
    

</template>

<script lang="js">
    import { defineComponent } from 'vue';
    import Cookies from 'js-cookie';
    import UserCreateDialog from '@/components/UserInfo/UserCreateDialog.vue';

    export default defineComponent({
        components: {
            UserCreateDialog
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
                    { title: '使用者帳號', key: 'username' },
                    { title: '使用者名稱', key: 'displayname' },
                    { title: '狀態', key: 'status' },
                    { title: '編輯', key: 'edit', sortable: false },
                    { title: '刪除', key: 'delete', sortable: false },
                ],

                searchKey: '',
                isCreateDialogVisible: false
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
                    .then(r => {
                        if (!r.ok) {
                            throw Error(`${r.status}@${r.statusText}`);
                        }
                        return r.json();
                    })
                    .then(json => {
                        this.userinfos = json.data;
                        console.log(this.userinfos);
                        this.loading = false;
                        return;
                    })
                    .catch(error => {
                        this.FetchErrorHandle(error);
                    });
            },

            OpenCreateDialog() {              
                this.isCreateDialogVisible = true;
            },

            CloseUserCreateDialog() {
                console.log('CloseUserCreateDialog()');
                this.isCreateDialogVisible = false;
                this.GetUserInfos();
            },

            FetchErrorHandle(error) {
                const errArr = error.toString().split('@');
                this.loading = false;
                const errorCode = errArr[0];
                if (errorCode.includes(401)) {
                    Cookies.remove(import.meta.env.VITE_COOKIE_LOGIN_TOKEN);
                    this.$router.push('/')
                }
            }
        },
    });
</script>

<style>
    .search-block-row {
        height: 50px;
    }

    .search-block-select-row {
        height: 70px;
    }
</style>