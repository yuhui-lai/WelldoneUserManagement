<template>
    <v-dialog v-model="showDialog" max-width="500px" @click:outside="Close">
        <v-alert v-if="showAlert" type="error" dismissible rounded="lg">
            {{ errorMessage }}
        </v-alert>

        <v-card :loading="loading">
            <v-form ref="form" @submit.prevent="Create">
                <template v-slot:progress>
                    <v-progress-linear color="deep-purple" height="10" indeterminate></v-progress-linear>
                </template>
                <v-card-title>新增角色</v-card-title>
                <v-card-text>
                    <v-card-subtitle class="mb-4">產品別</v-card-subtitle>
                    <v-select :items="productOptions" item-text="label" item-value="value"
                        v-model="reqModel.ProductCode" outlined :rules="requiredRule"></v-select>
                    <v-card-subtitle class="mb-4">角色名稱</v-card-subtitle>
                    <v-text-field v-model="reqModel.RoleName" :rules="requiredRule"></v-text-field>
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
    },
    data() {
        return {
            showDialog: true,
            productClassesApi: '/api/Role/ProductClasses',
            createRoleApi: '/api/Role/Create',
            reqModel:{
                OperatorId: '',
                ProductCode: '',
                RoleName: ''
            },
            productOptions: [
                { title: '', value: '' }
            ],

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
        this.GetProductClasses();
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
        async Create() {
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
            this.reqModel.OperatorId = Cookies.get(import.meta.env.VITE_COOKIE_USERID);

            fetch(this.createRoleApi, {
                method: "POST",
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
        },

        SuccessAlert() {
            Swal.fire({
                icon: "success",
                timer: 1000, // 彈窗顯示的時間 (2000 毫秒 = 2 秒)
                showConfirmButton: false, // 隱藏確認按鈕
                timerProgressBar: true, // 顯示進度條
                position: 'top-end', // 移到右上角
            });
        }
    },
});
</script>

<style></style>
