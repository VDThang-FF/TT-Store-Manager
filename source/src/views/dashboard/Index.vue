<template>
    <div>Tính năng đang trong quá trình phát triển</div>
</template>

<script setup lang="ts">
import GetPrototype from '@/assets/scripts/prototype/GetPrototype';
import ENUM from '@/enums';

const Prototype = new GetPrototype();
if (Prototype != null)
    console.log(Prototype.globals().$CompareObj({ name: "Thắng", des: { demo: "Năm" } }, { name: "Thắng", des: { demo: "Năm" } }, true, false));

</script>   

<style scoped>
</style>