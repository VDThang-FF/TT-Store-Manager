import axios from "axios";
const _baseURL = import.meta.env.VITE_BASE_API_URL?.toString();

const APIS = axios.create({
    baseURL: _baseURL
});

// Class base call API
class BaseAPIS {
    _subsystemCode: string = '';

    constructor(subsystemCode: string){
    }
}