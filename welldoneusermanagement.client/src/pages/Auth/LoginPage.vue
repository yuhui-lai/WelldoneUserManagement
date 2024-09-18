<template>
    <v-container class="fill-height" fluid>
        <v-row align="center" justify="center">
            <v-col cols="12" sm="10" md="8" lg="6" xl="6">
                <v-alert v-if="error" 
                         type="error"
                         dismissible
                         max-width="450"
                         min-width="390"
                         class="elevation-12 pa-4 mx-auto"
                         rounded="lg">
                    {{ errorMessage }}
                </v-alert>
                <v-card class="elevation-12 pa-8 mx-auto" rounded="lg" max-width="450" min-width="390">
                    <v-card-title class="text-h6 font-weight-bold justify-center mb-4 text-wrap text-center">
                        Welldone 使用者及權限管理系統
                    </v-card-title>
                    <v-card-text>
                        <v-form ref="form" @submit.prevent="PasswordLogin">
                            <div v-if="isPasswordLogin">
                                <v-text-field v-model="account"
                                              label="username"
                                              prepend-inner-icon="mdi-account-circle"
                                              type="email"
                                              required
                                              variant="outlined"
                                              class="mb-4"
                                              :rules="usernameRule"></v-text-field>
                                <v-text-field v-model="password"
                                              label="password"
                                              prepend-inner-icon="mdi-lock"
                                              :type="showPassword ? 'text' : 'password'"
                                              required
                                              variant="outlined"
                                              :append-inner-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
                                              @click:append-inner="showPassword = !showPassword"
                                              :rules="passwordRule"></v-text-field>

                                <v-btn type="submit"
                                       color="primary"
                                       block
                                       class="mt-4"
                                       size="large"
                                       :loading="loading">
                                    登入
                                </v-btn>

                                <v-btn 
                                       color="background"
                                       block
                                       class="mt-4"
                                       size="large"
                                       prepend-icon="mdi-qrcode-scan"
                                       @click="SwitchQrcodeLogin">
                                    Qrcode 登入
                                </v-btn>
                            </div>
                            <div v-else>
                                <div v-if="error"
                                     class="d-flex align-center justify-center">
                                    <v-btn color="error"
                                           class="mt-4"
                                           prepend-icon="mdi-refresh"
                                           @click="SwitchQrcodeLogin">
                                        重新產生Qrcode
                                    </v-btn>
                                </div>
                                <div v-else
                                     class="d-flex align-center justify-center">
                                    <div id="qrcodeLoading-div" v-if="qrcodeLoading" class="d-flex align-center justify-center">
                                        <v-progress-circular color="primary"
                                                             :size="50"
                                                             :width="7"
                                                             indeterminate></v-progress-circular>
                                    </div>

                                    <vue-qr v-else :text="qrcodeImgSrc" :size="200"></vue-qr>
                                </div>
                                <v-btn color="background"
                                       block
                                       class="mt-4"
                                       size="large"
                                       prepend-icon="mdi-keyboard-close"
                                       @click="SwitchPasswordLogin">
                                    帳密登入
                                </v-btn>
                            </div>
                        </v-form>
                    </v-card-text>
                </v-card>
            </v-col>
        </v-row>
    </v-container>
</template>

<script lang="js">
    import { defineComponent } from 'vue';
    import Cookies from 'js-cookie';
    import vueQr from 'vue-qr/src/packages/vue-qr.vue';

    export default defineComponent({
        components: {
            vueQr
        },
        directives: {
        },
        filters: {
        },
        props: {
        },
        data() {
            return {
                // 帳密登入
                loginApi: 'api/Auth/PasswordLogin',
                account: '',
                password: '',
                showPassword: false,
                loading: false,
                error: false,
                errorMessage: '',
                isPasswordLogin: true,
                usernameRule: [v => !!v || '必填'],
                passwordRule: [v => !!v || '必填'],
                // qrcode登入
                qrcodeLoginPrepareApi: 'api/Auth/QrcodeLoginPrepare',
                qrcodeLoginApi: 'api/Auth/QrcodeLogin',
                qrcodeImgSrc: '',
                qrcodeLoading: false
            }
        },
        mounted() {
        },
        methods: {
            // 帳密登入
            async PasswordLogin() {
                const { valid } = await this.$refs.form.validate();
                if (!valid)
                    return;
                this.loading = true;
                this.error = false;

                let headers = {
                    "Content-Type": "application/json",
                    "Accept": "application/json"
                };
                let body = {
                    "Username": this.account,
                    "Password": this.password
                };

                fetch(this.loginApi, {
                    method: "POST",
                    headers: headers,
                    body: JSON.stringify(body)
                })
                    .then(r => r.json())
                    .then(json => {
                        if (json.success) {
                            let token = json.data.token;
                            //console.log(`token: ${token}`);
                            this.error = false;
                            this.LoginCookie(token);
                            if(Cookies.get('login-token')){
                                this.$router.push('/userlist')
                            }
                        }
                        else {
                            this.error = true;
                            this.errorMessage = '登入失敗，請檢查您的帳號和密碼。';
                        }
                        this.loading = false;
                        return;
                    });
            },
            // 登入存cookie
            LoginCookie(token) {
                Cookies.set('login-token', token, { expires: 1 });
            },
            // 切換至Qrcode登入
            SwitchQrcodeLogin() {
                this.isPasswordLogin = false;
                this.qrcodeImgSrc = '';
                this.QrcodeLoginPrepare();
            },
            // 切換至帳密登入
            SwitchPasswordLogin() {
                this.isPasswordLogin = true;
            },
            // Qrcode登入準備
            QrcodeLoginPrepare() {
                this.error = false;
                this.qrcodeLoading = true;
                fetch(this.qrcodeLoginPrepareApi, {
                    method: "POST"
                })
                    .then(r => r.json())
                    .then(json => {
                        if (json.success) {
                            console.log(`qrcodeGuid: ${json.data.qrcodeGuid}`);
                            this.qrcodeImgSrc = json.data.qrcodeGuid;
                            this.qrcodeLoading = false;
                            const guid = this.qrcodeImgSrc.split(':')[1];
                            this.QrSse(guid);
                        }
                        else {
                            this.error = true;
                            this.errorMessage = '登入失敗';
                        }
                    })
                    .catch((error) => {
                        this.error = true;
                        this.errorMessage = '登入失敗';
                        console.log(error);
                    });
            },
            // 訂閱Qrcode登入SSE
            QrSse(guid) {
                console.log(`VITE_QRCODE_LOGIN_SSE:${import.meta.env.VITE_QRCODE_LOGIN_SSE}`);
                let source = new EventSource(`${import.meta.env.VITE_QRCODE_LOGIN_SSE}/${guid}`);
                source.addEventListener("message", (e) => {
                    console.log(`e.data: ${e.data}`);
                    this.QrcodeLogin(guid, e.data);
                });
                source.addEventListener("error", (e) => {
                    console.log(`e.data: ${e.data}`);
                });
            },
            // Qrcode登入
            QrcodeLogin(guid, tempToken) {      
                this.error = false;
                let headers = {
                    "Content-Type": "application/json",
                    "Accept": "application/json"
                };
                let body = {
                    "Guid": guid,
                    "TempToken": tempToken
                };

                fetch(this.qrcodeLoginApi, {
                    method: "POST",
                    headers: headers,
                    body: JSON.stringify(body)
                })
                    .then(r => r.json())
                    .then(json => {
                        if (json.success) {
                            let token = json.data.token;
                            //console.log(`token: ${token}`);
                            this.error = false;
                            this.LoginCookie(token);
                            if(Cookies.get('login-token')){
                                this.$router.push('/userlist')
                            }
                        }
                        else {
                            this.error = true;
                            this.errorMessage = '登入失敗';
                        }
                    })
                    .catch((error) => {
                        this.error = true;
                        this.errorMessage = '登入失敗';
                        console.log(error);
                    });
            }
        },
    });
</script>

<style>
    #qrcodeLoading-div {
        height: 200px;
        width: 200px
    }

    .v-application {
        background-color: #f5f5f5 !important;
    }

    .v-card {
        width: 100%;
    }

    @media (max-width: 600px) {
        .v-card {
            margin: 0 16px;
        }
    }
</style>