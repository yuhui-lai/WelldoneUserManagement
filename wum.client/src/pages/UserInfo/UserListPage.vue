<template>
    <v-container width="100vw">
        <v-sheet class="px-4 py-8 mb-4" elevation="4" rounded>
            <v-form>
                <v-row>
                    <v-col>
                        <h1 class="font-weight-black">使用者列表</h1>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col cols="4" class="text-right">
                        <p class="text-h6 font-weight-medium">
                            使用者帳號
                        </p>
                    </v-col>
                    <v-col cols="6">
                        <v-text-field class="search-block-row" clearable variant="outlined"
                            v-model="searchKey.Username"></v-text-field>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col cols="4" class="text-right">
                        <p class="text-h6 font-weight-medium">
                            使用者名稱
                        </p>
                    </v-col>
                    <v-col cols="6">
                        <v-text-field class="search-block-row" clearable variant="outlined"
                            v-model="searchKey.Displayname"></v-text-field>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col cols="4" class="text-right">
                        <p class="text-h6 font-weight-medium">
                            狀態
                        </p>
                    </v-col>
                    <v-col cols="6">
                        <v-select class="search-block-select-row" :items="statusOptions"
                            v-model="searchKey.Status"></v-select>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col cols="4" class="text-right">
                        <p class="text-h6 font-weight-medium">
                            國籍
                        </p>
                    </v-col>
                    <v-col cols="6">
                        <VueSelect v-model="searchKey.Country" :options="countryList" />
                    </v-col>
                </v-row>
                <v-row>
                    <v-col cols="4" class="text-right">
                        <p class="text-h6 font-weight-medium"></p>
                    </v-col>
                    <v-col cols="8" class="search-block-row">
                        <v-btn color="primary" prepend-icon="mdi-magnify" elevation="4" class="mr-4"
                            v-on:click="GetUserInfos">
                            查詢
                        </v-btn>
                        <v-btn prepend-icon="mdi-broom" elevation="4">
                            清除
                        </v-btn>
                    </v-col>
                </v-row>
            </v-form>
        </v-sheet>

        <v-sheet class="d-flex px-4 py-8 mb-4" elevation="4" height="60vh" rounded>
            <v-data-table :headers="tableHeader" :items="userinfos" item-key="id" :loading="loading" fixed-header>
                <template v-slot:top>
                    <v-toolbar flat color="surface">
                        <v-spacer></v-spacer>
                        <v-btn class="mb-2" prepend-icon="mdi-plus" color="info" rounded variant="outlined"
                            v-on:click="OpenCreateDialog" elevation="4">
                            新增
                        </v-btn>
                    </v-toolbar>
                </template>
                <template v-slot:item.status="{ item }">
                    <span>{{ item.status ? '啟用' : '未啟用' }}</span>
                </template>
                <template v-slot:item.edit="{ item }">
                    <v-btn color="primary" prepend-icon="mdi-pencil" rounded v-on:click="OpenEditDialog(item.id)"
                        elevation="4">修改</v-btn>
                </template>
                <template v-slot:item.delete="{ item }">
                    <v-btn color="warning" prepend-icon="mdi-close" rounded v-on:click="OpenDeleteDialog(item.id)"
                        elevation="4">刪除</v-btn>
                </template>
            </v-data-table>
        </v-sheet>
        <UserCreateDialog v-if="isCreateDialogVisible" @close="CloseUserCreateDialog" />
        <UserDeleteDialog :delId="deleteId" v-if="isDeleteDialogVisible" @close="CloseUserDeleteDialog" />
        <UserEditDialog :editId="edit_Id" v-if="isEditDialogVisible" @close="CloseUserEditDialog" />
    </v-container>


</template>

<script lang="js">
    import { defineComponent } from 'vue';
    import Cookies from 'js-cookie';
    import VueSelect from "vue3-select-component";
    import UserCreateDialog from '@/components/UserInfo/UserCreateDialog.vue';
    import UserDeleteDialog from '@/components/UserInfo/UserDeleteDialog.vue';
    import UserEditDialog from '@/components/UserInfo/UserEditDialog.vue';

    export default defineComponent({
        components: {
            UserCreateDialog,
            VueSelect,
            UserDeleteDialog,
            UserEditDialog,
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
                getCountriesApi: '/api/Country',
                userinfos: [],
                tableHeader: [
                    { title: '#', key: 'id' },
                    { title: '使用者帳號', key: 'username' },
                    { title: '使用者名稱', key: 'displayname' },
                    { title: '狀態', key: 'status' },
                    { title: '編輯', key: 'edit', sortable: false },
                    { title: '刪除', key: 'delete', sortable: false },
                ],
                searchKey: {
                    Username: '',
                    Displayname: '',
                    Status: '',
                    Country: ''
                },  
                statusOptions: [
                    { title: '', value: '' },
                    { title: '啟用', value: true },
                    { title: '未啟用', value: false }
                ], 
                //country: '',
                countryList: [],
                // 新增
                isCreateDialogVisible: false,
                // 刪除
                isDeleteDialogVisible: false,
                deleteId: '',
                // 編輯
                isEditDialogVisible: false,
                edit_Id: '',
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
            this.GetCountries();
            this.GetUserInfos();
        },
        updated() {
        },
        activated() {
        },
        deactivated() {
        },
        beforeUnmount() {
        },
        unmounted() {
        },
        methods: {
            GetUserInfos() {
                this.loading = true;
                let token = Cookies.get(import.meta.env.VITE_COOKIE_LOGIN_TOKEN);
                //console.log(token);
                let headers = {
                    'Authorization': `Bearer ${token}`
                };

                let url = this.userInfoApi + "?";
                url += (this.searchKey.Username? `Username=${this.searchKey.Username}&` :"");
                url += (this.searchKey.Displayname? `Displayname=${this.searchKey.Displayname}&` :"");
                url += (this.searchKey.Status!=null? `Status=${this.searchKey.Status}&` :"");
                url += (this.searchKey.Country? `Country=${this.searchKey.Country}&` :"");
                //console.log(url);
                fetch(url, {
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
                //console.log('CloseUserCreateDialog()');
                this.isCreateDialogVisible = false;
                this.GetUserInfos();
            },

            OpenDeleteDialog(id) {
                this.deleteId = id;
                this.isDeleteDialogVisible = true;
            },

            CloseUserDeleteDialog() {
                this.isDeleteDialogVisible = false;
                this.deleteId = '';
                this.GetUserInfos();
            },

            OpenEditDialog(id) {
                this.edit_Id = id;
                this.isEditDialogVisible = true;
            },

            CloseUserEditDialog() {
                this.isEditDialogVisible = false;
                this.edit_Id = '';
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
            },

            GetCountries() {
                this.showAlert = false;
                this.loading = true;
                fetch(this.getCountriesApi, {
                    method: "GET",
                })
                    .then(r => {
                        if (!r.ok) {
                            throw Error(`${r.status}@${r.statusText}`);
                        }
                        return r.json();
                    })
                    .then(json => {
                        this.countryList = json.data;
                        this.loading = false;
                        return;
                    }).catch(error => {
                        this.FetchErrorHandle(error);
                    });
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