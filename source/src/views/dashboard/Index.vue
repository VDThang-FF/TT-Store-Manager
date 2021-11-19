<template>
    <div>Tính năng đang trong quá trình phát triển</div>
</template>

<script setup lang="ts">
import GetPrototype from '@/assets/scripts/prototype/GetPrototype';
import ENUM from '@/enums';

const Prototype = new GetPrototype();
if (Prototype != null)
    console.log(Prototype.globals().$FormatMoney(-1000000, ",", ".", 2, false));

</script>   

<style scoped>
</style>