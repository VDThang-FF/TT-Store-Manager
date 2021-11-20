import CryptoJS from 'crypto-js';

class TCrypto {
    secretKey: string = CryptoJS.enc.Utf8.parse('7061737323313233');
    ivKey: string = CryptoJS.enc.Utf8.parse('7061737323313233');

    /**
     * Hàm thực hiện mã hóa
     * @param data 
     * @returns 
     * created by vdthang 19.11.2021
     */
    encrypt(data: Object) {
        return CryptoJS.AES.encrypt(JSON.stringify(data), this.secretKey, {
            keySize: 128 / 8,
            iv: this.ivKey,
            mode: CryptoJS.mode.CBC,
            padding: CryptoJS.pad.Pkcs7
        }).toString();
    }

    /**
     * Hàm thực hiện giải mã
     * @param ciphertext 
     * @returns 
     * created by vdthang 19.11.2021
     */
    decrypt(ciphertext: string) {
        try {

            var bytes = CryptoJS.AES.decrypt(ciphertext, this.secretKey, {
                keySize: 128 / 8,
                iv: this.ivKey,
                mode: CryptoJS.mode.CBC,
                padding: CryptoJS.pad.Pkcs7
            });
            var decryptedData = JSON.parse(bytes.toString(CryptoJS.enc.Utf8));
            return decryptedData;
        } catch (error) {
            console.log("Đoạn string mã hóa sai định dạng!")
        }
    }
}

export default TCrypto;