import { createRouter, createWebHistory } from 'vue-router';
import Cookies from 'js-cookie';

const routes = [
    {
        path: '/',
        name: 'index',
        component: () => import('@/layouts/MainLayout.vue'),
        meta: { requireAuth: true }, // 用來作為此頁是否需要權限驗證的設定
        children: [
            {
                path: '/userlist',
                component: () => import('@/pages/UserInfo/UserListPage.vue'),
            },
            {
                path: '/rolelist',
                component: () => import('@/pages/Role/RoleListPage.vue'),
            },
            {
                path: '/hello',
                component: () => import('@/components/HelloWorld.vue'),
            },
            {
                path: '/',
                redirect: '/login'
            },
        ]
    },
    {
        path: '/login',
        component: () => import('@/pages/Auth/LoginPage.vue'),
    },
    {
        path: '/logout',
        component: () => import('@/pages/Auth/LogoutPage.vue'),
        meta: { requireAuth: true }, // 用來作為此頁是否需要權限驗證的設定
    }
];

const router = createRouter({
    history: createWebHistory(),
    routes
});

router.beforeEach(async (to, from, next) => {
    // 看看 to 和 from 兩個 arguments 會吐回什麼訊息
    //console.log('to: ', to)
    //console.log('from: ', from)

    // 目的路由在meta上是否有設置requireAuth: true
    if (to.meta.requireAuth) {
        // 獲取Cookies當中的login資訊並取得token
        const token = Cookies.get(import.meta.env.VITE_COOKIE_LOGIN_TOKEN)
        //console.log(token)
        if (token) {
            // 如果token不為空，且確實有這個欄位則讓路由變更
            if (token.length > 0 || token === undefined) {
                next()
            } else {
                // 未通過則導回login頁面
                next('/login')
            }
        } else {
            next('/login')
        }
    } else {
        next()
    }
})

export default router;