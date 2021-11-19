import { App } from 'vue';
import ENUM from '@/enums';
import Moment from 'moment';
import FNumber from 'accounting';

class InitPrototype {

    constructor(app: App) {
        this.initPrototype(app);
    }

    /**
     * Hàm thực hiện khởi tạo prototype dùng chung cho toàn bộ chương trình
     * created by vdthang 18.11.2021
     */
    initPrototype(app: App) {
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

        // Format tiền tệ
        app.config.globalProperties.$FormatMoney = (source, thousand = '.', decimal = ",", precision = 0, originalNegative = true) => {
            if (source == null || source == undefined || typeof (source) != 'number')
                return 0;

            var formatObj = {
                pos: "%s %v",
                neg: "%s (%v)",
                zero: "%s 0"
            }
            if (originalNegative)
                formatObj.neg = "%s -%v";

            return FNumber.formatMoney(source, {
                symbol: "",
                precision: precision,
                thousand: thousand,
                decimal: decimal,
                format: formatObj
            })
        };

        // Format ngày tháng năm giờ phút giây
        app.config.globalProperties.$FormatDateTime = (source, formatType = ENUM.DateType.DD_MM_YYYY) => {
            if (source == null || source == undefined || typeof (source) != 'object')
                return "";

            switch (formatType) {
                case ENUM.DateType.DD_MM_YYYY:
                    return Moment(source).format('DD/MM/YYYY');
                case ENUM.DateType.MM_DD_YYYY:
                    return Moment(source).format("MM/DD/YYYY");
                case ENUM.DateType.YYYY_MM_DD:
                    return Moment(source).format("YYYY/MM/DD");
                case ENUM.DateType.DD_MM_YYYY_HH_mm_ss:
                    return Moment(source).format("DD/MM/YYYY HH:mm:ss");
                case ENUM.DateType.MM_DD_YYYY_HH_mm_ss:
                    return Moment(source).format("MM/DD/YYYY HH:mm:ss");
                case ENUM.DateType.YYYY_MM_DD_HH_mm_ss:
                    return Moment(source).format("YYYY/MM/DD HH:mm:ss");
            }
        }
    }
}

export default InitPrototype;