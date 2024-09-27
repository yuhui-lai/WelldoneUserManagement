<template>
    <v-container width="100vw">
        <v-sheet class="px-4 py-8 mb-4" elevation="4" rounded>
            <v-form>
                <v-row>
                    <v-col>
                        <h1 class="font-weight-black">角色管理</h1>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col cols="4" class="text-right">
                        <p class="text-h6 font-weight-medium">
                            產品別
                        </p>
                    </v-col>
                    <v-col cols="6">
                        <v-select class="search-block-select-row" :items="productOptions"
                            v-model="searchKey.ProductCode"></v-select>
                    </v-col>
                </v-row>
                <v-row>
                    <v-col cols="4" class="text-right">
                        <p class="text-h6 font-weight-medium"></p>
                    </v-col>
                    <v-col cols="8" class="search-block-row">
                        <v-btn color="primary" prepend-icon="mdi-magnify" elevation="4" class="mr-4"
                            v-on:click="GetRoleList">
                            查詢
                        </v-btn>
                        <v-btn prepend-icon="mdi-broom" elevation="4" v-on:click="ClearFilter">
                            清除
                        </v-btn>
                    </v-col>
                </v-row>
            </v-form>
        </v-sheet>

        <v-sheet class="d-flex px-4 py-8 mb-4" elevation="4" height="60vh" rounded>
            <v-data-table :headers="tableHeader" :items="roleList" item-key="id" :loading="loading" fixed-header>
                <template v-slot:top>
                    <v-toolbar flat color="surface">
                        <v-spacer></v-spacer>
                        <v-btn class="mb-2" prepend-icon="mdi-plus" color="info" rounded variant="outlined"
                            v-on:click="OpenCreateDialog" elevation="4">
                            新增
                        </v-btn>
                    </v-toolbar>
                </template>
                <template v-slot:item.permission="{ item }">
                    <v-btn color="primary" prepend-icon="mdi-account-key" rounded
                        v-on:click="OpenPermissionEditDialog(item.id)" elevation="4">權限修改</v-btn>
                </template>
                <template v-slot:item.edit="{ item }">
                    <v-btn color="secondary" prepend-icon="mdi-pencil" rounded v-on:click="OpenBasicEditDialog(item.id)"
                        elevation="4">修改</v-btn>
                </template>
                <template v-slot:item.delete="{ item }">
                    <v-btn color="warning" prepend-icon="mdi-close" rounded v-on:click="OpenDeleteDialog(item.id)"
                        elevation="4">刪除</v-btn>
                </template>
            </v-data-table>
        </v-sheet>
        <RoleCreateDialog v-if="isCreateDialogVisible" @close="CloseUserCreateDialog" />
        <RoleDeleteDialog :delId="delId" v-if="isDeleteDialogVisible" @close="CloseUserDeleteDialog" />
        <RoleBasicEditDialog :editId="editBasicId" v-if="isEditBasicInfoDialogVisible" @close="CloseBasicEditDialog" />
        <RolePermissionEditDialog :editId="editPermissionId" v-if="isEditPermissionDialogVisible"
            @close="ClosePermissionEditDialog" />
    </v-container>


</template>

<script lang="js">
import { defineComponent } from 'vue';
import Cookies from 'js-cookie';
import RoleCreateDialog from '@/components/Role/RoleCreateDialog.vue';
import RoleDeleteDialog from '@/components/Role/RoleDeleteDialog.vue';
import RoleBasicEditDialog from '@/components/Role/RoleBasicEditDialog.vue';
import RolePermissionEditDialog from '@/components/Role/RolePermissionEditDialog.vue';

export default defineComponent({
    components: {
        RoleCreateDialog,
        RoleDeleteDialog,
        RoleBasicEditDialog,
        RolePermissionEditDialog
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
            roleListApi: '/api/Role/List',
            productClassesApi: '/api/Role/ProductClasses',
            roleList: [],
            tableHeader: [
                { title: '#', key: 'id' },
                { title: '產品別', key: 'productName' },
                { title: '角色名稱', key: 'roleName' },
                { title: '權限設定', key: 'permission', sortable: false },
                { title: '編輯', key: 'edit', sortable: false },
                { title: '刪除', key: 'delete', sortable: false },
            ],
            searchKey: {
                ProductCode: ''
            },
            productOptions: [
                { title: '', value: '' }
            ],
            // 新增
            isCreateDialogVisible: false,
            // 刪除
            isDeleteDialogVisible: false,
            delId: '',
            // 編輯
            isEditBasicInfoDialogVisible: false,
            editBasicId: '', 
            isEditPermissionDialogVisible: false,
            editPermissionId: '',
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
        this.GetProductClasses();
        this.GetRoleList();
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
        GetRoleList() {
            this.loading = true;
            const token = Cookies.get(import.meta.env.VITE_COOKIE_LOGIN_TOKEN);
            //console.log(token);
            let headers = {
                'Authorization': `Bearer ${token}`
            };

            let url = this.roleListApi + "?";
            url += (this.searchKey.ProductCode ? `ProductCode=${this.searchKey.ProductCode}&` : "");
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
                    this.roleList = json.data;
                    //console.log(this.roleList);
                    this.loading = false;
                    return;
                })
                .catch(error => {
                    this.FetchErrorHandle(error);
                });
        },

        ClearFilter(){
            this.searchKey = {
                ProductCode: ''
            };
        },

        OpenCreateDialog() {
            this.isCreateDialogVisible = true;
        },

        CloseUserCreateDialog() {
            //console.log('CloseUserCreateDialog()');
            this.isCreateDialogVisible = false;
            this.GetRoleList();
        },

        OpenDeleteDialog(id) {
            this.delId = id;
            this.isDeleteDialogVisible = true;
        },

        CloseUserDeleteDialog() {
            this.isDeleteDialogVisible = false;
            this.delId = '';
            this.GetRoleList();
        },

        OpenPermissionEditDialog(id) {
            this.editPermissionId = id;
            this.isEditPermissionDialogVisible = true;
        },

        ClosePermissionEditDialog() {
            this.isEditPermissionDialogVisible = false;
            this.editPermissionId = '';
        },

        OpenBasicEditDialog(id) {
            this.editBasicId = id;
            this.isEditBasicInfoDialogVisible = true;
        },

        CloseBasicEditDialog() {
            this.isEditBasicInfoDialogVisible = false;
            this.editBasicId = '';
            this.GetRoleList();
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

        GetProductClasses() {
            this.showAlert = false;
            this.loading = true;
            const token = Cookies.get(import.meta.env.VITE_COOKIE_LOGIN_TOKEN);
            //console.log(token);
            let headers = {
                'Authorization': `Bearer ${token}`
            };
            fetch(this.productClassesApi, {
                method: "GET",
                headers: headers
            })
                .then(r => {
                    if (!r.ok) {
                        throw Error(`${r.status}@${r.statusText}`);
                    }
                    return r.json();
                })
                .then(json => {
                    const productAllOption = [{ title: '', value: '' }];
                    this.productOptions = productAllOption.concat(json.data);
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