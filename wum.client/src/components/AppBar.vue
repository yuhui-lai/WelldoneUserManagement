<template>
    <v-app-bar color="primary" prominent>
        <v-app-bar-nav-icon variant="text" @click.stop="drawer = !drawer"></v-app-bar-nav-icon>
        <v-toolbar-title>使用者權限及管理系統</v-toolbar-title>
        <v-spacer></v-spacer>
        <!--<v-btn icon="mdi-dots-vertical" variant="text"></v-btn>-->
        <v-menu open-on-hover>
            <template v-slot:activator="{ props }">
                <v-btn icon="mdi-dots-vertical" variant="text" v-bind="props"></v-btn>
            </template>
            <v-list>
                <v-list-item>
                    <v-list-item-title>
                        <v-btn prepend-icon="mdi-logout" :to="'/logout'" block>
                            登出
                        </v-btn>
                    </v-list-item-title>
                </v-list-item>
            </v-list>
        </v-menu>
    </v-app-bar>

    <v-navigation-drawer v-model="drawer" temporary>
        <v-list>
            <v-list-item>
                <v-btn prepend-icon="mdi-account-details" :to="'/userlist'" block>
                    使用者列表
                </v-btn>
            </v-list-item>
            <v-list-item>
                <v-btn prepend-icon="mdi-test-tube" :to="'/hello'" block>
                    Hello
                </v-btn>
            </v-list-item>
        </v-list>

    </v-navigation-drawer>

</template>

<script lang="js">
import { defineComponent } from 'vue';

export default defineComponent({
    data() {
        return {
            drawer: false,
        }
    },
    watch: {
        group() {
            this.drawer = false
        },
    }
});
</script>

<style></style>