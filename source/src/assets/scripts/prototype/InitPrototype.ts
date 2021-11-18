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
        // Check null or empty string
        app.config.globalProperties.$IsNullOrEmpry = (source) => {
            if (source == null || source == undefined || source.trim() == '')
                return true;
            return false;
        };

        // Check email validate
        app.config.globalProperties.$IsEmail = (source) => {
            if (source == null || source == undefined || source.trim() == '')
                return false;

            const re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            return re.test(String(source).toLowerCase());
        };
    }

    /**
     * Hàm thực hiện khởi tạo prototype cho loại Number
     * created by vdthang 18.11.2021
     */
    initPrototypeNumber() {
    }
}

export default InitPrototype;