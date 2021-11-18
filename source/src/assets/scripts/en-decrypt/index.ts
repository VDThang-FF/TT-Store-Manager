import { KeyManagementServiceClient } from '@google-cloud/kms';
const crc32c = require('fast-crc32c');

class TTSecure {
    projectId: string = 'TTStoreManager';
    locationId: string = 'vp-tt-01';
    idRing: string = 'kr-01';
    idKey: string = 'kk-01';
    readonly client = new KeyManagementServiceClient();
    readonly locationName = this.client.locationPath(this.projectId, this.locationId);
    readonly keyRingName = this.client.keyRingPath(this.projectId, this.locationId, this.idRing);

    genKeyRingId: string = '';
    genKeyId: string = '';

    constructor() {
    }

    /**
     * Hàm thực hiện tạo Key Ring
     * @returns 
     * created by vdthang 18.11.2021
     */
    async createKeyRing() {
        const [keyRing] = await this.client.createKeyRing({
            parent: this.locationName,
            keyRingId: this.idRing,
        });

        return keyRing;
    }

    /**
     * Hàm thực hiện tạo key đối xứng
     * @returns 
     * created by vdthang 18.11.2021
     */
    async createKeySymmetric() {
        const [key] = await this.client.createCryptoKey({
            parent: this.keyRingName,
            cryptoKeyId: this.idKey,
            cryptoKey: {
                purpose: 'ENCRYPT_DECRYPT',
                versionTemplate: {
                    algorithm: 'GOOGLE_SYMMETRIC_ENCRYPTION',
                },
            },
        });

        return key;
    }

    /**
     * Hàm thực hiện mã hõa plaintext
     * @param plainText
     * created by vdthang 18.11.2021
     */
    async encrypt(plainText: string) {
        const keyName = this.client.cryptoKeyPath(this.projectId, this.locationId, this.idRing, this.idKey);
        const plaintextCrc32c = crc32c.calculate(plainText);

        const [encryptResponse] = await this.client.encrypt({
            name: keyName,
            plaintext: plainText,
            plaintextCrc32c: {
                value: plaintextCrc32c,
            },
        });

        const ciphertext = encryptResponse.ciphertext;

        if (!encryptResponse.verifiedPlaintextCrc32c)
            throw new Error('Encrypt: request corrupted in-transit');

        if (crc32c.calculate(ciphertext) !== Number(encryptResponse?.ciphertextCrc32c?.value))
            throw new Error('Encrypt: response corrupted in-transit');

        return ciphertext;
    }

    /**
     * Hàm thực hiện giải mã ciphertext
     * @param ciphertext 
     * created by vdthang 18.11.2021
     */
    async decrypt(ciphertext: string) {
        const keyName = this.client.cryptoKeyPath(this.projectId, this.locationId, this.idRing, this.idKey);
        const ciphertextCrc32c = crc32c.calculate(ciphertext);

        const [decryptResponse] = await this.client.decrypt({
            name: keyName,
            ciphertext: ciphertext,
            ciphertextCrc32c: {
                value: ciphertextCrc32c,
            },
        });

        if (crc32c.calculate(decryptResponse.plaintext) !== Number(decryptResponse?.plaintextCrc32c?.value))
            throw new Error('Decrypt: response corrupted in-transit');

        const plaintext = decryptResponse?.plaintext?.toString();
        return plaintext;
    }
}

export default TTSecure;