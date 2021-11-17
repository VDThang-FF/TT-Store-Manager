import { createRouter, createWebHistory } from "vue-router";
import MainLayout from '@/layouts/main/Index.vue';

const routes = [
    {
        path: '/',
        name: 'DashboardLayout',
        component: MainLayout,
        redirect: { name: "Dashboard" },
        children: [
            {
                path: '',
                name: 'Dashboard',
                component: () => import('@/views/dashboard/Index.vue')
            }
        ]
    }, 
    {
        path: '/product',
        name: 'ProductLayout',
        component: MainLayout,
        redirect: { name: "Product" },
        children: [
            {
                path: '',
                name: 'Product',
                component: () => import('@/views/product/Index.vue')
            }
        ]
    },
    {
        path: '/send-product',
        name: 'SendProductLayout',
        component: MainLayout,
        redirect: { name: "SendProduct" },
        children: [
            {
                path: '',
                name: 'SendProduct',
                component: () => import('@/views/sendproduct/Index.vue')
            }
        ]
    },
    {
        path: '/material',
        name: 'MaterialLayout',
        component: MainLayout,
        redirect: { name: "Material" },
        children: [
            {
                path: '',
                name: 'Material',
                component: () => import('@/views/material/Index.vue')
            }
        ]
    }
]

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes
});

export default router;