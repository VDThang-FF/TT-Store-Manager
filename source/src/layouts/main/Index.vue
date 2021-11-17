<template>
    <div class="main-header">
        <div class="main-logo p-d-flex p-jc-center p-ai-center">
            <img src="@/assets/images/logo.png" />
        </div>
    </div>
    <div class="main-menu">
        <!-- <Menu :model="items"></Menu> -->
        <Menu :model="items" class="main-menu-box">
            <template #item="{ item }">
                <router-link
                    :to="item.to"
                    custom
                    v-slot="{ href, navigate, isActive, isExactActive }"
                >
                    <div
                        @click="navigate"
                        :class="{ 'active-link': isActive, 'active-link-exact': isExactActive }"
                        :href="href"
                        class="item-menu-box p-d-flex p-ai-center"
                    >
                        <i :class="item.icon" class="icon-menu p-mr-2"></i>
                        {{ item.label }}
                    </div>
                </router-link>
            </template>
        </Menu>
    </div>
    <div class="main-content">
        <router-view></router-view>
    </div>
</template>

<script setup lang="ts">
import Menu from 'primevue/menu';
import { ref } from 'vue'

const items = [
    {
        label: 'Tổng quan',
        icon: 'pi pi-chart-bar',
        to: '/'
    },
    {
        label: 'Gửi hàng',
        icon: 'pi pi-shopping-cart',
        to: '/send-product'
    },
    {
        label: 'Kho hàng',
        icon: 'pi pi-wallet',
        to: '/material'
    },
    {
        label: 'Sản phẩm',
        icon: 'pi pi-book',
        to: '/product'
    }
];
</script>

<style lang="scss" scoped>
.main-header {
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    height: 52px;
    z-index: 10;
    background-color: $B-White;
    box-sizing: border-box;
    box-shadow: 0 0 4px $BX-Shadow-Primary;

    .main-logo {
        position: absolute;
        top: 0;
        left: 0;
        bottom: 0;
        width: 200px;
    }
}

.main-menu {
    position: absolute;
    top: 52px;
    left: 0px;
    width: 200px;
    bottom: 0px;
    z-index: 5;
    background-color: $B-White;
    border-right: 1px solid $BD-Border-Primary;
    box-sizing: border-box;
    padding: 10px;
}

.main-content {
    position: absolute;
    left: 200px;
    top: 52px;
    bottom: 0px;
    right: 0px;
    z-index: 1;
    box-sizing: border-box;
    background-color: $BG-Gray;
    padding: 24px;
}
</style>