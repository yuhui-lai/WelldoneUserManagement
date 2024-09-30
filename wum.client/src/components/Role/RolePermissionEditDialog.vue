<template>
    <v-dialog v-model="showDialog" max-width="500px" @click:outside="Close">
        <v-alert v-if="showAlert" type="error" dismissible rounded="lg">
            {{ errorMessage }}
        </v-alert>

        <v-card :loading="loading">
            <v-form ref="form" @submit.prevent="Edit">
                <template v-slot:progress>
                    <v-progress-linear color="deep-purple" height="10" indeterminate></v-progress-linear>
                </template>
                <v-card-title>編輯權限</v-card-title>
                <v-card-text>
                    <v-card-subtitle class="mb-4">角色名稱 {{ reqModel.roleName }}</v-card-subtitle>
                    <v-data-table :headers="tableHeader" :items="reqModel.rolePermissionItems" item-key="mainItem"
                        :loading="loading" fixed-header hide-default-footer :group-by="groupBy">
                        <template v-slot:group-header="{ item, columns, toggleGroup, isGroupOpen }">
                            <tr>
                                <td :colspan="columns.length">
                                    <v-row>
                                        <v-col cols="auto">
                                            <VBtn :icon="isGroupOpen(item) ? '$expand' : '$next'" size="small"
                                                variant="text" @click="toggleGroup(item)"></VBtn>
                                            {{ item.value }}
                                        </v-col>
                                        <v-col cols="auto">
                                            <v-checkbox @change="MainItemAllCheck($event, item.value)"></v-checkbox>
                                        </v-col>

                                    </v-row>

                                </td>
                            </tr>
                        </template>
                        <template v-slot:item.read="{ item }">
                            <v-checkbox v-model="item.read"></v-checkbox>
                        </template>
                        <template v-slot:item.edit="{ item }">
                            <v-checkbox v-model="item.write"></v-checkbox>
                        </template>
                        <template v-slot:item.delete="{ item }">
                            <v-checkbox v-model="item.delete"></v-checkbox>
                        </template>
                        <template v-slot:item.export="{ item }">
                            <v-checkbox v-model="item.export"></v-checkbox>
                        </template>
                    </v-data-table>

                </v-card-text>
                <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn type="submit" color="primary" variant="outlined" elevation="4">提交</v-btn>
                    <v-btn v-on:click="Close" color="info" variant="outlined" elevation="4">取消</v-btn>
                    <v-spacer></v-spacer>
                </v-card-actions>
            </v-form>
        </v-card>
    </v-dialog>
</template>

<script lang="js">
import { defineComponent } from 'vue';
import Cookies from 'js-cookie';
import Swal from 'sweetalert2';

export default defineComponent({
    components: {
    },
    directives: {
    },
    filters: {
    },
    props: {
        editId: {
            type: Number,
            required: true
        }
    },
    data() {
        return {
            showDialog: true,
            getPermissionApi: '/api/Role/Permission/',
            editRolePermissionApi: '/api/Role/EditPermission',
            tableHeader: [
                //{ title: '#', key: 'mainItem' },
                { title: '頁面', key: 'subItem', sortable: false },
                { title: '', key: 'all', sortable: false },
                { title: '讀取', key: 'read', sortable: false },
                { title: '編輯', key: 'edit', sortable: false },
                { title: '刪除', key: 'delete', sortable: false },
                { title: '匯出', key: 'export', sortable: false },
            ],
            groupBy: [
                {
                    key: 'mainItem',
                    order: 'asc',
                },
            ],
            reqModel: {},

            showAlert: false,
            alertType: 'error',
            errorMessage: '',
            loading: false,

            requiredRule: [v => !!v || '必填'],
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
        this.GetPermissionInfo();
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
        this.showAlert = false;
    },
    methods: {
        async Edit() {
            const { valid } = await this.$refs.form.validate();
            if (!valid)
                return;
            this.showAlert = false;
            this.loading = true;
            const token = Cookies.get(import.meta.env.VITE_COOKIE_LOGIN_TOKEN);
            let headers = {
                "Content-Type": "application/json",
                "Accept": "application/json",
                'Authorization': `Bearer ${token}`
            };
            this.reqModel.operatorId = Cookies.get(import.meta.env.VITE_COOKIE_USERID);
            fetch(this.editRolePermissionApi, {
                method: "PUT",
                headers: headers,
                body: JSON.stringify(this.reqModel)
            })
                .then(r => {
                    if (!r.ok) {
                        throw Error(`${r.status}@${r.statusText}`);
                    }
                    return r.json();
                })
                .then(json => {
                    let data = json.data;
                    this.SuccessAlert();
                    this.loading = false;
                    this.Close();
                    return;
                }).catch(error => {
                    this.FetchErrorHandle(error);
                });
        },

        Close() {
            this.showDialog = false;
            this.$emit('close');
        },

        SetAlert(isShow, type, msg) {
            this.showAlert = isShow;
            this.alertType = type;
            this.errorMessage = msg;
        },

        FetchErrorHandle(error) {
            console.log(`fetch error: ${error}`);
            const errArr = error.toString().split('@');
            this.loading = false;
            const errorCode = errArr[0];
            this.SetAlert(true, 'error', errArr[1]);
            if (errorCode.includes(401)) {
                Cookies.remove(import.meta.env.VITE_COOKIE_LOGIN_TOKEN);
                this.$router.push('/');
            }
        },

        GetPermissionInfo() {
            this.showAlert = false;
            this.loading = true;
            const token = Cookies.get(import.meta.env.VITE_COOKIE_LOGIN_TOKEN);
            let headers = {
                'Authorization': `Bearer ${token}`
            };
            fetch(`${this.getPermissionApi}${this.editId}`, {
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
                    this.reqModel = json.data;
                    this.loading = false;
                    return;
                }).catch(error => {
                    this.FetchErrorHandle(error);
                });
        },

        SuccessAlert() {
            Swal.fire({
                icon: "success",
                timer: 1000, // 彈窗顯示的時間 (2000 毫秒 = 2 秒)
                showConfirmButton: false, // 隱藏確認按鈕
                timerProgressBar: true, // 顯示進度條
                position: 'top-end', // 移到右上角
            });
        },

        MainItemAllCheck(event, mainItem){
            const checked = event.target.checked;
            this.reqModel.rolePermissionItems.forEach(i => {
                if (i.mainItem === mainItem){
                    i.read = checked;
                    i.write = checked;
                    i.delete = checked;
                    i.export = checked;
                }
            });
        }
    },
});
</script>

<style></style>
