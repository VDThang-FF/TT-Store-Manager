const ENUM = {
    // Mã code response
    Code: {

    },

    // Loại format ngày tháng năm giờ phút giây
    DateType: {
        DD_MM_YYYY: 1,
        MM_DD_YYYY: 2,
        YYYY_MM_DD: 3,
        DD_MM_YYYY_HH_mm_ss: 4,
        MM_DD_YYYY_HH_mm_ss: 5,
        YYYY_MM_DD_HH_mm_ss: 6
    },

    // Loại dữ liệu
    DataType: {
        Array: 1,
        Object: 2,
        Number: 3,
        Date: 4,
        String: 5
    }
}

export default ENUM;