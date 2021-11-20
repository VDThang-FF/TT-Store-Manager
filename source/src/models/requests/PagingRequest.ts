class PagingRequest {
    PageIndex: int = 1;
    PageSize: int = 25;
    Filter: string = null;
    Prefix: string = null;

    constructor(pageIndex: int, pageSize: int, filter: string, prefix: string) {
        this.PageIndex = pageIndex;
        this.PageSize = pageSize;
        this.Filter = filter;
        this.Prefix = prefix;
    }
}

export default PagingRequest;