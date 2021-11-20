<template>
    <div>Tính năng đang trong quá trình phát triển</div>
</template>

<script setup lang="ts">
import TPrototype from '@/assets/scripts/prototype/GetPrototype';
import ProductAPI from '@/apis/implement/Product';
import PagingRequest from '@/models/requests/PagingRequest';

var request = new PagingRequest();
request.Filter = [
    {
        Prefix: null,
        PropName: "name",
        Condition: "=&all",
        Value: null
    }
]
new ProductAPI("Products").getPaging(request);

</script>   

<style scoped>
</style>