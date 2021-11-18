import { App } from 'vue';

class InitPrototype {

    constructor(app: App) {
        this.initPrototypeString(app);
        this.initPrototypeNumber();
    }

    /**
     * Hàm thực hiện khởi tạo prototype cho loại string
     * created by vdthang 18.11.2021
     */
    initPrototypeString(app: App) {
        app.config.globalProperties.$demo = 1;
    }

    /**
     * Hàm thực hiện khởi tạo prototype cho loại Number
     * created by vdthang 18.11.2021
     */
    initPrototypeNumber() {
    }
}

export default InitPrototype;