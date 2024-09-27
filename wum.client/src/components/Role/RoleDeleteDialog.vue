<template>
    <v-dialog v-model="showDialog" max-width="500px" @click:outside="Close">
        <v-alert v-if="showAlert" type="error" dismissible rounded="lg">
            {{ errorMessage }}
        </v-alert>

        <v-card :loading="loading">
            <template v-slot:progress>
                <v-progress-linear color="deep-purple" height="10" indeterminate></v-progress-linear>
            </template>
            <v-card-title>刪除角色</v-card-title>
            <v-card-text>
                <p class="text-subtitle-1 font-weight-light text-center">
                    是否要刪除?
                </p>
            </v-card-text>
            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn variant="outlined" color="error" prepend-icon="mdi-delete-alert" rounded
                    v-on:click="Delete">確定</v-btn>
                <v-btn variant="outlined" v-on:click="Close" color="primary" prepend-icon="mdi-exit-to-app"
                    rounded>取消</v-btn>
                <v-spacer></v-spacer>
            </v-card-actions>
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
        delId: {
            type: Number,
            required: true
        }
    },
    data() {
        return {
            showDialog: true,
            showAlert: false,
            alertType: 'error',
            errorMessage: '',
            loading: false,
            deleteUserApi: '/api/Role/Delete',
            reqModel:{
                OperatorId: '',
                RoleId: ''
            }
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
        Delete() {
            this.showAlert = false;
            this.loading = true;
            const token = Cookies.get(import.meta.env.VITE_COOKIE_LOGIN_TOKEN);
            this.reqModel.OperatorId = Cookies.get(import.meta.env.VITE_COOKIE_USERID);
            this.reqModel.RoleId = this.delId;
            let headers = {
                "Content-Type": "application/json",
                "Accept": "application/json",
                'Authorization': `Bearer ${token}`
            };

            fetch(this.deleteUserApi, {
                method: "DELETE",
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
                    console.log(`Delete(): ${data}`);
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

        SuccessAlert() {
            Swal.fire({
                icon: "success",
                timer: 1000, // 彈窗顯示的時間 (2000 毫秒 = 2 秒)
                showConfirmButton: false, // 隱藏確認按鈕
                timerProgressBar: true, // 顯示進度條
                position: 'top-end', // 移到右上角
            });
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
    },
});
</script>

<style></style>
