import { createRouter, createWebHistory } from 'vue-router';
import Cookies from 'js-cookie';

const routes = [
    {
        path: '/',
        name: 'index',
        component: () => import('@/layouts/MainLayout.vue'),
        meta: { requireAuth: true }, // �Ψӧ@�������O�_�ݭn�v�����Ҫ��]�w
        children: [
            {
                path: '/userlist',
                component: () => import('@/pages/UserInfo/UserListPage.vue'),
            },
            {
                path: '/hello',
                component: () => import('@/components/HelloWorld.vue'),
            },
            {
                path: '/',
                redirect: '/login'
            }
        ]
    },
    {
        path: '/login',
        component: () => import('@/pages/Auth/LoginPage.vue'),
    }
];

const router = createRouter({
    history: createWebHistory(),
    routes
});

router.beforeEach(async (to, from, next) => {
    // �ݬ� to �M from ��� arguments �|�R�^����T��
    //console.log('to: ', to)
    //console.log('from: ', from)

    // �ت����Ѧbmeta�W�O�_���]�mrequireAuth: true
    if (to.meta.requireAuth) {
        // ���Cookies����login��T�è��otoken
        const token = Cookies.get('login-token')
        //console.log(token)
        if (token) {
            // �p�Gtoken�����šA�B�T�꦳�o�����h�������ܧ�
            if (token.length > 0 || token === undefined) {
                next()
            } else {
                // ���q�L�h�ɦ^login����
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