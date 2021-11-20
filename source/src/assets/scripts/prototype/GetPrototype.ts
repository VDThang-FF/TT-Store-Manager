import { getCurrentInstance } from "vue";

class TPrototype {
    constructor() {

    }

    /**
     * Hàm thực hiện lấy toàn bộ global properties
     * @returns 
     * created by vdthang 18.11.2021
     */
    globals() {
        return getCurrentInstance()?.appContext.config.globalProperties;
    }

    /**
     * Hàm thực hiện lấy toàn bộ các props của component
     * @returns 
     * created by vdthang 18.11.2021
     */
    props() {
        return getCurrentInstance()?.props;
    }

    /**
     * Hàm thực hiện lấy toàn bộ dữ liệu emit của components
     * @returns 
     * created by vdthang 18.11.2021
     */
    emits() {
        return getCurrentInstance()?.emit;
    }

    /**
     * Hàm thực hiện lấy toàn bộ reference của component
     * @returns 
     * created by vdthang 18.11.2021
     */
    refs() {
        return getCurrentInstance()?.refs;
    }
}

export default TPrototype;