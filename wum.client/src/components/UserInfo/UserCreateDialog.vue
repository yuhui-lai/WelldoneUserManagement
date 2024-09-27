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
                <v-card-title>新增用戶</v-card-title>
                <v-card-text>

                    <v-card-subtitle class="mb-4">基本資訊</v-card-subtitle>
                    <v-text-field label="*使用者帳號" v-model="username" :rules="requiredRule"></v-text-field>
                    <v-text-field label="*使用者名稱" v-model="displayname" :rules="requiredRule"></v-text-field>
                    <v-text-field label="*電子郵件" v-model="email" :rules="requiredRule.concat(emailRule)"></v-text-field>
                    <VueSelect v-model="country" :options="countryList" placeholder="國籍" />

                    <v-card-subtitle class="mt-6 mb-4">帳戶狀態</v-card-subtitle>
                    <v-select :items="statusOptions" item-text="label" item-value="value" label="狀態" v-model="status"
                        outlined></v-select>

                    <v-card-subtitle class="mb-4">安全資訊</v-card-subtitle>
                    <v-text-field v-model="password" label="密碼" prepend-inner-icon="mdi-lock"
                        :type="showPassword ? 'text' : 'password'" variant="outlined"
                        :append-inner-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
                        @click:append-inner="showPassword = !showPassword" :rules="requiredRule"></v-text-field>
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
    import VueSelect from "vue3-select-component";
    import Swal from 'sweetalert2';

    export default defineComponent({
        components: {
            VueSelect
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
                getCountriesApi: '/api/Country',
                createUserApi: '/api/UserInfo',
                username: '',
                displayname: '',
                email: '',
                country: '',
                status: true,
                statusOptions: [
                    { title: '啟用', value: true },
                    { title: '未啟用', value: false }
                ],
                password: '',
                showPassword: false,
                showAlert: false,
                alertType: 'error',
                errorMessage: '',
                loading: false,
                countryList: [],

                emailRule: [(v) => /.+@.+\..+/.test(v) || '不是正確的email'],
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
            this.Init();
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
            Init() {
                this.username = '';
                this.displayname = '';
                this.email = '';
                this.country = '';
                this.status = true;
                this.password = '';
                this.showPassword = false;
                this.showAlert = false;
                this.loading= false;
                this.GetCountries();
            },

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
                const operatorId = Cookies.get(import.meta.env.VITE_COOKIE_USERID);
                let body = {
                    "OperatorId": operatorId,
                    "Username": this.username,
                    "DisplayName": this.displayname,
                    "Email": this.email,
                    "Country": this.country,
                    "Status": this.status,
                    "Password": this.password
                };
                
                fetch(this.createUserApi, {
                    method: "POST",
                    headers: headers,
                    body: JSON.stringify(body)
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

<style>
</style>
