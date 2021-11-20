import axios from "axios";
import TPrototype from "@/assets/scripts/prototype/GetPrototype";
import TCrypto from '@/assets/scripts/en-decrypt/Crypto';
import PagingRequest from '@/models/requests/PagingRequest';

const _baseURL = import.meta.env.VITE_BASE_API_URL?.toString();

const _TPrototype = new TPrototype();
const _TCrypto = new TCrypto();

const _APIS = axios.create({
    baseURL: _baseURL
});

// Class base call API
class BaseAPIS {
    _subsystemCode: string = '';

    constructor(subsystemCode: string) {
        if (_TPrototype.globals().$IsNullOrEmpry(subsystemCode))
            throw new Error("Mã phân hệ chưa được gán");
        else
            this._subsystemCode = subsystemCode;
    }

    /**
     * Hàm thực hiện lấy dữ liệu phân trang
     * @param data 
     * created by vdthang 19.11.2021
     */
    getPaging(data: PagingRequest) {
        // Thực hiện mã hóa filter
        data.Filter = _TCrypto.encrypt(data.Filter);
        return _APIS.post(_baseURL?.concat(this._subsystemCode, '/Paging'), data);
    }
}

export default BaseAPIS;